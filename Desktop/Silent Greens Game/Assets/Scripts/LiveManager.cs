using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class LiveManager : MonoBehaviour
{

    public int maxLives = 3;
    public int currentLives;
    //public GameObject gameOverPanel;
    public Image[] lifeIcons;
    public Sprite lifeOnSprite;
    public Sprite lifeOffSprite;
    private float lifeCooldown = 0f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (currentLives <= 0)
            currentLives = maxLives;

        UpdateLifeUI();
    }


    public void LoseLife()
    {
        if (lifeCooldown > 0f)
            return;

        if (PauseManager.justResumed)
            return;

        lifeCooldown = 0.75f; // block repeated hits

        currentLives--;

        int index = Mathf.Clamp(currentLives, 0, lifeIcons.Length - 1);

        if (currentLives <= 0)
        {
            lifeIcons[index].sprite = lifeOffSprite;
            GameOver();
            return;
        }

        OopsPopUp.instance.PlayOops();
        StartCoroutine(AnimateLifeLoss(index));

        var levelManager = FindFirstObjectByType<LevelManager>();
        levelManager.LoadLevel(levelManager.CurrentLevelIndex);
    }





    IEnumerator AnimateLifeLoss(int idx)
    {
        RectTransform icon = lifeIcons[idx].rectTransform;
        Vector3 startPos = icon.localPosition;
        float duration = 0.2f;
        float time = 0;

        while (time < duration)
        {
            icon.localPosition = startPos + (Vector3)UnityEngine.Random.insideUnitCircle * 4f;
            time += Time.deltaTime;
            yield return null;
        }
        icon.localPosition = startPos;

       
        lifeIcons[idx].sprite=lifeOffSprite  ;
    }

    void UpdateLifeUI()
    {
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            if (i < currentLives)
                lifeIcons[i].sprite = lifeOnSprite;
            else
                lifeIcons[i].sprite = lifeOffSprite;
        }
    }
    void GameOver()
    {
        Debug.Log("Game Over!");

      
        GameUI gameUI = FindFirstObjectByType<GameUI>();
        if (gameUI != null)
            gameUI.ShowGameOver();
        else
            Debug.LogError("GameUI not found in scene!");

      
        // Time.timeScale = 0f;
    }


    // Update is called once per frame
    void Update()
    {
        if (lifeCooldown > 0f)
            lifeCooldown -= Time.unscaledDeltaTime;
    }

}
