using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject optionsPanel;
    public GameObject continueBtn;
    public GameObject playBtn;
    void Start()
    {
        optionsPanel.SetActive(false);
        if(PlayerPrefs.GetInt("LastUnlockedLevel", 1) ==1)
        {
           
            if (continueBtn) continueBtn.gameObject.SetActive(false);
        }    
    }

    public void OnPlayButton()
    {
        optionsPanel.SetActive(true);
        playBtn.SetActive(false);

    }
    public void OnNewGame()
    {
        PlayerPrefs.DeleteKey("LastUnlockedLevel");
        LevelProgress.LastUnlockedLevel = 1;     
        PlayerPrefs.Save();

        SceneManager.LoadScene("LevelSelect");
    }

    public void OnContinueGame()
    {
        SceneManager.LoadScene("LevelSelect");
    }

     public void OnSettings()
    {
    
        Debug.Log("Settings Opened");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
