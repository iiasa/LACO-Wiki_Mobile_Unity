using UnityEngine;
using System.Collections;
using Unitycoding.UIWidgets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class LegendItem
{     public int m_LegendRed;     public int m_LegendGreen;     public int m_LegendBlue;     public string m_LegendName;     public int m_LegendItemId;     public string m_LegendValue;
    

    public LegendItem()
    {
    }
}


public class SamplePoint
{
    public int m_SampleId;
    public int m_SampleLegendId;
    public int m_SampleValidated;
    public string m_SampleLat;
    public string m_SampleLng;


    public SamplePoint()
    {
    }
}

public class EventSystemValidations : MonoBehaviour
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

       // PlayerPrefs.DeleteAll();
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

        //     loginSuccessful("laco-wiki-app:///#access_token=eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IndwWDRxblFtTzVOWG1kbExUUXd6Vk53WWlZMCIsImtpZCI6IndwWDRxblFtTzVOWG1kbExUUXd6Vk53WWlZMCJ9.eyJpc3MiOiJodHRwczovL2Rldi5sYWNvLXdpa2kubmV0L2lkZW50aXR5IiwiYXVkIjoiaHR0cHM6Ly9kZXYubGFjby13aWtpLm5ldC9pZGVudGl0eS9yZXNvdXJjZXMiLCJleHAiOjE1NTU0MTIxMjgsIm5iZiI6MTU1NTQwODUyOCwiY2xpZW50X2lkIjoid2ViYXBpIiwic2NvcGUiOiJ3ZWJhcGkiLCJzdWIiOiI3MSIsImF1dGhfdGltZSI6MTU1NTQwODUyNywiaWRwIjoiR2VvV2lraSIsIm5hbWUiOiJUb2JpYXMgU3R1cm4iLCJlbWFpbCI6InRvYmlhcy5zdHVybkB2b2wuYXQiLCJyb2xlIjoiVXNlciIsImFtciI6WyJleHRlcm5hbCJdfQ.Yhw9SxK_mxEFpGCgHuhL11eI-SmTANAOBx2X-QzMx5D9LHFJmhTwwcMQvvQjIM9KBvhUmJxNTGlh4oeYlfoJM8uJvcBfLozuyy_n2qPh4FwWWYihZcn-iFEqm8PJqSA6Nm3tYe4H1MiDZyuidF6fXPgW0o6eUhEGGWB2EwbaeSvxXj7ow0xG_XWrU6ipKVrPLk79Jt4YZnH6tOa7pNe2MIcMmG2lxA-L4ccAj3OowvNTqJ8ifRYIXGN5octDTa9-Px4x4fL6ivrukeUjhedQoPNpY0jXyNAtrKdvRS-STdU13o-1toSDb4JbiADx0BIu7i5OZpSdByKFl9nzRxPLiw&token_type=Bearer&expires_in=3600&scope=webapi");


        m_AddedTexts = new ArrayList();
        // createValidationsList();

        loadValidationSessions();

        m_SessionBtnBack.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Back");//"Zurück";

        m_SessionBtnStart.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Start");
        m_SessionBtnStop.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Stop");

        showSession(false);

        bool isSessionActive = false;
        string sessions = PlayerPrefs.GetString("ActiveSessions");
        string newsessions = "";
        string[] splitArray = sessions.Split(char.Parse(" "));
        for (int i = 0; i < splitArray.Length; i++)
        {
            string valid = splitArray[i];
            if (valid != "" && valid != " ")
            {
                isSessionActive = true;
            }
        }
        if (!isSessionActive)
        {
            m_ButtonNext.SetActive(false);
        } else {
            m_ButtonNext.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void updateStates()
    {

        m_TextTitle.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("ChooseValidation");
        m_ButtonBack.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Back");//"Back";
        m_ButtonNext.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Continue");//"Back";
        m_LoadingText.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Loading");//"Loading...";

    }


    ArrayList m_ValidationSessionIds;
    ArrayList m_ValidationSessionNames;

    public GameObject m_Content;
    public GameObject m_NameS;
    public GameObject m_UploadQuest;
    public GameObject m_DownloadQuest;
    public GameObject m_StopQuest;

    ArrayList m_AddedTexts;
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


        string sessions = PlayerPrefs.GetString("ActiveSessions");
        string[] splitArray = sessions.Split(char.Parse(" "));


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
            m_AddedTexts.Add(copy);
            copy.GetComponentInChildren<UnityEngine.UI.Text>().text = (string)m_ValidationSessionNames[i];


            copy = (GameObject)GameObject.Instantiate(m_UploadQuest);
            copy.transform.SetParent(m_Content.transform, false);
            copy.SetActive(true);
            rectTransform = copy.GetComponent<RectTransform>();
            curpos = rectTransform.localPosition.y;
            curposx = rectTransform.localPosition.x;
            curpos -= posoffset;//i * heightentry;
            rectTransform.localPosition = new Vector2(curposx, curpos);
            m_AddedTexts.Add(copy);

            m_Buttons.Add(copy);
            m_ButtonsIds.Add(i + "");
            copy.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("ShowDetails");//"UPLOAD";

            UnityEngine.UI.Button b = copy.GetComponent<UnityEngine.UI.Button>();
            AddListener(b, i + "");



            string curvalidationid = (string)m_ValidationSessionIds[i];

            bool bActive = false;
            for (int active = 0; active < splitArray.Length && !bActive; active++)
            {
                if (splitArray[active] == curvalidationid)
                {
                    bActive = true;
                }
            }


            if (bActive == false)
            {
                copy = (GameObject)GameObject.Instantiate(m_DownloadQuest);
                copy.transform.SetParent(m_Content.transform, false);
                copy.SetActive(true);
                rectTransform = copy.GetComponent<RectTransform>();
                curpos = rectTransform.localPosition.y;
                curposx = rectTransform.localPosition.x;
                curpos -= posoffset;//i * heightentry;
                rectTransform.localPosition = new Vector2(curposx, curpos);
                m_AddedTexts.Add(copy);

                m_Buttons.Add(copy);
                m_ButtonsStart.Add(copy);
                m_ButtonsStop.Add(copy);
                copy.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Start");//"UPLOAD";

                b = copy.GetComponent<UnityEngine.UI.Button>();
                AddListenerDownload(b, i + "");
            }
            else
            {
                copy = (GameObject)GameObject.Instantiate(m_DownloadQuest);
                copy.transform.SetParent(m_Content.transform, false);
                copy.SetActive(false);
                rectTransform = copy.GetComponent<RectTransform>();
                curpos = rectTransform.localPosition.y;
                curposx = rectTransform.localPosition.x;
                curpos -= posoffset;//i * heightentry;
                rectTransform.localPosition = new Vector2(curposx, curpos);
                m_AddedTexts.Add(copy);

                m_ButtonsStart.Add(copy);
                copy.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Start");//"UPLOAD";

                b = copy.GetComponent<UnityEngine.UI.Button>();
                AddListenerDownload(b, i + "");



                copy = (GameObject)GameObject.Instantiate(m_StopQuest);
                copy.transform.SetParent(m_Content.transform, false);
                copy.SetActive(true);
                rectTransform = copy.GetComponent<RectTransform>();
                curpos = rectTransform.localPosition.y;
                curposx = rectTransform.localPosition.x;
                curpos -= posoffset;//i * heightentry;
                rectTransform.localPosition = new Vector2(curposx, curpos);
                m_AddedTexts.Add(copy);

                m_ButtonsStop.Add(copy);
                copy.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Stop");//"UPLOAD";

                b = copy.GetComponent<UnityEngine.UI.Button>();
                AddListenerStop(b, i + "");
            }


            posoffset += heightentry;


        }

        Debug.Log("Validation list created");

    }


    void AddListener(UnityEngine.UI.Button b, string value)
    {
        b.onClick.AddListener(() => OnValidationClickedValue(value));
    }


    string m_ValidationId;
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

        m_ValidationName = (string)m_ValidationSessionNames[index];
        m_ValidationId = (string)m_ValidationSessionIds[index];
        Debug.Log("ValidationId: " + m_ValidationSessionIds[index]);
        loadValidationSession((string)m_ValidationSessionIds[index]);
    }


    void AddListenerDownload(UnityEngine.UI.Button b, string value)
    {
        b.onClick.AddListener(() => OnDownloadClickedValue(value));
    }

    bool m_bToDemoMap = false;
    public void OnDownloadClickedValue(string param)
    {
        Debug.Log("OnDownloadClickedValue: " + param);


        int index = int.Parse(param);

        m_ValidationName = (string)m_ValidationSessionNames[index];
        m_ValidationId = (string)m_ValidationSessionIds[index];

        string sessions = PlayerPrefs.GetString("ActiveSessions");
        string[] splitArray = sessions.Split(char.Parse(" "));
        bool bAlreadyAdded = false;
        for (int i = 0; i < splitArray.Length && !bAlreadyAdded; i++) {
            if(splitArray[i] == m_ValidationId) {
                bAlreadyAdded = true;
            }
        }

        if(!bAlreadyAdded) {
            sessions = sessions + " " + m_ValidationId;
        }
        PlayerPrefs.SetString("ActiveSessions", sessions);
        PlayerPrefs.Save();

        Debug.Log("ActiveSessionsSaved");
        m_bToDemoMap = true;
        loadValidationSession((string)m_ValidationSessionIds[index]);
    }

    public void OnStartValidation()
    {
        m_LoadingText.SetActive(true);
        m_LoadingBack.SetActive(true);

        string sessions = PlayerPrefs.GetString("ActiveSessions");
        string[] splitArray = sessions.Split(char.Parse(" "));
        bool bAlreadyAdded = false;
        for (int i = 0; i < splitArray.Length && !bAlreadyAdded; i++)
        {
            if (splitArray[i] == m_ValidationId)
            {
                bAlreadyAdded = true;
            }
        }

        if (!bAlreadyAdded)
        {
            sessions = sessions + " " + m_ValidationId;
        }
        PlayerPrefs.SetString("ActiveSessions", sessions);
        PlayerPrefs.Save();

        Application.LoadLevel("DemoMap");
    }


    void AddListenerStop(UnityEngine.UI.Button b, string value)
    {
        b.onClick.AddListener(() => OnStopClickedValue(value));
    }


    public void OnStopClickedValue(string param)
    {
        Debug.Log("OnStopClickedValue: " + param);


        int index = int.Parse(param);

        m_ValidationName = (string)m_ValidationSessionNames[index];
        m_ValidationId = (string)m_ValidationSessionIds[index];

        string sessions = PlayerPrefs.GetString("ActiveSessions");
        string newsessions = "";
        string[] splitArray = sessions.Split(char.Parse(" "));
        bool bAlreadyAdded = false;
        for (int i = 0; i < splitArray.Length; i++)
        {
            if (splitArray[i] != m_ValidationId)
            {
                if (newsessions == "")
                {
                    newsessions = splitArray[i];
                } else {
                    newsessions = newsessions + " " + splitArray[i];
                }
            }
        }

        PlayerPrefs.SetString("ActiveSessions", newsessions);
        PlayerPrefs.Save();


        int buttonindex = m_ButtonsStart.Count - index - 1;
        GameObject go = (GameObject)m_ButtonsStart[buttonindex];
        go.SetActive(true);

        go = (GameObject)m_ButtonsStop[buttonindex];
        go.SetActive(false);
    }

    void disableStopButton(string valid) 
    {
        Debug.Log("disableStopButton: " + valid);
        for (int i = 0; i < m_ValidationSessionIds.Count; i++) {
            Debug.Log("m_ValidationSessionIds: " + m_ValidationSessionIds[i]);
            if(m_ValidationSessionIds[i] == valid) {
                int index = m_ValidationSessionIds.Count - i - 1;
                Debug.Log("Stop validation");
                GameObject go = (GameObject)m_ButtonsStop[index];
                go.SetActive(false);

                go = (GameObject)m_ButtonsStart[index];
                go.SetActive(true);
            }   
        }
    }

    public void OnBackClicked()
    {
        Application.LoadLevel("LoginLacoWiki");
    }
    public void OnNextClicked()
    {
        Application.LoadLevel("DemoMap");
    }


    public void stopSession()
    {
        string sessions = PlayerPrefs.GetString("ActiveSessions");
        string newsessions = "";
        string[] splitArray = sessions.Split(char.Parse(" "));
        bool bAlreadyAdded = false;
        for (int i = 0; i < splitArray.Length; i++)
        {
            if (splitArray[i] != m_ValidationId)
            {
                if (newsessions == "")
                {
                    newsessions = splitArray[i];
                }
                else
                {
                    newsessions = newsessions + " " + splitArray[i];
                }
            }
        }

        Debug.Log("stopSessions: " + newsessions);
        PlayerPrefs.SetString("ActiveSessions", newsessions);
        PlayerPrefs.Save();

        m_SessionBtnStart.SetActive(true);
        m_SessionBtnStop.SetActive(false);

        disableStopButton(m_ValidationId);
    }

    //--------------------------
    // Validation sessions

    public void loadValidationSessions()
    {
        m_LoadingText.SetActive(true);

      /*  string url = "http://dev.laco-wiki.net/api/mobile/validationsessions";
        //string url = "https://laco-wiki.net/api/mobile/validationsessions";
        Debug.Log("Url: " + url);

        string token = PlayerPrefs.GetString("Token");
        Debug.Log("token: " + token);

        WWWForm form = new WWWForm();
        form.AddField("param", "param");

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Authorization", "Bearer " + token);

        WWW www = new WWW(url, form.data, headers);
       // WWW www = new WWW(url, null, headers);
        StartCoroutine(WaitForValidationSessions(www));
        */
        StartCoroutine(ReadingValidationSessions());
    }

    int m_ReadingWhich;
    IEnumerator ReadingValidationSessions()
    {
        string url = "https://dev.laco-wiki.net/api/mobile/validationsessions";
        //string url = "https://laco-wiki.net/api/mobile/validationsessions";
        UnityWebRequest www = UnityWebRequest.Get(url);
        string token = PlayerPrefs.GetString("Token");
        www.SetRequestHeader("Authorization", "Bearer " + token);

        Debug.Log("Url: " + url);
        Debug.Log("header: " + "Bearer " + token);

        yield return www.Send();//www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Error: " + www.error);
            if (www.error == "HTTP/1.1 401 Unauthorized")
            {
                Debug.Log("Unauthorized access");
                Application.LoadLevel("LoginLacoWiki");
            }
        }
        else
        {
            // Show results as text
            Debug.Log("Success: " + www.downloadHandler.text);
            m_ValidationSessionIds = new ArrayList();
            m_ValidationSessionNames = new ArrayList();

            JSONObject j = new JSONObject(www.downloadHandler.text);
            m_ReadingWhich = -1;
            accessValidationSessions(j);

            createValidationsList();
            m_LoadingText.SetActive(false);
        }
    }

    /*
    IEnumerator WaitForValidationSessions(WWW www)
    {
        yield return www;
        if (www.error == null)
        {
            Debug.Log("WWW ValidationSessions data: " + www.text);

            m_ValidationSessionIds = new ArrayList();
            m_ValidationSessionNames = new ArrayList();

            JSONObject j = new JSONObject(www.text);
            m_ReadingWhich = -1;
            accessValidationSessions(j);

            createValidationsList();
            m_LoadingText.SetActive(false);
        }
        else
        {
            Debug.Log("Could not load validation sessions");
            Debug.Log("WWW Error: " + www.error);
            Debug.Log("WWW Error 2: " + www.text);

            if (www.error == "401 Unauthorized")
            {
                Debug.Log("Unauthorized access");
                Application.LoadLevel("LoginLacoWiki");
            }
        }
    }*/

    void accessValidationSessions(JSONObject obj)
    {
        switch (obj.type)
        {
            case JSONObject.Type.OBJECT:
                for (int i = 0; i < obj.list.Count; i++)
                {
                    string key = (string)obj.keys[i];
                    JSONObject j = (JSONObject)obj.list[i];

                    if (key == "validationSessionID")
                    {
                        m_ReadingWhich = 1;
                    }
                    else if (key == "validationSessionName")
                    {
                        m_ReadingWhich = 2;
                    }
                    accessValidationSessions(j);
                }
                break;
            case JSONObject.Type.ARRAY:
                //  Debug.Log ("Array");
                foreach (JSONObject j in obj.list)
                {
                    accessValidationSessions(j);
                }
                break;
            case JSONObject.Type.STRING:
                if (m_ReadingWhich == 2)
                {
                    //  Debug.Log("string: " + obj.str);
                    m_ValidationSessionNames.Add(obj.str);
                }
                break;
            case JSONObject.Type.NUMBER:
                if (m_ReadingWhich == 1)
                {
                    // Debug.Log("number: " + obj.n);
                    m_ValidationSessionIds.Add("" + obj.n);
                }

                break;
            case JSONObject.Type.BOOL:
                //      Debug.Log("bool: " + obj.b);
                break;
            case JSONObject.Type.NULL:
                //  Debug.Log("NULL");
                break;
        }
    }

    public void loadValidationSession(string sessionid)
    {
        m_LoadingText.SetActive(true);
        m_LoadingBack.SetActive(true);

      /*  //string url = "http://dev.laco-wiki.net/api/mobile/validationsessions/" + sessionid;
        string url = "https://laco-wiki.net/api/mobile/validationsessions/" + sessionid;
        Debug.Log("Url: " + url);

        string token = PlayerPrefs.GetString("Token");
        Debug.Log("token: " + token);

        WWWForm form = new WWWForm();
        form.AddField("param", "param");


        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Authorization", "Bearer " + token);

        WWW www = new WWW(url, form.data, headers);

        StartCoroutine(WaitForValidationSession(www));
        */
        StartCoroutine(ReadingValidationSession(sessionid));
    }

    IEnumerator ReadingValidationSession(string sessionid)
    {
        string url = "https://dev.laco-wiki.net/api/mobile/validationsessions/" + sessionid;
        //string url = "https://laco-wiki.net/api/mobile/validationsessions/" + sessionid;
        UnityWebRequest www = UnityWebRequest.Get(url);
        string token = PlayerPrefs.GetString("Token");
        www.SetRequestHeader("Authorization", "Bearer " + token);

        Debug.Log("header: " + "Bearer " + token);

        yield return www.Send();//www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Error: " + www.error);
            if (www.error == "HTTP/1.1 401 Unauthorized")
            {
                Debug.Log("Unauthorized access");
                Application.LoadLevel("LoginLacoWiki");
            }
        }
        else
        {
            Debug.Log("WWW ValidationSession data: " + www.downloadHandler.text);

            m_ValidationLegend = new ArrayList();

            JSONObject j = new JSONObject(www.downloadHandler.text);
            m_ReadingWhich = -1;
            accessValidationSession(j);

            Debug.Log("m_ValidationId: " + m_ValidationId);
            Debug.Log("m_ValidationName: " + m_ValidationName);
            Debug.Log("m_ValidationDataSet: " + m_ValidationDataSet);
            Debug.Log("m_ValidationSample: " + m_ValidationSample);
            Debug.Log("m_ValidationDescription: " + m_ValidationDescription);
            Debug.Log("m_ValidationMethod: " + m_ValidationMethod);
            Debug.Log("m_ValidationSamplesTotal: " + m_ValidationSamplesTotal);
            Debug.Log("m_ValidationSamplesValidated: " + m_ValidationSamplesValidated);


            PlayerPrefs.SetString("SessionName_" + m_ValidationId, m_ValidationName);
            PlayerPrefs.SetString("SessionDataSet_" + m_ValidationId, m_ValidationDataSet);
            PlayerPrefs.SetString("SessionSample_" + m_ValidationId, m_ValidationSample);
            PlayerPrefs.SetString("SessionDesc_" + m_ValidationId, m_ValidationDescription);
            PlayerPrefs.SetString("SessionMethod_" + m_ValidationId, m_ValidationMethod);
            PlayerPrefs.SetInt("SessionSamplesTotal_" + m_ValidationId, m_ValidationSamplesTotal);
            PlayerPrefs.SetInt("SessionSamplesValidated_" + m_ValidationId, m_ValidationSamplesValidated);




            Debug.Log("m_CurLegendSettingsDistance: " + m_CurLegendSettingsDistance);
            Debug.Log("m_CurCardinalDirectionPhotosOptional: " + m_CurCardinalDirectionPhotosOptional);
            Debug.Log("m_CurOpportunisticValidationsEnabled: " + m_CurOpportunisticValidationsEnabled);
            Debug.Log("m_CurPointPhotoOptional: " + m_CurPointPhotoOptional);
            Debug.Log("m_CurTakeCardinalDirectionPhotos: " + m_CurTakeCardinalDirectionPhotos);
            Debug.Log("m_CurTakePointPhoto: " + m_CurTakePointPhoto);

            PlayerPrefs.SetInt("SessionSettingsDistance_" + m_ValidationId, m_CurLegendSettingsDistance);
            PlayerPrefs.SetInt("SessionSettingsCardinalDirectionPhotosOptional_" + m_ValidationId, m_CurCardinalDirectionPhotosOptional);
            PlayerPrefs.SetInt("SessionSettingsOpportunisticValidationsEnabled_" + m_ValidationId, m_CurOpportunisticValidationsEnabled);
            PlayerPrefs.SetInt("SessionSettingsPointPhotoOptional_" + m_ValidationId, m_CurPointPhotoOptional);
            PlayerPrefs.SetInt("SessionSettingsTakeCardinalDirectionPhotos_" + m_ValidationId, m_CurTakeCardinalDirectionPhotos);
            PlayerPrefs.SetInt("SessionSettingsTakePointPhoto_" + m_ValidationId, m_CurTakePointPhoto);


            int nrlegenditems = m_ValidationLegend.Count;

            PlayerPrefs.SetInt("SessionLegendNrItems_" + m_ValidationId, nrlegenditems);

            for (int i = 0; i < nrlegenditems; i++)
            {
                LegendItem legend = (LegendItem)m_ValidationLegend[i];

                PlayerPrefs.SetInt("Session_" + m_ValidationId + "_Legend_" + i + "_Red", legend.m_LegendRed);
                PlayerPrefs.SetInt("Session_" + m_ValidationId + "_Legend_" + i + "_Green", legend.m_LegendGreen);
                PlayerPrefs.SetInt("Session_" + m_ValidationId + "_Legend_" + i + "_Blue", legend.m_LegendBlue);
                PlayerPrefs.SetString("Session_" + m_ValidationId + "_Legend_" + i + "_Name", legend.m_LegendName);
                PlayerPrefs.SetInt("Session_" + m_ValidationId + "_Legend_" + i + "_ItemId", legend.m_LegendItemId);
                PlayerPrefs.SetString("Session_" + m_ValidationId + "_Legend_" + i + "_Value", legend.m_LegendValue);
            }

            PlayerPrefs.Save();

            if (m_bToDemoMap)
            {
                Application.LoadLevel("DemoMap");
            }
            else
            {
                m_LoadingText.SetActive(false);
                m_LoadingBack.SetActive(false);

                showSession(true);
            }
        }
    }
    /*
    IEnumerator WaitForValidationSession(WWW www)
    {
        yield return www;
        if (www.error == null)
        {
            Debug.Log("WWW ValidationSession data: " + www.text);

            m_ValidationLegend = new ArrayList();

            JSONObject j = new JSONObject(www.text);
            m_ReadingWhich = -1;
            accessValidationSession(j);

            Debug.Log("m_ValidationId: " + m_ValidationId);
            Debug.Log("m_ValidationName: " + m_ValidationName);
            Debug.Log("m_ValidationDataSet: " + m_ValidationDataSet);
            Debug.Log("m_ValidationSample: " + m_ValidationSample);
            Debug.Log("m_ValidationDescription: " + m_ValidationDescription);
            Debug.Log("m_ValidationMethod: " + m_ValidationMethod);
            Debug.Log("m_ValidationSamplesTotal: " + m_ValidationSamplesTotal);
            Debug.Log("m_ValidationSamplesValidated: " + m_ValidationSamplesValidated);


            PlayerPrefs.SetString("SessionName_" + m_ValidationId, m_ValidationName);
            PlayerPrefs.SetString("SessionDataSet_" + m_ValidationId, m_ValidationDataSet);
            PlayerPrefs.SetString("SessionSample_" + m_ValidationId, m_ValidationSample);
            PlayerPrefs.SetString("SessionDesc_" + m_ValidationId, m_ValidationDescription);
            PlayerPrefs.SetString("SessionMethod_" + m_ValidationId, m_ValidationMethod);
            PlayerPrefs.SetInt("SessionSamplesTotal_" + m_ValidationId, m_ValidationSamplesTotal);
            PlayerPrefs.SetInt("SessionSamplesValidated_" + m_ValidationId, m_ValidationSamplesValidated);




            Debug.Log("m_CurLegendSettingsDistance: " + m_CurLegendSettingsDistance);
            Debug.Log("m_CurCardinalDirectionPhotosOptional: " + m_CurCardinalDirectionPhotosOptional);
            Debug.Log("m_CurOpportunisticValidationsEnabled: " + m_CurOpportunisticValidationsEnabled);
            Debug.Log("m_CurPointPhotoOptional: " + m_CurPointPhotoOptional);
            Debug.Log("m_CurTakeCardinalDirectionPhotos: " + m_CurTakeCardinalDirectionPhotos);
            Debug.Log("m_CurTakePointPhoto: " + m_CurTakePointPhoto);

            PlayerPrefs.SetInt("SessionSettingsDistance_" + m_ValidationId, m_CurLegendSettingsDistance);
            PlayerPrefs.SetInt("SessionSettingsCardinalDirectionPhotosOptional_" + m_ValidationId, m_CurCardinalDirectionPhotosOptional);
            PlayerPrefs.SetInt("SessionSettingsOpportunisticValidationsEnabled_" + m_ValidationId, m_CurOpportunisticValidationsEnabled);
            PlayerPrefs.SetInt("SessionSettingsPointPhotoOptional_" + m_ValidationId, m_CurPointPhotoOptional);
            PlayerPrefs.SetInt("SessionSettingsTakeCardinalDirectionPhotos_" + m_ValidationId, m_CurTakeCardinalDirectionPhotos);
            PlayerPrefs.SetInt("SessionSettingsTakePointPhoto_" + m_ValidationId, m_CurTakePointPhoto);


            int nrlegenditems = m_ValidationLegend.Count;

            PlayerPrefs.SetInt("SessionLegendNrItems_" + m_ValidationId, nrlegenditems);

            for (int i = 0; i < nrlegenditems; i++)
            {
                LegendItem legend = (LegendItem)m_ValidationLegend[i];

                PlayerPrefs.SetInt("Session_" + m_ValidationId + "_Legend_" + i + "_Red", legend.m_LegendRed);
                PlayerPrefs.SetInt("Session_" + m_ValidationId + "_Legend_" + i + "_Green", legend.m_LegendGreen);
                PlayerPrefs.SetInt("Session_" + m_ValidationId + "_Legend_" + i + "_Blue", legend.m_LegendBlue);
                PlayerPrefs.SetString("Session_" + m_ValidationId + "_Legend_" + i + "_Name", legend.m_LegendName);
                PlayerPrefs.SetInt("Session_" + m_ValidationId + "_Legend_" + i + "_ItemId", legend.m_LegendItemId);
                PlayerPrefs.SetString("Session_" + m_ValidationId + "_Legend_" + i + "_Value", legend.m_LegendValue);
            }

            PlayerPrefs.Save();

            if (m_bToDemoMap)
            {
                Application.LoadLevel("DemoMap");
            }
            else
            {
                m_LoadingText.SetActive(false);
                m_LoadingBack.SetActive(false);

                showSession(true);
            }

        }
        else
        {
            Debug.Log("Could not load validation session");
            Debug.Log("WWW Error: " + www.error);
            Debug.Log("WWW Error 2: " + www.text);

            if (www.error == "401 Unauthorized")
            {
                Debug.Log("Unauthorized access");
                Application.LoadLevel("LoginLacoWiki");
            }
        }
    }*/

    void accessValidationSession(JSONObject obj)
    {
        switch (obj.type)
        {
            case JSONObject.Type.OBJECT:
                for (int i = 0; i < obj.list.Count; i++)
                {
                    string key = (string)obj.keys[i];
                    JSONObject j = (JSONObject)obj.list[i];

                    if (key == "associatedDataSetName")
                    {
                        m_ReadingWhich = 1;
                    }
                    else if (key == "associatedSampleName")
                    {
                        m_ReadingWhich = 2;
                    }
                    else if (key == "description")
                    {
                        m_ReadingWhich = 3;
                    }
                    else if (key == "validationMethodName")
                    {
                        m_ReadingWhich = 4;
                    }
                    else if (key == "samplesTotal")
                    {
                        m_ReadingWhich = 5;
                    }
                    else if (key == "samplesValidated")
                    {
                        m_ReadingWhich = 6;
                    }
                    else if (key == "blue")
                    {
                        m_ReadingWhich = 7;
                    }
                    else if (key == "green")
                    {
                        m_ReadingWhich = 8;
                    }
                    else if (key == "red")
                    {
                        m_ReadingWhich = 9;
                    }
                    else if (key == "className")
                    {
                        m_ReadingWhich = 10;
                    }
                    else if (key == "legendItemID")
                    {
                        m_ReadingWhich = 11;
                    }
                    else if (key == "value")
                    {
                        m_ReadingWhich = 12;

                        Debug.Log("Red: " + m_CurLegendRed);
                        Debug.Log("Green: " + m_CurLegendGreen);
                        Debug.Log("Blue: " + m_CurLegendBlue);
                        Debug.Log("LegendName: " + m_CurLegendName);
                        Debug.Log("ItemId: " + m_CurLegendItemId);
                        Debug.Log("Value: " + m_CurLegendValue);

                        LegendItem legend = new LegendItem();
                        legend.m_LegendRed = m_CurLegendRed;
                        legend.m_LegendGreen = m_CurLegendGreen;
                        legend.m_LegendBlue = m_CurLegendBlue;
                        legend.m_LegendName = m_CurLegendName;
                        legend.m_LegendItemId = m_CurLegendItemId;
                        legend.m_LegendValue = m_CurLegendValue;
                        m_ValidationLegend.Add(legend);
                    }
                    else if (key == "cardinalDirectionPhotosOptional")
                    {
                        m_ReadingWhich = 13;
                    }
                    else if (key == "distance")
                    {
                        m_ReadingWhich = 14;
                    }
                    else if (key == "opportunisticValidationsEnabled")
                    {
                        m_ReadingWhich = 15;
                    }
                    else if (key == "pointPhotoOptional")
                    {
                        m_ReadingWhich = 16;
                    }
                    else if (key == "takeCardinalDirectionPhotos")
                    {
                        m_ReadingWhich = 17;
                    }
                    else if (key == "takePointPhoto")
                    {
                        m_ReadingWhich = 18;
                    }

                    accessValidationSession(j);
                }
                break;
            case JSONObject.Type.ARRAY:
                //  Debug.Log ("Array");
                foreach (JSONObject j in obj.list)
                {
                    accessValidationSession(j);
                }
                break;
            case JSONObject.Type.STRING:
                if (m_ReadingWhich == 1)
                {
                    m_ValidationDataSet = obj.str;
                }
                else if (m_ReadingWhich == 2)
                {
                    m_ValidationSample = obj.str;
                }
                else if (m_ReadingWhich == 3)
                {
                    m_ValidationDescription = obj.str;
                }
                else if (m_ReadingWhich == 4)
                {
                    m_ValidationMethod = obj.str;
                }
                else if (m_ReadingWhich == 10)
                {
                    m_CurLegendName = obj.str;
                }
                else if (m_ReadingWhich == 12)
                {
                    m_CurLegendValue = obj.str;
                }

                m_ReadingWhich = -1;


                break;
            case JSONObject.Type.NUMBER:
                if (m_ReadingWhich == 5)
                {
                    m_ValidationSamplesTotal = (int)obj.n;
                }
                else if (m_ReadingWhich == 6)
                {
                    m_ValidationSamplesValidated = (int)obj.n;
                }


                else if (m_ReadingWhich == 7)
                {
                    m_CurLegendRed = (int)obj.n;
                }
                else if (m_ReadingWhich == 8)
                {
                    m_CurLegendGreen = (int)obj.n;
                }
                else if (m_ReadingWhich == 9)
                {
                    m_CurLegendBlue = (int)obj.n;
                }
                else if (m_ReadingWhich == 11)
                {
                    m_CurLegendItemId = (int)obj.n;
                }

                else if (m_ReadingWhich == 14)
                {
                    m_CurLegendSettingsDistance = (int)obj.n;
                }

                m_ReadingWhich = -1;

                break;
            case JSONObject.Type.BOOL:
                if (m_ReadingWhich == 13)
                {
                    if (obj.b)
                    {
                        m_CurCardinalDirectionPhotosOptional = 1;
                    }
                    else
                    {
                        m_CurCardinalDirectionPhotosOptional = 0;
                    }
                }
                if (m_ReadingWhich == 15)
                {
                    if (obj.b)
                    {
                        m_CurOpportunisticValidationsEnabled = 1;
                    }
                    else
                    {
                        m_CurOpportunisticValidationsEnabled = 0;
                    }
                }
                if (m_ReadingWhich == 16)
                {
                    if (obj.b)
                    {
                        m_CurPointPhotoOptional = 1;
                    }
                    else
                    {
                        m_CurPointPhotoOptional = 0;
                    }
                }
                if (m_ReadingWhich == 17)
                {
                    if (obj.b)
                    {
                        m_CurTakeCardinalDirectionPhotos = 1;
                    }
                    else
                    {
                        m_CurTakeCardinalDirectionPhotos = 0;
                    }
                }
                if (m_ReadingWhich == 18)
                {
                    if (obj.b)
                    {
                        m_CurTakePointPhoto = 1;
                    }
                    else
                    {
                        m_CurTakePointPhoto = 0;
                    }
                }


                m_ReadingWhich = -1;
                //      Debug.Log("bool: " + obj.b);
                break;
            case JSONObject.Type.NULL:
                //  Debug.Log("NULL");
                break;
        }
    }

    public GameObject m_SessionBack;
    public GameObject m_SessionBtnBack;
    public GameObject m_SessionImageBack;
    public GameObject m_SessionTitle;
    public GameObject m_SessionScrollView;
    public GameObject m_SessionMethod;
    public GameObject m_SessionDataset;
    public GameObject m_SessionSample;
    public GameObject m_SessionDescription;
    public GameObject m_SessionProgress;
    public GameObject m_SessionCircle;
    public GameObject m_SessionCircleProc;
    public GameObject m_SessionBtnStart;
    public GameObject m_SessionBtnStop;

    public void showSession(bool bShow)
    {
        m_SessionTitle.GetComponentInChildren<UnityEngine.UI.Text>().text = m_ValidationName;
        m_SessionMethod.GetComponentInChildren<UnityEngine.UI.Text>().text = m_ValidationMethod;
        m_SessionDataset.GetComponentInChildren<UnityEngine.UI.Text>().text = m_ValidationDataSet;
        m_SessionSample.GetComponentInChildren<UnityEngine.UI.Text>().text = m_ValidationSample;
        m_SessionDescription.GetComponentInChildren<UnityEngine.UI.Text>().text = m_ValidationDescription;
        m_SessionProgress.GetComponentInChildren<UnityEngine.UI.Text>().text = m_ValidationSamplesValidated + " / " + m_ValidationSamplesTotal + " validated";
        float proc = (float)m_ValidationSamplesValidated / (float)m_ValidationSamplesTotal;
        m_SessionCircle.GetComponent<Image>().material.SetFloat("_Circle", proc);
        proc *= 100.0f;
        int iproc = (int)proc;
        m_SessionCircleProc.GetComponentInChildren<UnityEngine.UI.Text>().text = iproc + " %";

        m_SessionTitle.SetActive(bShow);
        m_SessionBack.SetActive(bShow);
        m_SessionBtnBack.SetActive(bShow);
        m_SessionImageBack.SetActive(bShow);
        m_SessionScrollView.SetActive(bShow);
        m_SessionBtnStart.SetActive(bShow);
        m_SessionBtnStop.SetActive(bShow);

        if(bShow) {

            string sessions = PlayerPrefs.GetString("ActiveSessions");
            string[] splitArray = sessions.Split(char.Parse(" "));
            bool bAlreadyAdded = false;
            for (int i = 0; i < splitArray.Length && !bAlreadyAdded; i++)
            {
                if (splitArray[i] == m_ValidationId)
                {
                    bAlreadyAdded = true;
                }
            }

            if(bAlreadyAdded) {
                m_SessionBtnStop.SetActive(true);
            } else {
                m_SessionBtnStop.SetActive(false);
            }
        }


    }

    public void closeSession()
    {
        showSession(false);
    }


    //------------------------------
    // Sample points

    public void loadSamplePoints()
    {
        m_LoadingText.SetActive(true);
        m_LoadingBack.SetActive(true);

        string url = "https://dev.laco-wiki.net/api/mobile/validationsessions/" + m_ValidationId + "/sampleItems";
       // string url = "https://laco-wiki.net/api/mobile/validationsessions/" + m_ValidationId + "/sampleItems";

        string token = PlayerPrefs.GetString("Token");
        Debug.Log("token: " + token);

        WWWForm form = new WWWForm();
        form.AddField("param", "param");


        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Authorization", "Bearer " + token);

        WWW www = new WWW(url, form.data, headers);

        StartCoroutine(WaitForSamplePoints(www));
    }


    ArrayList m_Samples;
    IEnumerator WaitForSamplePoints(WWW www)
    {
        yield return www;
        if (www.error == null)
        {
            Debug.Log("WWW SamplePoints data: " + www.text);

            m_Samples = new ArrayList();
                
            JSONObject j = new JSONObject(www.text);
            m_ReadingWhich = -1;
            accessSamplePoints(j);

            int nrsamples = m_Samples.Count;
            Debug.Log("nrsamples: " + nrsamples);



            PlayerPrefs.SetInt("Session_" + m_ValidationId + "_NrSamples", nrsamples);
            for (int i = 0; i < nrsamples; i++) {

                SamplePoint point = (SamplePoint)m_Samples[i];
                PlayerPrefs.SetInt("Session_" + m_ValidationId + "_Sample_" + i + "_Id", point.m_SampleId);
                PlayerPrefs.SetInt("Session_" + m_ValidationId + "_Sample_" + i + "_LegendId", point.m_SampleLegendId);
                PlayerPrefs.SetInt("Session_" + m_ValidationId + "_Sample_" + i + "_Validated", point.m_SampleValidated);
                PlayerPrefs.SetString("Session_" + m_ValidationId + "_Sample_" + i + "_Lat", point.m_SampleLat);
                PlayerPrefs.SetString("Session_" + m_ValidationId + "_Sample_" + i + "_Lng", point.m_SampleLng);
            }

            PlayerPrefs.Save();


            m_LoadingText.SetActive(false);
            m_LoadingBack.SetActive(false);
        }
        else
        {
            Debug.Log("Could not load sample points");
            Debug.Log("WWW Error: " + www.error);
            Debug.Log("WWW Error 2: " + www.text);

            if (www.error == "401 Unauthorized")
            {
                Debug.Log("Unauthorized access");
                Application.LoadLevel("LoginLacoWiki");
            }
        }
    }

    public static string getBetween(string strSource, string strStart, string strEnd)
    {
        int Start, End;
        if (strSource.Contains(strStart) && strSource.Contains(strEnd))
        {
            Start = strSource.IndexOf(strStart, 0) + strStart.Length;
            End = strSource.IndexOf(strEnd, Start);
            return strSource.Substring(Start, End - Start);
        }
        else
        {
            return "";
        }
    }

    int m_SampleId;
    int m_SampleLegendId;
    int m_SampleValidated;
    string m_SampleLat;
    string m_SampleLng;
    void accessSamplePoints(JSONObject obj)
    {
        switch (obj.type)
        {
            case JSONObject.Type.OBJECT:
                for (int i = 0; i < obj.list.Count; i++)
                {
                    string key = (string)obj.keys[i];
                    JSONObject j = (JSONObject)obj.list[i];

                   // Debug.Log("Key: " + key);
                    if (key == "legendItemID")
                    {
                        m_ReadingWhich = 1;
                    } 
                    else if (key == "sampleItemID")
                    {
                        m_ReadingWhich = 2;
                    }
                    else if (key == "validated")
                    {
                        m_ReadingWhich = 3;
                    }
                    else if (key == "geometryString")
                    {
                        m_ReadingWhich = 4;
                    }



                    accessSamplePoints(j);
                }
                break;
            case JSONObject.Type.ARRAY:
                //  Debug.Log ("Array");
                foreach (JSONObject j in obj.list)
                {
                    accessSamplePoints(j);
                }
                break;
            case JSONObject.Type.STRING:
                if (m_ReadingWhich == 4)
                {
                    string coordinate = getBetween(obj.str, "(", ")");
                  //  Debug.Log("Coordinate: " + coordinate);
                    string[] splitArray = coordinate.Split(char.Parse(" "));
                    m_SampleLat = splitArray[0];
                    m_SampleLng = splitArray[1];
                }
                m_ReadingWhich = -1;


                break;
            case JSONObject.Type.NUMBER:
                if (m_ReadingWhich == 1)
                {
                    m_SampleLegendId = (int)obj.n;
                }
                else if (m_ReadingWhich == 2)
                {
                    m_SampleId = (int)obj.n;
                }

                m_ReadingWhich = -1;

                break;
            case JSONObject.Type.BOOL:
                if (m_ReadingWhich == 3)
                {
                    if (obj.b)
                    {
                        m_SampleValidated = 1;
                    }
                    else
                    {
                        m_SampleValidated = 0;
                    }

                    SamplePoint point = new SamplePoint();
                    point.m_SampleId = m_SampleId;
                    point.m_SampleLegendId = m_SampleLegendId;
                    point.m_SampleValidated = m_SampleValidated;
                    point.m_SampleLat = m_SampleLat;
                    point.m_SampleLng = m_SampleLng;

                    m_Samples.Add(point);

                   // Debug.Log(">> m_SampleId: " + m_SampleId + " m_SampleLegendId: " + m_SampleLegendId + " m_SampleValidated: " + m_SampleValidated + " m_SampleLat: " + m_SampleLat + " m_SampleLng: " + m_SampleLng);
                }

                m_ReadingWhich = -1;
                //      Debug.Log("bool: " + obj.b);
                break;
            case JSONObject.Type.NULL:
                //  Debug.Log("NULL");
                break;
        }
    }
}
