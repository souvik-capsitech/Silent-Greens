using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialUI;
    public float displayTime = 5f;

    void Start()
    {
        ShowTutorial();
    }

    void ShowTutorial()
    {
        tutorialUI.SetActive(true);
        
        Invoke(nameof(HideTutorial), displayTime);
    }

    void HideTutorial()
    {
        tutorialUI.SetActive(false);
    }
}
