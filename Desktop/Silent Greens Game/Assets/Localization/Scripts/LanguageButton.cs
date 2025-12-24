using UnityEngine;

public class LanguageButton : MonoBehaviour
{
    public void English()
    {
        LocalizationManager.Instance?.SetLanguage("en");
    }

    public void Portuguese()
    {
        LocalizationManager.Instance?.SetLanguage("pt-BR");
    }

    public void Indonesian()
    {
        LocalizationManager.Instance?.SetLanguage("id");
    }
}
