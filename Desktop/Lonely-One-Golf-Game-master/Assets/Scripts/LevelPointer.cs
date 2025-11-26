using UnityEngine;

public class LevelPointer : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float verticalOffset = 80f; 

    private RectTransform pointer;
    private RectTransform targetButton;

    void Awake()
    {
        pointer = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (targetButton != null)
        {
            
            Vector2 targetPos = targetButton.anchoredPosition + new Vector2(0, verticalOffset);

            pointer.anchoredPosition = Vector2.Lerp(
                pointer.anchoredPosition,
                targetPos,
                Time.deltaTime * moveSpeed
            );
        }
    }

    public void MoveTo(Transform btn)
    {
        targetButton = btn.GetComponent<RectTransform>();
    }
}
