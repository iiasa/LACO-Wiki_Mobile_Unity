using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoTask : MonoBehaviour
{
    public CompassStrip m_CompassStrip ;
  //  public TaskManual m_TaskManual ;
   // public TaskSequential m_TaskSequential ;

    public SensorImage m_SensorImage ;

    public Text StatusText ;
    //public Button m_ButtonFinish ;
    public Button m_ButtonBack ;
    public Button m_ButtonNext ;

    public DebugText m_DebugText ;
    public GameObject m_DebugView ;

    public GameObject PanelCompass ;
   // public GameObject PanelManual ;
 //   public GameObject PanelSequential;

    private IPhotoTask m_PhotoTask = null ;

    private readonly string[] DirNames = new string[] { "North", "East", "South", "West" };
    private bool m_bUseCompass = true ;

    private enum EMethod
    {
        Kompass = 0 ,
        Manual = 1 ,
        Sequential = 2 ,
    }
    private EMethod m_Method ;

    //private T FindChild < T > ( string name )
    //{
    //    Transform child = transform.Find(name);
    //    if (child == null) return default(T);
    //    return child.gameObject.GetComponent<T>();
    //}

    private void TakePicture ( string name )
    {
        m_SensorImage.TakePicture ( name , true ) ;
    }

    public void TakePicture ( int direction )
    {
        // string Filename = "Pic_" + DirNames [ direction ] ;
        string Filename = "";
        int nrquestsdone = 0;
        if (PlayerPrefs.HasKey("NrQuestsDone"))
        {
            nrquestsdone = PlayerPrefs.GetInt("NrQuestsDone");
        }
        else
        {
            nrquestsdone = 0;
        }

        int photo = direction + 1;
        Filename = "Quest_Img_" + nrquestsdone + "_" + photo;


        TakePicture ( Filename ) ;

        //--------------------------
        // Save meta data

        saveMetaData(photo, nrquestsdone);

       // if (direction < 4)
       // {
        if(photo < 5) {
            m_PhotoTask.SetFinished(direction);
        }


        //--------------------------

        EnableButtons();
        //if ( m_PhotoTask.IsAllFinished() )
        //{
        //    m_ButtonFinish.interactable = true;
        //}
    }

    public void TakePic1()
    {
        TakePicture(0);
    }
    public void TakePic2()
    {
        TakePicture(1);
    }
    public void TakePic3()
    {
        TakePicture(2);
    }
    public void TakePic4()
    {
        TakePicture(3);
    }

    void saveMetaData(int m_CurrentStep, int m_NrQuestsDone)
    {
        Debug.Log("Photo saveMetaData nrquests: " + m_NrQuestsDone + " step: " + m_CurrentStep);
        PlayerPrefs.SetInt("Quest_" + m_NrQuestsDone + "_" + "PhotoTaken" + "_" + m_CurrentStep, 1);


        bool m_bCompassEnabled = true;
        if (m_bCompassEnabled)
        {
            PlayerPrefs.SetFloat("Quest_" + m_NrQuestsDone + "_" + m_CurrentStep + "_Heading", Input.compass.trueHeading);
            PlayerPrefs.SetFloat("Quest_" + m_NrQuestsDone + "_" + m_CurrentStep + "_AccX", Input.acceleration.x);
            PlayerPrefs.SetFloat("Quest_" + m_NrQuestsDone + "_" + m_CurrentStep + "_AccY", Input.acceleration.y);
            PlayerPrefs.SetFloat("Quest_" + m_NrQuestsDone + "_" + m_CurrentStep + "_AccZ", Input.acceleration.z);
        }
        else
        {
            PlayerPrefs.SetFloat("Quest_" + m_NrQuestsDone + "_" + m_CurrentStep + "_Heading", -1.0f);
            PlayerPrefs.SetFloat("Quest_" + m_NrQuestsDone + "_" + m_CurrentStep + "_AccX", -1.0f);
            PlayerPrefs.SetFloat("Quest_" + m_NrQuestsDone + "_" + m_CurrentStep + "_AccY", -1.0f);
            PlayerPrefs.SetFloat("Quest_" + m_NrQuestsDone + "_" + m_CurrentStep + "_AccZ", -1.0f);
        }
        int compassenabled = m_bCompassEnabled ? 1 : 0;
        PlayerPrefs.SetInt("Quest_" + m_NrQuestsDone + "_" + m_CurrentStep + "_CompassEnabled", compassenabled);


        float tilted = Input.acceleration.z;
        if (tilted > 1.0f)
        {
            tilted = 1.0f;
        }
        else if (tilted < -1.0f)
        {
            tilted = -1.0f;
        }
        tilted *= 90.0f;
        if (m_bCompassEnabled)
        {
            PlayerPrefs.SetFloat("Quest_" + m_NrQuestsDone + "_" + m_CurrentStep + "_Tilt", tilted);
        }
        else
        {
            PlayerPrefs.SetFloat("Quest_" + m_NrQuestsDone + "_" + m_CurrentStep + "_Tilt", -1.0f);
        }

        float lat = Input.location.lastData.latitude;
        float lng = Input.location.lastData.longitude;

        PlayerPrefs.SetFloat("Quest_" + m_NrQuestsDone + "_" + m_CurrentStep + "_Lat", lat);
        PlayerPrefs.SetFloat("Quest_" + m_NrQuestsDone + "_" + m_CurrentStep + "_Lng", lng);

        //PlayerPrefs.SetFloat ("Quest_" + m_NrQuestsDone + "_" + m_CurrentStep + "_Accuracy", Input.compass.headingAccuracy);
        PlayerPrefs.SetFloat("Quest_" + m_NrQuestsDone + "_" + m_CurrentStep + "_Accuracy", Input.location.lastData.horizontalAccuracy);



        string theTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:sszz");
        string theTime2 = theTime;//theTime.Replace ("+", "%2B");
                                  //theTime2 += "00";
        Debug.Log("CurrentTimestamp: " + theTime2);
        PlayerPrefs.SetString("Quest_" + m_NrQuestsDone + "_" + m_CurrentStep + "_Timestamp", theTime2);



        PlayerPrefs.Save();
    }

    public void Cancel()
    {
        Application.Quit();
    }

    public void Reset()
    {
        m_PhotoTask.Reset();
        //m_ButtonFinish.interactable = false;
        EnableButtons();
    }

    public void Back ()
    {
        Debug.Log("PhotoTask -> Back clicked");
        if (m_PhotoTask != null)
        {
            Debug.Log("m_PhotoTask != null");
            m_PhotoTask.Back();
            EnableButtons();
        } else {
            StopSensor();

            Debug.Log("m_PhotoTask == null");

            PlayerPrefs.SetInt("DynQuestionsToLastState", 1);
            PlayerPrefs.Save();
            //Application.LoadLevel("DynamicQuestions");
            Application.LoadLevel("Validation");
        }
    }

    public void StopSensor()
    {
        m_SensorImage.StopSensor();
    }

    public void Next ()
    {
        m_PhotoTask.Next();
       // Application.LoadLevel("QuestFinished");
        //Application.Quit();
    }

    private bool isBackButtonEnabled()
    {
        if (m_PhotoTask == null) return false;
        return m_PhotoTask.IsAnyFinished();
    }

    private bool isNextButtonEnabled()
    {
        if (m_PhotoTask == null) return false;
        return m_PhotoTask.IsAllFinished();
    }

    private void checkBackButton()
    {
        m_ButtonBack.interactable = true;//isBackButtonEnabled () ;
    }

    private void checkNextButton()
    {
        m_ButtonNext.interactable = true;//isNextButtonEnabled();
    }
    private void EnableButtons()
    {
        checkBackButton();
        checkNextButton();
    }
    public void ShowConsole ( bool show )
    {
        m_DebugView.SetActive ( show ) ;
    }

    public void UseCompass ( bool b )
    {
        m_bUseCompass = b;
        PanelCompass   . SetActive (  m_bUseCompass ) ;
        m_CompassStrip . SetActive (  m_bUseCompass ) ;
       // PanelManual    . SetActive ( !m_bUseCompass ) ;
       // m_PhotoTask = ( b ? ( IPhotoTask ) m_CompassStrip : m_TaskManual ) ;
        Reset();
    }

    public void UseMethod ( int value )
    {
        m_Method = ( EMethod ) value ;
        PanelCompass.SetActive ( EMethod.Kompass == m_Method ) ;
        m_CompassStrip.SetActive ( EMethod.Kompass == m_Method ) ;
      //  PanelManual.SetActive ( EMethod.Manual == m_Method ) ;
      //  PanelSequential.SetActive ( EMethod.Sequential == m_Method ) ;
      /*  switch ( m_Method )
        {
            case EMethod.Kompass    : m_PhotoTask = m_CompassStrip   ; break ;
            case EMethod.Manual     : m_PhotoTask = m_TaskManual     ; break ;
            case EMethod.Sequential : m_PhotoTask = m_TaskSequential ; break ;
        }*/
        Reset();
    }

    public void SetPortrait ( bool value )
    {
        Screen.orientation = ( value ? ScreenOrientation.Portrait : ScreenOrientation.LandscapeLeft ) ;
        //if ( value)
        //{
        //    Screen.orientation = ScreenOrientation.Portrait ;
        //}
        //else
        //{
        //    Screen.orientation = ScreenOrientation.LandscapeLeft ;
        //}
        m_SensorImage.SetPortrait ( value ) ;
    }

    // Use this for initialization
    void Start ()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep ;
        SetPortrait ( false ) ;
        //UseCompass ( true ) ;
        // setup ui
        //m_ButtonFinish.interactable = false;
        m_ButtonBack.interactable = true;//false;
        m_ButtonNext.interactable = true;//false;
        m_DebugView.SetActive(false);
      //  PanelManual.SetActive(false);
     //   PanelSequential.SetActive(false);
        m_PhotoTask = m_CompassStrip;

        if ((!LocalizationSupport.StringsLoaded))
            LocalizationSupport.LoadStrings();

        m_ButtonBack.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("BtnBack");
        m_ButtonNext.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Next");


    }

    // Update is called once per frame
    void Update ()
    {
        if ( Input.GetKeyDown ( KeyCode.Escape ) ) Application.Quit() ;

        m_DebugText.Clear();
        //m_SensorImage.printDebugInfo(m_DebugText);
        m_PhotoTask.UpdateTask() ;
    }
}
