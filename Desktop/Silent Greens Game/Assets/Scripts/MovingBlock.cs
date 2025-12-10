using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float moveDistance = 2f;
    public float moveSpeed = 2f;

    private Vector3 startPos;
    private Vector3 endPos;
    private bool isMoving = true;
    void Start()
    {
        startPos = transform.position;
        endPos = startPos + Vector3.right * moveDistance;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            isMoving ? endPos : startPos,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, endPos) < 0.05f)
            isMoving = false;
        else if (Vector3.Distance(transform.position, startPos) < 0.05f)
            isMoving = true;
    }
}

