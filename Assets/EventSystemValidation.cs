using UnityEngine;
using System.Collections;
using Unitycoding.UIWidgets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EventSystemValidation : MonoBehaviour
{


    public GameObject m_ButtonBack;
    public GameObject m_ButtonNext;
    public GameObject m_TextTitle;


    private Rect windowRect = new Rect(20, 20, 120, 50);


    private MessageBox messageBox;
    private MessageBox verticalMessageBox;

    private int m_Show = 0;


    public GameObject m_LoadingBack;
    public GameObject m_LoadingText;
    public GameObject m_TextResult;

    public GameObject m_ScrollView;

    public void ForceLandscapeLeft()
    {
        StartCoroutine(ForceAndFixLandscape());
    }

    IEnumerator ForceAndFixLandscape()
    {
        yield return new WaitForSeconds(0.01f);
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToLandscapeLeft = false;
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToPortrait = true;
        //  }
        yield return new WaitForSeconds(0.5f);
        //}
    }

    // Use this for initialization
    void Start()
    {
        ForceLandscapeLeft();

        if ((!LocalizationSupport.StringsLoaded))
            LocalizationSupport.LoadStrings();

        updateStates();


        m_LoadingText.SetActive(false);
        m_LoadingBack.SetActive(false);
        messageBox = UIUtility.Find<MessageBox>("MessageBox");


        if (messageBox == null)
        {
            Debug.Log("No message box set");
        }
        else
        {
            Debug.Log("Message set");
        }


        // createValidationsList();

        loadValidationSessions();

        m_ButtonNext.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void updateStates()
    {
        m_TextTitle.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("ChooseValidation");
        m_ButtonBack.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Back");//"Back";
        m_ButtonNext.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Next");//"Back";
        m_LoadingText.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Loading");//"Loading...";

    }


    ArrayList m_ValidationSessionIds;
    ArrayList m_ValidationSessionNames;

    public GameObject m_Content;
    public GameObject m_NameS;
    public GameObject m_UploadQuest;
    public GameObject m_DownloadQuest;
    public GameObject m_StopQuest;

    ArrayList m_Buttons;
    ArrayList m_ButtonsIds;
    ArrayList m_ButtonsStart;
    ArrayList m_ButtonsStop;

    public void createValidationsList()
    {
        int nrentries = m_ValidationSessionNames.Count;
        int nrentriesactive = m_ValidationSessionNames.Count;

        m_Buttons = new ArrayList();
        m_ButtonsStart = new ArrayList();
        m_ButtonsStop = new ArrayList();
        m_ButtonsIds = new ArrayList();


        RectTransform rectTransform2 = m_Content.GetComponent<RectTransform>();
        float scalex = rectTransform2.sizeDelta.x;
        float scaley = rectTransform2.sizeDelta.y;
        float heightentry = 260.0f;//280.0f;//350.0f;
        rectTransform2.sizeDelta = new Vector2(scalex, heightentry * nrentriesactive + 100.0f);


        float posoffset = 0;
        int nrentriesadded = 0;
        int curreport = 1;
        for (int i = nrentries - 1; i >= 0; i--)
        {
            Debug.Log("Create validation entry: " + i);

            GameObject copy;
            RectTransform rectTransform;
            float curpos;
            float curposx;
            int currank;
            string text;

            nrentriesadded++;

            copy = (GameObject)GameObject.Instantiate(m_NameS);
            copy.transform.SetParent(m_Content.transform, false);
            copy.SetActive(true);
            rectTransform = copy.GetComponent<RectTransform>();
            curpos = rectTransform.localPosition.y;
            curposx = rectTransform.localPosition.x;
            curpos -= posoffset;//i * heightentry;
            rectTransform.localPosition = new Vector2(curposx, curpos);
            copy.GetComponentInChildren<UnityEngine.UI.Text>().text = (string)m_ValidationSessionNames[i];


            copy = (GameObject)GameObject.Instantiate(m_UploadQuest);
            copy.transform.SetParent(m_Content.transform, false);
            copy.SetActive(true);
            rectTransform = copy.GetComponent<RectTransform>();
            curpos = rectTransform.localPosition.y;
            curposx = rectTransform.localPosition.x;
            curpos -= posoffset;//i * heightentry;
            rectTransform.localPosition = new Vector2(curposx, curpos);

            m_Buttons.Add(copy);
            m_ButtonsIds.Add(i + "");
            copy.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Select");//"UPLOAD";

            UnityEngine.UI.Button b = copy.GetComponent<UnityEngine.UI.Button>();
            AddListener(b, i + "");



            posoffset += heightentry;


        }

        Debug.Log("Validation list created");

    }


    void AddListener(UnityEngine.UI.Button b, string value)
    {
        b.onClick.AddListener(() => OnValidationClickedValue(value));
    }


    string m_ValidationId;
    int m_ClassificationId;
    string m_ValidationName;
    string m_ValidationDataSet;
    string m_ValidationSample;
    string m_ValidationDescription;
    string m_ValidationMethod;
    int m_ValidationSamplesTotal;
    int m_ValidationSamplesValidated;
    ArrayList m_ValidationLegend;

    int m_CurLegendRed;
    int m_CurLegendGreen;
    int m_CurLegendBlue;
    string m_CurLegendName;
    int m_CurLegendItemId;
    string m_CurLegendValue;


    int m_CurLegendSettingsDistance;
    int m_CurCardinalDirectionPhotosOptional;     int m_CurOpportunisticValidationsEnabled;     int m_CurPointPhotoOptional;     int m_CurTakeCardinalDirectionPhotos;     int m_CurTakePointPhoto; 

    public void OnValidationClickedValue(string param)
    {
        Debug.Log("OnValidationClickedValue: " + param);

        int index = int.Parse(param);
        m_ValidationId = (string)m_ValidationSessionIds[index];

        startClassification();
    }


    public void OnBackClicked()
    {
        Application.LoadLevel("DemoMap");
    }
    public void OnNextClicked()
    {
        int nrquestsdone = 0;
        if (PlayerPrefs.HasKey("NrQuestsDone"))
        {
            nrquestsdone = PlayerPrefs.GetInt("NrQuestsDone");
        }
        else
        {
            nrquestsdone = 0;
        }


        PlayerPrefs.SetString("Quest_" + nrquestsdone + "_ValidationId", m_ValidationId);
        PlayerPrefs.SetInt("Quest_" + nrquestsdone + "_ClassificationId", m_ClassificationId);

        string strtime = PlayerPrefs.GetString("CurQuestStartQuestTime");
        PlayerPrefs.SetString("Quest_" + nrquestsdone + "_" + "StartQuestTime", strtime);

        PlayerPrefs.Save();


        bool bTakePhoto = false;
        if (PlayerPrefs.GetInt("SessionSettingsTakeCardinalDirectionPhotos_" + m_ValidationId) == 1)
            bTakePhoto = true;
        if (PlayerPrefs.GetInt("SessionSettingsTakePointPhoto_" + m_ValidationId) == 1)
            bTakePhoto = true;


        if(bTakePhoto) {
            Application.LoadLevel("CameraDirs");
        } else {
            Application.LoadLevel("QuestFinished");
        }
    }


    //--------------------------
    // Validation sessions

    public void loadValidationSessions()
    {
        int nrquestsdone = 0;
        if (PlayerPrefs.HasKey("NrQuestsDone"))
        {
            nrquestsdone = PlayerPrefs.GetInt("NrQuestsDone");
        }
        else
        {
            nrquestsdone = 0;
        }

        int addnewpoint = PlayerPrefs.GetInt("Quest_" + nrquestsdone + "_TrainingPoint");

        if (addnewpoint == 1)
        {
            m_ValidationSessionIds = new ArrayList();
            m_ValidationSessionNames = new ArrayList();

            int nrvalidations = 0;
            string curvalidationid = "";
            string sessions = PlayerPrefs.GetString("ActiveSessions");
            string[] splitArray = sessions.Split(char.Parse(" "));
            for (int i = 0; i < splitArray.Length; i++)
            {
                if (splitArray[i] != "" && splitArray[i] != " ")
                {
                    string valid = splitArray[i];

                    int enabled = PlayerPrefs.GetInt("SessionSettingsOpportunisticValidationsEnabled_" + valid);
                    if (enabled == 1)
                    {
                        string name = PlayerPrefs.GetString("SessionName_" + valid);

                        m_ValidationSessionIds.Add(valid);
                        m_ValidationSessionNames.Add(name);

                        curvalidationid = valid;
                        nrvalidations++;
                    }
                }
            }

            if (nrvalidations > 1)
            {
                m_TextTitle.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("SelectValidation");
                createValidationsList();
            } else {
                m_ValidationId = curvalidationid;
                startClassification();
            }
          //  m_LoadingText.SetActive(false);
        } else {
            m_ValidationId = PlayerPrefs.GetString("CurQuestValidationId");
            Debug.Log("Start Classification with validationid: " + m_ValidationId);
            startClassification();
        }

        /*

        string url = "http://dev.laco-wiki.net/api/mobile/validationsessions";

        string token = PlayerPrefs.GetString("Token");
        Debug.Log("token: " + token);

        WWWForm form = new WWWForm();
        form.AddField("param", "param");


        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Authorization", "Bearer " + token);

        WWW www = new WWW(url, form.data, headers);

        StartCoroutine(WaitForValidationSessions(www));*/
    }


    public GameObject m_ScrollViewClassification;
    public GameObject m_ContentClassification;
    public GameObject m_NameSClassification;
    public GameObject m_UploadQuestClassification;
    public GameObject m_DownloadQuestClassification;
    public GameObject m_StopQuestClassification;

    void startClassification()
    {
        m_ScrollView.SetActive(false);
        m_TextTitle.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("SelectClassification");

        m_ScrollViewClassification.SetActive(true);
        createClassificationList();
    }


    public void createClassificationList()
    {
        int nrentries = PlayerPrefs.GetInt("SessionLegendNrItems_" + m_ValidationId);
        Debug.Log("createClassificationList " + m_ValidationId + " nrentries: " + nrentries);
        m_Buttons = new ArrayList();
        m_ButtonsStart = new ArrayList();
        m_ButtonsStop = new ArrayList();
        m_ButtonsIds = new ArrayList();


        RectTransform rectTransform2 = m_ContentClassification.GetComponent<RectTransform>();
        float scalex = rectTransform2.sizeDelta.x;
        float scaley = rectTransform2.sizeDelta.y;
        float heightentry = 140.0f;//260.0f;//280.0f;//350.0f;
        rectTransform2.sizeDelta = new Vector2(scalex, heightentry * nrentries + 100.0f);


        float posoffset = 0;
        int nrentriesadded = 0;
        int curreport = 1;
        for (int i = nrentries - 1; i >= 0; i--)
        {
            Debug.Log("Create validation entry: " + i);

            GameObject copy;
            RectTransform rectTransform;
            float curpos;
            float curposx;
            int currank;
            string text;

            nrentriesadded++;

            copy = (GameObject)GameObject.Instantiate(m_NameSClassification);
            copy.transform.SetParent(m_ContentClassification.transform, false);
            copy.SetActive(true);
            rectTransform = copy.GetComponent<RectTransform>();
            curpos = rectTransform.localPosition.y;
            curposx = rectTransform.localPosition.x;
            curpos -= posoffset;//i * heightentry;
            rectTransform.localPosition = new Vector2(curposx, curpos);
            string legendname = PlayerPrefs.GetString("Session_" + m_ValidationId + "_Legend_" + i + "_Name");
            copy.GetComponentInChildren<UnityEngine.UI.Text>().text = legendname;

            UnityEngine.UI.Image image = copy.GetComponentInChildren<UnityEngine.UI.Image>();
            if(image != null) {
                float colorr = PlayerPrefs.GetInt("Session_" + m_ValidationId + "_Legend_" + i + "_Red");
                float colorg = PlayerPrefs.GetInt("Session_" + m_ValidationId + "_Legend_" + i + "_Green");
                float colorb = PlayerPrefs.GetInt("Session_" + m_ValidationId + "_Legend_" + i + "_Blue");
                /*colorr *= 1.3f;
                colorg *= 1.3f;
                colorb *= 1.3f;*/
               /* if (colorr > 255) colorr = 255;
                if (colorg > 255) colorg = 255;
                if (colorb > 255) colorb = 255;
                if (colorr < 100) colorr = 100;
                if (colorg < 100) colorg = 100;
                if (colorb < 100) colorb = 100;*/
                colorr = (255 - colorr) * 0.5f + colorr;
                colorg = (255 - colorg) * 0.5f + colorg;
                colorb = (255 - colorb) * 0.5f + colorb;
             //   Debug.Log("Color " + i + " r: " + colorr + " g: " + colorg + " b: " + colorb);
                image.color = new Color32((byte)colorr, (byte)colorg, (byte)colorb, 255);
            }

            UnityEngine.UI.Toggle b = copy.GetComponent<UnityEngine.UI.Toggle>();
            AddListenerClassification(b, i + "");


            m_Buttons.Add(copy);
            /*
            copy = (GameObject)GameObject.Instantiate(m_UploadQuestClassification);
            copy.transform.SetParent(m_ContentClassification.transform, false);
            copy.SetActive(true);
            rectTransform = copy.GetComponent<RectTransform>();
            curpos = rectTransform.localPosition.y;
            curposx = rectTransform.localPosition.x;
            curpos -= posoffset;//i * heightentry;
            rectTransform.localPosition = new Vector2(curposx, curpos);

            m_Buttons.Add(copy);
            m_ButtonsIds.Add(i + "");
            copy.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Select");//"UPLOAD";

            UnityEngine.UI.Button b = copy.GetComponent<UnityEngine.UI.Button>();
            AddListenerClassification(b, i + "");
*/


            posoffset += heightentry;


        }

        Debug.Log("Validation list created");

    }


    void AddListenerClassification(UnityEngine.UI.Toggle b, string value)
    {
         b.onValueChanged.AddListener(delegate {
            OnClassificationClicked(value);
         });

       // b.onValueChanged.AddListener(() => OnClassificationClicked(value));
       // b.onClick.AddListener(() => OnValidationClickedValue(value));
    }

    bool m_bIgnoreClicks = false;
    public void OnClassificationClicked(string param)
    {
        if(m_bIgnoreClicks) {
            return;
        }
       // Debug.Log("OnClassificationClicked: " + param);

        m_bIgnoreClicks = true;
        GameObject gameObject;
        UnityEngine.UI.Toggle b;
        for (int i = 0; i<m_Buttons.Count; i++) {
            gameObject = (GameObject)m_Buttons[i];
            b = gameObject.GetComponent<UnityEngine.UI.Toggle>();
            b.isOn = false;
        }

        int index = int.Parse(param);
        gameObject = (GameObject)m_Buttons[m_Buttons.Count-1-index];
        b = gameObject.GetComponent<UnityEngine.UI.Toggle>();
        b.isOn = true;

        m_ClassificationId = PlayerPrefs.GetInt("Session_" + m_ValidationId + "_Legend_" + index + "_ItemId");
        string itemid = "" + m_ClassificationId;
        Debug.Log("OnClassificationClicked: " + itemid);
        m_bIgnoreClicks = false;

        m_ButtonNext.SetActive(true);

    }

}
