using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;
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
            ShowCancelPopup();
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
        Application.Quit();
    }

    public void OnCancelExit()
    {
        cancelPopUp.SetActive(false);
        Time.timeScale = 1f;
    }
}
