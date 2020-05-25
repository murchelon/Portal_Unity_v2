using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    [Header ("Main Settings")]
    public Portal linkedPortal;
    public MeshRenderer screen;


    // Private variables

    // rendered texture que contem a visao da camera do portal remoto e que vai ser aplicada no material do portal local
    RenderTexture viewTexture;

    // camera do portal local
    Camera portalCam;

 

    void Start()
    {
        // Inicializa todos os parametros da instancia do portal que esta em cena
        Init_Portal();
    }


    void Init_Portal()
    {
        // define as posicoes inici
        Init_CameraPosition();

        Init_ScreenRenders();
    }


   
    void Init_CameraPosition()
    {
    
        portalCam = GetComponentInChildren<Camera>();
        portalCam.enabled = true;

        Vector3 newPortalCamPosition = new Vector3(portalCam.transform.position.x, Camera.main.transform.position.y, portalCam.transform.position.z);
        portalCam.transform.position = newPortalCamPosition;

    }

    void Init_ScreenRenders()
    {



        // 

        Renderer rend = linkedPortal.screen.GetComponent<MeshRenderer>();

        if (rend != null)
        {
            Material renderPlane_material = Resources.Load("Materials/renderPlane_material", typeof(Material)) as Material;

            rend.sharedMaterial = renderPlane_material;
        }

        //linkedPortal.screen.material = "";


        if (viewTexture == null || viewTexture.width != Screen.width || viewTexture.height != Screen.height)
        {
            if (viewTexture != null)
            {
                viewTexture.Release();
            }

            viewTexture = new RenderTexture(Screen.width, Screen.height, 16);

            // Render the view from the portal camera to the view texture
            portalCam.targetTexture = viewTexture;

            // Display the view texture on the screen of the linked portal
            linkedPortal.screen.material.SetTexture("_MainTex", viewTexture);

        }
    }

}