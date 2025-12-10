using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelLoader
{
    public static int levelToLoad;

    public static void LoadGameplayLevel(int levelNumber)
    {
        levelToLoad = levelNumber;
        SceneManager.LoadScene("GamePlay");  
    }
}
