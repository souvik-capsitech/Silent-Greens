using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static ScoreManager instance;
    public int score = 0;
    public int cleanStreak = 0;
    public int comboReq = 3;
    public int comboBonus = 15;
    public Action<int> OnScoreUpdated;
    public Action<int> OnComboTriggered;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

    }

    public void AddDirectShot()
    {
        score += 5;
        cleanStreak++;
        Debug.Log("DirectHit");

        if(cleanStreak>= comboReq)
        {
            score += comboBonus;
            cleanStreak = 0;

            Debug.Log("Combo");

            OnComboTriggered?.Invoke(score);
        }
        OnScoreUpdated?.Invoke(score);
    }

    public void AddNormalShot()
    {
        score += 2;
        cleanStreak = 0;

        Debug.Log("Normal Shot");

        OnScoreUpdated?.Invoke(score);
    }

    public void ResetScore()
    {
        score = 0;
        cleanStreak = 0;

        OnScoreUpdated?.Invoke(score);
    }

    public void SaveHighScore()
    {
        int best = PlayerPrefs.GetInt("HighScore", 0);

        if(score>best)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
        }
    }

    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
