using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [Header("Panels")]
    public GameObject optionsPanel;
    public GameObject settingsPanel;
    public GameObject languagePanel;

    [Header("Buttons")]
    public GameObject continueBtn;
    public GameObject playBtn;

    [Header("Audio")]
    public AudioClip music;

    const string LANG_SELECTED_KEY = "LANG_SELECTED";

    void Start()
    {
      
        SoundManager.Instance.PlayMusic(music);

     
        optionsPanel.SetActive(false);
        settingsPanel.SetActive(false);
        languagePanel.SetActive(false);

    
        int last = PlayerPrefs.GetInt("LastUnlockedLevel", 0);
        continueBtn.SetActive(last > 0);
    }


    public void OnPlayButton()
    {
        settingsPanel.SetActive(false);

        int langSelected = PlayerPrefs.GetInt(LANG_SELECTED_KEY, 0);

        if (langSelected == 0)
        {
       
            languagePanel.SetActive(true);
        }
        else
        {
            OpenOptionsPanel();
        }
    }

    
    public void OnLanguageConfirmed()
    {
        PlayerPrefs.SetInt(LANG_SELECTED_KEY, 1);
        PlayerPrefs.Save();

        languagePanel.SetActive(false);
        OpenOptionsPanel();
    }

    void OpenOptionsPanel()
    {
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
}
