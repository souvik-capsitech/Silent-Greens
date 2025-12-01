using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Camera cam;
    public float defaultSize = 5f;
    public float zoomSize = 3f;
    public float zoomSpeed = 2f;
    public float zoomDistance = 2f;
    public float zoomYOffset = 1f;
    private float originalCamY;

    [HideInInspector] public Transform ball;
    [HideInInspector] public Transform hole;

    private float targetSize;
    private float fixedCamY;
    public float minY;
    public float maxY;


 
  private void Start()
    {
        if (cam == null) cam = Camera.main;

      
        originalCamY = cam.transform.position.y;

        fixedCamY = originalCamY;

        targetSize = defaultSize;
        cam.orthographicSize = defaultSize;
    }

    private void Update()
    {
        if (cam == null || ball == null || hole == null) return;

        float dist = Vector2.Distance(ball.position, hole.position);
        bool closeToHole = dist <= zoomDistance;

       
        targetSize = closeToHole ? zoomSize : defaultSize;

        cam.orthographicSize = Mathf.Lerp(
            cam.orthographicSize,
            targetSize,
            Time.deltaTime * zoomSpeed
        );

      
        Vector3 pos = cam.transform.position;

        if (closeToHole)
        {
            
            pos.y = Mathf.Lerp(pos.y, hole.position.y + zoomYOffset, Time.deltaTime * zoomSpeed);
        }
        else
        {
       
            pos.y = fixedCamY;
        }
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        cam.transform.position = pos;
        
    }

    public void ResetCamera()
    {
        targetSize = defaultSize;
        cam.orthographicSize = defaultSize;

        
        fixedCamY = originalCamY;

        Vector3 pos = cam.transform.position;
        pos.y = originalCamY;
        cam.transform.position = pos;

      
        ball = null;
        hole = null;
    }

}
