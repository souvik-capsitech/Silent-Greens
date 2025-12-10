using UnityEngine;

public class Trajectory : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject dotPrefab;
    public int dotCount = 20;
    public Transform ball;

    public LayerMask groundLayer;

    GameObject[] dots;

    float dotSpacing = 0.1f;
    Vector2 gravity;


    void Start()
    {
        gravity = Physics2D.gravity;
        GenerateDots();
    }

    void GenerateDots()
    {
        dots =  new GameObject[dotCount];
        for(int i=0; i<dotCount; i++)
        {
            dots[i] = Instantiate(dotPrefab);

            TrajectoryDot td = dots[i].GetComponent<TrajectoryDot>();
            td.ball = ball;
            dots[i].SetActive(false);

        }
    }

    public void Show(Vector2 startPos, Vector2 velocity)
    {
        for (int i = 0; i < dotCount; i++)
        {
            float t = i * dotSpacing;

           
            Vector2 pos = startPos + velocity * t + 0.5f * gravity * (t *t);

            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 0.1f, groundLayer);


            if (hit.collider != null)
            {

                dots[i].SetActive(false);
                break;
            }
            dots[i].transform.position = pos;
            dots[i].SetActive(true);
        }
    }
    public void Hide()
    {
        if (dots == null || dots.Length == 0) return;

        foreach (var dot in dots)
            if (dot != null)
                dot.SetActive(false);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
