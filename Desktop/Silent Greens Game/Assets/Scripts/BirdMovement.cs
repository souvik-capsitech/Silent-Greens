using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    public float speed = 2f;
    public float endX = 12f; 

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (transform.position.x > endX)
            Destroy(gameObject);
    }
}
