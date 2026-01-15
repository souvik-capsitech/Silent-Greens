//using UnityEngine;
//using System.Collections;

//public class UIPanelTransition : MonoBehaviour
//{
//    public float duration = 0.25f;
//    public Vector3 startScale = new Vector3(0.9f, 0.9f, 0.9f);

//    CanvasGroup canvasGroup;

//    void Awake()
//    {
//        canvasGroup = GetComponent<CanvasGroup>();
//    }

//    void OnEnable()
//    {
//        StopAllCoroutines();
//        StartCoroutine(AnimateIn());
//    }

//    public void HidePanel()
//    {
//        StopAllCoroutines();
//        StartCoroutine(AnimateOut());
//    }

//    IEnumerator AnimateIn()
//    {
//        float t = 0f;
//        canvasGroup.alpha = 0f;
//        transform.localScale = startScale;

//        while (t < duration)
//        {
//            t += Time.unscaledDeltaTime;
//            float progress = t / duration;

//            canvasGroup.alpha = Mathf.Lerp(0f, 1f, progress);
//            transform.localScale = Vector3.Lerp(startScale, Vector3.one, progress);

//            yield return null;
//        }

//        canvasGroup.alpha = 1f;
//        transform.localScale = Vector3.one;
//    }

//    IEnumerator AnimateOut()
//    {
//        float t = 0f;
//        float startAlpha = canvasGroup.alpha;
//        Vector3 startS = transform.localScale;

//        while (t < duration)
//        {
//            t += Time.unscaledDeltaTime;
//            float progress = t / duration;

//            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, progress);
//            transform.localScale = Vector3.Lerp(startS, startScale, progress);

//            yield return null;
//        }

//        canvasGroup.alpha = 0f;
//        gameObject.SetActive(false);
//    }
//}


using UnityEngine;
using System.Collections;

public class UIPanelSlideTransition : MonoBehaviour
{
    public float duration = 0.4f;
    public float dropOffset = 300f;
    public float bounceScale = 1.05f;

    CanvasGroup canvasGroup;
    RectTransform rectTransform;
    Vector2 targetPos;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        targetPos = rectTransform.anchoredPosition;
    }

    void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateIn());
    }

    public void HidePanel()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateOut());
    }

    IEnumerator AnimateIn()
    {
        float t = 0f;
        canvasGroup.alpha = 0f;
        rectTransform.anchoredPosition = targetPos + Vector2.up * dropOffset;
        transform.localScale = Vector3.one;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float progress = t / duration;

            float eased = EaseOutBounce(progress);

            canvasGroup.alpha = Mathf.Lerp(0f, 1f, progress);
            rectTransform.anchoredPosition =
                Vector2.Lerp(targetPos + Vector2.up * dropOffset, targetPos, eased);

            yield return null;
        }

        // Small overshoot scale
        transform.localScale = Vector3.one * bounceScale;
        yield return new WaitForSecondsRealtime(0.05f);
        transform.localScale = Vector3.one;
    }

    IEnumerator AnimateOut()
    {
        float t = 0f;
        Vector2 startPos = rectTransform.anchoredPosition;

        while (t < duration * 0.6f)
        {
            t += Time.unscaledDeltaTime;
            float progress = t / (duration * 0.6f);

            canvasGroup.alpha = Mathf.Lerp(1f, 0f, progress);
            rectTransform.anchoredPosition =
                Vector2.Lerp(startPos, startPos + Vector2.up * dropOffset, progress);

            yield return null;
        }

        gameObject.SetActive(false);
    }

    float EaseOutBounce(float x)
    {
        if (x < 1 / 2.75f)
            return 7.5625f * x * x;
        else if (x < 2 / 2.75f)
        {
            x -= 1.5f / 2.75f;
            return 7.5625f * x * x + 0.75f;
        }
        else if (x < 2.5 / 2.75)
        {
            x -= 2.25f / 2.75f;
            return 7.5625f * x * x + 0.9375f;
        }
        else
        {
            x -= 2.625f / 2.75f;
            return 7.5625f * x * x + 0.984375f;
        }
    }
}
