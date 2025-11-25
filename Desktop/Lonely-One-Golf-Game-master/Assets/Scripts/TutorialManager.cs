using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialUI; // Assign your tutorial panel here
    public float displayTime = 5f; // How long the tutorial stays visible

    void Start()
    {
        ShowTutorial();
    }

    void ShowTutorial()
    {
        tutorialUI.SetActive(true);
        // Automatically hide after displayTime seconds
        Invoke(nameof(HideTutorial), displayTime);
    }

    void HideTutorial()
    {
        tutorialUI.SetActive(false);
    }
}
