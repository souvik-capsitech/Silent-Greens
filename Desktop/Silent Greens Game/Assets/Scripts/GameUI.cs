    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class GameUI : MonoBehaviour
    {
      public GameObject gameOverPanel;
    public TMPro.TextMeshProUGUI finalScoreText;
    public TMPro.TextMeshProUGUI bestScoreText;
    public GameObject pauseButton;

    public void PlayGame()
        {
            SceneManager.LoadScene("LevelSelect");
        }
        public void RestartLevel()
        {
            Time.timeScale = 1f;
        if (pauseButton != null)
            pauseButton.SetActive(true);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    public void Home()
    {
        Time.timeScale = 1f;
        if (pauseButton != null)
            pauseButton.SetActive(true);
        SceneManager.LoadScene("MainMenu");
    }


    public void ShowGameOver()
    {
       
        ScoreManager.instance.SaveHighScore();

     
        finalScoreText.text = ScoreManager.instance.score.ToString();
        int best = PlayerPrefs.GetInt("HighScore", 0);
        bestScoreText.text = best.ToString();

        Debug.Log(" Current Score: " + finalScoreText.text);
        Debug.Log(" Best Score: " + bestScoreText.text);


        LevelManager lm = FindAnyObjectByType<LevelManager>();
        if (lm != null)
            LevelLoader.levelToLoad = lm.CurrentLevelIndex;
        gameOverPanel.SetActive(true);
        if (pauseButton != null)
            pauseButton.SetActive(false);

        Time.timeScale = 0f;
    }


    public void QuitLevel()
    {
        Time.timeScale = 1f;

        if (pauseButton != null)
            pauseButton.SetActive(true);

        SceneManager.LoadScene("LevelSelect");
    }
    public void QuitGame()

        {
            Debug.Log("Quit Game");


    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
        }
    }
