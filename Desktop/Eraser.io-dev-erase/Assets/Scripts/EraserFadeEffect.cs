using UnityEngine;
using System.Collections;

public class EraseFadeEffect : MonoBehaviour
{
    public SpriteRenderer sprite;
    public float fadeInTime = 0.1f;
    public float fadeOutTime = 0.2f;

    Coroutine currentRoutine;

    void Awake()
    {
        if (!sprite) sprite = GetComponent<SpriteRenderer>();
        sprite.color = new Color(1, 1, 1, 0);
        gameObject.SetActive(false);
    }

    public void Play(Vector3 position)
    {
        transform.position = position;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        gameObject.SetActive(true);
        currentRoutine = StartCoroutine(FadeRoutine());
    }

    IEnumerator FadeRoutine()
    {
       
        float t = 0;
        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            sprite.color = new Color(1, 1, 1, t / fadeInTime);
            yield return null;
        }

        t = 0;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            sprite.color = new Color(1, 1, 1, 1 - (t / fadeOutTime));
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
