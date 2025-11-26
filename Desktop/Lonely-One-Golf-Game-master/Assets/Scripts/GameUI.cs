    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class GameUI : MonoBehaviour
    {
        public GameObject gameOverPanel;


        public void PlayGame()
        {
            SceneManager.LoadScene("LevelSelect");
        }
        public void RestartLevel()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void ShowGameOver()
        {
            gameOverPanel.SetActive(true);
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
