//using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float worldSpeed = 2f;

    [Header("UI Panels")]
    public GameObject homePanel;
    public GameObject gameplayPanel;
    public GameObject gameOverPanel;

    [Header("Gameplay UI")]
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highDistanceText;

    [Header("Game Over UI")]
    public TextMeshProUGUI finalDistanceText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI highDistanceGameOverText;
    //public TextMeshProUGUI newRecordText;

    int score;
    float distance;
    Vector3 startPos;
    public Transform player;

    // High scores
    private int highScore;
    private float highDistance;

    void Awake()
    {
        Instance = this;
        LoadHighScores();
    }

    void Start()
    {
        if (player != null)
        {
            startPos = player.position;
        }
        ShowHome();
    }

    void Update()
    {
        if (!gameplayPanel.activeSelf) return;

        distance += worldSpeed * Time.deltaTime;

        // Update gameplay UI
        distanceText.text = $"Distance: {Mathf.FloorToInt(distance)} m";
        scoreText.text = $"Score: {score}";

        // Show high distance during gameplay
        if (highDistanceText != null)
        {
            highDistanceText.text = $"Best: {Mathf.FloorToInt(highDistance)} m";
        }
    }

    void LoadHighScores()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highDistance = PlayerPrefs.GetFloat("HighDistance", 0f);
        Debug.Log($"Loaded - High Score: {highScore}, High Distance: {highDistance}m");
    }

    void SaveHighScores()
    {
        // Update high score if current score is better
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            Debug.Log($"New High Score: {highScore}");
        }

        // Update high distance if current distance is better
        if (distance > highDistance)
        {
            highDistance = distance;
            PlayerPrefs.SetFloat("HighDistance", highDistance);
            Debug.Log($"New High Distance: {highDistance}m");
        }

        PlayerPrefs.Save();
    }

    public void ShowHome()
    {
        Time.timeScale = 0f;
        homePanel.SetActive(true);
        gameplayPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        score = 0;
        distance = 0;

        // Reset player position
        if (player != null)
        {
            player.position = startPos;

            // Let GroundBuilder position player correctly
            GroundBuilder groundBuilder = FindFirstObjectByType<GroundBuilder>();
            if (groundBuilder != null)
            {
                groundBuilder.SendMessage("PositionPlayerOnGround", SendMessageOptions.DontRequireReceiver);
            }
        }

        homePanel.SetActive(false);
        gameplayPanel.SetActive(true);
        gameOverPanel.SetActive(false);
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void GameOver()
    {
        Time.timeScale = 0f;

        // Save high scores before showing game over
        SaveHighScores();

        gameplayPanel.SetActive(false);
        gameOverPanel.SetActive(true);

        // Show current game stats
        finalDistanceText.text = $"Distance: {Mathf.FloorToInt(distance)} m";
        finalScoreText.text = $"Score: {score}";

        // Show high scores
        if (highScoreText != null)
        {
            highScoreText.text = $"High Score: {highScore}";
        }

        if (highDistanceGameOverText != null)
        {
            highDistanceGameOverText.text = $"Best Distance: {Mathf.FloorToInt(highDistance)} m";
        }
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Optional: Reset high scores (for testing)
    public void ResetHighScores()
    {
        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.DeleteKey("HighDistance");
        highScore = 0;
        highDistance = 0;
        Debug.Log("High scores reset!");
    }
}
