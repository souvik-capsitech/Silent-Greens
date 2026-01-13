using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    public GameObject explosionEffect;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ball"))
        {
            Break();
        }
    }


    void Break()
    {
        
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        
        Destroy(gameObject);
    }
}
