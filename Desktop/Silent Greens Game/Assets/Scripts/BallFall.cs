using System.Collections;
using UnityEngine;

public class BallFallDetector : MonoBehaviour
{
    public float fallLimit = -15f;
    private Vector3 startPos;
    private Rigidbody2D rb;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (transform.position.y < fallLimit)
        {
            HandleFall();
        }
    }

    void HandleFall()
    {
        LiveManager lifeManager = FindFirstObjectByType<LiveManager>();

       
        lifeManager.LoseLife();

       
        if (lifeManager.currentLives > 0)
        {
            ResetBall();
        }
       
    }

    void ResetBall()
    {
      
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }
}
