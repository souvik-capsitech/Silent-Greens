using UnityEngine;
using UnityEngine.UI;

public class FeverManager : MonoBehaviour
{
    [Header("UI")]
    public Image fillImage;
    public float maxFever = 100f;

    [Header("Fever Settings")]
    public float feverIncrease = 15f;
    public float feverDecay = 10f;
    public float feverThreshold = 70f;
    public float slideSpeed = 3f;

    [Header("Eraser Scale")]
    public Transform eraser;
    public float normalEraserScale = 1f;
    public float feverEraserScale = 1.5f;
    public float maxFeverEraserScale = 2.2f;

    [Header("Eraser Fever Visual")]
    public SpriteRenderer eraserRenderer;
    public GameObject eraserGlow;
    public Color normalEraserColor = Color.white;
    public Color feverEraserColor = Color.yellow;

    [Header("Player Fever Visual")]
    public GameObject feverBar;

    float feverValue;
    bool isFeverActive;
    bool isErasing;


    [Header("Fever Text")]
    public FeverTextEffect feverText;

    public bool IsFeverActive => isFeverActive;

    void Start()
    {
        feverValue = 0f;
        fillImage.fillAmount = 0f;

        if (feverBar != null)
            feverBar.SetActive(false);

        if (eraserRenderer != null)
            eraserRenderer.color = normalEraserColor;

        if (eraserGlow != null)
            eraserGlow.SetActive(false);
    }

    void Update()
    {
      
        if (!isErasing)
            feverValue -= feverDecay * Time.deltaTime;

        feverValue = Mathf.Clamp(feverValue, 0, maxFever);

 
        float targetFill = feverValue / maxFever;
        fillImage.fillAmount = Mathf.MoveTowards(
            fillImage.fillAmount,
            targetFill,
            slideSpeed * Time.deltaTime
        );

  
        if (isFeverActive)
        {
           
            eraser.localScale = Vector3.Lerp(
                eraser.localScale,
                Vector3.one * maxFeverEraserScale,
                10f * Time.deltaTime
            );

        
            if (eraserGlow != null)
            {
             
                eraserGlow.transform.localScale = Vector3.one ;
            }
        }
        else
        {
            
            float targetScale = Mathf.Lerp(
                normalEraserScale,
                feverEraserScale,
                fillImage.fillAmount
            );

            eraser.localScale = Vector3.Lerp(
                eraser.localScale,
                Vector3.one * targetScale,
                10f * Time.deltaTime
            );
        }

        CheckFeverState();
        isErasing = false;
    }

    public void AddFever()
    {
        isErasing = true;
        feverValue += feverIncrease;
        feverValue = Mathf.Clamp(feverValue, 0, maxFever);
    }
    void CheckFeverState()
    {
        if (!isFeverActive && feverValue >= feverThreshold)
        {
            isFeverActive = true;

            if (feverBar != null)
                feverBar.SetActive(true);

            if (eraserRenderer != null)
                eraserRenderer.color = feverEraserColor;

            if (eraserGlow != null)
                eraserGlow.SetActive(true);

           
            if (feverText != null)
                feverText.Play();
        }

    
        if (isFeverActive && feverValue <= 0f)
        {
            isFeverActive = false;

            if (feverBar != null)
                feverBar.SetActive(false);

            if (eraserRenderer != null)
                eraserRenderer.color = normalEraserColor;

            if (eraserGlow != null)
                eraserGlow.SetActive(false);
        }
    }

}
