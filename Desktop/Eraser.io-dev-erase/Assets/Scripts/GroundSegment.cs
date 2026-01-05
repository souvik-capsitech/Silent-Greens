using UnityEngine;

public class GroundSegment : MonoBehaviour
{
    private EdgeCollider2D edgeCollider;
    private SpriteRenderer spriteRenderer;
    private LineRenderer lineRenderer;
    private float segmentWidth;
    private bool isErased = false;

    public void Initialize(float width, float heightDifference = 0f)
    {
        segmentWidth = width;

        

        // Setup EdgeCollider2D
        edgeCollider = GetComponent<EdgeCollider2D>();
        if (edgeCollider == null)
            edgeCollider = gameObject.AddComponent<EdgeCollider2D>();

        // Create smooth edge points connecting to previous segment
        Vector2[] points = new Vector2[2];
        points[0] = new Vector2(-segmentWidth / 2f, heightDifference); // Connect to previous
        points[1] = new Vector2(segmentWidth / 2f, 0f); // Current position
        edgeCollider.points = points;
        edgeCollider.edgeRadius = 0.05f;

        // Setup visual line
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
        lineRenderer.sortingOrder = 1;
        lineRenderer.useWorldSpace = false;

        // Draw line connecting segments
        lineRenderer.SetPosition(0, new Vector3(-segmentWidth / 2f, heightDifference, 0f));
        lineRenderer.SetPosition(1, new Vector3(segmentWidth / 2f, 0f, 0f));

        // Disable sprite renderer - we're using LineRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;
    }

    public void EraseAt(Vector2 worldPos)
    {
        if (isErased) return;

        // Convert world position to local
        Vector2 localPos = transform.InverseTransformPoint(worldPos);

        // Check if within segment bounds (wider detection for easier erasing)
        if (Mathf.Abs(localPos.x) > segmentWidth / 2f || Mathf.Abs(localPos.y) > 0.5f)
            return;

        // Erase this segment
        isErased = true;
        edgeCollider.enabled = false;
        lineRenderer.enabled = false;

        if (spriteRenderer != null)
            spriteRenderer.enabled = false;
    }

    public void Restore()
    {
        isErased = false;
        edgeCollider.enabled = true;
        lineRenderer.enabled = true;
    }
}