using UnityEngine;

public class LevelAnalytics : MonoBehaviour
{
    [Header("Game Level Index (0-based)")]
    public int levelIndex;

    private int attemptCount = 1;
    private bool finished = false;
    private bool adShownForThisLevel = false;

    void Start()
    {
        attemptCount = 1;
        finished = false;
        adShownForThisLevel = false;

        int analyticsLevel = levelIndex + 1;

        if (FirebaseManager.Instance != null)
        {
            FirebaseManager.Instance.LogLevelStart(analyticsLevel);
        }
    }

    public void LogFail()
    {
        if (finished) return;

        int analyticsLevel = levelIndex + 1;

    
        if (FirebaseManager.Instance != null)
        {
            FirebaseManager.Instance.LogLevelFail(analyticsLevel, attemptCount);
        }

        if (attemptCount >= 7 && !adShownForThisLevel)
        {
            adShownForThisLevel = true;

            if (InterstitialAdManager.Instance != null)
            {
                InterstitialAdManager.Instance.ShowInterstitialIfReady();
            }
        }

        attemptCount++;
    }

    public void LogComplete()
    {
        if (finished) return;

        finished = true;
        int analyticsLevel = levelIndex + 1;

        if (FirebaseManager.Instance != null)
        {
            FirebaseManager.Instance.LogLevelComplete(analyticsLevel, attemptCount);
        }
    }
}
