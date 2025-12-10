using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int levelNumber;
    //public GameObject lockIcon;
    public Button button;

    void Start()
    {
        int unlocked = LevelProgress.LastUnlockedLevel;

        if (levelNumber <= unlocked)
        {
            //lockIcon.SetActive(false);
            button.interactable = true;
        }
        else
        {
            //lockIcon.SetActive(true); 
            button.interactable = false;
        }
    }

    public void OnClick()
    {
        LevelLoader.LoadGameplayLevel(levelNumber);
    }
}
