using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject optionsPanel;
    public GameObject continueBtn;
    public GameObject playBtn;
    public GameObject settingsPanel;

    public AudioClip music;
    void Start()
    {

        SoundManager.Instance.PlayMusic(music);
        optionsPanel.SetActive(false);
        int last = PlayerPrefs.GetInt("LastUnlockedLevel", 0);

        if (last > 0)
            continueBtn.gameObject.SetActive(true);
        else
            continueBtn.gameObject.SetActive(false);
    }

    public void OnPlayButton()
    {
        settingsPanel.SetActive(false);
        optionsPanel.SetActive(true);
        playBtn.SetActive(false);

    }
    public void OnNewGame()
    {
        PlayerPrefs.DeleteKey("LastUnlockedLevel");
        LevelProgress.LastUnlockedLevel = 0;     
        TutorialManager.IsTutorialShown = false;
        PlayerPrefs.DeleteKey("WindTutorialShown");
        PlayerPrefs.Save();

        SceneManager.LoadScene("LevelSelect");
    }

    public void OnContinueGame()
    {
        SceneManager.LoadScene("LevelSelect");
    }

     public void OnSettings()
    {
        settingsPanel.SetActive(true);
        
    
        Debug.Log("Settings Opened");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
