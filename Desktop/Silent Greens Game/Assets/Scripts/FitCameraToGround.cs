using UnityEngine;

public class DynamicCameraFit : MonoBehaviour
{
    public SpriteRenderer ground;
    public float topPadding = 3f;  
    public float bottomPadding = 1f;
    public float horizontalOffset = 0f;

    void Start()
    {
        AdjustCamera();
    }

    void AdjustCamera()
    {
        if (ground == null) return;

        Camera cam = Camera.main;

        float screenAspect = (float)Screen.width / Screen.height;

       
        float groundWidth = ground.bounds.size.x;

       
        float sizeForWidth = groundWidth / (2f * screenAspect);

       
        cam.orthographicSize = sizeForWidth;

        Vector3 pos = cam.transform.position;
        pos.x = ground.transform.position.x + horizontalOffset;

    
        pos.y = ground.bounds.min.y + cam.orthographicSize + bottomPadding;

        cam.transform.position = pos;
    }

}
