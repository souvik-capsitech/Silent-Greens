using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayButtonAnim : MonoBehaviour
{
    public Image buttonImage;
    public Sprite spriteA;
    public Sprite spriteB;
    public float bounceSize = 1.12f;
    public float bounceTime = 0.25f;

    Vector3 originalScale;
    bool flip = false;

    void Start()
    {
        originalScale = transform.localScale;
        StartCoroutine(AnimateLoop());
    }

    IEnumerator AnimateLoop()
    {
        while (true)
        {
            buttonImage.sprite = flip ? spriteA : spriteB;
            flip = !flip;

            Vector3 upScale = originalScale * bounceSize;

            float t = 0;
            while (t < bounceTime)
            {
                t += Time.deltaTime;
                transform.localScale = Vector3.Lerp(originalScale, upScale, t / bounceTime);
                yield return null;
            }

            t = 0;
            while (t < bounceTime)
            {
                t += Time.deltaTime;
                transform.localScale = Vector3.Lerp(upScale, originalScale, t / bounceTime);
                yield return null;
            }
        }
    }
}
