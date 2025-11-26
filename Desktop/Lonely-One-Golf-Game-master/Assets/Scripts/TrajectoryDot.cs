using UnityEngine;

public class TrajectoryDot : MonoBehaviour
{
    public Transform ball;
    public float hideDist = 0.2f;

    void Update()
    {
        if (ball == null) return;

        
        if (Vector2.Distance(ball.position, transform.position) < hideDist)
        {
            gameObject.SetActive(false);
        }
    }
}
