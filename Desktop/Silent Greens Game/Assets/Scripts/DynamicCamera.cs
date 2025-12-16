using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DynamicCameraLandscape : MonoBehaviour
{

    public float designWidth = 2960f;
    public float designHeight = 1440f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        AdjustCamera();
    }

    void AdjustCamera()
    {
        float targetAspect = designWidth / designHeight;  
        float deviceAspect = (float)Screen.width / Screen.height;

        if (deviceAspect < targetAspect)
        {
            float scale = targetAspect / deviceAspect;
            cam.orthographicSize *= scale;
        }
       
    }
}
