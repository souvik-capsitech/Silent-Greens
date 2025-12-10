    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class GameUI : MonoBehaviour
    {
      public GameObject gameOverPanel;
    public TMPro.TextMeshProUGUI finalScoreText;
    public TMPro.TextMeshProUGUI bestScoreText;
    public void PlayGame()
        {
            SceneManager.LoadScene("LevelSelect");
        }
        public void RestartLevel()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    public void Home()
    {
        Time.timeScale = 1f;
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

        gameOverPanel.SetActive(true);
        //Time.timeScale = 0f;
    }


    public void QuitLevel()
    {
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
