using UnityEngine;

public class TrajectoryDot : MonoBehaviour
{
    public Transform ball;
    public float hideDist = 0.25f;

    void Update()
    {
        if (ball == null) return;

        float dist = Vector2.Distance(ball.position, transform.position);

     
        gameObject.SetActive(dist > hideDist);
    }
}
