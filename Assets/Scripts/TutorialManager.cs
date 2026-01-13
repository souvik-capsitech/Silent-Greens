using UnityEngine;

public static class TutorialManager
{
    private const string KEY = "TutorialShown";

    public static bool IsTutorialShown
    {
        get => PlayerPrefs.GetInt(KEY, 0) == 1;
        set => PlayerPrefs.SetInt(KEY, value ? 1 : 0);
    }
}
