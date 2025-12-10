using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro; 

public class WindTutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;
    public RectTransform spotlightCircle;
    public Button gotItButton;
    public TMP_Text tutorialText; 

    public float animationDuration = 1f;
    public float startSize = 2000f;
    public float endSize = 200f;
    public float textFadeDuration = 0.5f; 

    private Transform currentTriangle;

    private void Awake()
    {
        tutorialPanel.SetActive(false);
        spotlightCircle.gameObject.SetActive(false);
        tutorialText.gameObject.SetActive(false); 
        gotItButton.onClick.AddListener(CloseTutorial);
    }

    public void PlayWindTutorial(GameObject triangle, string textToShow)
    {
        if (triangle == null) return;

        currentTriangle = triangle.transform;

        tutorialPanel.SetActive(true);
        spotlightCircle.gameObject.SetActive(true);

        spotlightCircle.sizeDelta = new Vector2(startSize, startSize);

        tutorialText.gameObject.SetActive(false);
        tutorialText.text = textToShow;
        tutorialText.canvasRenderer.SetAlpha(0f);

        StopAllCoroutines();
        StartCoroutine(AnimateSpotlight());
    }


    private IEnumerator AnimateSpotlight()
    {
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / animationDuration);

            float size = Mathf.Lerp(startSize, endSize, t);
            spotlightCircle.sizeDelta = new Vector2(size, size);

            UpdateSpotlightPosition();

            yield return null;
        }

        spotlightCircle.sizeDelta = new Vector2(endSize, endSize);
        UpdateSpotlightPosition();

    
        StartCoroutine(FadeInText());
       
    }

    private IEnumerator FadeInText()
    {
        tutorialText.gameObject.SetActive(true);
        float elapsed = 0f;

        while (elapsed < textFadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / textFadeDuration);
            tutorialText.canvasRenderer.SetAlpha(alpha);
            yield return null;
        }

        tutorialText.canvasRenderer.SetAlpha(1f);
    }

  
    private IEnumerator TypewriterText()
    {
        tutorialText.gameObject.SetActive(true);
        tutorialText.text = "";
        string fullText = "Your tutorial text here";

        for (int i = 0; i <= fullText.Length; i++)
        {
            tutorialText.text = fullText.Substring(0, i);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void UpdateSpotlightPosition()
    {
        if (currentTriangle == null) return;

        Vector3 worldPos = currentTriangle.position;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            spotlightCircle.parent as RectTransform,
            screenPos,
            tutorialPanel.GetComponentInParent<Canvas>().worldCamera,
            out localPos
        );

        spotlightCircle.anchoredPosition = localPos;
    }

    private void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
        spotlightCircle.gameObject.SetActive(false);
        tutorialText.gameObject.SetActive(false);
        currentTriangle = null;

        PlayerPrefs.SetInt("WindTutorialShown", 1);
        PlayerPrefs.Save();
    }
}
