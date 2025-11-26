using UnityEngine;

public static class LevelProgress
{
    private const string KEY = "LastUnlockedLevel";

  
    public static int LastUnlockedLevel
    {
        get { return PlayerPrefs.GetInt(KEY, 1); }
        set { PlayerPrefs.SetInt(KEY, value); }
    }

    public static void UnlockNextLevel(int currentLevel)
    {
        if (currentLevel >= LastUnlockedLevel)
        {
            LastUnlockedLevel = currentLevel + 1;
        }
    }
}
