using UnityEngine;

public class LevelAnalytics : MonoBehaviour
{
    [Header("Game Level Index (0-based)")]
    public int levelIndex; 

    int attemptCount = 1;
    bool finished = false;

    void Start()
    {
        attemptCount = 1;
        finished = false;

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
