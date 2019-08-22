using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassStrip : MonoBehaviour , IPhotoTask
{
    public static Color ColorPending  = new Color ( 1.0f , 0.0f , 0.0f , 0.8f ) ;
    public static Color ColorFocus    = new Color ( 1.0f , 1.0f , 0.0f , 0.8f ) ;
    public static Color ColorFinished = new Color ( 0.0f , 1.0f , 0.0f , 0.8f ) ;
    public float TaskPitch;
    public static float LabelDistance = 400.0f ;

    //public bool m_Flash = true ;
    public bool m_FlashingFocus = true ;
    public bool m_UseSensorFusion = true  ;
    public bool m_SnapIfAccurate  = false ;

    public Toggle m_ToggleFlash;
    public Toggle m_ToggleSnap;
    public Toggle m_ToggleSensorFusion;
    public Text StatusText;

    public DebugText m_DebugText ;

    bool m_bTakingPointPhoto = false;
    public GameObject m_CompassStrip;
    public GameObject m_ButtonTakePicture;

    private int Smooth = 7;
    private PhotoTask m_PhotoTask ;
    private readonly string[] PanelNames  = new string[] { "North", "East", "South", "West" };
    private readonly string[] PanelTitles = new string[] { "Panel_N" , "Panel_E", "Panel_S", "Panel_W" } ;
    private readonly TaskPanel[] Panels = new TaskPanel [ 8 ] ;

    private SmoothedOrientation m_SmoothedOrientation = new SmoothedOrientation();
    private SensorFusion m_SensorFusion = new SensorFusion();
    private GameObject KompassFrame;

    private Stack < int > m_Dones = new Stack < int > () ;

    public void SetActive ( bool value )
    {
        gameObject.SetActive(value);
        KompassFrame.SetActive(value);
    }
    public static Color getPanelColor ( TaskPanel.EColorStatus status )
    {
        switch ( status )
        {
            case TaskPanel.EColorStatus.CS_Pending  : return ColorPending  ;
            case TaskPanel.EColorStatus.CS_Focus    : return ColorFocus    ;
            case TaskPanel.EColorStatus.CS_Finished : return ColorFinished ;
            default : return ColorPending ;
        }
    }

    public void UseSensorFusion(bool b)
    {
        m_UseSensorFusion = b ;
    }

    public void SnapIfAccurate(bool snap)
    {
        m_SnapIfAccurate = snap ;
    }

    public void FlashIfAccurate(bool flash)
    {
        m_FlashingFocus = flash ;
    }

    public void Reset ()
    {
        for ( int i = 0 ; i < 8 ; i++ )
        {
            Panels[i].SetStatus ( TaskPanel.ETaskStatus.TS_Pending ) ;
        }
        m_Dones.Clear() ;
    }

    public void Back ()
    {
        if ( IsAnyFinished () )
        {
            int n = m_Dones.Pop() ;
            Panels [ n ] . SetStatus ( TaskPanel.ETaskStatus.TS_Pending ) ;

            m_bTakingPointPhoto = false;
        } else {
            if (m_bTakingPointPhoto)
            {
                m_bTakingPointPhoto = false;
            }
            else
            {

                 m_PhotoTask.StopSensor();


            PlayerPrefs.SetInt("DynQuestionsToLastState", 1);
                PlayerPrefs.Save();
               // Application.LoadLevel("DynamicQuestions");
                Application.LoadLevel("Validation");
            }
        }
    }

    public void SetFinished ( int n )
    {
        Panels[n].SetFinished() ;
        m_Dones.Push ( n ) ;
    }


    public bool IsAllFinished ()
    {
        for ( int i = 0 ; i < 4 ; i++ )
        {
            if ( !Panels[i].IsFinished() ) return false ;
        }
        return true ;
    }

    public bool IsAnyFinished()
    {
        return m_Dones.Count != 0 ;
    }

    private void TakePicture ( int direction )
    {
        m_PhotoTask.TakePicture ( direction ) ;
    }

    string m_StringTakePhoto;
    string m_StringHoldStill;
    string m_StringLandscape;
    string m_StringFocus;

    // Use this for initialization
    void Start ()
    {
        //Screen.sleepTimeout = SleepTimeout.NeverSleep ;
        DeviceInput.Init() ;

        // setup smoothing class
        Smoothed.SetSmoothingStrength ( Smooth ) ;

        // sensor fusion needs gyroscope
        m_UseSensorFusion = DeviceInput.HasGyro ;

        m_ToggleFlash.isOn = m_FlashingFocus;
        m_ToggleSnap.isOn = m_SnapIfAccurate ;
        m_ToggleSensorFusion.isOn = m_UseSensorFusion ;
        if ( !DeviceInput.HasGyro ) m_ToggleSensorFusion.interactable = false ;

        // find parent
        m_PhotoTask = transform.parent.gameObject.GetComponent<PhotoTask> () ;
        // setup label objects
        for ( int i = 0 ; i < 4 ; i++ )
        {
            // TaskPanel
            Panels[i] = transform.Find ( PanelTitles[i] ) . gameObject.GetComponent<TaskPanel> () ;
            Panels[i].Heading = 90.0f * i ;
            Panels[i].Pitch = TaskPitch ;
            Panels[i].Name = PanelNames [ i ] ;
        }
        // setup frame
        // Panel_Frame
        Transform TrFrame = transform.Find ( "Panel_Frame" ) ;
        KompassFrame = TrFrame.gameObject ;

        TrFrame.SetParent ( transform.parent , false ) ;

        m_StringTakePhoto = LocalizationSupport.GetString("TakePhotoPoint");
        m_StringHoldStill = LocalizationSupport.GetString("TakePhotoHoldStill");
        m_StringLandscape = LocalizationSupport.GetString("TakePhotoLandscape");
        m_StringFocus = LocalizationSupport.GetString("TakePhotoFocus");
    }

    public float m_Distance = 2.0f;

    // Update is called once per frame
    public void UpdateTask ()
    {
        float DeltaTime = Time.deltaTime;
        //m_Timer.Update(DeltaTime);
        DeviceInput.Update();
        Smoothed.SetTimeStep(DeltaTime);
        //ClearString();

        SOrientation AccMagOrientation = DeviceInput.AccMagOrientation;
        Quaternion RotationChange = DeviceInput.GetRotationChange(DeltaTime);

        SOrientation Orientation;

        if (DeviceInput.HasGyro) m_SensorFusion.Update(AccMagOrientation, RotationChange, 0.9f);
        m_SmoothedOrientation.Update(AccMagOrientation);

        if (m_UseSensorFusion)
        {
            Orientation = m_SensorFusion.Orientation;
        }
        else
        {
            Orientation = m_SmoothedOrientation.Orientation;
        }
        m_DebugText.PrintText(Orientation);
        m_DebugText.NewLine () ;
        float heading = Orientation.Yaw ;
        float pitch = Orientation.Pitch ;
        float tilt = Orientation.Roll ;
        int iFocus = -1 ;
        for ( int i = 0 ; i < 4 ; i++ )
        {
            // TaskPanel
            float Dist = Panels[i].UpdateAngles ( heading , pitch) ;
            if (!Panels[i].IsFinished() )
            {
                if (m_Distance >= Dist && m_Distance >= Mathf.Abs ( tilt ) )
                {
                    iFocus = i ;
                    if ( 0.6f < Panels[i].GetFocusTime() )
                    {
                        TakePicture ( i ) ;
                    }
                    else
                    {
                        Panels[i].SetStatus ( TaskPanel.ETaskStatus.TS_Focus , m_FlashingFocus ) ;
                    }
                }
                else
                {
                    Panels[i].SetStatus ( TaskPanel.ETaskStatus.TS_Pending ) ;
                }
            }
        }

        bool bAllFinished = IsAllFinished();

        if(bAllFinished) {
            m_bTakingPointPhoto = true;
        }


        if(m_bTakingPointPhoto) {
            m_CompassStrip.SetActive(false);
            m_ButtonTakePicture.SetActive(true);
        } else {
            m_CompassStrip.SetActive(true);
            m_ButtonTakePicture.SetActive(false);
        }



        if (bAllFinished || m_bTakingPointPhoto)
        {
            StatusText.text = m_StringTakePhoto;
        } 
        else if ( iFocus >= 0 )
        {
            StatusText.text = m_StringHoldStill;//"Halten Sie das Handy kurz ruhig" ;
            if ( iFocus >= 0 && m_SnapIfAccurate )
            {
                tilt = 0.0f ;
                float SnappedHeading = Panels[iFocus].Heading ;
                float SnappedPitch = Panels[iFocus].Pitch ;
                for ( int i = 0 ; i < 4 ; i++ )
                {
                    Panels[i].UpdateAngles ( SnappedHeading , SnappedPitch ) ;
                }
            }
        }
        else if ( Mathf.Abs ( tilt ) > 20.0f )
        {
            StatusText.text = m_StringLandscape;//"Halte das Handy im Landschaftsmodus" ;
        }
        else
        {
            StatusText.text = m_StringFocus;//"Bringe die roten Quadrate in den Rahmen um Fotos nach Norden, Osten, Süden und Westen zu machen";
        }
        m_DebugText.PrintText( "iFocus" , iFocus ) ;

        float RotZ = Div.getClamped ( -2.0f * tilt , -35.0f , 35.0f ) ;
        transform.localRotation = Quaternion.Euler ( 0.0f , 0.0f , RotZ ) ;
    }


    public void Next()
    {
        if(!m_bTakingPointPhoto) {
            m_bTakingPointPhoto = true;
        } else {
            m_PhotoTask.StopSensor();

            int m_NrQuestsDone = 0;
            if (PlayerPrefs.HasKey("NrQuestsDone"))
            {
                m_NrQuestsDone = PlayerPrefs.GetInt("NrQuestsDone");
            }
            else
            {
                m_NrQuestsDone = 0;
            }

            string startcameratime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            PlayerPrefs.SetString("Quest_" + m_NrQuestsDone + "_" + "EndCameraTime", startcameratime);
            PlayerPrefs.Save();

            Application.LoadLevel("QuestFinished");
        }
    }

    public void TakePointPhoto()
    {
        TakePicture(4);
        m_PhotoTask.StopSensor();

        int m_NrQuestsDone = 0;
        if (PlayerPrefs.HasKey("NrQuestsDone"))
        {
            m_NrQuestsDone = PlayerPrefs.GetInt("NrQuestsDone");
        }
        else
        {
            m_NrQuestsDone = 0;
        }

        string startcameratime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        PlayerPrefs.SetString("Quest_" + m_NrQuestsDone + "_" + "EndCameraTime", startcameratime);
        PlayerPrefs.Save();

        Application.LoadLevel("QuestFinished");
    }


    public void TakePhoto1()
    {
        TakePicture(0);
    }
    public void TakePhoto2()
    {
        TakePicture(1);
    }
    public void TakePhoto3()
    {
        TakePicture(2);
    }
    public void TakePhoto4()
    {
        TakePicture(3);
    }
}
