using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreUI : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text comboText;

    private Vector3 comboOriginalScale;

    private void Start()
    {
        comboOriginalScale = comboText.transform.localScale;

        comboText.text = "";
        comboText.transform.localScale = Vector3.zero;

        ScoreManager.instance.OnScoreUpdated += UpdateScore;
        ScoreManager.instance.OnComboTriggered += ShowCombo;
    }

    private void UpdateScore(int newScore)
    {
        scoreText.text = "" + newScore;
    }

    private void ShowCombo(int newScore)
    {
        scoreText.text = "" + newScore;


if (ComboFloat.instance != null)
            ComboFloat.instance.PlayCombo("COMBO! +15");


}



    private void OnDestroy()
    {
        ScoreManager.instance.OnScoreUpdated -= UpdateScore;
        ScoreManager.instance.OnComboTriggered -= ShowCombo;
    }
}
