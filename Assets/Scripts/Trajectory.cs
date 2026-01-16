using UnityEngine;

public class Trajectory : MonoBehaviour
{
    public GameObject dotPrefab;
    public int dotCount = 30;           
    public Transform ball;
    public LayerMask groundLayer;

    private GameObject[] dots;
    private float dotSpacing = 0.05f;  
    private Vector2 gravity;

    void Start()
    {
        gravity = Physics2D.gravity;
        GenerateDots();
    }

    void GenerateDots()
    {
        dots = new GameObject[dotCount];
        for (int i = 0; i < dotCount; i++)
        {
            dots[i] = Instantiate(dotPrefab);
            dots[i].SetActive(false);
        }
    }

    public void Show(Vector2 startPos, Vector2 velocity)
    {
        if (dots == null || dots.Length == 0) return;

        for (int i = 0; i < dotCount; i++)
        {
            float t = i * dotSpacing;

            
            Vector2 pos = startPos + velocity * t + 0.5f * gravity * (t * t);

          
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 0.1f, groundLayer);
            if (hit.collider != null)
            {
                dots[i].SetActive(false);
                break;
            }

            dots[i].transform.position = pos;
            dots[i].SetActive(true);

            float speed = velocity.magnitude;
            float scale = Mathf.Lerp(0.3f, 0.8f, speed / 8f); 
            dots[i].transform.localScale = Vector3.one * 0.1f;

        
            var sr = dots[i].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                float alpha = Mathf.Lerp(0.2f, 1f, 1f - (float)i / dotCount);
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
            }
        }
    }

    public void Hide()
    {
        if (dots == null || dots.Length == 0) return;
        foreach (var dot in dots)
            if (dot != null) dot.SetActive(false);
    }
}
