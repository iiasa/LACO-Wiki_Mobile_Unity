using UnityEngine;
using System;
using System.IO;

public class LocalizationSetup : MonoBehaviour
{
    public string Language = "English";
    public bool UseSystemLanguage = true;
    public string StringsXmlResource = "Strings";

    private void Awake()
    {
        //Setting whether to use system language first. If it is on, then setting current language has no effect.
        LocalizationSupport.UseSystemLanguage = UseSystemLanguage;
		/*
		if (Language.CompareTo ("English") == 0 || Language.CompareTo ("German") == 0) {
			
		} else {
			Language = "English";
		}*/

        LocalizationSupport.CurrentLanguage = LocalizationSupport.StringToLanguage(Language);


        string originalPath = LocalizationSupport.StringsXMLPath;

        try
        {
            LocalizationSupport.StringsXMLPath = Path.GetFileNameWithoutExtension(StringsXmlResource);
        }
        catch (Exception)
        {
            Debug.LogError("Localization Configuration: Strings XML Path string is invalid!");
        }

        if ((!LocalizationSupport.StringsLoaded) || (originalPath != StringsXmlResource))
            LocalizationSupport.LoadStrings();
    }
}
