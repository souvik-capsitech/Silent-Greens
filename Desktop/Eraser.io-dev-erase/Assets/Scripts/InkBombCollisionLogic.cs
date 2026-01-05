//using System;
//using System.Diagnostics;
using UnityEngine;

public class InkBombCollisionLogic : MonoBehaviour
{
    [Header("Drift Settings")]
    public float backwardDriftMin = -10f;
    public float backwardDriftMax = -5f;
    public float randomVariation = 1f;

    [Header("Physics")]
    public float gravityScale = 1.5f;
    public float maxLifetime = 15f;

    private Rigidbody2D rb;
    private float baseWindDrift;
    private float wobbleTimer = 0f;
    private float lifetimeTimer = 0f;
    private bool hasHitPlayer = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("InkBomb needs Rigidbody2D!");
            return;
        }

        baseWindDrift = Random.Range(backwardDriftMin, backwardDriftMax);
        rb.gravityScale = gravityScale;

        Debug.Log($"InkBomb drift: {baseWindDrift}");
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        // Add subtle wobble for more natural motion
        wobbleTimer += Time.fixedDeltaTime;
        float wobble = Mathf.Sin(wobbleTimer * 3f) * randomVariation;

        // Apply backward drift + wobble
        float currentDrift = baseWindDrift + wobble;
        rb.linearVelocity = new Vector2(currentDrift, rb.linearVelocity.y);
    }

    void Update()
    {
        // Auto-destroy old bombs
        lifetimeTimer += Time.deltaTime;
        if (lifetimeTimer > maxLifetime)
        {
            Destroy(gameObject);
            return;
        }

        // Only destroy if WELL BELOW screen bottom (generous margin)
        Camera cam = Camera.main;
        if (cam != null)
        {
            float bottomY = cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).y;
            float leftX = cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, 0f)).x;

            // Destroy only when FAR below screen or FAR left of screen
            if (transform.position.y < bottomY - 5f || transform.position.x < leftX - 8f)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"InkBomb hit: {collision.name} (Tag: {collision.tag})");

        if (collision.CompareTag("Player") && !hasHitPlayer)
        {
            hasHitPlayer = true;
            Debug.Log("Player hit by InkBomb!");
            GameManager.Instance.GameOver();
            Destroy(gameObject);
        }
        
    }
}