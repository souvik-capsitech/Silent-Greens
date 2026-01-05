using System.Collections;
using TMPro;
using UnityEngine;

public class FeverTextEffect : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float fadeInTime = 0.2f;
    public float stayTime = 0.6f;
    public float fadeOutTime = 0.3f;

    Coroutine routine;

    void Awake()
    {
        if (!text) text = GetComponent<TextMeshProUGUI>();
        SetAlpha(0);
        gameObject.SetActive(false);
    }

    public void Play()
    {
        if (routine != null)
            StopCoroutine(routine);

        gameObject.SetActive(true);
        routine = StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        float t = 0;
        while (t < fadeInTime)
        {
            t += Time.unscaledDeltaTime;
            float a = t / fadeInTime;
            SetAlpha(a);

            
            transform.localScale = Vector3.one * (1f + Mathf.Sin(Time.unscaledTime * 10f) * 0.05f);

            yield return null;
        }


        yield return new WaitForSecondsRealtime(stayTime);

     
        t = 0;
        while (t < fadeOutTime)
        {
            t += Time.unscaledDeltaTime;
            SetAlpha(1f - (t / fadeOutTime));
            yield return null;
        }

        transform.localScale = Vector3.one;
        gameObject.SetActive(false);


    }

    void SetAlpha(float a)
    {
        Color c = text.color;
        c.a = a;
        text.color = c;
    }
}
