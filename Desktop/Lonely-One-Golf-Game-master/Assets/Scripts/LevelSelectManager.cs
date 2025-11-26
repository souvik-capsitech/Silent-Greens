using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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
    public RectTransform[] levelButtons;  

    [Header("Settings")]
    public int totalLevels = 15;
    private int lastUnlockedLevel;
    private float swipeAmount = 0.25f;

    private void OnEnable()
    {
        
        lastUnlockedLevel = PlayerPrefs.GetInt("LastUnlockedLevel", 1);

        scrollRect.horizontalNormalizedPosition = (lastUnlockedLevel - 1) / (float)(totalLevels - 1);
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
        levelText.text = lastUnlockedLevel + " / " + totalLevels;
    }

   
    private void UpdatePointerPosition()
    {
        if (pointer != null && levelButtons.Length > 0)
        {
            int idx = Mathf.Clamp(lastUnlockedLevel - 1, 0, levelButtons.Length - 1);
            pointer.position = levelButtons[idx].position;
        }
    }

  
    public void SetLastUnlockedLevel(int level)
    {
        lastUnlockedLevel = Mathf.Clamp(level, 1, totalLevels);
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
