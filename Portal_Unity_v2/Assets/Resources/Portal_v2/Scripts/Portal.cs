using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;


// para debug no visual studio:
// System.Diagnostics.Debug.WriteLine("teste de detete");


public class Portal : MonoBehaviour {

    [Header ("Main Settings")]

    // portal a ser conectado (destino/linked portal) a esta instancia
    public Portal linkedPortal;


    // formatos de plano
    public enum PlaneShape { Square, Sphere };
    public PlaneShape planeShape;


    // Private variables

    // rendered texture que contem a visao da camera do portal remoto e que vai ser aplicada no material do portal local
    private RenderTexture viewTexture;

    // camera do portal local
    private Camera portalCam;



    private void Start()
    {
        // Inicializa todos os parametros da instancia do portal que esta em cena
        Init_Portal();
    }



    private void Init_Portal()
    {
        // define as posicoes inici

        //Debug.Log(planeShape);

        Init_CameraPosition();

        Init_ScreenRenders();
    }



    private void Init_CameraPosition()
    {
    
        portalCam = GetComponentInChildren<Camera>();
        portalCam.enabled = true;

        Vector3 newPortalCamPosition = new Vector3(portalCam.transform.position.x, Camera.main.transform.position.y, portalCam.transform.position.z);
        portalCam.transform.position = newPortalCamPosition;

    }

    private void Init_ScreenRenders()
    {

        // recupera o Mesh do renderPlane do portal linkado para poder definir o material e a rendered texture dele
        //Renderer rend = linkedPortal.renderPlane.GetComponent<MeshRenderer>();
        Renderer linkedPortalRenderPlane = linkedPortal.GetMeshFromRenderPlane(planeShape.ToString());
       
        // define o material com o shader ja aplicado, na mesh recuperada
        if (linkedPortalRenderPlane != null)
        {
            Material renderPlane_material = Resources.Load("Portal_v2/Materials/renderPlane_material", typeof(Material)) as Material;
            linkedPortalRenderPlane.material = renderPlane_material;
        }



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
            linkedPortalRenderPlane.material.SetTexture("_MainTex", viewTexture);

        }
    }

    public Renderer GetMeshFromRenderPlane(string meshShape)
    {

        Renderer ret = new Renderer();
        string shapeToLook = "";
        
        if (meshShape == "Square")
        {
            shapeToLook = "PLANE_RENDER_SQUARE";
        }
        else if (meshShape == "Sphere")
        {
            shapeToLook = "PLANE_RENDER_SPHERE";
        }

        foreach (var component in GetComponentsInChildren<Component>())
        {

            if (component.tag == shapeToLook)
            {
                ret = component.GetComponent<MeshRenderer>();
                break;

            }

        }

        return ret;
    }

}