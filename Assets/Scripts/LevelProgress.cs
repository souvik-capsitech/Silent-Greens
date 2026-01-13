using UnityEngine;

public static class LevelProgress
{
    private const string KEY = "LastUnlockedLevel";

  
    public static int LastUnlockedLevel
    {
        get { return PlayerPrefs.GetInt(KEY, 0); }
        set { PlayerPrefs.SetInt(KEY, value); }
    }

    public static void UnlockNextLevel(int levelJustCompleted)
    {
        int nextLevel = levelJustCompleted + 1;
        if (nextLevel > LastUnlockedLevel)
        {
            LastUnlockedLevel =nextLevel;
        }
    }

}
