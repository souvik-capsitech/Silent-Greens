using UnityEngine;
using TMPro;

public class OopsPopUp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static OopsPopUp instance;
    public  TextMeshProUGUI oopsText;

     void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void PlayOops()
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(Animate());
    }

    System.Collections.IEnumerator Animate()
    {
        transform.localScale = Vector3.zero;
        Color c = oopsText.color;
        c.a = 1;
        oopsText.color = c;


        float t = 0f;
        float duration = 1f;

        while(t<duration)
        {
            t+= Time.deltaTime;

            transform.localScale = Vector3.Lerp(
                Vector3.zero,
                Vector3.one,
                t * 2f
            );

            float alpha = Mathf.Lerp(1f, 0f, t / duration);
            c.a = alpha;
            oopsText.color = c;
        yield return null;
        }
    gameObject.SetActive(false);
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
