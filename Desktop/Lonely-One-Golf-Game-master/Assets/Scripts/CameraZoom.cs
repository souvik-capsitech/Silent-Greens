using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera cam;              // Main Camera
    public float defaultSize = 5f;  // Normal camera size
    public float zoomSize = 3f;     // Zoomed-in size
    public float zoomSpeed = 2f;    // Smooth zoom speed
    public float zoomDistance = 2f; // Distance to trigger zoom

    [HideInInspector]
    public Transform ball;          // Assigned by LevelManager
    [HideInInspector]
    public Transform hole;          // Assigned by LevelManager

    private float targetSize;

    private void Start()
    {
        if (cam == null) cam = Camera.main;
        targetSize = defaultSize;
        cam.orthographicSize = defaultSize;
    }

    private void Update()
    {
        if (cam == null || ball == null || hole == null) return;

        // Check distance to hole
        float dist = Vector2.Distance(ball.position, hole.position);

        // If ball is near hole, zoom in
        targetSize = (dist <= zoomDistance) ? zoomSize : defaultSize;

        // Smoothly interpolate camera size
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
    }

    // Call this when loading a new level to reset zoom
    public void ResetCamera()
    {
        targetSize = defaultSize;
        cam.orthographicSize = defaultSize;
        ball = null;
        hole = null;
    }
}
