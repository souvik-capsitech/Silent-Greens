using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FingerTrajectoryTutorial : MonoBehaviour
{
    public RectTransform greenDot;
    public RectTransform finger;
    public RectTransform line;

    public float dragDistance = 130f;
    public float dragTime = 0.6f;
    public float waitTime = 0.3f;

    private Vector2 fingerStartPos;
    private bool playing = true;

    void Start()
    {
        fingerStartPos = finger.anchoredPosition;

        line.gameObject.SetActive(false);

        StartCoroutine(TutorialLoop());
    }

    IEnumerator TutorialLoop()
    {
        while (playing)
        {
            
            finger.anchoredPosition = fingerStartPos;
            line.gameObject.SetActive(false);
            line.sizeDelta = new Vector2(300f, 0);

            yield return new WaitForSeconds(0.2f);

            Vector2 dragPosition = fingerStartPos - new Vector2(dragDistance, dragDistance * 0.35f);

            line.gameObject.SetActive(true);

            float t = 0;
            while (t < dragTime)
            {
                t += Time.deltaTime;
                float p = t / dragTime;

                
                finger.anchoredPosition = Vector2.Lerp(fingerStartPos, dragPosition, p);

         
                line.sizeDelta = new Vector2(300f, Mathf.Lerp(0, dragDistance, p));

                yield return null;
            }

        
            yield return new WaitForSeconds(waitTime);

           
            t = 0;
            while (t < dragTime)
            {
                t += Time.deltaTime;
                float p = t / dragTime;

                finger.anchoredPosition = Vector2.Lerp(dragPosition, fingerStartPos, p);
                line.sizeDelta = new Vector2(300f, Mathf.Lerp(dragDistance, 0, p));

                yield return null;
            }

            line.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.25f);
        }
    }

    public void StopTutorial()
    {
        playing = false;
        gameObject.SetActive(false);
    }
}
