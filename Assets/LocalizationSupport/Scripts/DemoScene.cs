using UnityEngine;
using UnityEngine.UI;
using System;

public class DemoScene : MonoBehaviour
{
    public Text proceduralStringText;
    public Toggle autodetectToggle;
    public Button nextButton;
    public Button prevButton;

    private static string[] languages = new string[3] { "English", "German", "Polish" };
    private static int currentLanguage = 0;

    private static bool autodetectCache = false;
    private bool sceneLoading;

    //****** Here is how you change language at runtime. ******
    //Alternatively, you can setup language settings for the whole game in editor using LocalizationSupportSetup prefab (refer to the second demo scene).
    private void UpdateLanguage()
    {
        //First set whether to overwrite any language setting with system language.
        LocalizationSupport.UseSystemLanguage = autodetectToggle.isOn;

        //Set the language you want. Has no effect if UseSystemLanguage is on.
        LocalizationSupport.CurrentLanguage = LocalizationSupport.StringToLanguage(languages[currentLanguage]);

        //Reload the level to apply changes to UI. Note LocalizationSupport.GetString(string) will work without reloading the scene.
        Application.LoadLevel(0);
    }

    private void Start()
    {
        if (autodetectToggle.isOn != autodetectCache)
        {
            sceneLoading = true;
            autodetectToggle.isOn = autodetectCache;
        }
    }

    public void AutodetectLanguageChecked()
    {
        nextButton.interactable = !autodetectToggle.isOn;
        prevButton.interactable = !autodetectToggle.isOn;

        if (!sceneLoading)
        {
            autodetectCache = autodetectToggle.isOn;
            UpdateLanguage();
        }
    
        sceneLoading = false;
    }

    public void NextButtonPressed()
    {
        currentLanguage++;

        if (currentLanguage > 2)
            currentLanguage = 0;

        UpdateLanguage();
    }

    public void PrevButtonPressed()
    {
        currentLanguage--;

        if (currentLanguage < 0)
            currentLanguage = 2;

        UpdateLanguage();
    }
}
