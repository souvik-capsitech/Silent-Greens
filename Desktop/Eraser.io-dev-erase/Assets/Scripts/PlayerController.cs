using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    bool isGameOver = false;

    public float fallDeathY = -6f;

    // Reference to fever manager
    FeverManager feverManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        feverManager = FindFirstObjectByType<FeverManager>(); // safe for single manager
    }

    void FixedUpdate()
    {
        if (isGameOver) return;

        // Stop horizontal movement
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    void Update()
    {
        if (isGameOver) return;

        // Instant death if falling
        if (transform.position.y < fallDeathY)
        {
            TriggerGameOver();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isGameOver) return;

        // If Fever is active → immune to everything
        if (feverManager != null && feverManager.IsFeverActive)
        {
            // Ignore collisions during fever
            return;
        }

        // Otherwise normal death
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("InkBomb"))
        {
            TriggerGameOver();
        }
    }

    void TriggerGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        GameManager.Instance.GameOver();
    }
}
