using System.Collections;
using UnityEngine;



public class DayNightManager : MonoBehaviour
{
    [Header("Camera")]
    public Camera mainCam;

    [Header("Sky Colors")]
    public Color dayColor = new Color(0.5f, 0.8f, 1f);
    public Color eveningColor = new Color(1f, 0.6f, 0.3f);
    public Color nightColor = new Color(0.02f, 0.05f, 0.1f);

    [Header("Transition Settings")]
    public float transitionDuration = 1.5f;

    private void Awake()
    {
        if (mainCam == null)
            mainCam = Camera.main;
    }

    public void SetTimeOfDay(TimeOfDayType type)
    {
        StopAllCoroutines();

        switch (type)
        {
            case TimeOfDayType.Day:
                StartCoroutine(FadeToColor(dayColor));
                break;
            case TimeOfDayType.Evening:
                StartCoroutine(FadeToColor(eveningColor));
                break;
            case TimeOfDayType.Night:
                StartCoroutine(FadeToColor(nightColor));
                break;
        }
    }

    private IEnumerator FadeToColor(Color targetColor)
    {
        float t = 0;
        Color startColor = mainCam.backgroundColor;

        while (t < 1)
        {
            t += Time.deltaTime / transitionDuration;
            mainCam.backgroundColor = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }
    }
}
