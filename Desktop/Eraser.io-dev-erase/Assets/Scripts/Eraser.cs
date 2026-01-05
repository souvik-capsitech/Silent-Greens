//using System.Diagnostics;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

public class Eraser : MonoBehaviour
{
    private Camera cam;
    private CircleCollider2D col;
    public EraseFadeEffect eraseEffect;

    public FeverManager feverManager;

    void Start()
    {
        cam = Camera.main;
        col = GetComponent<CircleCollider2D>();
        col.enabled = false;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(cam.transform.position.z);
        transform.position = cam.ScreenToWorldPoint(mousePos);
    }

    void Update()
    {
        FollowPointer();
        HandleInput();
    }

    void HandleInput()
    {
        col.enabled = Input.GetMouseButton(0);
    }

    void FollowPointer()
    {
        if (!UnityEngine.Application.isFocused) return;

        Vector3 mousePos = Input.mousePosition;
        if (mousePos.x < 0 || mousePos.y < 0 ||
            mousePos.x > Screen.width || mousePos.y > Screen.height)
            return;

        mousePos.z = Mathf.Abs(cam.transform.position.z);
        transform.position = cam.ScreenToWorldPoint(mousePos);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!col.enabled) return;

        if (collision.CompareTag("Obstacle"))
        {
            GameManager.Instance.AddScore(20);
            Destroy(collision.gameObject);
            eraseEffect.Play(collision.transform.position);
            feverManager.AddFever();


            Debug.Log("Erased!");
            return;
        }
        if (collision.CompareTag("InkBomb"))
        {
            if (feverManager.IsFeverActive)
                return; 

            GameManager.Instance.AddScore(10);
            Destroy(collision.gameObject);
            feverManager.AddFever();
            return;
        }

        if (collision.CompareTag("Ground"))
        {
            GroundSegment segment = collision.GetComponent<GroundSegment>();
            if (segment != null)
            {
                segment.EraseAt(transform.position);
            }
            return;
        }
    }
}