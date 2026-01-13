using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject pausePanel;
    //public static bool justResumed;
    private bool isPaused = false;
    void Start()
    {
        pausePanel.SetActive(false);
    }

    public void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        FindAnyObjectByType<PlayerMovement>()?.CancelInputOnPause();
    }

    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;

        Input.ResetInputAxes();

        PlayerMovement pm = FindAnyObjectByType<PlayerMovement>();
        if (pm != null)
        {
            pm.OnGameResumed(); 
        }
    }


    //void ClearResumeFlag()
    //{
    //    justResumed = false;
    //}
    public void Restart()
    {
        Time.timeScale = 1f;
        LevelManager lm = FindAnyObjectByType<LevelManager>();
        if (lm != null)
            LevelLoader.levelToLoad = lm.CurrentLevelIndex;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Home()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void Levels()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LevelSelect");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
