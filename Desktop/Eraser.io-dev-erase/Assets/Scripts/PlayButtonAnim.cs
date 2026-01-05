using System.Collections;
using UnityEngine;

public class PlayButtonAnim : MonoBehaviour
{
    public float bounceSize = 1.12f;
    public float bounceTime = 0.25f;
    public float pauseTime = 0.2f;

    Vector3 originalScale;
    Coroutine animRoutine;

    void OnEnable()
    {
        originalScale = transform.localScale;
        animRoutine = StartCoroutine(AnimateLoop());
    }

    void OnDisable()
    {
        if (animRoutine != null)
            StopCoroutine(animRoutine);

        transform.localScale = originalScale;
    }

    IEnumerator AnimateLoop()
    {
        while (true)
        {
            Vector3 upScale = originalScale * bounceSize;

           
            float t = 0;
            while (t < bounceTime)
            {
                t += Time.unscaledDeltaTime; 
                transform.localScale = Vector3.Lerp(originalScale, upScale, t / bounceTime);
                yield return null;
            }

     
            t = 0;
            while (t < bounceTime)
            {
                t += Time.unscaledDeltaTime;
                transform.localScale = Vector3.Lerp(upScale, originalScale, t / bounceTime);
                yield return null;
            }

            yield return new WaitForSecondsRealtime(pauseTime);
        }
    }
}
