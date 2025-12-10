using UnityEngine;
using TMPro;

public class ComboFloat : MonoBehaviour
{
    public static ComboFloat instance;
    public TextMeshProUGUI comboText;


private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void PlayCombo(string message)
    {
        comboText.text = message;
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(Animate());
    }

    System.Collections.IEnumerator Animate()
    {
        transform.localScale = Vector3.zero;

        Color c = comboText.color;
        c.a = 1;
        comboText.color = c;

        float t = 0f;
        float duration = 1f;

        while (t < duration)
        {
            t += Time.deltaTime;

            transform.localScale = Vector3.Lerp(
                Vector3.zero,
                Vector3.one,
                t * 2f
            );

            float alpha = Mathf.Lerp(1f, 0f, t / duration);
            c.a = alpha;
            comboText.color = c;

            yield return null;
        }

        gameObject.SetActive(false);
    }


}
