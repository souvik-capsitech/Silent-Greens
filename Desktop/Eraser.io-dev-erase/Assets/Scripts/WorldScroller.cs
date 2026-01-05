using UnityEngine;

public class WorldScroller : MonoBehaviour
{

    public float scrollSpeed = 7f;
    public float speedIncreaseRate = 0.1f; // Speed increase per second
    public float maxSpeed = 20f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scrollSpeed = Mathf.Min(scrollSpeed + speedIncreaseRate * Time.deltaTime, maxSpeed);

        transform.Translate(Vector2.left * scrollSpeed * Time.deltaTime);

    }
}
