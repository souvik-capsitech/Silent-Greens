using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackButtonHandler : MonoBehaviour
{
    public GameObject cancelPopUp;
    public GameObject pauseButton;

    void Start()
    {
        cancelPopUp.SetActive(false);

        if (pauseButton != null)
            pauseButton.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleBackButton();
        }
    }

    void HandleBackButton()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "GamePlay")
        {
            if (!cancelPopUp.activeSelf)
                ShowCancelPopup();
        }
        else if (sceneName == "LevelSelect")
        {
            QuitGame();
        }
    }

    void ShowCancelPopup()
    {
        cancelPopUp.SetActive(true);

        if (pauseButton != null)
            pauseButton.SetActive(false);

        Time.timeScale = 0f;
    }

    public void OnConfirmExit()
    {
        Time.timeScale = 1f;
        QuitGame();
    }

    public void OnCancelExit()
    {
        cancelPopUp.SetActive(false);

        if (pauseButton != null)
            pauseButton.SetActive(true);

        Time.timeScale = 1f;
    }

    void QuitGame()
    {
        Debug.Log("Quit Game");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
