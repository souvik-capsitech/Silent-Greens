using UnityEngine;

public class WindTutorial : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform targetWorldObject;
    public RectTransform spotlightUI;
    public Canvas canvas;
    Camera mainCam;
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetWorldObject == null) return;
        Vector3 screenPos = mainCam.WorldToScreenPoint(targetWorldObject.position);

        Vector2 uiPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPos,
            canvas.worldCamera,
            out uiPos
        );

        spotlightUI.localPosition = uiPos;

    }
}
