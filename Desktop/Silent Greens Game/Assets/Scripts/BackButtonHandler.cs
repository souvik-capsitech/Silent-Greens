
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class BackButtonHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject cancelPopUp;
    void Start()
    {
        cancelPopUp.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
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
        Time.timeScale = 0f;
    }

    public void OnConfirmExit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LevelSelect");
    }

    public void OnCancelExit()
    {
        cancelPopUp.SetActive(false);
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
