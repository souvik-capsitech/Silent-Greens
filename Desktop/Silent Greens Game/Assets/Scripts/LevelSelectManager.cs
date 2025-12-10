using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LevelSelectManager : MonoBehaviour
{
    [Header("UI References")]
    public ScrollRect scrollRect;
    public Button homeBtn;
    public Button swipeLeftBtn;
    public Button swipeRightBtn;
    public TextMeshProUGUI levelText;

    [Header("Pointer & Buttons")]
    public RectTransform pointer;
    public LevelPointer levelPointer;

    public RectTransform[] levelButtons;

    [Header("Settings")]
    public int totalLevels = 15;
    private int lastUnlockedLevel;
    private float swipeAmount = 0.25f;

    private void OnEnable()
    {
        StartCoroutine(InitAfterFrame());
    }

    private IEnumerator InitAfterFrame()
    {
        yield return null;

        
        lastUnlockedLevel = PlayerPrefs.GetInt("LastUnlockedLevel", 0);

        UpdateLevelText();
        UpdatePointerPosition();
    }

    private void Start()
    {
        

        homeBtn.onClick.AddListener(GoHome);
        swipeLeftBtn.onClick.AddListener(SwipeLeft);
        swipeRightBtn.onClick.AddListener(SwipeRight);
    }

    private void UpdateLevelText()
    {
        levelText.text = (lastUnlockedLevel) + " / " + totalLevels;

    }

    private void UpdatePointerPosition()
    {
        if (pointer != null && levelButtons.Length > 0)
        {
            int idx = Mathf.Clamp(lastUnlockedLevel, 0, levelButtons.Length - 1);

            
            levelPointer.MoveTo(levelButtons[idx]);
        }
    }


    public void SetLastUnlockedLevel(int level)
    {
    
        lastUnlockedLevel = Mathf.Clamp(level, 0, totalLevels - 1);
        PlayerPrefs.SetInt("LastUnlockedLevel", lastUnlockedLevel);

        UpdateLevelText();
        UpdatePointerPosition();
    }

    public void GoHome()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SwipeLeft()
    {
        scrollRect.horizontalNormalizedPosition =
            Mathf.Clamp01(scrollRect.horizontalNormalizedPosition - swipeAmount);
    }

    public void SwipeRight()
    {
        scrollRect.horizontalNormalizedPosition =
            Mathf.Clamp01(scrollRect.horizontalNormalizedPosition + swipeAmount);
    }
}
