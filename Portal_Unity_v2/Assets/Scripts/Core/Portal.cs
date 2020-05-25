using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class Portal : MonoBehaviour {

    [Header ("Main Settings")]

    // portal a ser conectado a este sendo instanciado
    public Portal linkedPortal;

    // plano renderizando a camera, neste portal
    // 1
    public MeshRenderer renderPlane;
    
    //2
    //private MeshRenderer renderPlane;

    // formatos de plano
    public enum PlaneShape { Square, Sphere };
    public PlaneShape planeShape;


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

        Debug.Log(planeShape);

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



        // define o render plane deste portal



        // 1
        Renderer rend = linkedPortal.renderPlane.GetComponent<MeshRenderer>();

        if (rend != null)
        {
            Material renderPlane_material = Resources.Load("Materials/Core/renderPlane_material", typeof(Material)) as Material;
            rend.sharedMaterial = renderPlane_material;
        }


        //// 2
        //renderPlane = GameObject.Find("renderPlaneSquare").GetComponent<MeshRenderer>();

        //if (renderPlane != null)
        //{
        //    Material renderPlane_material = Resources.Load("Materials/Core/renderPlane_material", typeof(Material)) as Material;

        //    renderPlane.sharedMaterial = renderPlane_material;
        //}




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

            // 1
            linkedPortal.renderPlane.material.SetTexture("_MainTex", viewTexture);

            // 2
            //linkedPortal.renderPlane.material.SetTexture("_MainTex", viewTexture);
        }
    }

}