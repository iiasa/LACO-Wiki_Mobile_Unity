using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationSupport : MonoBehaviour
{
    public LocalizationSetup LocalizationSetupObjectOptional;

    public static bool UseSystemLanguage
    {
        get { return useSystemLanguage; }
        set
        {
            useSystemLanguage = value;

            if (useSystemLanguage)
            {
                previousLanguage = currentLanguage;
                currentLanguage = Application.systemLanguage;
            }
        }
    }

    public static string StringsXMLPath
    {
        get { return stringsXMLPath; }
        set { stringsXMLPath = value; }
    }

    public static SystemLanguage CurrentLanguage
    {
        get { return currentLanguage; }
        set
        {
            if (!useSystemLanguage)
            {
                previousLanguage = currentLanguage;
                currentLanguage = value;
            }
        }
    }

    public static bool StringsLoaded
    {
        get { return ((previousLanguage == currentLanguage) && (strings != null)); }
    }

    private static string stringsXMLPath = "Strings";
    private static SystemLanguage currentLanguage = SystemLanguage.English;
    private static bool useSystemLanguage = true;

    private static XmlDocument stringsXml;
    private static Dictionary<string, string> strings = null;
    private static Dictionary<string, string> images = null;
    private static SystemLanguage previousLanguage = currentLanguage;

    private void Awake()
    {
        if ((!StringsLoaded) && (LocalizationSetupObjectOptional == null))
            LoadStrings();
    }

    private void Start()
    {
        if (!StringsLoaded)
            return;

        Text[] textObjectsInScene = Resources.FindObjectsOfTypeAll<Text>();

        foreach (Text text in textObjectsInScene)
        {
            if (strings.ContainsKey(text.text))
                text.text = strings[text.text];
        }

        Image[] imageObjectsInScene = Resources.FindObjectsOfTypeAll<Image>();

        foreach (Image image in imageObjectsInScene)
        {
            if (images.ContainsKey(image.gameObject.name))
            {
                if (!LocalizationSpriteLibrary.SpriteExists(images[image.gameObject.name]))
                {
                    Debug.LogWarning("Localization Support: An image named: " + images[image.gameObject.name] + " is declared, but not present in the sprite library. Skipping.");
                    continue;
                }

                image.sprite = LocalizationSpriteLibrary.GetSprite(images[image.gameObject.name]);
            }
        }
    }

    public static void LoadStrings()
    {
        TextAsset textAsset = (TextAsset)Resources.Load(stringsXMLPath);
        stringsXml = new XmlDocument();

        try
        {
            stringsXml.LoadXml(textAsset.text);
        }
        catch (Exception)
        {
            throw new Exception("Localization Support: Could not load strings XML file. It is either empty or doesn't exist.");
        }

        XmlNode currentLanguageNode = null;

        if (stringsXml.GetElementsByTagName("strings").Count == 0)
            throw new Exception("Localization Support: Strings XML is invalid.");

        XmlNodeList languagesAvailable = stringsXml.GetElementsByTagName("strings").Item(0).ChildNodes;
        foreach (XmlNode language in languagesAvailable)
        {
            if (language.LocalName == currentLanguage.ToString())
            {
                currentLanguageNode = language;
                break;
            }
        }

		//-----------------------
		// Language not found
		if (currentLanguageNode == null) {
			currentLanguage = SystemLanguage.English;
			languagesAvailable = stringsXml.GetElementsByTagName("strings").Item(0).ChildNodes;
			foreach (XmlNode language in languagesAvailable)
			{
				if (language.LocalName == currentLanguage.ToString())
				{
					currentLanguageNode = language;
					break;
				}
			}
		}
		//-----------------------

        if (currentLanguageNode == null)
            throw new Exception("Localization Support: Strings for current language (" + currentLanguage.ToString() + ") are not present.");

        strings = new Dictionary<string, string>();
        images = new Dictionary<string, string>();

        XmlNodeList stringsList = currentLanguageNode.ChildNodes;
        foreach (XmlNode node in stringsList)
        {
            string stringToAdd = node.Attributes["id"].Value;

            if (strings.ContainsKey(stringToAdd))
                Debug.LogError("Localization Support: Duplicate string found: " + stringToAdd);
            else
            {
				if (node.LocalName == "string") {
					string strrep = node.InnerText.Replace ("\\n", "\n");
					strings.Add (stringToAdd, strrep);
				} else if (node.LocalName == "image")
                    images.Add(stringToAdd, node.InnerText);
            }
        }

        previousLanguage = currentLanguage;
    }

    public static string GetString(string id)
    {
        if (strings == null)
        {
            Debug.LogError("Localization Support: Tried to get string: " + id + " while Strings XML is not loaded!");
            return null;
        }

        if (strings.ContainsKey(id))
            return strings[id];

        Debug.LogError("Localization Support: Tried to get string which does not exist: " + id);
        return null;
    }

    public static Sprite GetImage(string id)
    {
        if (images == null)
        {
            Debug.LogError("Localization Support: Tried to get image: " + id + " while Strings XML is not loaded!");
            return null;
        }

        if (images.ContainsKey(id))
        {
            if (!LocalizationSpriteLibrary.SpriteExists(images[id]))
            {
                Debug.LogError("Localization Support: Tried to get image: " + id + " while it doesn't exist in the sprite library!");
                return null;
            }

            return LocalizationSpriteLibrary.GetSprite(images[id]);
        }

        Debug.LogError("Localization Support: Tried to get image which does not exist: " + id);
        return null;
    }

    public static SystemLanguage StringToLanguage(string language)
    {
        try
        {
            SystemLanguage result = (SystemLanguage)Enum.Parse(typeof(SystemLanguage), language, true);
            return result;
        }
        catch (Exception)
        {
            throw new Exception("Localization Configuration: Language name is invalid!");
        }
    }
}
