//#define DEBUGAPP


using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using Unitycoding.UIWidgets;

#if DONTUSESIGNAL
using Signalphire;
#endif
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Text;

#if DONTUSESIGNAL
using Signalphire;
#endif
//using  UTNotifications;
//using Vatio.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UI.Pagination;

//using NatShareU;



public class DemoMap : MonoBehaviour
{
    public Transform camera2D;
    public Transform camera3D;

    public Shader tileShader;

    public float CameraChangeTime = 1;

    private GUIStyle activeRowStyle;
    private float animValue;
    private OnlineMaps api;
    private OnlineMapsTileSetControl control;
    private bool is2D = false;//true;
    private bool isCameraModeChange;
    private GUIStyle rowStyle;
    private string search = "";
    private OnlineMapsMarker searchMarker;

    private Transform fromTransform;
    private Transform toTransform;

    public GameObject m_MapDeactivated;

    bool m_bIn2dMap;
    bool m_bTo2dMap;
    float m_To2dMapTimer;
    //  public GameObject m_BtnTo2dMap;


    bool m_bTo3dMap;
    float m_To3dMapTimer;
    // public GameObject m_BtnTo3dMap;

    bool m_bDebug;


    private MessageBoxNews messageBox;
    private MessageBox messageBoxSmall;
    private MessageBox messageBoxSmall2;/*
    int m_ShowMsgBox;
    bool m_bShownMsgBox;
    bool m_bShowingMsgBox;
    bool m_bMsgBoxLoaded;
    string m_MsgBoxText;
    public Image m_MsgBoxImg;*/


    //private OnlineMapsMarker playerMarker;
    private OnlineMapsMarker3D playerMarker3D = null;
    //private OnlineMapsMarker playerDirection;
    float m_DirectionSize = 20.0f;
    float m_DirectionSizeWhite = 30.0f;

    public FotoQuestPin[] m_Pins;
    public int m_NrPins;
    public FotoQuestPin m_SelectedPin;
    public bool m_bSelectedPin = false;
    bool m_bPinAlreadyReviewed = false;
    //  public OnlineMapsDrawingLine m_LineToPin = null;
    /*public FotoQuestPin[] m_PinsDone;
    public int m_NrPinsDone;*/
    bool m_bHasAlreadyLoadedPins = false;
    float m_PinMinRadius = 100.0f;

    //-------------------------------
    // Additional info
    private Vector2 m_PlayerPositionStart;
    int m_NrPlayerPositions;
    private Vector2 m_PlayerLastPosition;
    private Vector2[] m_PlayerPositions;
    private string m_QuestSelectedTime;
    float m_DistanceWalked;
    private Vector2 m_DistanceWalkedLastCheckpoint;
    //-------------------------------

   /* int m_InnerDistance = 30;
    int m_NearDistance = 100;
    int m_OuterDistance = 500;*/

    bool m_bInReachOfPoint;

    /*
    public PuzzlePin[] m_PuzzlePins;
    public int m_NrPuzzlePins;*/
    /*
    public Texture2D m_Pin;
    public Texture2D m_PinRed;
    public Texture2D m_PinYellow;*/

    float m_ConquerBackShiftX;
    bool m_bConquerShiftSet;
    int m_bConquerShiftPosition;

    public GameObject m_MenuSlidin;
    public GameObject m_MenuToggleButton;
    bool m_bMenuOpened;


    //-----------------------
    // Add new point

    bool m_bAddingNewPointsEnabled = true;
    public GameObject m_MenuAddNewPointType;
    public GameObject m_MenuAddNewPointTypeButton1;
    public GameObject m_MenuAddNewPointTypeButton2;
    public GameObject m_MenuAddNewPoint;
    public GameObject m_MenuAddNewPointHelpText;
    public GameObject m_MenuAddNewPointPublicText;
    public GameObject m_MenuAddNewPointPublicYes;
    bool m_bMenuAddNewPointOpened = false;

    bool m_bAddingNewPoint = false;
    int m_AddingNewPointIter = 0;

    bool m_bAddingNewPointChooseLocation = false;

    private OnlineMapsMarker3D m_NewPointMarker = null;
    bool m_bAddingPinPositionSet = false;
    Vector2 m_AddingPinPosition;

    public void StartAddingNewPoint()
    {
        if(m_bAddingNewPoint) {
            return;
        }
        m_bAddingNewPoint = true;
        m_AddingNewPointIter = 0;
        deselectPoint();
        removePins();
        //  showActivity(false, true);

        m_bAddingNewPointChooseLocation = false;

        m_NewPointMarker = null;
        m_bMenuAddNewPointOpened = true;
        SlidingMenu menu = (SlidingMenu)m_MenuAddNewPoint.GetComponent(typeof(SlidingMenu));
        menu.SlideToValue(0.5f/*0.27f/*0.28f*/);

        m_MenuAddNewPoint.SetActive(true);
        m_MenuAddNewPointHelpText.SetActive(false);
        m_MenuAddNewPointPublicText.SetActive(false);
        m_MenuAddNewPointPublicYes.SetActive(false);
        m_MenuAddNewPointType.SetActive(true);         m_MenuAddNewPointTypeButton1.SetActive(true);         m_MenuAddNewPointTypeButton2.SetActive(true);

        OnlineMaps.instance.position = m_PlayerPosition;
        if (OnlineMaps.instance.zoom < 17)
            OnlineMaps.instance.zoom = 17;

        updateToPositionButtons(true);
    }

    public void StartAddingNewPointChooseLocation()
    {
        if (m_bAddingNewPointChooseLocation)
        {
            return;
        }
        m_bAddingNewPointChooseLocation = true;
        m_AddingNewPointIter = 0;
        deselectPoint();
        removePins();
        //  showActivity(false, true);

        m_NewPointMarker = null;
        m_bMenuAddNewPointOpened = true;
        SlidingMenu menu = (SlidingMenu)m_MenuAddNewPoint.GetComponent(typeof(SlidingMenu));
        menu.SlideToValue(0.3f/*0.27f/*0.28f*/);

        m_MenuAddNewPoint.SetActive(true);
        m_MenuAddNewPointHelpText.SetActive(true);
        m_MenuAddNewPointPublicText.SetActive(false);
        m_MenuAddNewPointPublicYes.SetActive(false);
        m_MenuAddNewPointType.SetActive(false);
        m_MenuAddNewPointTypeButton1.SetActive(false);
        m_MenuAddNewPointTypeButton2.SetActive(false);

        OnlineMaps.instance.position = m_PlayerPosition;
        if (OnlineMaps.instance.zoom < 17)
            OnlineMaps.instance.zoom = 17;

        updateToPositionButtons(true);
    }

    public void CloseAddingNewPoint()
    {
        m_bAddingNewPoint = false;
        m_bMenuAddNewPointOpened = false;
        m_bAddingNewPointChooseLocation = false;
        SlidingMenu menu = (SlidingMenu)m_MenuAddNewPoint.GetComponent(typeof(SlidingMenu));
        menu.SlideToValue(-0.1f/*0.27f/*0.28f*/);

        m_bAddingPinPositionSet = false;
        m_NewPointMarker = null;

        m_bIgnoreClick = true;
        m_IgnoreClickIter = 0;
     //   showActivity(true, true);
        removePins();
        addPins();

        updateToPositionButtons(true);
    }

    private void AddNewPin(Vector3 pos)
    {
        if(m_AddingNewPointIter < 20) {
            return;
        }
        //  Debug.Log("##### AddNewPin #####");
        Vector3 mouseGeoLocation = OnlineMapsControlBase.instance.GetCoords(pos);

        m_AddingPinPosition = mouseGeoLocation;
        OnlineMapsControlBase3D control2 = GetComponent<OnlineMapsControlBase3D>();

        if (m_bAddingPinPositionSet)
        {
            OnlineMaps api = OnlineMaps.instance;
            if (m_NewPointMarker != null)
            {
                m_NewPointMarker.SetPosition(m_AddingPinPosition.x, m_AddingPinPosition.y);
                /*
                OnlineMaps map = OnlineMaps.instance;
                double tlx, tly, brx, bry;
                map.GetTopLeftPosition(out tlx, out tly);
                map.GetBottomRightPosition(out brx, out bry);

                m_NewPointMarker.Update(tlx, tly, brx, bry, map.zoom);*/
            }
        }
        else
        {
            OnlineMapsMarker3D marker = control2.AddMarker3D(mouseGeoLocation, m_PinPlaneYellow);
            m_NewPointMarker = marker;


            Transform markerTransform = marker.transform;

            float proc = (m_CameraPitch - 37.0f) / (90.0f - 37.0f);
            if (proc < 0.0f)
            {
                proc = 0.0f;
            }
            else if (proc > 1.0f)
            {
                proc = 1.0f;
            }
            float pinorienation = m_PinOrientationX * (1.0f - proc);

            Quaternion quatrot = Quaternion.Euler(pinorienation, -m_CameraAngle + 90, 0);

            if (markerTransform != null) markerTransform.rotation = quatrot;
            m_bAddingPinPositionSet = true;
        }


        SlidingMenu menu = (SlidingMenu)m_MenuAddNewPoint.GetComponent(typeof(SlidingMenu));
        //menu.SlideToValue(0.38f/*0.27f/*0.28f*/);
        menu.SlideToValue(0.3f/*0.27f/*0.28f*/);


        m_MenuAddNewPointHelpText.SetActive(false);
        m_MenuAddNewPointPublicText.SetActive(true);
        m_MenuAddNewPointPublicYes.SetActive(true);
    }

    //=================================


    public Texture2D m_LineTexture;

    public GameObject m_PinPlaneYellow;
    public GameObject m_PinPlaneGreen;

    public GameObject m_PinDone;
    public GameObject m_Pin;

    public GameObject m_PinCircle;
    public GameObject m_PinClusterText;
    public GameObject m_PlanePosition;
    public bool m_bNearPinSetup;
    public float m_NearPinX;
    public float m_NearPinY;


    int m_bLoadMarkers = 0;

    public GameObject m_ButtonToPosBright;
    public GameObject m_ButtonToPosDark;
    public GameObject m_ButtonToPosBrightBottom;
    public GameObject m_ButtonToPosDarkBottom;

    public GameObject m_ButtonAddPin;
    public GameObject m_ButtonAddPinBottom;
    public GameObject m_ButtonLayerBright;
    public GameObject m_ButtonLayerDark;


    //public GameObject m_TextInput;

    bool m_bStartReadGPS = true;
    bool m_bFollowingGPS = true;//true;

    public GameObject m_IconLogin;
    public GameObject m_IconLogout;

    public GameObject m_TextTest;

    /*bool m_bMenuOpen;
    float m_MenuOpenTimer;*/

    //  bool m_bMenuSelectionOpen;
    /*public GameObject m_MenuSelectionBack;
    public GameObject m_MenuSelectionBack2;
    public GameObject m_MenuSelectionHello;
    public GameObject m_MenuSelectionHelloPortrait;*/
    public GameObject m_MenuSelectionButtonLogout;
    public GameObject m_MenuSelectionButtonIntroduction;
    public GameObject m_MenuSelectionButtonTerms;
    public GameObject m_MenuSelectionButtonChat;
    public GameObject m_MenuSelectionButtonManual;
    public GameObject m_MenuSelectionButtonGuidelines;
    public GameObject m_MenuSelectionButtonContact;
    public GameObject m_MenuSelectionButtonHomepage;
    public GameObject m_MenuSelectionButtonCancel;
    public GameObject m_MenuSelectionButtonClose;
    /*
    public GameObject m_Sky;
    public GameObject m_Sky2;*/

    /*
    public GameObject m_Tooltip;
    public GameObject m_TooltipBack;
    public GameObject m_TooltipBackBig;
    public GameObject m_TooltipText;
    public GameObject m_ButtonShowPictures;

    public GameObject m_TooltipLeft;
    public GameObject m_TooltipBackLeft;
    public GameObject m_TooltipBackBigLeft;
    public GameObject m_TooltipBackBigLeftLeft;
    public GameObject m_TooltipTextLeft;
    public GameObject m_ButtonShowPicturesLeft;*/


    //public GameObject m_ButtonPickPuzzle;
    public GameObject m_ButtonStartQuest;
    public GameObject m_TextStartQuest;
    //  public GameObject m_ButtonStartQuestHighlighted;
    //  public GameObject m_ButtonNearlyStartQuest;
   // public GameObject m_ButtonPointNotReachable;

    public GameObject m_ButtonMoveCloser;
    public GameObject m_TextMoveCloser;
    public GameObject m_TextAlreadyRated;

    public GameObject m_StartQuestBackground;
    public GameObject m_StartQuestBackgroundLine;


    /*float m_FoundPuzzleTimer;
    public GameObject m_TextFoundPuzzle;
    public GameObject m_BackFoundPuzzle;*/

    bool m_bShowWelcome;
    float m_WelcomeTimer;
    public GameObject m_TextWelcome;
    public GameObject m_BackWelcome;

    bool m_bCheckInternet;
    float m_CheckInternetTimer;

    public GameObject m_TextDebug;
    public GameObject m_BackDebug;
    string m_StrTextDebug;
    public GameObject m_TextLocationOutside;
    public GameObject m_BackLocationOutside;


    public GameObject m_DebugLine;
    public GameObject m_DebugLine2;

    float m_LocationLatDisabled;
    float m_LocationLngDisabled;





    bool m_bZoomTouched;

    //-------------------
    // Animations
    //public Animator m_AnimLeaderboard;

    int m_NrQuestsDone;

    //int m_PuzzlesInReachWait;

    private void ChangeMode()
    {
        /*is2D = !is2D;

        animValue = 0;
        isCameraModeChange = true;

        Camera c = Camera.main;
        fromTransform = is2D ? camera3D : camera2D;
        toTransform = is2D ? camera2D : camera3D;

        c.orthographic = false;
        if (!is2D)
            c.fieldOfView = 28;//15;//28;
            */
    }

   

    void enableZoomButtons()
    {
        /*  if (api.zoom <= 6) {//10) {// 12) {
            m_ButtonZoomOutTop.SetActive (false);
        } else {*/
        /*      m_ButtonZoomOutTop.SetActive (true);
        //  }

            if (api.zoom >= 20) {
                m_ButtonZoomInTop.SetActive (false);
            } else {
                m_ButtonZoomInTop.SetActive (true);
            }*/
    }
    public void zoomOut()
    {
        //  Debug.Log(">> zoomOut");
        m_bHasZoomed = true;
        m_bZoomTouched = true;
        //if(api.zoom <= 10) {
        /*if(api.zoom <= 6) {//10) {//12) {
        m_ButtonZoomOutTop.SetActive (false);
        return;
    }*/

        api.zoom--;
        //closePinInfo ();
        enableZoomButtons();
        m_bForceUpdate = true;
        //      updatePins ();
        //   Debug.Log("Zoom: " + api.zoom);
      //  loadPins();
        showPins();

    }
    public void zoomIn()
    {
        //  Debug.Log(">> zoomIn");
        m_bHasZoomed = true;
        m_bZoomTouched = true;
        api.zoom++;
        m_bForceUpdate = true;
        //closePinInfo ();
        enableZoomButtons();

        //  updatePins ();
        //  Debug.Log("Zoom: " + api.zoom);
     //   loadPins();
        showPins();
    }
    public void followGPS()
    {
        m_bFollowingGPS = true;
        //  m_ButtonFollowGPS.SetActive (false);
        toPlayerPosition();

      //  loadPins();
        showPins();
    }


    public class MarkerComparer2 : System.Collections.Generic.IComparer<OnlineMapsMarker3D>
    {
        public DemoMap m_pDemoMap;

        public int Compare(OnlineMapsMarker3D m1, OnlineMapsMarker3D m2)
        {
            //    Debug.Log("Marker compare");


            if (m1.position.y > m2.position.y) return -1;
            if (m1.position.y < m2.position.y) return 1;
            return 0;
        }
    }


    public class MarkerComparer : System.Collections.Generic.IComparer<OnlineMapsMarker>
    {
        public DemoMap m_pDemoMap;

        public int Compare(OnlineMapsMarker m1, OnlineMapsMarker m2)
        {
            //    Debug.Log("Marker compare");


            if (m1.position.y > m2.position.y) return -1;
            if (m1.position.y < m2.position.y) return 1;
            return 0;
        }
    }


    IEnumerator changeFramerate()
    {
        yield return new WaitForSeconds(1);
        Application.targetFrameRate = 30;
    }

    public void ForceLandscapeLeft()
    {
        StartCoroutine(ForceAndFixLandscape());
    }

    IEnumerator ForceAndFixLandscape()
    {
        yield return new WaitForSeconds(0.01f);
        /*for (int i = 0; i < 3; i++) {
        if (i == 0) {
            Screen.orientation = ScreenOrientation.Portrait;
        }  else if (i == 1) {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }  else {*/
        //  Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToLandscapeLeft = false;
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToPortrait = true;
        //  }
        yield return new WaitForSeconds(0.5f);
        //}
    }

    ArrayList m_QuestsMade;

    bool m_bBlackGui = false;
    /*ArrayList m_PuzzlesPicked_ChunkId;
    ArrayList m_PuzzlesPicked_Id;
    int m_NrPuzzlesPicked;*/
    private void Start()
    {
        StartCoroutine(changeFramerate());
        ForceLandscapeLeft();

        if ((!LocalizationSupport.StringsLoaded))
            LocalizationSupport.LoadStrings();

        /*   m_DistanceBack2d.SetActive(false);
           m_DistanceText2.SetActive(false);
           m_DistanceText.SetActive(false);
           m_DistanceBack.SetActive(false);*/

        m_TextLocationOutside.SetActive(true);
        m_BackLocationOutside.SetActive(true);
        UnityEngine.UI.Text textdebug = m_TextLocationOutside.GetComponent<UnityEngine.UI.Text>();
        if (Application.systemLanguage == SystemLanguage.German)
        {
            //  m_ButtonStartQuest.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Ich befinde mich genau am Punkt.";
            //      m_ButtonPointNotReachable.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Ich kann den Punkt nicht erreichen.";

            //textdebug.text = "FotoQuest Go ist momentan nur in Österreich verfübar.";
        }
        else
        {
            //m_ButtonPointNotReachable.GetComponentInChildren<UnityEngine.UI.Text> ().text = "I can't reach\nthe point.";
            //textdebug.text = "FotoQuest Go is currently only available in Austria.";
        }
        m_BackLocationOutside.GetComponent<Image>().color = new Color32(255, 255, 255, 240);//new Color32(255,255,255,240);

        m_TextStartQuest.GetComponent<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("StartQuest");
       // m_ButtonPointNotReachable.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("ShowDetails");//"Ich kann den Punkt nicht erreichen.";

       // m_TextDoActivity.GetComponent<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("DoActivity");



        m_MenuAddNewPointHelpText.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("AddPinTitle");
        m_MenuAddNewPointPublicText.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("AddPinIsPublicTitle");
        m_MenuAddNewPointPublicYes.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Create");

        m_MenuAddNewPointType.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("OpportunisticAdd");
        m_MenuAddNewPointTypeButton1.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("OpportunisticAdd1");
        m_MenuAddNewPointTypeButton2.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("OpportunisticAdd2");

        m_TextAlreadyRated.GetComponent<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("AlreadyReviewed");

        m_bHasAlreadyLoadedPins = false;

        m_PinInfoClosedIter = 0;
        m_bPinInfoClosed = false;

        m_NotificationIter = 0;
        m_bMenuOpened = false;
        m_MenuToggleButton.SetActive(false);

        //-------------------
        // Check if adding points button should be enabled
        string sessions = PlayerPrefs.GetString("ActiveSessions");
        Debug.Log("Active sessions: " + sessions);
        string newsessions = "";
        string[] splitArray = sessions.Split(char.Parse(" "));
        m_bAddingNewPointsEnabled = false;
        for (int i = 0; i < splitArray.Length && m_bAddingNewPointsEnabled == false; i++)
        {
            string valid = splitArray[i];
            if (valid != "" && valid != " ")
            {
                int enabled = PlayerPrefs.GetInt("SessionSettingsOpportunisticValidationsEnabled_" + valid);
                if(enabled == 1) {
                    m_bAddingNewPointsEnabled = true;
                }
                Debug.Log("SessionSettingsOpportunisticValidationsEnabled_" + valid + ": " + enabled);
            }
        }

       // m_bAddingNewPointsEnabled = true;
        //------------------

        m_bInReachOfPoint = false;

        m_bZoomTouched = false;

        //  m_ButtonFollowGPSTop.SetActive(false);

        m_bIn2dMap = true;//false;
        m_CameraPitch = 90.0f;
        m_bTo2dMap = false;
        m_bTo3dMap = false;
        m_To2dMapTimer = 0.0f;
        /*m_BtnTo2dMap.SetActive (true);
        m_BtnTo3dMap.SetActive (false);*/
        /*  m_BtnTo2dMap.SetActive(false);
          m_BtnTo3dMap.SetActive(true);
          m_BtnTo3dMap.SetActive(false);*/


        //  PlayerPrefs.DeleteAll ();
        m_bConquerShiftSet = false;

        m_bDebug = false;//false;//true;//false;//false;//true;// true;//true;// true;//true;//false;//true;//false;//false;//false;//true;//true;
        if (PlayerPrefs.HasKey("DebugEnabled"))
        {
            if (PlayerPrefs.GetInt("DebugEnabled") == 1)
            {
                m_bDebug = true;
            }
        }
        //m_bDebug = true;

        m_MapDeactivated.SetActive(false); // Todo: Comment this out for final release



        messageBox = UIUtility.Find<MessageBoxNews>("MessageBoxNews");
        messageBoxSmall = UIUtility.Find<MessageBox>("MessageBoxSmall");
        messageBoxSmall2 = UIUtility.Find<MessageBox>("MessageBoxSmall2");
        /*
    int openmsg = PlayerPrefs.GetInt ("OpenMsg");
    if (PlayerPrefs.HasKey("OpenMsg") && openmsg != 1) {
        m_bShownMsgBox = true;
    } else {
        PlayerPrefs.SetInt ("OpenMsg", 0);
        PlayerPrefs.Save ();
    }*/


        m_bLineInited = false;
        m_bLineVisible = false;
        //        g_LevelPos = new Vector2(20, 40);
        //      g_LevelSize = new Vector2(500, 50);

        m_PlayerPositions = new Vector2[50];

        //  m_LocationLatDisabled = 48.210033f;
        //  m_LocationLngDisabled = 16.363449f;
        m_LocationLatDisabled = -10.0f;
        m_LocationLngDisabled = 0.0f;
        // RELEASE MODE: turn this to 0, 0
        /*
        m_InnerDistance = 30;
        m_NearDistance = 100;
        m_OuterDistance = 500;*/

        if (m_bDebug /*|| true*/) // Todo: Comment true out again
        {
            m_LocationLatDisabled = 48.210033f;
            m_LocationLngDisabled = 16.363449f;
            /*  m_LocationLatDisabled = 48.210033f;
                m_LocationLngDisabled = 17.363449f;*/


            //m_LocationLatDisabled = 48.210033f + 0.002f;//- 0.005f;
            //  m_LocationLngDisabled = 17.363449f - 0.02f;

            /*m_InnerDistance = 1000;
            m_NearDistance = 100000;
            m_OuterDistance = 100000000;
*//*
            if (m_bDebug)
            {

                m_InnerDistance = 50000000;
                m_NearDistance = 50000000;


                m_OuterDistance = 50000000;
            }*/
            /*
            m_InnerDistance =  5;
            m_NearDistance =   5;
            m_OuterDistance = 10;*/
            /*
            m_LocationLatDisabled = 52.3619f;//52.379189f;//48.210033f;
            m_LocationLngDisabled = 4.84782f;//16.363449f;
            m_LocationLngDisabled = 4.84882f;//16.363449f;*/
        }

        /*  m_InnerDistance = 30;
            m_NearDistance = 100;
            m_OuterDistance = 500;
*/






        //======================

        m_bShowWelcome = false;
        m_WelcomeTimer = 0.0f;
        m_TextWelcome.SetActive(false);
        m_BackWelcome.SetActive(false);



        m_bCheckInternet = false;
        m_CheckInternetTimer = 0.0f;

        /*  m_MenuSelectionHello.SetActive (false);
            m_MenuSelectionHelloPortrait.SetActive (false);
    */
        m_bCameraHeadingSet = false;

        m_bLocationGPSDisabled = false;
        m_LocationGPSDisabledIter = 0;

        //m_PuzzlesInReachWait = 0;

        m_StrTextDebug = "";

        m_bNearPinSetup = false;

        m_TextDebug.SetActive(true);
        m_BackDebug.SetActive(true);

        textdebug = m_TextDebug.GetComponent<UnityEngine.UI.Text>();
        /*if (Application.systemLanguage == SystemLanguage.German) {
            textdebug.text = "GPS-Position wird ermittelt..";
        } else {
            textdebug.text = "Loading GPS location...";
        }*/
        textdebug.text = LocalizationSupport.GetString("LoadingGPS");

        m_BackDebug.GetComponent<Image>().color = new Color32(255, 255, 255, 240);//new Color32(255,255,255,240);







        /*      m_bSelectedPuzzle = false;
                m_bPuzzleInReach = false;
                m_PuzzleInReachMarker = null;*/

        /*m_ButtonPickPuzzle.SetActive (false);
        m_TextFoundPuzzle.SetActive (false);
        m_BackFoundPuzzle.SetActive (false);*/

        m_bLoadMarkers = 0;

        //PlayerPrefs.DeleteAll (); // Comment this out
        m_bLocationEnabled = false;



        if (PlayerPrefs.HasKey("NrQuestsDone"))
        {
            m_NrQuestsDone = PlayerPrefs.GetInt("NrQuestsDone");
        }
        else
        {
            m_NrQuestsDone = 0;
        }

        m_bAskSurvey = false;
        if (m_NrQuestsDone >= 5)
        {
            //if (PlayerPrefs.HasKey ("SurveyParkDone") == false) {
            if (PlayerPrefs.HasKey("ProfileSaved") == false && PlayerPrefs.HasKey("PlayerMail"))
            {
                m_bAskSurvey = true;
            }
        }


        //Debug.Log ("Quests made: " + m_NrQuestsDone);
        m_QuestsMade = new ArrayList();
        for (int i = 0; i < m_NrQuestsDone; i++)
        {
            string strdeleted = "Quest_" + i + "_Del";
            int deleted = 0;
            if (PlayerPrefs.HasKey(strdeleted))
            {
                deleted = PlayerPrefs.GetInt(strdeleted);
            }

            if (deleted == 0)
            {
                //string stralreadyuploadedquest = "Quest_" + i + "_Uploaded";
                //if (PlayerPrefs.HasKey (stralreadyuploadedquest) == false) {
                string questdone = PlayerPrefs.GetString("Quest_" + i + "_Id");
                m_QuestsMade.Add(questdone);

                //  Debug.Log ("Quest made: " + questdone);
            }
        }


        if (PlayerPrefs.HasKey("PlayerId"))
        {
            m_MenuSelectionButtonLogout.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Logout");//"Logout";
            m_IconLogin.SetActive(false);
            m_IconLogout.SetActive(true);
        }
        else
        {
            m_MenuSelectionButtonLogout.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("RegisterLogin");//"Register/Login";
            m_IconLogin.SetActive(true);
            m_IconLogout.SetActive(false);
        }
        m_TextLocationOutside.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("LoadingPoints");

        m_MenuSelectionButtonIntroduction.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("MenuIntroduction");//"Einleitung";
        m_MenuSelectionButtonIntroduction.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("MenuProfile");//"Einleitung";
        m_MenuSelectionButtonTerms.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("MenuTerms");//"Teilnahmebedingungen";
        m_MenuSelectionButtonChat.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("MenuNotifications");//"Benachrichtigungen";
        m_MenuSelectionButtonManual.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("MenuIntroduction");//"Anleitung";
        m_MenuSelectionButtonGuidelines.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("MenuOfflineMap");//"Richtlinien";
        m_MenuSelectionButtonContact.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("MenuImpressum");//"Impressum";
        m_MenuSelectionButtonHomepage.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("MenuContact");//"Kontakt";
        m_MenuSelectionButtonCancel.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("MenuClose");//"Schließen";
        m_MenuSelectionButtonClose.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("MenuClose");//"Schließen";


        //m_TextWelcome.GetComponent<UnityEngine.UI.Text> ().text = "Welcome to FotoQuest Go. There's a point right in your reach. Try to go there.";
        m_TextWelcome.GetComponent<UnityEngine.UI.Text>().text = "Welcome.";


        m_ButtonStartQuest.SetActive(false);
        //m_ButtonStartQuestHighlighted.SetActive (false);
       // m_ButtonPointNotReachable.SetActive(false);
        //m_ButtonNearlyStartQuest.SetActive (false);
        m_StartQuestBackground.SetActive(false);
        m_StartQuestBackgroundLine.SetActive(false);
        m_ButtonMoveCloser.SetActive(false);
        m_TextMoveCloser.SetActive(false);
        m_TextAlreadyRated.SetActive(false);


        m_bStartReadGPS = true;
        m_bDragging = false;

        Input.compass.enabled = true;
        Input.compensateSensors = true;

        Input.location.Start();

        /*  m_ButtonToPosBrightBottom.SetActive (true);
            m_ButtonToPosDarkBottom.SetActive (false);
            m_ButtonToPosBright.SetActive (false);
            m_ButtonToPosDark.SetActive (false);*/

        m_bBlackGui = true;
        OnlineMaps.instance.mapType = "google.satellite";
        m_bBlackGui = true;
        m_ButtonLayerBright.SetActive(false);
        m_ButtonLayerDark.SetActive(true);/*
        if (PlayerPrefs.HasKey("MapType"))
        {
            int maptype = PlayerPrefs.GetInt("MapType");
            if (maptype == 0)
            {
                m_bBlackGui = false;
                m_ButtonLayerBright.SetActive(true);
                m_ButtonLayerDark.SetActive(false);

                OnlineMaps.instance.mapType = "google.satellite";


                //  m_ButtonStartQuest.GetComponentInChildren<UnityEngine.UI.Text> ().color = new Color32 (40, 40, 40, 255);
                //  m_ButtonPointNotReachable.GetComponentInChildren<UnityEngine.UI.Text> ().color = new Color32 (40, 40, 40, 255);
            }
            else if (maptype == 1)
            {
                OnlineMaps.instance.mapType = "google.terrain";
                m_bBlackGui = true;
                m_ButtonLayerBright.SetActive(false);
                m_ButtonLayerDark.SetActive(true);

                //      m_ButtonStartQuest.GetComponentInChildren<UnityEngine.UI.Text> ().color = new Color32 (40, 40, 40, 255);
                //          m_ButtonPointNotReachable.GetComponentInChildren<UnityEngine.UI.Text> ().color = new Color32 (40, 40, 40, 255);
            }
            else
            {
                OnlineMaps.instance.mapType = "google.relief";

                m_bBlackGui = true;

                m_ButtonLayerBright.SetActive(false);
                m_ButtonLayerDark.SetActive(true);

                //  m_ButtonStartQuest.GetComponentInChildren<UnityEngine.UI.Text> ().color = new Color32 (120, 120, 120, 255);
                //      m_ButtonPointNotReachable.GetComponentInChildren<UnityEngine.UI.Text> ().color = new Color32 (120, 120, 120, 255);
            }
        }*/

        m_ButtonLayerBright.SetActive(false);
        m_ButtonLayerDark.SetActive(false);

        updateToPositionButtons(true);

        control = (OnlineMapsTileSetControl)OnlineMapsControlBase.instance;
        api = OnlineMaps.instance;

        //   Debug.Log("Start map");
        m_SelectedPin = new FotoQuestPin();
        m_SelectedPin.m_Marker = null;
        //        m_SelectedPin.m_Marker2 = null;
        m_bSelectedPin = false;

        m_Pins = new FotoQuestPin[2001];
        for (int i = 0; i < 2001; i++)
        {
            m_Pins[i] = new FotoQuestPin();
        }
        m_NrPins = 0;




        OnlineMapsControlBase3D control2 = GetComponent<OnlineMapsControlBase3D>();

        //  playerMarker3D = control2.AddMarker3D (Vector2.zero, m_PlanePosition);


        if (control2 == null)
        {
            Debug.Log("You must use the 3D control (Texture or Tileset).");
            return;
        }


        OnlineMapsControlBase.instance.allowUserControl = true;// false;//true;//false;
        OnlineMaps.instance.OnChangeZoom = OnChangeZoom;
        //  OnlineMaps.instance.OnChangePosition += OnMapDrag;
        toUserLocation();

        MarkerComparer pComparer = new MarkerComparer();
        MarkerComparer2 pComparer2 = new MarkerComparer2();
        pComparer.m_pDemoMap = this;
        pComparer2.m_pDemoMap = this;

        /*RectTransform rect = m_ButtonZoomIn.GetComponent<RectTransform> ();
        m_ButtonHeightPosition = rect.localPosition.y;
*/
        OnlineMapsTile.OnTileDownloaded = OnTileDownloaded;
        OnlineMaps.instance.OnStartDownloadTile = OnStartDownloadTile;


        //          OnlineMapsControlBase3D control2 = GetComponent<OnlineMapsControlBase3D>();
        control2.OnMapPress = OnMapPress;
        control2.OnMapRelease = OnMapRelease;
        control2.OnMapZoom = OnMapZoom;

        //----------------
        // Set 3d mode
        Camera cam = Camera.main;
        //  fromTransform = is2D ? camera3D : camera2D;
        //  toTransform = is2D ? camera2D : camera3D;
        cam.orthographic = false;
        //c.fieldOfView = 28;
        //---------------

        //updateProgress ();
        //loadProgress ();
        //loadPinsAlreadyDone ();

        //   showActivity(true, false);

#if UNITY_ANDROID
#if ASDFASDFASDF
        NativeEssentials.Instance.Initialize();
        PermissionsHelper.StatusResponse sr;
        PermissionsHelper.StatusResponse sr2;
        PermissionsHelper.StatusResponse sr3;// = PermissionsHelper.StatusResponse.;//NativeEssentials.Instance.GetAndroidPermissionStatus(PermissionsHelper.Permissions.CAMERA);
        sr =NativeEssentials.Instance.GetAndroidPermissionStatus(PermissionsHelper.Permissions.ACCESS_FINE_LOCATION);
        sr2 =NativeEssentials.Instance.GetAndroidPermissionStatus(PermissionsHelper.Permissions.ACCESS_COARSE_LOCATION);
        sr3 =NativeEssentials.Instance.GetAndroidPermissionStatus(PermissionsHelper.Permissions.CAMERA);
        if (sr == PermissionsHelper.StatusResponse.PERMISSION_RESPONSE_GRANTED && sr2 == PermissionsHelper.StatusResponse.PERMISSION_RESPONSE_GRANTED) {

        } else {
            if (sr == PermissionsHelper.StatusResponse.PERMISSION_RESPONSE_GRANTED && sr2 == PermissionsHelper.StatusResponse.PERMISSION_RESPONSE_GRANTED) {
            } else {
                NativeEssentials.Instance.RequestAndroidPermissions(new string[] {PermissionsHelper.Permissions.ACCESS_FINE_LOCATION, PermissionsHelper.Permissions.ACCESS_COARSE_LOCATION, PermissionsHelper.Permissions.CAMERA
                });
            }
        }
#endif
#endif

        createPins();
        loadSamplePoints(0);
    }


    // Line
    private bool m_bLineInited;
    private bool m_bLineVisible;
    public Material material;
    private Vector2[] coords;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh mesh;
    GameObject m_ContainerLine;

    public float m_SizeLine = 10;
    public Vector2 uvScale = new Vector2(2, 1);
    private float _size;

    void showLine()
    {
        if (!m_bLineInited)
        {
            initLine();
        }
        if (m_bLineVisible)
        {
            return;
        }
        m_ContainerLine.SetActive(true);
        m_bLineVisible = true;
    }

    void hideLine()
    {
        if (!m_bLineInited)
        {
            return;
        }
        if (!m_bLineVisible)
        {
            return;
        }

        m_ContainerLine.SetActive(false);
        m_bLineVisible = false;
    }

    void initLine()
    {
        OnlineMaps api = OnlineMaps.instance;


        // Create a new GameObject.
        GameObject container = new GameObject("Dotted Line");
        m_ContainerLine = container;

        // Create a new Mesh.
        meshFilter = container.AddComponent<MeshFilter>();
        meshRenderer = container.AddComponent<MeshRenderer>();

        mesh = meshFilter.sharedMesh = new Mesh();
        mesh.name = "Dotted Line";

        meshRenderer.material = material;
        material.renderQueue = 2950;

        // Init coordinates of points.
        coords = new Vector2[2];

        coords[0] = new Vector2(16.363449f, 48.210033f);
        coords[1] = new Vector2(16.353449f, 48.220033f);

        m_bLineInited = true;
        m_bLineVisible = true;
    }



    private void UpdateLine()
    {
        if (m_bLineInited == false)
        {
            return;
        }



        double tlx, tly, brx, bry;
        api.GetCorners(out tlx, out tly, out brx, out bry);


        double ttlx, ttly, tbrx, tbry;
        api.GetTileCorners(out ttlx, out ttly, out tbrx, out tbry, api.zoom);

        int zoom = api.zoom;

        _size = m_SizeLine;

        float totalDistance = 0;
        Vector3 lastPosition = Vector3.zero;

        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();
        List<int> triangles = new List<int>();

        //   m_LinePositions = null;
        //  m_LinePositions = new List<Vector3>();

        List<Vector3> positions = new List<Vector3>();
        /*
        Vector3 playerposition = OnlineMapsTileSetControl.instance.GetWorldPosition(m_PlayerPosition);
        float playerscreenposx = playerMarker3D.transform.position.x - playerposition.x;
        float playerscreenposy = playerMarker3D.transform.position.z - playerposition.z;
     //   Debug.Log("playerposx: " + playerposition.x + " y: " + playerposition.y + " z: " + playerposition.z);
     //   Debug.Log("playerscreenposx: " + playerscreenposx + " y: " + playerscreenposy);
*/
        for (int i = 0; i < coords.Length; i++)
        {
            // Get world position by coordinates
            //Vector3 position = OnlineMapsTileSetControl.instance.GetWorldPosition(coords[i]);
            double mx, my;
            api.projection.CoordinatesToTile(coords[i].x, coords[i].y, zoom, out mx, out my);

            int maxX = 1 << zoom;

            double sx = tbrx - ttlx;
            double mpx = mx - ttlx;
            if (sx < 0) sx += maxX;
            /*
            if (checkMapBoundaries)
            {
                if (mpx < 0) mpx += maxX;
                else if (mpx > maxX) mpx -= maxX;
            }
            else
            {*/
            double dx1 = Math.Abs(mpx - ttlx);
            double dx2 = Math.Abs(mpx - tbrx);
            double dx3 = Math.Abs(mpx - tbrx + maxX);
            if (dx1 > dx2 && dx1 > dx3) mpx += maxX;
            //  }

            double px = mpx / sx;
            double pz = (ttly - my) / (ttly - tbry);

            // _relativePosition = new Vector3((float)px, 0, (float)pz);

            OnlineMapsTileSetControl tsControl = control as OnlineMapsTileSetControl;

            //  if (tsControl != null)
            {
                px = -tsControl.sizeInScene.x / 2 - (px - 0.5) * tsControl.sizeInScene.x;
                pz = tsControl.sizeInScene.y / 2 + (pz - 0.5) * tsControl.sizeInScene.y;
            }
            Vector3 position = new Vector3((float)px, 0.0f, (float)pz);



            //Debug.Log("posx: " + playerMarker3D.transform.position.x + " y: " + playerMarker3D.transform.position.y + " z: " + playerMarker3D.transform.position.z);
            //  m_LinePositions.Add(position);
            positions.Add(position);

            if (i != 0)
            {
                // Calculate angle between coordinates.
                float a = OnlineMapsUtils.Angle2DRad(lastPosition, position, 90);

                // Calculate offset
                Vector3 off = new Vector3(Mathf.Cos(a) * m_SizeLine, 0, Mathf.Sin(a) * m_SizeLine);

                // Init verticles, normals and triangles.
                int vCount = vertices.Count;

                vertices.Add(lastPosition + off);
                vertices.Add(lastPosition - off);
                vertices.Add(position + off);
                vertices.Add(position - off);

                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);

                triangles.Add(vCount);
                triangles.Add(vCount + 3);
                triangles.Add(vCount + 1);
                triangles.Add(vCount);
                triangles.Add(vCount + 2);
                triangles.Add(vCount + 3);

                totalDistance += (lastPosition - position).magnitude;
            }

            lastPosition = position;
        }

        float tDistance = 0;

        for (int i = 1; i < positions.Count; i++)
        {
            float distance = (positions[i - 1] - positions[i]).magnitude;

            // Updates UV
            uvs.Add(new Vector2(0.0f, 0));
            uvs.Add(new Vector2(0.0f, 1));

            tDistance += distance;

            float proc = distance / 200.0f;//300.0f;// 200.0f;//500.0f;
            uvs.Add(new Vector2(proc, 0));
            uvs.Add(new Vector2(proc, 1));
        }

        // Update mesh
        mesh.vertices = vertices.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();

        // Scale texture
        Vector2 scale = new Vector2(totalDistance / m_SizeLine, 1);
        scale.Scale(uvScale);
        meshRenderer.material.mainTextureScale = scale;
    }



    private static string GetTilePath(OnlineMapsTile tile)
    {
        string[] parts =
        {
            Application.persistentDataPath,
            "OnlineMapsTileCache",
            tile.mapType.provider.id,
            tile.mapType.id,
            tile.zoom.ToString(),
            tile.x.ToString(),
            tile.y + ".png"
        };
        return string.Join("/", parts);
    }

    private void OnStartDownloadTile(OnlineMapsTile tile)
    {
        // Get local path.
        string path = GetTilePath(tile);

        //Debug.Log("OnStartDownloadTile: " + path);

        // If the tile is cached.
        if (File.Exists(path) /*&& false*/)
        {
            // Load tile texture from cache.
            Texture2D tileTexture = new Texture2D(256, 256);
            tileTexture.LoadImage(File.ReadAllBytes(path));
            tileTexture.wrapMode = TextureWrapMode.Clamp;

            // Send texture to map.
            if (OnlineMaps.instance.target == OnlineMapsTarget.texture)
            {
                tile.ApplyTexture(tileTexture);
                OnlineMaps.instance.buffer.ApplyTile(tile);
            }
            else
            {
                tile.texture = tileTexture;
                tile.status = OnlineMapsTileStatus.loaded;
            }

            // Redraw map.
            OnlineMaps.instance.Redraw();
        }
        else
        {
            // If the tile is not cached, download tile with a standard loader.
            OnlineMaps.instance.StartDownloadTile(tile);
        }
    }

    void OnTileDownloaded(OnlineMapsTile tile)
    {
        string path = GetTilePath(tile);

        //Debug.Log("OnTileDownloaded: " + path);

        // Cache tile.
        FileInfo fileInfo = new FileInfo(path);
        DirectoryInfo directory = fileInfo.Directory;
        if (!directory.Exists) directory.Create();

        File.WriteAllBytes(path, tile.www.bytes);
    }

    void toUserLocation()
    {
        // Debug.Log("Start location service");
        m_bLocationGPSDisabledReading = true;

        StartCoroutine(StartLocations());
    }



    private Vector2 m_PlayerPosition;
    bool m_bPlayerPositionRead = false;
    private void OnLocationChanged(Vector2 position)
    {
        // Change the position of the marker.
        m_PlayerPosition = position;
        m_bPlayerPositionRead = true;

        //-----------------------------
        // Update distance walked
        if (m_bSelectedPin)
        {
            float stepDistance = OnlineMapsUtils.DistanceBetweenPoints(m_PlayerPosition, m_DistanceWalkedLastCheckpoint).magnitude;
            stepDistance *= 1000.0f;
            if (stepDistance > 10.0f)
            {
                m_DistanceWalkedLastCheckpoint = position;
                m_DistanceWalked += stepDistance;

               /* if (m_bPathLoaded)
                { // Update line
                    OnlineMapsFindDirection.Find(new Vector2((float)m_SelectedPin.m_Lng, (float)m_SelectedPin.m_Lat),
                            new Vector2((float)m_PlayerPosition.x, (float)m_PlayerPosition.y
                            )).OnComplete += OnGoogleDirectionsComplete;
                }*/
            }
        }
        //-----------------------------

        if (m_bSelectedPin)
        {
            float distanceToLast = OnlineMapsUtils.DistanceBetweenPoints(m_PlayerPosition, m_PlayerLastPosition).magnitude;
            distanceToLast *= 1000.0f;
            if (distanceToLast > 100.0f)
            {
                m_PlayerLastPosition = position;
                if (m_NrPlayerPositions < 50)
                {
                    m_PlayerPositions[m_NrPlayerPositions] = m_PlayerPosition;
                    m_NrPlayerPositions++;
                }
            }
        }

        //if (!m_bIn2dMap) {
        /*if (m_bInReachOfPoint) {
            Vector2 pinpos;
            pinpos.x = (float)m_SelectedPin.m_Lng;
            pinpos.y = (float)m_SelectedPin.m_Lat;

            OnlineMaps.instance.position = pinpos;
        } else {
            OnlineMaps.instance.position = m_PlayerPosition;
        }*/

        if (m_bFollowingGPS)
        {
            OnlineMaps.instance.position = m_PlayerPosition;
        }

        OnlineMaps api = OnlineMaps.instance;
        if (playerMarker3D != null)
        {
            playerMarker3D.SetPosition(position.x, position.y);


            /*OnlineMaps map = OnlineMaps.instance;
            double tlx, tly, brx, bry;
            map.GetTopLeftPosition (out tlx, out tly);
            map.GetBottomRightPosition (out brx, out bry);

            playerMarker3D.Update (tlx, tly, brx, bry, map.zoom);*/
        }

        addLineToPin();
    }

    private void toPlayerPosition()
    {
        /*if (m_bInReachOfPoint) {
            Vector2 pinpos;
            pinpos.x = (float)m_SelectedPin.m_Lng;
            pinpos.y = (float)m_SelectedPin.m_Lat;

            OnlineMaps.instance.position = pinpos;
        } else {*/
        OnlineMaps.instance.position = m_PlayerPosition;
        //}
        /*
        OnlineMaps.instance.position = m_PlayerPosition;*/
        //  OnlineMaps.instance.zoom = 16;//7;

        //  updatePins ();
    }

    float playerrot = 0.0f;
    int m_ChangePositionIter = 0;
    bool m_bLocationEnabled;
    //float m_Walking = 0.0f;

    int m_LocationGPSDisabledIter = 0;

    bool m_bCameraHeadingSet;
    float m_CameraHeading;

    float m_CameraAngle = 90.0f;
    float m_CameraAngleMove = 90.0f;
    float m_CameraAngleTransition = 90.0f;
    float m_CameraPitch = 37.0f;
    float m_TouchPosX;
    float m_TouchPosY;

    int m_NotificationIter = 0;


    int m_frameCounter = 0;
    float m_timeCounter = 0.0f;
    float m_lastFramerate = 0.0f;
    public float m_refreshTime = 0.5f;

    bool m_bUpdatePins = true;
    public void shouldPinsBeUpdated()
    {
        m_bUpdatePins = !m_bUpdatePins;
        /*OnlineMapsControlBase3D control2 = GetComponent<OnlineMapsControlBase3D>();
            control2.setUpdateControl (m_bUpdatePins);*/
    }



    /*
    public void testRemoveAllPins()
    {
        OnlineMapsControlBase3D control2 = GetComponent<OnlineMapsControlBase3D>();
        control2.RemoveAllMarker3D ();
    }*/


    public GameObject m_TextFPS;
    public GameObject m_TextStatus;
    int m_NrItersUpdate = 0;
    int m_NrItersLoad = 0;
    bool m_bTouchMoved = false;
    bool m_bTouchMoving = false;
    float m_MoveOffsetTest = 0.0f;
    bool m_bForceUpdate = true;
    bool m_bInitCamera = true;
    float m_CurTouchMove = 0.0f;
    int m_FrameRateIter = 0;
    int m_FrameRate = 30;

#if DEBUGAPP
        int updateTool = 0;
        int updateText = 0;
        int updateRotation = 0;
#endif

    bool m_bZoom = false;
    float m_ZoomDistance = 0.0f;
    bool m_bHasZoomed = false;

    int m_AskSurvey = 0;
    bool m_bAskedSurvey = false;
    bool m_bAskSurvey = false;

    Vector3 m_MousePosition;
    float m_MouseDistance = 0.0f;
    private void Update()
    {
        // Debug.Log("posx: " + playerMarker3D.transform.position.x + " y: " + playerMarker3D.transform.position.y + " z: " + playerMarker3D.transform.position.z);
        UpdateLine();
#if DEBUGAPP
        // Stop time
        if( m_timeCounter < m_refreshTime )
        {
            m_timeCounter += Time.deltaTime;
            m_frameCounter++;
        }
        else
        {
            //This code will break if you set your m_refreshTime to 0, which makes no sense.
            m_lastFramerate = (float)m_frameCounter/m_timeCounter;
            m_frameCounter = 0;
            m_timeCounter = 0.0f;
            m_TextFPS.GetComponent<UnityEngine.UI.Text> ().text = "fps: " + m_lastFramerate;
        }
#endif


        //-----------------
        // Ask if wanting to do survey
        /*    if (m_bAskSurvey && !m_bAskedSurvey)
            {
                m_AskSurvey++;
                if (m_AskSurvey > 10)
                {
                    m_bAskedSurvey = true;
                    UnityEngine.Events.UnityAction<string> ua = new UnityEngine.Events.UnityAction<string>(OnMsgBoxSurveyClicked);
                    string[] options = { LocalizationSupport.GetString("QuestionProfileTimeNo"), LocalizationSupport.GetString("QuestionProfileTimeYes") };
                    messageBoxSmall.Show("", LocalizationSupport.GetString("QuestionProfileTime"), ua, options);
                }
            }*/
        //-----------------

        if (m_bIgnoreClick)
        {
            m_IgnoreClickIter++;
            if (m_IgnoreClickIter > 10)
            {
                m_bIgnoreClick = false;
            }
        }


        /* if (!m_bUpdatePins)
         {
             Debug.Log("Dont update pins");
             return;
         }*/
#if DEBUGAPP
        m_NrItersUpdate++;
#endif

        if (m_bPinInfoClosed)
        {
            m_PinInfoClosedIter++;
            if (m_PinInfoClosedIter > 3)
            {
                m_bPinInfoClosed = false;
                m_PinInfoClosedIter = 0;
            }
        }

        if(m_bAddingNewPoint) {
            m_AddingNewPointIter++;
        }


        // For test
        /*if (m_bSelectedPin) {
                System.DateTime date = System.DateTime.Parse (m_QuestSelectedTime);
                System.TimeSpan travelTime = System.DateTime.Now - date; 
            Debug.Log ("Time passed in seconds since selection: " + travelTime.Seconds);
        }*/

        if (Input.GetMouseButtonDown(0))
        {
            //   Debug.Log("#### CLICK #####");
            m_MousePosition = Input.mousePosition;
            m_MouseDistance = 0.0f;
        }
        if (Input.GetMouseButton(0))
        {
            float _x = Input.mousePosition.x - m_MousePosition.x;
            float _y = Input.mousePosition.y - m_MousePosition.y;
            m_MouseDistance += Mathf.Sqrt(_x * _x + _y * _y);
            m_MousePosition = Input.mousePosition;
        }

        if (m_bAddingNewPoint && m_bAddingNewPointChooseLocation && Input.GetMouseButtonUp(0))
        {
            //   Debug.Log("#### CLICK #####");
            Vector3 pos = Input.mousePosition;

            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                pointerId = -1,
            };

            pointerData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (m_MouseDistance < 30.0f)
            {
                bool bHitPin = false;
                if (results.Count > 0)
                {
                    //  Debug.Log("Nr results pick gui: " + results.Count);
                    //Debug.Log("pick gui element name: " + results[0].gameObject.name);
                }
                else
                {
                    //   Debug.Log("Mouse down -> check hit");
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        //     Debug.Log("Mouse Down Hit the following object: " + hit.collider.name);
                        if (hit.collider.name == "Map" || hit.collider.name.CompareTo("PlaneYellow(Clone)") == 0)
                        {
                            //         Debug.Log("> Clicked on map");
                        }
                        else
                        {
                            //        Debug.Log("> Clicked on marker");
                            bHitPin = true;
                        }
                    }
                }

                if (pos.x < Screen.width * 0.3f && pos.y < Screen.width * 0.3f)
                {
                    //      Debug.Log("touched corner");
                }
                else if (!bHitPin)
                {
                    AddNewPin(pos);
                }
            }
        }

        //===========================
        // Handling input
#if ASDFASDFASDF
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_bMenuOpened)
            {
                /*     SideMenu menu = (SideMenu)m_MenuSlidin.GetComponent(typeof(SideMenu));
                     menu.SlideOut();
                     m_MenuToggleButton.SetActive(false);
                     m_bMenuOpened = false;*/
            }
            else
            {
                //     Application.Quit();
            }/*
            if (m_bMenuSelectionOpen) {
                OnCloseMenuSelection ();
            } else if (m_bMenuOpen) {
                OnMenuCloseClicked ();
            } else {
                Application.Quit (); 
            }*/
        }
#endif

        if (Input.touchCount == 2 /*&& !m_bIn2dMap*/)
        {
            m_bHasZoomed = true;
            /*  float touchposx1 = Input.GetTouch (0).position.x;
                float touchposy1 = Input.GetTouch (0).position.y;


                float touchposx2 = Input.GetTouch (1).position.x;
                float touchposy2 = Input.GetTouch (1).position.y;

                float vecx = touchposx1 - touchposx2;
                float vecy = touchposy1 - touchposy2;
                float distance = Mathf.Sqrt (vecx * vecx + vecy * vecy);
                if (m_bZoom == false) {
                    m_ZoomDistance = distance;
                    m_bZoom = true;
                } else {
                    float distchange = distance - m_ZoomDistance;
                    if (distchange > m_ZoomDistance * 0.2f || distchange < m_ZoomDistance * -0.2f) {
                        if (distchange < 0.0f) {
                            zoomOut ();
                            m_ZoomDistance = distance;
                        } else {
                            zoomIn ();
                            m_ZoomDistance = distance;
                        }
                    }
                }
    */
            //m_TextInput.GetComponent<UnityEngine.UI.Text> ().text = "x1: " + touchposx1 + " y: " + touchposy1 + "\nx2: " + touchposx2 + " y2: " + touchposy2;
        }
        else
        {
            m_bZoom = false;
            m_ZoomDistance = 0.0f;
        }

        /*if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("###BUTTON UP: m_bTouchMoved: " + m_bTouchMoved + " haszoomed: " + m_bHasZoomed);
        }*/

        //  Debug.Log ("m_bTouchMoved: " + m_bTouchMoved + " haszoomed: " + m_bHasZoomed);
        if (Input.GetMouseButtonUp(0) && !m_bTouchMoved && !m_bHasZoomed && !m_bAddingNewPoint)
        {
            //     Debug.Log("#### CLICK #####");
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                pointerId = -1,
            };

            pointerData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                //       Debug.Log("Nr results pick gui: " + results.Count);
                //      Debug.Log("pick gui element name: " + results[0].gameObject.name);
            }
            else
            {
                //       Debug.Log("Mouse down -> check hit");
                //  RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit[] hits;
                hits = Physics.RaycastAll(ray);
                //      Debug.Log("nrhits: " + hits.Length);
                GameObject closest = null;
                float closestDist = 0.0f;
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider.name == "Map")
                    {
                        //         Debug.Log("> Clicked on map");
                        // deselectPoint();
                    }
                    else
                    {
                        //hits.collider.gameObject

                        Transform transform = hits[i].collider.gameObject.GetComponent<Transform>();
                        float distance = Vector3.Distance(transform.position, ray.origin);
                        //       Debug.Log("Cur distance: " + distance);

                        if (closest == null)
                        {
                            closest = hits[i].collider.gameObject;
                            closestDist = distance;
                        }
                        else if (distance < closestDist)
                        {
                            closest = hits[i].collider.gameObject;
                            closestDist = distance;
                        }
                    }
                }
                if (!m_bIgnoreClick)
                {
                    if (closest != null)
                    {
                        if (m_bMenuOpened)
                        {
                            ToggleMenu();
                        }
                        else
                        {
                            OnGOClick(closest);
                        }
                    }
                    else
                    {
                       
                        if (m_bMenuOpened)
                        {
                            ToggleMenu();
                        }
                        else
                        {
                            closePin();
                        }
                    }
                }
                /*Physics.RaycastAll()
                if (Physics.Raycast (ray, out hit)) {
                    Debug.Log ("Mouse Down Hit the following object: " + hit.collider.name);
                    if (hit.collider.name == "Map") {
                        Debug.Log ("> Clicked on map");
                    } else {
                        Debug.Log ("> Clicked on marker");
                        //hit.collider.gameObject
                        OnGOClick (hit.collider.gameObject);
                    }
                }*/
            }
        }

        if (Input.touchCount == 1)
        {
            // touch on screen
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                m_TouchPosX = Input.GetTouch(0).position.x;
                m_TouchPosY = Input.GetTouch(0).position.y;

                //   Debug.Log("Touch began");
                m_bTouchMoved = false;
                m_CurTouchMove = 0.0f;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                float difmovex = Input.GetTouch(0).position.x - m_TouchPosX;
                float curdifmovex = difmovex;
                if (curdifmovex < 0.0f)
                {
                    curdifmovex *= -1.0f;
                }
                m_CurTouchMove += curdifmovex;

                if (m_bFollowingGPS)
                {
                    m_CameraAngleMove = -m_CameraHeading + 90;//m_CameraHeading;
                }
                m_bFollowingGPS = false;
                if (m_CurTouchMove > 10.0f)
                {
                    m_bTouchMoved = true;
                }

                m_bTouchMoving = true;

                //  Debug.Log("Touch moved");


                if (!m_bIn2dMap)
                {
                    m_CameraAngleMove += difmovex * 0.3f;
                }

                if (m_CameraAngleMove > 360)
                {
                    m_CameraAngleMove -= 360;
                }
                else if (m_CameraAngleMove < 0)
                {
                    m_CameraAngleMove += 360;
                }


                /*float difmovey = Input.GetTouch (0).position.y - m_TouchPosY;
                m_CameraPitch -= difmovey * 0.3f;
                if (m_CameraPitch <= 37.0f) {
                    m_CameraPitch = 37.0f;
                } else if (m_CameraPitch >= 90.0f) {
                    m_CameraPitch = 90.0f;
                }*/

                m_TouchPosX = Input.GetTouch(0).position.x;
                m_TouchPosY = Input.GetTouch(0).position.y;
            }
        }
        else if (Input.touchCount <= 0)
        {
            m_bTouchMoving = false;
            m_bHasZoomed = false;

        }

        /*NSLog(@"
    if (Input.touchCount > 0 && m_bIn2dMap) {*/
        //  UpdateLine ();
        //      }

        /*
        if (m_bTo2dMap) {
            m_To2dMapTimer += 6.0f * Time.deltaTime;
            float proc = m_To2dMapTimer / 5.0f;
            if (proc > 1.0f) {
                proc = 1.0f;
                m_bTo2dMap = false;
                m_CameraPitch = 90.0f;
                m_bTouchMoving = true;
                Debug.Log ("to map 2d 1");
            } else {
                m_CameraPitch = 37.0f + (90.0f - 37.0f) * proc;
                    m_bTouchMoving = true;
                    Debug.Log ("to map 2d 2");
            }
            float dif = (90.0f - m_CameraAngleTransition);
            if (dif > 180)
                dif = 360 - dif;
            else if (dif < -180)
                dif = 360 + dif;
                    
            m_CameraAngleMove = dif * proc + m_CameraAngleTransition;
            //  m_CameraAngleMove = (90.0f - m_CameraAngleTransition) * proc + m_CameraAngleTransition;
            //  byte alpha = (byte)(238 * proc);
            //  m_DistanceBack2d.GetComponent<Image>().color = new Color32(68,154,231,alpha);
        } else if (m_bTo3dMap) {
            m_To3dMapTimer += 6.0f * Time.deltaTime;
            float proc = m_To3dMapTimer / 5.0f;
            if (proc > 1.0f) {
                m_bTo3dMap = false;
                m_CameraPitch = 37.0f;
                m_bTouchMoving = true;
                Debug.Log ("to map 3d 1");

                updatePins ();
                //Debug.Log ("Zoom: " + api.zoom);
                loadPins ();
            } else {
                m_CameraPitch = 90.0f + (37.0f - 90.0f) * proc;
                    m_bTouchMoving = true;
                    Debug.Log ("to map 3d 2");
            }
        }
*/





        //===========================
        // Update line

        // If size changed, then update line.
        /*if (m_SizeLine != _size) UpdateLine();
        addLineToPin (); */


        Camera c = Camera.main;



        //===========================
        // Check if map in Austria

        UnityEngine.UI.Text textdebug;
        /*if (m_bLocationEnabled) {
            if (OnlineMaps.instance.position.x >= 8.935 && OnlineMaps.instance.position.x <= 17.62669 &&
               OnlineMaps.instance.position.y >= 45.3919 && OnlineMaps.instance.position.y <= 49.9062) {
                m_BackLocationOutside.SetActive (false);
                m_TextLocationOutside.SetActive (false);
            } else {
                m_TextLocationOutside.SetActive (true);
                m_BackLocationOutside.SetActive (true);
            }
        }
*/

        //===========================
        // Location updates

     /*   bool bPositionChanged = false;
        {
            m_MoveOffsetTest += 0.00001f;
            Vector2 pos;
            pos.y = m_LocationLatDisabled + m_MoveOffsetTest;
            pos.x = m_LocationLngDisabled;// + m_Walking;
            OnLocationChanged(pos);
            OnChangePosition();
            bPositionChanged = true;
        }*/

//#if ASSADSASSDASSA
        bool bPositionChanged = false;
        m_ChangePositionIter++;
        if (m_ChangePositionIter > 20 && m_bLocationEnabled && !m_bDragging)
        {
            m_ChangePositionIter = 0;
            Vector2 pos;
            if (m_bLocationEnabled)
            {
                ///   m_MoveOffsetTest += 0.001f;
                pos.y = Input.location.lastData.latitude;// + m_MoveOffsetTest;// 30.0f;//48.210033f;
                pos.x = Input.location.lastData.longitude;//12.0f;//16.363449f;
                //hallihallo
           /*     m_MoveOffsetTest += 0.001f;
                pos.y = Input.location.lastData.latitude + m_MoveOffsetTest;// 30.0f;//48.210033f;
                pos.x = Input.location.lastData.longitude;//12.0f;//16.363449f;*/

                if (m_bPlayerPositionRead)
                {
                    float distanceDif = OnlineMapsUtils.DistanceBetweenPoints(m_PlayerPosition, pos).magnitude;
                    distanceDif *= 1000.0f;
                    if (distanceDif > 5.0f)
                    {
                        OnLocationChanged(pos);
                        OnChangePosition();
                        bPositionChanged = true;
                    }
                }
                else
                {
                    OnLocationChanged(pos);
                    OnChangePosition();
                    bPositionChanged = true;
                }
            }
            else
            {
              //  m_MoveOffsetTest += 0.001f;
                pos.y = m_LocationLatDisabled;
                pos.x = m_LocationLngDisabled;// + m_MoveOffsetTest;// + m_Walking;

                OnLocationChanged(pos);
            }
        }

        // Check if location service running/still running
        if (m_bLocationGPSDisabled /*&& false*/)
        {//TODO: Comment false out again
            m_LocationGPSDisabledIter++;
            if (m_LocationGPSDisabledIter > 100)
            {
                m_LocationGPSDisabledIter = 0;
                if (!m_bLocationGPSDisabledReading)
                {
                    toUserLocation();
                }
            }
        }
        else if (m_bLocationEnabled)
        {
            // If location has been enabled check that gps is still available
            if (!Input.location.isEnabledByUser || Input.location.status == LocationServiceStatus.Failed
                || Input.location.status != LocationServiceStatus.Running)
            {
                m_TextDebug.SetActive(true);
                m_BackDebug.SetActive(true);



                textdebug = m_TextDebug.GetComponent<UnityEngine.UI.Text>();

                if (!Input.location.isEnabledByUser)
                {
                    textdebug.text = LocalizationSupport.GetString("GPSGivePermission");/*
                    if (Application.systemLanguage == SystemLanguage.German) {
                        textdebug.text = "Bitte gib Urban Roots die Erlaubnis GPS benutzen zu dürfen.";
                    } else {
                        textdebug.text = "Please give Urban Roots the permission to use the location service.";
                    }*/
                }
                else
                {
                    textdebug.text = LocalizationSupport.GetString("GPSFailed");/*
                    if (Application.systemLanguage == SystemLanguage.German) {
                        textdebug.text = "Deine Position konnte nicht ermittelt werden.";
                    } else {
                        textdebug.text = "Your position could not be read.";
                    }*/
                }
                m_BackDebug.GetComponent<Image>().color = new Color32(255, 255, 255, 240);//new Color32(243,26,26,240);//new Color32(219,32,32,240);//Color32(255,20,20,240);
                m_bLocationGPSDisabled = true;
                m_bLocationGPSDisabledReading = false;
                m_bLocationEnabled = false;
            }
        }

//#endif


        if (m_bCheckInternet)
        {
            m_CheckInternetTimer += 50.0f;
            if (m_CheckInternetTimer > 10000.0f)
            {
                startCheckingInternet();
                m_CheckInternetTimer = 0.0f;
            }
        }



        //===========================
        // Making quest (show message how far away from point and show pin info)

        /*  if (m_bTouchMoving || m_bForceUpdate || m_bFollowingGPS || true)
          {
#if DEBUGAPP
              updateTool++;
#endif

              if (m_bSelectedPin)
              {
                  float proc = 1.0f;
                  bool bTooltipActive = true;
                  float duration = 10000000.0f; // 10000.0f
                  if (m_SelectedPinTimer < duration)
                  {
                  }
                  else
                  {
                      bTooltipActive = false;
                  }

                  if (bTooltipActive)
                  {
                      updatePinInfo();
                  }
              }
          }*/

        //===========================
        // Camera heading

        playerrot += 1.0f;
       /* float accx = Input.acceleration.x;
        float accy = Input.acceleration.y;
        float accz = Input.acceleration.z;
        float lenacc = Mathf.Sqrt(accx * accx + accy * accy + accz * accz);
        accx /= lenacc;
        accy /= lenacc;
        accz /= lenacc;
        float compx = Input.compass.rawVector.x;
        float compy = Input.compass.rawVector.y;
        float compz = Input.compass.rawVector.z;

        float lencomp = Mathf.Sqrt(compx * compx + compy * compy + compz * compz);
        compx /= lencomp;
        compy /= lencomp;
        compz /= lencomp;*/

        if (Input.acceleration.y < -0.75f)
        {
            playerrot = Mathf.Rad2Deg * Mathf.Atan2(Input.compass.rawVector.z, Input.compass.rawVector.x);
            playerrot += 90.0f;
            playerrot = 360 - playerrot;
        }
        else
        {
            playerrot = Input.compass.trueHeading;
        }

        if (playerMarker3D != null)
        {
            if (m_bCameraHeadingSet == false)
            {
                m_CameraHeading = playerrot;
                m_bCameraHeadingSet = true;
            }
            else
            {
                float dif = (playerrot - m_CameraHeading);
                if (dif > 180)
                {
                    dif -= 360.0f;
                }
                else if (dif < -180)
                {
                    dif += 360.0f;
                }

                m_CameraHeading += dif * 6.0f * Time.deltaTime;
                if (m_CameraHeading > 360)
                {
                    m_CameraHeading -= 360;
                }
                else if (m_CameraHeading < 0)
                {
                    m_CameraHeading += 360;
                }
            }



            Transform markerTransformPlayer = playerMarker3D.transform;
            if (markerTransformPlayer != null)
                markerTransformPlayer.rotation = Quaternion.Euler(0, m_CameraHeading, 0);//playerrot, 0);   
        }

        if (/*m_bTouchMoving || m_bFollowingGPS || */m_bInitCamera /*|| m_bForceUpdate*/)
        {
            m_bInitCamera = false;
#if DEBUGAPP
            updateRotation++;
#endif
           
            c.transform.position = new Vector3(-512, 512, 512);
        }

        //------------------------
        // Update text
        //Debug.Log ("### Update...");
        if (bPositionChanged || m_bForceUpdate)
        {
#if DEBUGAPP
            updateText++;
#endif

         //   Debug.Log("### Update text");
            m_bForceUpdate = false;
            RectTransform rect;

            bool bWasInReachOfPoint = m_bInReachOfPoint;
            m_bInReachOfPoint = false;
            if (m_bSelectedPin)
            {
             /*   if (m_bIn2dMap == false)
                {
                    m_DistanceBack.SetActive(true);
                    m_DistanceBackHorizon.SetActive(true);

                    //m_DistanceText.SetActive (true);
                }*/
                //  m_ButtonZoomIn.SetActive (false);
                //      m_ButtonZoomOut.SetActive (false);
                /*m_ButtonZoomInTop.SetActive (true);
                m_ButtonZoomOutTop.SetActive (true);*/

                /*if (m_bStartReadGPS || m_bFollowingGPS) {
                    m_ButtonFollowGPSTop.SetActive (false);
                } else {
                    m_ButtonFollowGPSTop.SetActive (true);
                }*/
                //  m_ButtonFollowGPS.SetActive (false);

                Vector2 pinpos;
                pinpos.x = (float)m_SelectedPin.m_Lng;
                pinpos.y = (float)m_SelectedPin.m_Lat;
                float stepDistance = OnlineMapsUtils.DistanceBetweenPoints(m_PlayerPosition, pinpos).magnitude;
                stepDistance *= 1000.0f;
                int meters = (int)stepDistance;

                UnityEngine.UI.Text text;
                UnityEngine.UI.Text text2;

                /*if (m_SelectedPin.m_bDone || m_SelectedPin.m_NrVisits > 0) {
                    m_ButtonShowPictures.SetActive (true);
                    m_ButtonShowPicturesLeft.SetActive (true);
                } else {*/
//                m_ButtonShowPictures.SetActive(false);
  //              m_ButtonShowPicturesLeft.SetActive(false);
                //}

                if (true /*m_SelectedPin.m_bDone == false && m_SelectedPin.m_NrVisits <= 0*/)
                {
                    /*text = m_DistanceText.GetComponent<UnityEngine.UI.Text> ();
                    text2 = m_DistanceText2.GetComponent<UnityEngine.UI.Text> ();*/

 //                   text = m_TooltipText.GetComponent<UnityEngine.UI.Text>(); // In IGN app show this info as pin info not in top bar
   //                 text2 = m_TooltipTextLeft.GetComponent<UnityEngine.UI.Text>();


                    bool bInReachOfPoint = false;


                    if (m_bPinAlreadyReviewed == false)
                    {
                        if (meters <= m_PinMinRadius)
                        {
                            m_ButtonStartQuest.SetActive(true);
                            m_ButtonStartQuest.GetComponent<Button>().interactable = true;

                          //  m_ButtonPointNotReachable.SetActive(true);

                            m_ButtonMoveCloser.SetActive(false);
                            m_TextMoveCloser.SetActive(false);
                            m_TextAlreadyRated.SetActive(false);

                            bInReachOfPoint = true;
                            m_bInReachOfPoint = true;
                            //m_bFollowingGPS = false; // Dont follow as soon as in reach of point
                            //m_ButtonFollowGPSTop.SetActive (false);

                            /*  if (!bWasInReachOfPoint && !m_bIn2dMap) { // Change location to pin if it wasnt at player position
                                    if (api.zoom < 19) {
                                        api.zoom = 19;
                                        enableZoomButtons ();
                                        updatePins ();
                                    }

                                    OnLocationChanged (m_PlayerPosition);
                                }*/
                            m_StartQuestBackground.SetActive(true);
                            m_StartQuestBackgroundLine.SetActive(true);
                            updateToPositionButtons(false);
                        }/*
                        else if (meters <= m_NearDistance)
                        {

                            m_ButtonStartQuest.SetActive(false);
                            m_ButtonMoveCloser.SetActive(true);
                            m_TextMoveCloser.SetActive(true);
                            m_TextAlreadyRated.SetActive(false);*/
                            /*
                            m_ButtonStartQuest.SetActive (true);
                            m_ButtonStartQuest.GetComponent<Button>().interactable = false;*/
                        /*
                         //   m_ButtonPointNotReachable.SetActive(true);

                            m_StartQuestBackground.SetActive(true);
                            m_StartQuestBackgroundLine.SetActive(true);
                            updateToPositionButtons(false);
                        }*/
                        else// if (meters <= m_OuterDistance)
                        {
                            //  m_ButtonNearlyStartQuest.SetActive (false);

                            m_ButtonStartQuest.SetActive(false);
                            m_ButtonMoveCloser.SetActive(true);
                            m_TextMoveCloser.SetActive(true);
                            m_TextAlreadyRated.SetActive(false);

                            /*m_ButtonStartQuest.SetActive (true);
                            m_ButtonStartQuest.GetComponent<Button>().interactable = false;*/

                            //  m_ButtonStartQuestHighlighted.SetActive (false);
                        //    m_ButtonPointNotReachable.SetActive(true);

                            m_StartQuestBackground.SetActive(true);
                            m_StartQuestBackgroundLine.SetActive(true);

                            updateToPositionButtons(false);
                        }
                    }
                    else
                    {

                        m_ButtonStartQuest.SetActive(false);
                        m_ButtonMoveCloser.SetActive(true);
                        m_TextMoveCloser.SetActive(false);
                        m_TextAlreadyRated.SetActive(true);

                     //   m_ButtonPointNotReachable.SetActive(true);

                        m_StartQuestBackground.SetActive(true);
                        m_StartQuestBackgroundLine.SetActive(true);

                        updateToPositionButtons(false);
                    }
                    /* else
                     {
                         m_ButtonStartQuest.SetActive(false);
                         m_ButtonMoveCloser.SetActive(false);
                         m_TextMoveCloser.SetActive(false);
                         //  m_ButtonStartQuestHighlighted.SetActive (false);
                         //      m_ButtonNearlyStartQuest.SetActive (false);
                      //   m_ButtonPointNotReachable.SetActive(false);

                         m_StartQuestBackground.SetActive(false);
                         m_StartQuestBackgroundLine.SetActive(false);

                         updateToPositionButtons(true);
                     }*/
                    
                    if (!bInReachOfPoint)
                    {

                        string move1 = LocalizationSupport.GetString("Move1");
                        string move2 = LocalizationSupport.GetString("Move2");
                     //   text.text = move1 + " " + meters + " " + move2;
                     //   text2.text = move1 + " " + meters + " " + move2;

                        /*if (Application.systemLanguage == SystemLanguage.German) {
                            text.text = "Bewege dich " + meters + " m näher zum Ziel...";
                            text2.text = "Bewege dich " + meters + " m näher zum Ziel...";
                        } else {
                            text.text = "Move " + meters + " m closer to the target...";
                            text2.text = "Move " + meters + " m closer to the target...";
                        }*/


                        m_TextMoveCloser.GetComponent<UnityEngine.UI.Text>().text = move1 + " " + meters + " " + move2;
                        //   m_TextStartQuest.GetComponent<UnityEngine.UI.Text> ().text = move1 + " " + meters + " " + move2;



//                        updatePinBackgrounds(false);
                    }
                    else
                    {
                        string atpoint = LocalizationSupport.GetString("AtPoint");
                    //    text.text = atpoint;
                     //   text2.text = atpoint;

                        // m_TextStartQuest.GetComponent<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString("StartQuest");

                        /*if (Application.systemLanguage == SystemLanguage.German) {
                        //  text.text = "Zielpunkt erreicht.\nDu kannst jetzt Bilder machen.";
                            //text.text = "Zielpunkt erreicht.\nDu kannst jetzt Bilder vom Punkt machen!";
                            text.text = "Super, du bist jetzt sehr nahe. GPS ist jetzt zu ungenau. Verwende deshalb nur die Karte als Orientierung um so nahe wie möglich an den Punkt zu gelangen.";
                            text2.text = "Super, du bist jetzt sehr nahe. GPS ist jetzt zu ungenau. Verwende deshalb nur die Karte als Orientierung um so nahe wie möglich an den Punkt zu gelangen.";
                        } else {
                    //      text.text = "Target point reached.\nYou can now take pictures.\nYou can now take pictures.";
                            //      text.text = "You are now very close to the point.\nLook at the map and try to get as close to the point as possible.\nThen start taking pictures.";
                            text.text = "Great, you are very close. Ignore your GPS location now and just use the map as guidance to get as close to the point as possible.";
                            text2.text = "Great, you are very close. Ignore your GPS location now and just use the map as guidance to get as close to the point as possible.";
                        }*/
//                        updatePinBackgrounds(true);
                    }
                }
                else
                {
                    // Quest has been already done
                   /* text = m_DistanceText.GetComponent<UnityEngine.UI.Text>();
                    text2 = m_DistanceText2.GetComponent<UnityEngine.UI.Text>();
                    string selectquest = LocalizationSupport.GetString("SelectQuest");
                    text.text = selectquest;
                    text2.text = selectquest;*/

                    /*
                    if (Application.systemLanguage == SystemLanguage.German ) {
                        text.text = "Wähle eine Quest aus!";
                        text2.text = "Wähle eine Quest aus!";
                    } else {
                        text.text = "Select a point to start your quest!";
                        text2.text = "Select a point to start your quest!";
                    }*/

                    m_ButtonStartQuest.SetActive(false);
                    //  m_ButtonStartQuestHighlighted.SetActive (false);
                    //      m_ButtonNearlyStartQuest.SetActive (false);
                //    m_ButtonPointNotReachable.SetActive(false);

                    m_StartQuestBackground.SetActive(false);
                    m_StartQuestBackgroundLine.SetActive(false);

                    updateToPositionButtons(true);
                }


                m_SelectedPinTimer += 1000.0f/*1600.0f */* Time.deltaTime;
                float proc = 1.0f;
                bool bTooltipActive = true;
                float duration = 10000000.0f; // 10000.0f
                if (m_SelectedPinTimer < duration /*+ 10000000.0f*/)
                {
                    proc = 1.0f;
                } /*else if (m_SelectedPinTimer < (duration + 300.0f)) {
                    proc = (m_SelectedPinTimer - duration) / (300.0f);
                proc = 1.0f - proc;
            } */else
                {
                    bTooltipActive = false;
                }


#if ASDFASDFASDFASDFASDFASDF
                if (bTooltipActive)
                {
                    text = m_TooltipText.GetComponent<UnityEngine.UI.Text> ();
                    text2 = m_TooltipTextLeft.GetComponent<UnityEngine.UI.Text> ();
                    if (m_SelectedPin.m_bDone == false) {
                        if (Application.systemLanguage == SystemLanguage.German) {
                            if (m_SelectedPin.m_NrVisits <= 0) {
                                //text.text = "Sei der erste der diesen Punkt besucht und hilf damit beim Umweltschutz.\n<i>Wert: " + m_SelectedPin.m_Weight + " Punkte</i>";
                        

                                float money = float.Parse (m_SelectedPin.m_Weight) / 100.0f;
                                string strtotal = money.ToString ("F2");//4F9F56FF//20A0DABF
                                text.text = LocalizationSupport.GetString("ToolTipBeFirst");//"Besuche als Erste(r) diesen Punkt!"; 
                                if (m_bInReachOfPoint) {
                                    text.text = LocalizationSupport.GetString("ToolTipInReach");//"Mache wenn du dich genau auf dem Punkt befindest die Bilder!";
                                }
                            } else {
                                string conqueredby = m_SelectedPin.m_Conquerer;
                                if (conqueredby.Length <= 0) {
                                    conqueredby = LocalizationSupport.GetString("ToolTipUnknown");//"Unbekannt";
                                }
                                //  text.text = "Bereits besucht von " + conqueredby + ".\nAnzahl Besucher: " + m_SelectedPin.m_NrVisits+ "\n<i>Wert: " + m_SelectedPin.m_Weight + " Punkte.</i>";
                                //      text.text = "Bereits besucht von " + conqueredby + ".\n<i>Bilder ansehen.</i>";
                            //  text.text = "Bereits besucht von " + conqueredby + ".\n";
                                //text.text = "Die Quest wurde bereits von " + conqueredby + " gemacht.";
                                text.text = LocalizationSupport.GetString("ToolTipAlreadyDone1") + " " + conqueredby + " " + LocalizationSupport.GetString("ToolTipAlreadyDone2");
                            }
                        } else {
                            if (m_SelectedPin.m_NrVisits <= 0) {
                                //text.text = "Be the first to photograph this point to help protect nature.\n<i>Score Points: " + m_SelectedPin.m_Weight + "</i>";
                                //text.text = "Be the first to visit this point to help protect nature.\n<i>Score Points: " + m_SelectedPin.m_Weight + "</i>";

                                float money = float.Parse (m_SelectedPin.m_Weight) / 100.0f;
                                string strtotal = money.ToString ("F2");//4F9F56FF//20A0DABF
                                text.text = LocalizationSupport.GetString("ToolTipBeFirst");//"Be the first to visit this point!";

                                if (m_bInReachOfPoint) {
                                    text.text = LocalizationSupport.GetString("ToolTipInReach");//"When you think you are exactly at the point, take the pictures!";
                                }

                            } else {
                                string conqueredby = m_SelectedPin.m_Conquerer;
                                if (conqueredby.Length <= 0) {
                                    conqueredby = LocalizationSupport.GetString("ToolTipUnknown");//"unknown";
                                }
                                /*text.text = "Quest already visited by " + conqueredby  +  ".\nNumber of visitors: " + m_SelectedPin.m_NrVisits + "\n<i>Score Points: " + m_SelectedPin.m_Weight + "</i>";
                                text.text = "Already visited by " + conqueredby  +  ".\n<color=#41A2F8FF><i>See pictures</i></color>";
                                text.text = "Already visited by " + conqueredby  +  ".\n<color=#41A2F8FF>See pictures</color>";
                            */
                            //  text.text = "Already visited by " + conqueredby + ".\n";
                                //text.text = "Quest already done by " + conqueredby + ".";
                                text.text = LocalizationSupport.GetString("ToolTipAlreadyDone1") + " " + conqueredby + " " + LocalizationSupport.GetString("ToolTipAlreadyDone2");

                            }
                        }
                    } else {

                        if (Application.systemLanguage == SystemLanguage.German) {
                                    string username = LocalizationSupport.GetString("ToolTipUnknown");//"Unbekannt";
                            if (PlayerPrefs.HasKey ("PlayerName")) {
                                username = PlayerPrefs.GetString ("PlayerName");
                            }

                            //  text.text = "Bereits besucht von " + username + ".\n<i>Bilder ansehen.</i>";
                        //  text.text = "Bereits besucht von " + username + ".\n";
                            //text.text = "Die Quest wurde bereits von " + username + " gemacht.";
                            text.text = LocalizationSupport.GetString("ToolTipAlreadyDone1") + " " + username + " " + LocalizationSupport.GetString("ToolTipAlreadyDone2");

                        } else {
                                    string username = LocalizationSupport.GetString("ToolTipUnknown");//"unknown";
                            if (PlayerPrefs.HasKey ("PlayerName")) {
                                username = PlayerPrefs.GetString ("PlayerName");
                            }

                            /*  text.text = "Already visited by " + username + ".\n<color=#41A2F8FF><i>See pictures.</i></color>";
                            text.text = "Already visited by " + username + ".\n<color=#41A2F8FF>See pictures</color>";
                        */
                                        //text.text = "Already visited by " + username + ".\n";
                        //  text.text = "Quest already done by " + username + ".";
                            text.text = LocalizationSupport.GetString("ToolTipAlreadyDone1") + " " + username + " " + LocalizationSupport.GetString("ToolTipAlreadyDone2");

                        }
                    }

                    text2.text = text.text;
                }
                else
                {
                    m_Tooltip.SetActive(false);
                    m_TooltipLeft.SetActive(false);
                }
#endif

            }
            else
            {
                /*if (!m_bIn2dMap) {
                    m_DistanceBack.SetActive (true);
                    m_DistanceBackHorizon.SetActive (true);
                    //m_DistanceText.SetActive (true);
                }*/

            /*    UnityEngine.UI.Text text;
                UnityEngine.UI.Text text2;
                text = m_DistanceText.GetComponent<UnityEngine.UI.Text>();
                text2 = m_DistanceText2.GetComponent<UnityEngine.UI.Text>();

                string selectquest = LocalizationSupport.GetString("SelectQuest");
                text.text = selectquest;
                text2.text = selectquest;*//*
                if (Application.systemLanguage == SystemLanguage.German) {
                    text.text = "Wähle eine Quest aus!";
                    text2.text = "Wähle eine Quest aus!";
                } else {
                    text.text = "Select a point to start your quest!";
                    text2.text = "Select a point to start your quest!";
                }*/

                /*float newpos = m_ButtonHeightPosition;

            m_ButtonZoomIn.SetActive (false);
            m_ButtonZoomOut.SetActive (false);*/
                /*m_ButtonZoomInTop.SetActive (true);
                m_ButtonZoomOutTop.SetActive (true);*/


//                m_Tooltip.SetActive(false);
  //              m_TooltipLeft.SetActive(false);


                /*

                if (m_bStartReadGPS || m_bFollowingGPS) {
                    m_ButtonFollowGPSTop.SetActive (false);
                } else {
                    m_ButtonFollowGPSTop.SetActive (true);
                }*/
                //  m_ButtonFollowGPS.SetActive (false);
            }
        }

        //if (m_bIn2dMap || m_bTo2dMap || m_bTo3dMap) {
        //UpdateLine ();
        //}

        if (m_bTouchMoving == false && Input.touchCount <= 0 && !m_bFollowingGPS)
        {
            if (m_FrameRate == 30)
            {
                m_FrameRateIter++;
                if (m_FrameRateIter > 400/*500*/)
                {
                    m_FrameRate = 10;
                    Application.targetFrameRate = 5;//5;
                }
            }

        }
        else
        {
            m_FrameRateIter = 0;
            if (m_FrameRate != 30)
            {
                m_FrameRate = 30;
                Application.targetFrameRate = 30;
            }
        }


#if DEBUGAPP
            m_TextStatus.GetComponent<UnityEngine.UI.Text> ().text = "m_bUpdatePins: " + m_bUpdatePins + " updateTool: " + updateTool + 
                " updateText: " + updateText + " updateRotation: " + updateRotation +
                " update: " + m_NrItersUpdate + " nrloads: " + m_NrItersLoad + " m_FrameRate: " + m_FrameRate + " m_FrameRateIter: " + m_FrameRateIter;
#endif
    }



    int m_CurrentPin = 0;
    int m_ReadingWhich = 0;
    int m_NearPinId = -1;
    int g_LoadMapsError = 0;
    int m_ReadingMainClass = -1;
    int m_ReadingSubClass = -1;
    int m_ReadingFeelingClass = -1;
    bool m_bReadingValue = false;
    bool m_bReadingSubValue = false;
    bool m_bReadingFeelingsValue = false;

   

    private bool pinAlreadyDone(string pinid)
    {
       /* if(PlayerPrefs.HasKey("PlayerId")) {
            string playerid = PlayerPrefs.GetString("PlayerId");
         //   Debug.Log("Playerid: " + playerid);
            if(PlayerPrefs.GetString("PlayerId") == creator) {
                return true;
            }
        }*/
        int nrquestsmade = m_QuestsMade.Count;
        //Debug.Log ("pinAlreadyDone nr: " + nrquestsmade);
        for (int i = 0; i < nrquestsmade; i++)
        {
            if (pinid.Equals(m_QuestsMade[i]))
            {
                //  Debug.Log ("Pin already done: " + pinid);
                return true;
            }

        }
        return false;
    }


    void clusterPins()
    {
      //  int nrpinsvisible = 0;
        float bordersize = Screen.width * 0.05f;
        float borderleft = -bordersize;
        float borderright = Screen.width + bordersize;
        float borderbottom = Screen.height + bordersize;
        // Calculate screen positions
        for (int i = 0; i < m_NrPins; i++)
        {
           /* if(m_FilterActivites != null) {
                bool bVisible = truefalse;

                // See if pin should be filtered
                for (int act = 0; act <m_Pins[i].m_PossibleAtivites.Count && !bVisible; act++) {
                    int curact = m_Pins[i].m_PossibleAtivites[act];

                    for (int check = 0; check < m_FilterActivites.Count && !bVisible; check++) {
                        if(m_FilterActivites[check] == curact) {
                            bVisible = true;
                        }
                    }
                }

                m_Pins[i].m_bVisible = bVisible;
                if(bVisible) {
                    Vector2 markerpos;
                    markerpos.x = (float)m_Pins[i].m_Lng;
                    markerpos.y = (float)m_Pins[i].m_Lat;
                    Vector2 screenPosition = OnlineMapsControlBase.instance.GetScreenPosition(markerpos);
                    m_Pins[i].m_ScreenPositionX = screenPosition.x;
                    m_Pins[i].m_ScreenPositionY = screenPosition.y;
                    m_Pins[i].m_Cluster = null;
                }
            } else {*/

                 Vector2 markerpos;
                markerpos.x = (float)m_Pins[i].m_Lng;
                markerpos.y = (float)m_Pins[i].m_Lat;
                Vector2 screenPosition = OnlineMapsControlBase.instance.GetScreenPosition(markerpos);
                m_Pins[i].m_ScreenPositionX = screenPosition.x;
                m_Pins[i].m_ScreenPositionY = screenPosition.y;
                m_Pins[i].m_Cluster = null;
           // }
            if(m_Pins[i].m_ScreenPositionX < borderleft || m_Pins[i].m_ScreenPositionX > borderright ||
               m_Pins[i].m_ScreenPositionY < borderleft || m_Pins[i].m_ScreenPositionY > borderbottom) {
                m_Pins[i].m_bVisible = false;
            } else {
                m_Pins[i].m_bVisible = true;//false;
              //  nrpinsvisible++;
            }


        }

      //  Debug.Log("Cluster pins nr visible: " + nrpinsvisible);


       /* if (api.zoom > 15)
        {
          //  return;
        }*/
        if (api.zoom > 17)
        {
              return;
        }

        // Merge clusters
        for (int i = 0; i < m_NrPins; i++)
        {
            //  if(m_Pins[i].m_Cluster == null) {
            for (int i2 = 0; i2 < m_NrPins; i2++)
            {
                if (i != i2)
                {
                    if (/*m_FilterActivites == null ||*/ (m_Pins[i].m_bVisible && m_Pins[i2].m_bVisible))
                    {
                        float vecx = m_Pins[i].m_ScreenPositionX - m_Pins[i2].m_ScreenPositionX;
                        float vecy = m_Pins[i].m_ScreenPositionY - m_Pins[i2].m_ScreenPositionY;
                        float dist = (float)Math.Sqrt(vecx * vecx + vecy * vecy);
                        if (dist < 100)// 50)// 100)// 40)
                        {
                            if (m_Pins[i].m_Cluster == null && m_Pins[i2].m_Cluster == null)
                            {
                                m_Pins[i].m_Cluster = new FotoQuestPinCluster();
                                m_Pins[i2].m_Cluster = m_Pins[i].m_Cluster;
                                m_Pins[i].m_Cluster.m_Childs.Add(m_Pins[i]);
                                m_Pins[i].m_Cluster.m_Childs.Add(m_Pins[i2]);
                            }
                            else if (m_Pins[i].m_Cluster == null && m_Pins[i2].m_Cluster != null)
                            {
                                m_Pins[i].m_Cluster = m_Pins[i2].m_Cluster;
                                m_Pins[i2].m_Cluster.m_Childs.Add(m_Pins[i]);
                            }
                            else if (m_Pins[i2].m_Cluster == null && m_Pins[i].m_Cluster != null)
                            {
                                m_Pins[i2].m_Cluster = m_Pins[i].m_Cluster;
                                m_Pins[i].m_Cluster.m_Childs.Add(m_Pins[i2]);
                            }
                            else if (m_Pins[i].m_Cluster != m_Pins[i2].m_Cluster)
                            {
                                FotoQuestPinCluster oldcluster = m_Pins[i2].m_Cluster;
                                for (int i3 = 0; i3 < m_NrPins; i3++)
                                {
                                    if (m_Pins[i3].m_Cluster == oldcluster)
                                    {
                                        m_Pins[i3].m_Cluster = m_Pins[i].m_Cluster;
                                        m_Pins[i3].m_Cluster.m_Childs.Add(m_Pins[i3]);
                                    }
                                }

                            }
                        }
                    }
                }
            }
            //  }
        }
    }

    private void addPins()
    {
        if (m_bAddingNewPoint)
        {
            Debug.Log("addPins -> m_bAddingNewPoint");
            return;
        }
        //  Debug.Log ("addPins nrpints: " + m_NrPins);

        //OnlineMaps api = OnlineMaps.instance;
        //OnlineMaps api = OnlineMaps.instance;
        //  Debug.Log ("mapx : " + api.bottomRightPosition.x + " y: " + api.bottomRightPosition.y + " x: " + api.topLeftPosition.x + " y: " + api.topLeftPosition.y);

        clusterPins();

        int nrpinsadded = 0;
        for (int i = 0; i < m_NrPins /*&& nrpinsadded < 300*/; i++)
        {
            if (/*m_FilterActivites == null || */m_Pins[i].m_bVisible)
            {
                if (m_Pins[i].m_Cluster != null)
                {
                    if (m_Pins[i].m_Cluster.m_Marker == null)
                    {
                        double lat = 0.0f;
                        double lng = 0.0f;
                        for (int i2 = 0; i2 < m_Pins[i].m_Cluster.m_Childs.Count; i2++)
                        {
                            lat += m_Pins[i].m_Cluster.m_Childs[i2].m_Lat;
                            lng += m_Pins[i].m_Cluster.m_Childs[i2].m_Lng;
                        }
                        lat /= (double)m_Pins[i].m_Cluster.m_Childs.Count;
                        lng /= (double)m_Pins[i].m_Cluster.m_Childs.Count;

                        OnlineMapsControlBase3D control2 = GetComponent<OnlineMapsControlBase3D>();

                        int nrpins = m_Pins[i].m_Cluster.m_Childs.Count;



                        OnlineMapsMarker3D dynamicMarker = control2.AddMarker3D_3(lng, lat, m_PinPlaneGreen, m_PinClusterText, nrpins);//m_PinPlaneYellow);

                        //dynamicMarker.OnClick = OnMarkerClick;
                        m_Pins[i].m_Cluster.m_Marker = dynamicMarker;
                    }
                }
                else
                {

                    if (/*m_Pins [i].m_NrVisits <= 0 && */pinAlreadyDone(m_Pins[i].m_Id))
                    {
                        // Add pin already done

                        //m_Pins [i].m_Marker = null;
                        Vector2 markerpos;
                        markerpos.x = (float)m_Pins[i].m_Lng;//pos.x + Random.Range (-0.1f, 0.1f);
                        markerpos.y = (float)m_Pins[i].m_Lat;//pos.y + Random.Range (-0.1f, 0.1f);


                        //  Vector2 screenPosition = OnlineMapsControlBase.instance.GetScreenPosition(markerpos);
                        // if (screenPosition.x >= 0 && screenPosition.x <= Screen.width && screenPosition.y >= 0 && screenPosition.y <= Screen.height)
                        {
                            OnlineMapsControlBase3D control2 = GetComponent<OnlineMapsControlBase3D>();
                            // OnlineMapsMarker3D dynamicMarker = control2.AddMarker3D(markerpos, m_PinSport/*m_PinPlane*/);//m_PinPlaneRed);//m_PinPlaneYellow);

                          /*  int highest = m_SelectedActivity;

                            if (highest == -1) highest = m_Pins[i].getHighestActivity();

                            OnlineMapsMarker3D dynamicMarker;
                            if (highest == 1)
                            {
                                dynamicMarker = control2.AddMarker3D_2(markerpos.x, markerpos.y, m_PinSitDone, m_PinCircle, m_Pins[i].m_ActivitySit, 1);//m_PinPlaneRed);
                            }
                            else if (highest == 2)
                            {
                                dynamicMarker = control2.AddMarker3D_2(markerpos.x, markerpos.y, m_PinPlayDone, m_PinCircle, m_Pins[i].m_ActivityPlay, 2);//m_PinPlaneRed);
                            }
                            else if (highest == 3)
                            {
                                dynamicMarker = control2.AddMarker3D_2(markerpos.x, markerpos.y, m_PinRomanticDone, m_PinCircle, m_Pins[i].m_ActivityRomantic, 3);//m_PinPlaneRed);
                            }
                            else if (highest == 4)
                            {
                                dynamicMarker = control2.AddMarker3D_2(markerpos.x, markerpos.y, m_PinSportDone, m_PinCircle, m_Pins[i].m_ActivitySport, 4);//m_PinPlaneRed);
                            }
                            else if (highest == 5)
                            {
                                dynamicMarker = control2.AddMarker3D_2(markerpos.x, markerpos.y, m_PinCreativeDone, m_PinCircle, m_Pins[i].m_ActivityCreative, 5);//m_PinPlaneRed);
                            }
                            else if (highest == 6)
                            {
                                dynamicMarker = control2.AddMarker3D_2(markerpos.x, markerpos.y, m_PinNatureDone, m_PinCircle, m_Pins[i].m_ActivityNature, 6);//m_PinPlaneRed);
                            }
                            else
                            {*/
                          //      dynamicMarker = control2.AddMarker3D_2(markerpos.x, markerpos.y, m_PinWinterDone, m_PinCircle, m_Pins[i].m_ActivityWinter, 7);//m_PinPlaneRed);
                         //   }
                            OnlineMapsMarker3D dynamicMarker;
                            dynamicMarker = control2.AddMarker3D(markerpos.x, markerpos.y, m_PinDone);
                        dynamicMarker.scale = 18.0f;//27.0f;//30.0f;//18.0f;
                            //dynamicMarker.OnClick = OnMarkerClick;
                            m_Pins[i].m_Marker = dynamicMarker;

                            //  Transform markerTransform = dynamicMarker.transform;
                        }
                    }
                    else // Add pin not already done
                    {
                        Vector2 markerpos;
                        markerpos.x = (float)m_Pins[i].m_Lng;//pos.x + Random.Range (-0.1f, 0.1f);
                        markerpos.y = (float)m_Pins[i].m_Lat;//pos.y + Random.Range (-0.1f, 0.1f);
                        {
                            nrpinsadded++;


                            OnlineMapsControlBase3D control2 = GetComponent<OnlineMapsControlBase3D>();


                            // OnlineMapsMarker3D dynamicMarker = control2.AddMarker3D(markerpos.x, markerpos.y, m_PinPlane);//m_PinPlaneRed);
                            // m_Pins[i].m_Marker = dynamicMarker;


                            OnlineMapsMarker3D dynamicMarker;

/*
                            int highest = m_SelectedActivity;

                            if (highest == -1) highest = m_Pins[i].getHighestActivity();

                            if (highest == 1)
                            {
                                dynamicMarker = control2.AddMarker3D_2(markerpos.x, markerpos.y, m_PinSit, m_PinCircle, m_Pins[i].m_ActivitySit, 1);//m_PinPlaneRed);
                            }
                            else if (highest == 2)
                            {
                                dynamicMarker = control2.AddMarker3D_2(markerpos.x, markerpos.y, m_PinPlay, m_PinCircle, m_Pins[i].m_ActivityPlay, 2);//m_PinPlaneRed);
                            }
                            else if (highest == 3)
                            {
                                dynamicMarker = control2.AddMarker3D_2(markerpos.x, markerpos.y, m_PinRomantic, m_PinCircle, m_Pins[i].m_ActivityRomantic, 3);//m_PinPlaneRed);
                            }
                            else if (highest == 4)
                            {
                                dynamicMarker = control2.AddMarker3D_2(markerpos.x, markerpos.y, m_PinSport, m_PinCircle, m_Pins[i].m_ActivitySport, 4);//m_PinPlaneRed);
                            }
                            else if (highest == 5)
                            {
                                dynamicMarker = control2.AddMarker3D_2(markerpos.x, markerpos.y, m_PinCreative, m_PinCircle, m_Pins[i].m_ActivityCreative, 5);//m_PinPlaneRed);
                            }
                            else if (highest == 6)
                            {
                                dynamicMarker = control2.AddMarker3D_2(markerpos.x, markerpos.y, m_PinNature, m_PinCircle, m_Pins[i].m_ActivityNature, 6);//m_PinPlaneRed);
                            }
                            else
                            {*/
                             //   dynamicMarker = control2.AddMarker3D_2(markerpos.x, markerpos.y, m_PinWinter, m_PinCircle, m_Pins[i].m_ActivityWinter, 7);//m_PinPlaneRed);
                           // }
                            dynamicMarker = control2.AddMarker3D(markerpos.x, markerpos.y, m_Pin);//m_PinPlaneRed);
                            dynamicMarker.scale = 18.0f;// 27.0f;//30.0f;//18.0f;
                            m_Pins[i].m_Marker = dynamicMarker;
                        }
                    }
                }
            }
        }



        //  updatePinOrientations ();

        addSelectedPinMarker();

    }

    public float m_PinOrientationX = 50;
    void updatePinOrientations()
    {
        return;
        /*float proc = (m_CameraPitch - 37.0f) / (90.0f - 37.0f);
        if (proc < 0.0f) {
            proc = 0.0f;
        } else if (proc > 1.0f) {
            proc = 1.0f;
        }

        float pinorienation = m_PinOrientationX * (1.0f - proc);*/
        float pinorientation = 0.0f;

        //Quaternion quatrot = Quaternion.Euler (pinorienation, -m_CameraAngle + 90, 0);

        Quaternion quatrot = Quaternion.Euler(0, 0, 0);
        for (int i = 0; i < m_NrPins; i++)
        {
            if (m_Pins[i].m_Marker != null)
            {
                Transform markerTransform = m_Pins[i].m_Marker.transform;
                if (markerTransform != null)
                    markerTransform.rotation = quatrot;//Quaternion.Euler (50, -m_CameraAngle + 90, 0);
            }
        }
        /*
    for (int i = 0; i < m_NrPinsDone; i++) {
        if (m_PinsDone [i].m_Marker != null) {
            Transform markerTransform = m_PinsDone [i].m_Marker.transform;
            if (markerTransform != null)
                markerTransform.rotation = quatrot;//Quaternion.Euler (50, -m_CameraAngle + 90, 0);
        }
    }*/


        if (m_SelectedPin.m_Marker != null)
        {
            Transform markerTransform = m_SelectedPin.m_Marker.transform;
            if (markerTransform != null)
                markerTransform.rotation = quatrot;//Quaternion.Euler (50, -m_CameraAngle + 90, 0);
        }


        if (m_NewPointMarker != null)
        {
            Transform markerTransform = m_NewPointMarker.transform;
            if (markerTransform != null)
                markerTransform.rotation = quatrot;
        }

    }


    private void OnGOClick(GameObject go)
    {
        Debug.Log("OnGOClick");
        if (go == null)
        {
        //    Debug.Log("On go clicked == null");
            return;
        }
        if (m_bAddingNewPoint)
        {
            return;
        }
        if (m_bIgnoreClick)
        {
   //         Debug.Log(">> On marker click activity just selected");
            return;
        }
        // Show in console marker label.
     //   Debug.Log(">> On GO clicked");
        bool bMarkerFound = false;

        if (m_SelectedPin.m_Marker != null && m_SelectedPin.m_Marker.instance == go)
        {
            m_SelectedPinTimer = 0.0f;
        }

        Debug.Log("OnGOClick2 nrpins: " + m_NrPins);

        for (int i = 0; i < m_NrPins && bMarkerFound == false; i++)
        {
            /*if (m_Pins [i].m_Marker == null) {
                Debug.Log ("Marker " + i + " = NULL!");
            }*/
            // Debug.Log("OnGOClick index: " + i);
            if (m_Pins[i].m_bVisible)
            {
                if (m_Pins[i].m_Cluster != null)
                {
                    if (m_Pins[i].m_Cluster != null && m_Pins[i].m_Cluster.m_Marker.instance == go)
                    {
                        Debug.Log("OnGOClick 33");
                        bMarkerFound = true;
                        //       Debug.Log("CLUSTER clicked");
                        api.zoom++;
                        Vector2 newpos = new Vector2();

                        double lat = 0.0f;
                        double lng = 0.0f;
                        for (int i2 = 0; i2 < m_Pins[i].m_Cluster.m_Childs.Count; i2++)
                        {
                            lat += m_Pins[i].m_Cluster.m_Childs[i2].m_Lat;
                            lng += m_Pins[i].m_Cluster.m_Childs[i2].m_Lng;
                        }
                        lat /= (double)m_Pins[i].m_Cluster.m_Childs.Count;
                        lng /= (double)m_Pins[i].m_Cluster.m_Childs.Count;

                        m_bFollowingGPS = false;
                        newpos.x = (float)lng;
                        newpos.y = (float)lat;
                        api.position = newpos;
                        return;
                    }
                }
                else if (m_Pins[i].m_Marker != null && m_Pins[i].m_Marker.instance == go /*&& !pinAlreadyDone(m_Pins[i].m_Id)*/)
                {

                    Debug.Log("OnGOClick 3");
                    bMarkerFound = true;
                    //   Debug.Log (">>># Pin selected: " + m_Pins [i].m_Id);

                    if (!pinAlreadyDone(m_Pins[i].m_Id))
                    {
                        m_bPinAlreadyReviewed = false;
                    }
                    else
                    {
                        m_bPinAlreadyReviewed = true;
                    }

                    //  m_bSelectedPuzzle = false;
                    m_bSelectedPin = true;
                    m_SelectedPinTimer = 0.0f;
                    m_SelectedPin.m_Id = m_Pins[i].m_Id;
                    m_SelectedPin.m_Lat = m_Pins[i].m_Lat;
                    m_SelectedPin.m_Lng = m_Pins[i].m_Lng;
                    //   m_SelectedPin.m_Color = m_Pins[i].m_Color;
                    //    m_SelectedPin.m_Weight = m_Pins[i].m_Weight;
                    m_SelectedPin.m_NrVisits = m_Pins[i].m_NrVisits;
                    m_SelectedPin.m_ValidationId = m_Pins[i].m_ValidationId;
                    //      m_SelectedPin.m_Conquerer = m_Pins[i].m_Conquerer;
                    m_SelectedPin.m_bDone = false;
                    m_PinMinRadius = 100.0f;
                    int distance = PlayerPrefs.GetInt("SessionSettingsDistance_" + m_Pins[i].m_ValidationId);
                    Debug.Log("PinMinRadius = " + distance);
                    if(distance > 10) {
                        m_PinMinRadius = distance;
                    }
                if(m_bDebug) {
                        m_PinMinRadius = 10000000;
                }

                    m_bConquerShiftSet = false;

                    bool bWasInReachOfPoint = m_bInReachOfPoint;
                    m_bInReachOfPoint = false;

                    m_QuestSelectedTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    m_PlayerPositionStart.x = m_PlayerPosition.x;
                    m_PlayerPositionStart.y = m_PlayerPosition.y;
                    m_PlayerLastPosition.x = m_PlayerPosition.x;
                    m_PlayerLastPosition.y = m_PlayerPosition.y;
                    m_NrPlayerPositions = 0;


                    m_DistanceWalkedLastCheckpoint.x = m_PlayerPosition.x;
                    m_DistanceWalkedLastCheckpoint.y = m_PlayerPosition.y;
                    m_DistanceWalked = 0.0f;

                    /*if (m_SelectedPin.m_bDone || m_SelectedPin.m_NrVisits > 0) {
                        m_ButtonShowPictures.SetActive (true);
                        m_ButtonShowPicturesLeft.SetActive (true);
                    } else {*/
                    //                m_ButtonShowPictures.SetActive(false);
                    //              m_ButtonShowPicturesLeft.SetActive(false);
                    //}

                    m_bForceUpdate = true;

                    //   showActivity(false, true);



                    //updatePinInfo();
                    m_bPathLoaded = false;

                  /*  OnlineMapsFindDirection.Find(new Vector2((float)m_Pins[i].m_Lng, (float)m_Pins[i].m_Lat),
                        new Vector2((float)m_PlayerPosition.x, (float)m_PlayerPosition.y
                        )).OnComplete += OnGoogleDirectionsComplete;*/

                    /*if(m_SelectedPin.m_NrVisits <= 0) { // Change zoom
                        Vector2 pinpos;
                        pinpos.x = (float)m_SelectedPin.m_Lng;
                        pinpos.y = (float)m_SelectedPin.m_Lat;
                        float stepDistance = OnlineMapsUtils.DistanceBetweenPoints(m_PlayerPosition, pinpos).magnitude;
                        stepDistance *= 1000.0f;
                        int meters = (int)stepDistance;

                        if (meters > m_InnerDistance && !m_bIn2dMap) {
                            if (meters >= 2000.0f) {
                            } else if (meters <= 2000.0f) {
                                if (api.zoom < 13) { // Zoom in but don't zoom out
                                    api.zoom = 13;
                                    enableZoomButtons ();
                                    updatePins ();
                                }
                            } else if (meters > 500.0f) {
                                if (api.zoom < 14) { // Zoom in but don't zoom out
                                    api.zoom = 14;
                                    enableZoomButtons ();
                                    updatePins ();
                                }
                            } else if (meters > 400.0f) {
                                if (api.zoom < 15) { // Zoom in but don't zoom out
                                    api.zoom = 15;
                                    enableZoomButtons ();
                                    updatePins ();
                                }
                            } else if (meters > 250.0f) {
                                if (api.zoom < 16) { // Zoom in but don't zoom out
                                    api.zoom = 16;
                                    enableZoomButtons ();
                                    updatePins ();
                                }
                            } else if (meters > 100.0f) {
                                if (api.zoom < 17) { // Zoom in but don't zoom out
                                    api.zoom = 17;
                                    enableZoomButtons ();
                                    updatePins ();
                                }
                            } else {
                                if (api.zoom < 18) { // Zoom in but don't zoom out
                                    api.zoom = 18;
                                    enableZoomButtons ();
                                    updatePins ();
                                }
                            }
                        }

                        if (meters <= m_InnerDistance) {
                            m_bInReachOfPoint = true;   
                            m_bFollowingGPS = false; // Dont follow as soon as in reach of point
                        //  m_ButtonFollowGPSTop.SetActive (false);

                            if (!bWasInReachOfPoint && !m_bIn2dMap) {
                                if(api.zoom < 19) {
                                    api.zoom = 19;
                                    enableZoomButtons ();
                                    updatePins ();
                                }
                            }
                        }
                        // Change position -> camera focus might be on pin now
                        OnLocationChanged (m_PlayerPosition);
                    }*/

                    Debug.Log("OnGOClick5");
                }
            }
        }


        Debug.Log("OnGOClick End");
        removePins();
        addPins();
    }


    bool m_bPathLoaded = false;
    private Vector2[] m_PathCoords;

    private void OnGoogleDirectionsComplete(string response)
    {
     //   Debug.Log("****** ONGOOGLEDIRECTIONSCOMPLETE ******");
      //  Debug.Log(response);

        m_bPathLoaded = false;
        //TextAsset textAsset = (TextAsset)Resources.Load(response);
        XmlDocument xmldoc = new XmlDocument();
        try
        {
            xmldoc.LoadXml(response);
            //xmldoc.LoadXml(textAsset.text);


            List<Vector2> coords = new List<Vector2>();

            bool bFirst = true;
            XmlNodeList steps = xmldoc.GetElementsByTagName("step");//.Item(0).ChildNodes;
      //      Debug.Log("Google direction. Nr steps: " + steps.Count);
            if (steps.Count < 500)
            {
                foreach (XmlNode step in steps)
                {
                    //  Debug.Log ("Node: " + step.Name);
                    XmlNodeList childs = step.ChildNodes;
                    foreach (XmlNode child in childs)
                    {
                        //   Debug.Log("Child: " + child.Name);
                        if (child.Name.CompareTo("start_location") == 0)
                        {
                            //      Debug.Log ("> startlocation");
                            XmlNodeList coordinate = child.ChildNodes;
                            //      Debug.Log ("Lat: " + coordinate.Item (0).InnerText);
                            //  Debug.Log ("Lng: " + coordinate.Item (1).InnerText);

                            if (bFirst)
                            {
                                coords.Add(new Vector2(float.Parse(coordinate.Item(1).InnerText), float.Parse(coordinate.Item(0).InnerText)));
                            }
                            bFirst = false;
                        }
                        else if (child.Name.CompareTo("end_location") == 0)
                        {
                            //  Debug.Log ("> endlocation");
                            XmlNodeList coordinate = child.ChildNodes;
                            //  Debug.Log ("Lat: " + coordinate.Item (0).InnerText);
                            //      Debug.Log ("Lng: " + coordinate.Item (1).InnerText);

                            coords.Add(new Vector2(float.Parse(coordinate.Item(1).InnerText), float.Parse(coordinate.Item(0).InnerText)));
                        }
                    }
                }

                if (coords.Count > 0)
                {
                 //   Debug.Log("Path created with " + coords.Count + " coordinates");

                    m_PathCoords = new Vector2[coords.Count];
                    for (int i = 0; i < coords.Count; i++)
                    {
                        m_PathCoords[i] = coords[i];
                    }
                    m_bPathLoaded = true;
                }
            }
            else
            {
                m_bPathLoaded = false;
            }
        }
        catch (Exception)
        {
            throw new Exception("Could not load directions");
        }
        addLineToPin();
    }

    private void OnMarkerClick(OnlineMapsMarkerBase marker)
    {
        return;
    }

    private void OnMarkerClickDone(OnlineMapsMarkerBase marker)
    {
        return;
    }

    public void addSelectedPinMarker()
    {
        if (m_bSelectedPin == false)
        {
            return;
        }
        addLineToPin();
        return;
        /*
        OnlineMapsControlBase3D control2 = GetComponent<OnlineMapsControlBase3D>();

        if (m_SelectedPin.m_Marker != null)
        {
            OnlineMaps api = OnlineMaps.instance;
            control2.RemoveMarker3D(m_SelectedPin.m_Marker);
            //          api.RemoveMarker (m_SelectedPin.m_Marker);
            m_SelectedPin.m_Marker = null;
        }
        if (m_bSelectedPin == false)
        {
            return;
        }
        Vector2 markerpos;
        markerpos.x = (float)m_SelectedPin.m_Lng;//pos.x + Random.Range (-0.1f, 0.1f);
        markerpos.y = (float)m_SelectedPin.m_Lat;//pos.y + Random.Range (-0.1f, 0.1f);



        GameObject copypin = (GameObject)GameObject.Instantiate(m_PinPlane);
        GameObject pincircle = (GameObject)GameObject.Instantiate(m_PinCircle);
        Transform transform = pincircle.GetComponent<Transform>();
        transform.parent = copypin.GetComponent<Transform>();

        pincircle.GetComponent<Renderer>().material.color = new Color(117.0f / 255.0f, 150.0f / 255.0f, 124.0f / 255.0f, 1.0f);

        float valuecircle = UnityEngine.Random.Range(0.0f, 1.0f) * (float)Math.PI * 2.0f;
        pincircle.GetComponent<Renderer>().material.SetFloat("_Circle", valuecircle);


        OnlineMapsMarker3D dynamicMarker = control2.AddMarker3D(markerpos, copypin);//m_PinPlaneYellow);

        dynamicMarker.scale = 18.0f;
        dynamicMarker.OnClick = OnMarkerClick;
        m_SelectedPin.m_Marker = dynamicMarker;

        GameObject.Destroy(copypin);
        GameObject.Destroy(pincircle);*/

        /*if (m_SelectedPin.m_bDone) {
            OnlineMapsMarker3D dynamicMarker = control2.AddMarker3D (markerpos, m_PinPlaneGreenSelected);
            m_SelectedPin.m_Marker = dynamicMarker;
        } else if (m_SelectedPin.m_Color == "yellow") {
            OnlineMapsMarker3D dynamicMarker = control2.AddMarker3D (markerpos, m_PinPlaneGreenSelected);// m_PinPlaneYellow);
            m_SelectedPin.m_Marker = dynamicMarker;
        } else if (m_SelectedPin.m_Color == "red") {
            OnlineMapsMarker3D dynamicMarker = control2.AddMarker3D (markerpos, m_PinPlaneGreenSelected);// m_PinPlaneRed);
            m_SelectedPin.m_Marker = dynamicMarker;
        } else {*/
        /*  if (m_SelectedPin.m_NrVisits > 0) {
                OnlineMapsMarker3D dynamicMarker = control2.AddMarker3D (markerpos, m_PinPlaneGreen);
                m_SelectedPin.m_Marker = dynamicMarker;
            } else {*//*
            //  OnlineMapsMarker3D dynamicMarker = control2.AddMarker3D (markerpos, m_PinPlaneSelected);
                OnlineMapsMarker3D dynamicMarker = control2.AddMarker3D (markerpos, m_PinPlane);
                m_SelectedPin.m_Marker = dynamicMarker;*/
                      //}
                      //    }

        //  updatePinOrientations ();
      //  addLineToPin();
    }

    public void addLineToPin()
    {
        OnlineMaps api = OnlineMaps.instance;


        /*if (m_bSelectedPuzzle) {
            return;
        }*/
        if (m_bSelectedPin == false /*|| m_SelectedPin.m_bDone || m_SelectedPin.m_NrVisits > 0*/)
        {
            /*if (m_LineToPin != null) {
                api.RemoveDrawingElement (m_LineToPin);
                m_LineToPin = null;
            }*/
            if (m_bLineInited && m_bLineVisible)
            {
                hideLine();
            }
            return;
        }


       /* List<Vector2> points;
        Vector2 pos;
        Vector2 targpos;
        Color color;

        points = new List<Vector2>();
        pos.x = m_PlayerPosition.x;
        pos.y = m_PlayerPosition.y;
        points.Add(pos);

        targpos.x = (float)m_SelectedPin.m_Lng;
        targpos.y = (float)m_SelectedPin.m_Lat;
        points.Add(targpos);*/


/*        color.r = 1.0f;
        color.g = 1.0f;
        color.b = 1.0f;
        color.a = 1.0f;*/

        if (!m_bLineInited)
        {
            initLine();
        }
        else if (!m_bLineVisible)
        {
            showLine();
        }

        if (m_bPathLoaded == false)
        {
            coords = new Vector2[2];
            coords[0] = new Vector2(m_PlayerPosition.x, m_PlayerPosition.y);//new Vector2(48.210033f, 16.363449f);//new Vector2();
            coords[1] = new Vector2((float)m_SelectedPin.m_Lng, (float)m_SelectedPin.m_Lat);
        }
        else
        {
            coords = m_PathCoords;
        }
        UpdateLine();
    }

    bool m_bLastPositionSet = false;
    float m_SelectedPinTimer = 0.0f;
    int m_LastZoom;
    Vector2 m_LastPosition;

    float m_MapPressX;
    float m_MapPressY;
    bool m_bDragging = false;
    private void OnMapPress()
    {
        m_bDragging = true;
      //  Debug.Log("OnMapPress");

        if (m_bIn2dMap)
        {
            /*          OnlineMapsControlBase3D control2 = GetComponent<OnlineMapsControlBase3D> ();
                        control2.setUpdateControl (true);
                        control2.setAlwaysUpdateControl (true);*/
        }

        /*m_MapPressX = OnlineMaps.instance.position.x;
        m_MapPressY = OnlineMaps.instance.position.y;*/
    }

    private void OnMapRelease()
    {
        m_bDragging = false;
     //   Debug.Log("OnMapReleased");

        if (m_bIn2dMap)
        {
            /*  OnlineMapsControlBase3D control2 = GetComponent<OnlineMapsControlBase3D> ();
                control2.setUpdateControl (true);
                control2.setAlwaysUpdateControl (false);*/

            OnChangePosition();
        }

        /*float difx = OnlineMaps.instance.position.x - m_MapPressX;
        float dify = OnlineMaps.instance.position.y - m_MapPressY;
        if (difx < 0.0f)
            difx *= -1.0f;
        if (dify < 0.0f)
            dify *= -1.0f;


        if (difx > 0.0001f || dify > 0.0001f) {
            
            Debug.Log ("OnMapReleased -> moved");
                
            if (m_bStartReadGPS) {
                return;
            }
            Debug.Log ("OnMapReleased -> Start follow gps: " + m_bStartReadGPS);
            m_bFollowingGPS = false;
        }*/
    }


    private void OnMapZoom()
    {
        m_bForceUpdate = true;

        //closePinInfo ();
        enableZoomButtons();

        showPins();
    }

    private void OnChangePosition()
    {
        if (m_bLastPositionSet == false)
        {
            m_LastPosition = OnlineMaps.instance.position;
            m_LastZoom = OnlineMaps.instance.zoom;
            m_bLastPositionSet = true;
           // loadPins();
            showPins();
        }
        else /*if(!m_bIn2dMap)*/
        {
            // Load pins when map position has changed
            float difx = OnlineMaps.instance.position.x - m_LastPosition.x;
            float dify = OnlineMaps.instance.position.y - m_LastPosition.y;

            if (difx < 0.0f)
                difx *= -1.0f;
            if (dify < 0.0f)
                dify *= -1.0f;

            Vector2 topleft = OnlineMaps.instance.topLeftPosition;
            Vector2 bottomright = OnlineMaps.instance.bottomRightPosition;
            /*  Debug.Log ("tl x: " + topleft.x + " tly: " + topleft.y + " brx: " + bottomright.x + " bry: " + bottomright.y);
                Debug.Log ("m_LastPosition x: " + m_LastPosition.x + " y: " + m_LastPosition.y);
                Debug.Log ("Map change pos difx: " + difx + " dify: " + dify);
    */
            float mapwidth = topleft.x - bottomright.x;
            float mapheight = topleft.y - bottomright.y;
            if (mapwidth < 0.0f)
                mapwidth *= -1.0f;
            if (mapheight < 0.0f)
                mapheight *= -1.0f;

            float maxdifx = mapwidth * 0.05f;
            float maxdify = mapheight * 0.05f;

            if (difx > maxdifx || dify > maxdify || OnlineMaps.instance.zoom != m_LastZoom)
            {
                m_LastPosition = OnlineMaps.instance.position;
                m_LastZoom = OnlineMaps.instance.zoom;
                m_bLastPositionSet = true;
            //    loadPins();
                showPins();
            }
        }

        //UpdateLine ();

    }

    bool m_bLocationGPSDisabled;
    bool m_bLocationGPSDisabledReading;

    private void removePins()
    {
     //   Debug.Log("removePins");

        OnlineMaps api = OnlineMaps.instance;

        //if (m_bLoadMarkers == 0) {
        api.RemoveAllMarkers();
        //  }
        //      if (m_bLoadMarkers == 2) {
        api.RemoveAllDrawingElements();
        OnlineMapsControlBase3D control2 = GetComponent<OnlineMapsControlBase3D>();
        if (control2 != null)
        {
       //     Debug.Log("remove 3d markers");
            control2.RemoveAllMarker3D();
        }

        //m_LineToPin = null;
        m_SelectedPin.m_Marker = null;

        if (m_bLineInited && m_bLineVisible)
        {
            hideLine();
        }

        playerMarker3D = control2.AddMarker3D(m_PlayerPosition, m_PlanePosition);

        if (playerMarker3D != null)
        {
            Transform markerTransformPlayer = playerMarker3D.transform;
            if (markerTransformPlayer != null) markerTransformPlayer.rotation = Quaternion.Euler(0, m_CameraHeading, 0);
        }

    if (m_bAddingNewPoint && m_bAddingPinPositionSet)         {         m_NewPointMarker = control2.AddMarker3D(m_AddingPinPosition, m_PinPlaneYellow);
        }
        /*
        if (playerMarker3D != null)
        {
            playerMarker3D.SetPosition(m_PlayerPosition.x, m_PlayerPosition.y);


            OnlineMaps map = OnlineMaps.instance;
            double tlx, tly, brx, bry;
            map.GetTopLeftPosition(out tlx, out tly);
            map.GetBottomRightPosition(out brx, out bry);

            playerMarker3D.Update(tlx, tly, brx, bry, map.zoom);
        }
*/

        if (control2 == null)
        {
            Debug.Log("You must use the 3D control (Texture or Tileset).");
            return;
        }
    }

    void showPins()
    {
        removePins();
        addPins();
    }

    static bool g_locationstarted = false;

    IEnumerator StartLocations()
    {
        UnityEngine.UI.Text textdebug;
        textdebug = m_TextDebug.GetComponent<UnityEngine.UI.Text>();


        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {

            m_bLocationGPSDisabled = true;
            m_bLocationGPSDisabledReading = false;

         //   Debug.Log("Location ->disabled");
            Vector2 pos;
            pos.y = m_LocationLatDisabled;
            pos.x = m_LocationLngDisabled;
            OnLocationChanged(pos);

           // loadPins();



            if (m_bDebug == false/* && false*/) // Todo: Comment false out again
            {
                m_TextDebug.SetActive(true);
                m_BackDebug.SetActive(true);

                textdebug.text = LocalizationSupport.GetString("GPSGivePermission");
                /*if (Application.systemLanguage == SystemLanguage.German) {
                    textdebug.text = "Bitte gib FotoQuest Go die Erlaubnis GPS benutzen zu dürfen.";
                } else {
                    textdebug.text = "Please give FotoQuest Go the permission to use the location service.";
                }*/
                m_BackDebug.GetComponent<Image>().color = new Color32(255, 255, 255, 240);//new Color32(243,26,26,240);//new Color32(219,32,32,240);//new Color32(255,20,20,240);
            }
            else
            {
                m_TextDebug.SetActive(false);
                m_BackDebug.SetActive(false);
            }

            yield break;
        }

        Input.location.Stop();
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;

        while (Input.location.status == LocationServiceStatus.Initializing /*&& maxWait > 0*/)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }


        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {

            m_bLocationGPSDisabled = true;
            m_bLocationGPSDisabledReading = false;

         //   Debug.Log("Location -> Unable to determine device location");
            Vector2 pos;
            pos.y = m_LocationLatDisabled;
            pos.x = m_LocationLngDisabled;
            OnLocationChanged(pos);
          //  loadPins();


            m_TextDebug.SetActive(true);
            m_BackDebug.SetActive(true);
            textdebug.text = LocalizationSupport.GetString("GPSFailed");
            /*
            if (Application.systemLanguage == SystemLanguage.German) {
                textdebug.text = "Deine Position konnte nicht ermittelt werden.";
            } else {
                textdebug.text = "Your position could not be read.";
            }*/

            m_BackDebug.GetComponent<Image>().color = new Color32(255, 255, 255, 240);//new Color32(243,26,26,240);//new Color32(219,32,32,240);//new Color32(255,20,20,240);

            TextGenerator textGen = new TextGenerator();
            TextGenerationSettings generationSettings = textdebug.GetGenerationSettings(textdebug.rectTransform.rect.size);
            float width = textGen.GetPreferredWidth("asf", generationSettings);
            float height = textGen.GetPreferredHeight("asf", generationSettings);

            /*m_BackDebug.GetComponent<RectTransform> ().sizeDelta = new Vector2(m_TextDebug.GetComponent<RectTransform>().sizeDelta.x, 
                m_TextDebug.GetComponent<RectTransform>().sizeDelta.y);*/
            yield break;
        }
        else
        {

            m_bLocationGPSDisabled = false;
            m_bLocationGPSDisabledReading = false;
            m_bLocationEnabled = true;

            m_StrTextDebug += "readLocation 5 ";
            textdebug.text = m_StrTextDebug;


            Vector2 pos;
            pos.y = Input.location.lastData.latitude;// 30.0f;//48.210033f;
            pos.x = Input.location.lastData.longitude;//12.0f;//16.363449f;
            OnLocationChanged(pos);
            m_bStartReadGPS = false;

           // Debug.Log("GPS has been read");
          //  loadPins();



            m_TextDebug.SetActive(false);
            m_BackDebug.SetActive(false);
            m_MapDeactivated.SetActive(false);

            startCheckingInternet();

            // Access granted and location value could be retrieved
          //  Debug.Log("Location successful -> " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);

            //yield return new WaitForSeconds (1);
            yield break;
        }


        m_StrTextDebug += "readLocation 6 ";
        textdebug.text = m_StrTextDebug;
    }

    private void OnChangeZoom()
    {
        UpdateLine();
    }

    private void OnMapDrag()
    {
        /*   Debug.Log("OnMapDrag");
           Vector3 playerposition = OnlineMapsTileSetControl.instance.GetWorldPosition(m_PlayerPosition);
           float playerscreenposx = playerMarker3D.transform.position.x - playerposition.x;
           float playerscreenposy = playerMarker3D.transform.position.z - playerposition.z;
           Debug.Log("playerposx: " + playerposition.x + " y: " + playerposition.y + " z: " + playerposition.z);
           Debug.Log("playerscreenposx: " + playerscreenposx + " y: " + playerscreenposy);

           UpdateLine();*/
    }

    public void OnMenuClicked()
    {
        /*      m_bMenuOpen = true;
                m_MenuOpenTimer = 0.0f;

                Debug.Log ("OnMenuClicked");*/
    }
    public void OnMenuCloseClicked()
    {
        /*  m_bMenuOpen = false;
            m_MenuOpenTimer = 0.0f;*/
    }

    public void OnOpenMenuSelection()
    {
        //  m_bMenuSelectionOpen = true;
        //  m_MenuSelectionBack.SetActive (true);
        //      m_MenuSelectionBack2.SetActive (true);
        /*
    m_MenuSelectionButtonLogout.SetActive (true);
    m_MenuSelectionButtonIntroduction.SetActive (true);
    m_MenuSelectionButtonManual.SetActive (true);
    m_MenuSelectionButtonGuidelines.SetActive (true);
    m_MenuSelectionButtonContact.SetActive (true);
    m_MenuSelectionButtonHomepage.SetActive (true);
    m_MenuSelectionButtonCancel.SetActive (true);*/
    }
    public void OnCloseMenuSelection()
    {
        /*  m_bMenuSelectionOpen = false;
            m_MenuSelectionBack.SetActive (false);
            m_MenuSelectionBack2.SetActive (false);

                /*
            m_MenuSelectionButtonLogout.SetActive (false);
            m_MenuSelectionButtonIntroduction.SetActive (false);
            m_MenuSelectionButtonManual.SetActive (false);
            m_MenuSelectionButtonGuidelines.SetActive (false);
            m_MenuSelectionButtonContact.SetActive (false);
            m_MenuSelectionButtonHomepage.SetActive (false);
            m_MenuSelectionButtonCancel.SetActive (false);*/
    }

    public void OnLogutClicked()
    {
        if (m_bIgnoreClick) return;

        if (PlayerPrefs.HasKey("PlayerId"))
        {
            PlayerPrefs.SetInt("LoggedOut", 1);
            PlayerPrefs.SetInt("RegisterMsgShown", 0);

            PlayerPrefs.Save();
        }

        Application.targetFrameRate = 30;
        Application.LoadLevel("StartScreen");
    }
    public void OnIntroductionClicked()
    {
        if (m_bIgnoreClick) return;

        Application.targetFrameRate = 30;
        Application.LoadLevel("Introduction");
    }
    public void OnTermsClicked()
    {
        if (m_bIgnoreClick) return;

        if (Application.systemLanguage == SystemLanguage.German)
        {
            Application.OpenURL("https://www.geo-wiki.org/assets/game/terms/CityOases_Terms_DE.html");//"http://www.fotoquest-go.org/de/terms-of-use/");
        }
        else
        {
            Application.OpenURL("https://www.geo-wiki.org/assets/game/terms/CityOases_Terms.html");//"http://www.fotoquest-go.org/en/terms-of-use/");
        }
        //Application.OpenURL("https://www.geo-wiki.org/assets/game/terms/PicturePile_Terms.html");
        /*
        Application.targetFrameRate = 30;
        Application.LoadLevel("Terms");*/
    }


    public void OnManualClicked()
    {
        if (m_bIgnoreClick) return;

        Application.targetFrameRate = 30;
        Application.LoadLevel("Introduction");//Instructions2");
    }
    public void OnReportClicked()
    {
        if (m_bIgnoreClick) return;

        PlayerPrefs.SetFloat("CurQuestEndPositionX", m_PlayerPosition.x);
        PlayerPrefs.SetFloat("CurQuestEndPositionY", m_PlayerPosition.y);
        PlayerPrefs.Save();

        Application.targetFrameRate = 30;
        Application.LoadLevel("Report");
    }
    public void OnGuidelinesClicked()
    {
        if (m_bIgnoreClick) return;

       // Application.OpenURL("http://www.cityoases.eu");
        Application.targetFrameRate = 30;
        Application.LoadLevel("OfflineMap");
        /*
        Application.targetFrameRate = 30;
        Application.LoadLevel("Guidelines");*/
    }
    public void OnContactClicked()
    {
        if (m_bIgnoreClick) return;

        Application.targetFrameRate = 30;
        Application.LoadLevel("Contact");
    }
    public void OnLeaderboardClicked()
    {
        if (m_bIgnoreClick) return;

        Application.targetFrameRate = 30;
        Application.LoadLevel("Leaderboard");
    }

    public void OnChatClicked()
    {
        if (m_bIgnoreClick) return;

        Application.targetFrameRate = 30;
        Application.LoadLevel("Notifications");
    }

    void OnMsgBoxLoginClicked(string result)
    {
        m_bHasZoomed = true;
       // Debug.Log("OnMsgBoxClicked: " + result);
        if (result == "Login")
        {
            PlayerPrefs.SetInt("LoginReturnToQuests", 0);
            PlayerPrefs.SetInt("RegisterMsgShown", 0);
            PlayerPrefs.Save();

            Application.targetFrameRate = 30;
            Application.LoadLevel("StartScreen");
        }
    }

    void OnMsgBoxSurveyClicked(string result)
    {
        if (result.CompareTo(LocalizationSupport.GetString("QuestionProfileTimeYes")) == 0)
        {
            Application.targetFrameRate = 30;
            Application.LoadLevel("Profile");
        }
    }


    bool checkLoggedIn()
    {
        if (PlayerPrefs.HasKey("PlayerMail") == false)
        {
            UnityEngine.Events.UnityAction<string> ua = new UnityEngine.Events.UnityAction<string>(OnMsgBoxLoginClicked);
            if (Application.systemLanguage == SystemLanguage.German)
            {
                string[] options = { "Später", "Login" };
                messageBoxSmall.Show("", "Du musst dich anmelden, um auf deine Profilseite zu gelangen.", ua, options);
            }
            else
            {
                string[] options = { "Cancel", "Login" };
                messageBoxSmall.Show("", "You need to login to get to your profile site.", ua, options);
            }
            return false;
        }
        return true;
    }

    public void OnProfileClicked()
    {
        if (checkLoggedIn() == false)
        {
            return;
        }
        Application.targetFrameRate = 30;
        Application.LoadLevel("Profile");
    }
    /*
public void OnPuzzleClicked()
{
    Application.targetFrameRate = 30;
    Application.LoadLevel ("Puzzle");
}*/
    public void OnUploadClicked()
    {
        /*int nrquests = PlayerPrefs.GetInt ("NrQuestsDone");
        if (nrquests <= 0) {
            if (Application.systemLanguage == SystemLanguage.German ) {
                string[] options = { "Ok" };
                messageBoxSmall.Show ("", "Du hast noch keine Quests gemacht.", options);
            } else {
                string[] options = { "Ok" };
                messageBoxSmall.Show ("", "You have not made a quest yet.", options);
            }
            return;
        }*/

        Application.targetFrameRate = 30;
        // Application.LoadLevel("Demo-Scene");
      //  Application.LoadLevel("MiniCam");
        Application.LoadLevel("Quests");
    }

public void OnValidationsClicked()
    {
        /*int nrquests = PlayerPrefs.GetInt ("NrQuestsDone");
        if (nrquests <= 0) {
            if (Application.systemLanguage == SystemLanguage.German ) {
                string[] options = { "Ok" };
                messageBoxSmall.Show ("", "Du hast noch keine Quests gemacht.", options);
            } else {
                string[] options = { "Ok" };
                messageBoxSmall.Show ("", "You have not made a quest yet.", options);
            }
            return;
        }*/

        Application.targetFrameRate = 30;
        // Application.LoadLevel("Demo-Scene");
        //  Application.LoadLevel("MiniCam");
        Application.LoadLevel("Validations");
    }

    public void OnHomepageClicked()
    {
        Application.OpenURL("http://www.fotoquest-go.org");
    }


    public void OnProgressClicked()
    {
        Application.targetFrameRate = 30;
        Application.LoadLevel("Introduction");//Progress");
    }

    void saveQuestStats()
    {
        bool m_bPointInReach = true;
        if (PlayerPrefs.HasKey("CurQuestReached"))
        {
            int inreach = PlayerPrefs.GetInt("CurQuestReached");
            if (inreach == 0)
            {
                m_bPointInReach = false;
            }
        }

        int nrquestsdone = 0;

        if (PlayerPrefs.HasKey("NrQuestsDone"))
        {
            nrquestsdone = PlayerPrefs.GetInt("NrQuestsDone");
        }
        else
        {
            nrquestsdone = 0;
        }

        int iPointInReach = m_bPointInReach ? 1 : 0;
        PlayerPrefs.SetInt("Quest_" + nrquestsdone + "_PointReached", iPointInReach);

        string curquestid = PlayerPrefs.GetString("CurQuestId");
        PlayerPrefs.SetString("Quest_" + nrquestsdone + "_Id", curquestid);
        PlayerPrefs.SetInt("Quest_" + nrquestsdone + "_TrainingPoint", 0);

     //   PlayerPrefs.SetInt("MadeQuest", 1);
        PlayerPrefs.Save();
    }


public void OnStartCreatePointCurrentLocation()
    {
        if (m_bIgnoreClick) return;

        //  PlayerPrefs.SetInt("PickedPuzzle", 0);
        PlayerPrefs.SetString("CurQuestId", "-1");
         PlayerPrefs.SetFloat("CurQuestLat", (float)m_PlayerPosition.y);
         PlayerPrefs.SetFloat("CurQuestLng", (float)m_PlayerPosition.x);
        PlayerPrefs.SetString("CurQuestSelectedTime", m_QuestSelectedTime);
        PlayerPrefs.SetFloat("CurQuestStartPositionX", m_PlayerPositionStart.x);
        PlayerPrefs.SetFloat("CurQuestStartPositionY", m_PlayerPositionStart.y);
        PlayerPrefs.SetFloat("CurQuestEndPositionX", m_PlayerPosition.x);
        PlayerPrefs.SetFloat("CurQuestEndPositionY", m_PlayerPosition.y);
        PlayerPrefs.SetFloat("CurDistanceWalked", m_DistanceWalked);

        PlayerPrefs.SetInt("CurQuestNrPositions", m_NrPlayerPositions);
        for (int i = 0; i < m_NrPlayerPositions; i++)
        {
            PlayerPrefs.SetFloat("CurQuestPositionX_" + i, m_PlayerPositions[i].x);
            PlayerPrefs.SetFloat("CurQuestPositionY_" + i, m_PlayerPositions[i].y);
        }

        //        float weight = float.Parse(m_SelectedPin.m_Weight);
        //       PlayerPrefs.SetFloat("CurQuestWeight", weight);
        PlayerPrefs.SetInt("CurQuestReached", 1);

        string startquesttime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        PlayerPrefs.SetString("CurQuestStartQuestTime", startquesttime);


        int nrquestsdone = 0;
        if (PlayerPrefs.HasKey("NrQuestsDone"))
        {
            nrquestsdone = PlayerPrefs.GetInt("NrQuestsDone");
        }
        else
        {
            nrquestsdone = 0;
        }


        PlayerPrefs.SetString("Quest_" + nrquestsdone + "_Id", "-1");
        PlayerPrefs.SetInt("Quest_" + nrquestsdone + "_TrainingPoint", 1);
        PlayerPrefs.SetInt("Quest_" + nrquestsdone + "_TrainingPoint_Public", 1);
    PlayerPrefs.SetFloat("Quest_" + nrquestsdone + "_TrainingPoint_Lat", m_PlayerPosition.y);
    PlayerPrefs.SetFloat("Quest_" + nrquestsdone + "_TrainingPoint_Lng", m_PlayerPosition.x);

        // Debug.Log("lat: " + m_AddingPinPosition.y + " lng: " + m_AddingPinPosition.x);

        PlayerPrefs.SetInt("Quest_" + nrquestsdone + "_PointReached", 1);

        PlayerPrefs.SetInt("DynQuestionsToLastState", 0);

        PlayerPrefs.Save();

        Application.targetFrameRate = 30;
        Application.LoadLevel("Validation");
    }


    public void OnStartCreatePoint()
    {
        if (m_bIgnoreClick) return;


      //  PlayerPrefs.SetInt("PickedPuzzle", 0);         PlayerPrefs.SetString("CurQuestId", "-1");         PlayerPrefs.SetFloat("CurQuestLat", (float)m_AddingPinPosition.y);         PlayerPrefs.SetFloat("CurQuestLng", (float)m_AddingPinPosition.x);         PlayerPrefs.SetString("CurQuestSelectedTime", m_QuestSelectedTime);         PlayerPrefs.SetFloat("CurQuestStartPositionX", m_PlayerPositionStart.x);         PlayerPrefs.SetFloat("CurQuestStartPositionY", m_PlayerPositionStart.y);         PlayerPrefs.SetFloat("CurQuestEndPositionX", m_PlayerPosition.x);         PlayerPrefs.SetFloat("CurQuestEndPositionY", m_PlayerPosition.y);         PlayerPrefs.SetFloat("CurDistanceWalked", m_DistanceWalked);          PlayerPrefs.SetInt("CurQuestNrPositions", m_NrPlayerPositions);         for (int i = 0; i < m_NrPlayerPositions; i++)         {             PlayerPrefs.SetFloat("CurQuestPositionX_" + i, m_PlayerPositions[i].x);             PlayerPrefs.SetFloat("CurQuestPositionY_" + i, m_PlayerPositions[i].y);         }  //        float weight = float.Parse(m_SelectedPin.m_Weight);  //       PlayerPrefs.SetFloat("CurQuestWeight", weight);         PlayerPrefs.SetInt("CurQuestReached", 1);          string startquesttime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");         PlayerPrefs.SetString("CurQuestStartQuestTime", startquesttime);


        int nrquestsdone = 0;
        if (PlayerPrefs.HasKey("NrQuestsDone"))
        {
            nrquestsdone = PlayerPrefs.GetInt("NrQuestsDone");
        }
        else
        {
            nrquestsdone = 0;
        }


        PlayerPrefs.SetString("Quest_" + nrquestsdone + "_Id", "-1");
        PlayerPrefs.SetInt("Quest_" + nrquestsdone + "_TrainingPoint", 1);
        PlayerPrefs.SetInt("Quest_" + nrquestsdone + "_TrainingPoint_Public", 1);
        PlayerPrefs.SetFloat("Quest_" + nrquestsdone + "_TrainingPoint_Lat", m_AddingPinPosition.y);
        PlayerPrefs.SetFloat("Quest_" + nrquestsdone + "_TrainingPoint_Lng", m_AddingPinPosition.x);

       // Debug.Log("lat: " + m_AddingPinPosition.y + " lng: " + m_AddingPinPosition.x);

        PlayerPrefs.SetInt("Quest_" + nrquestsdone + "_PointReached", 1);

        PlayerPrefs.SetInt("DynQuestionsToLastState", 0);

        PlayerPrefs.Save();

        Application.targetFrameRate = 30;
        Application.LoadLevel("Validation");
    }

    public void OnStartCreatePointNotPublic()
    {
        if (m_bIgnoreClick) return;

      //  PlayerPrefs.SetInt("PickedPuzzle", 0);
        PlayerPrefs.SetString("CurQuestId", "-1");
        PlayerPrefs.SetFloat("CurQuestLat", (float)m_AddingPinPosition.y);
        PlayerPrefs.SetFloat("CurQuestLng", (float)m_AddingPinPosition.x);
        PlayerPrefs.SetString("CurQuestSelectedTime", m_QuestSelectedTime);
        PlayerPrefs.SetFloat("CurQuestStartPositionX", m_PlayerPositionStart.x);
        PlayerPrefs.SetFloat("CurQuestStartPositionY", m_PlayerPositionStart.y);
        PlayerPrefs.SetFloat("CurQuestEndPositionX", m_PlayerPosition.x);
        PlayerPrefs.SetFloat("CurQuestEndPositionY", m_PlayerPosition.y);
        PlayerPrefs.SetFloat("CurDistanceWalked", m_DistanceWalked);

        PlayerPrefs.SetInt("CurQuestNrPositions", m_NrPlayerPositions);
        for (int i = 0; i < m_NrPlayerPositions; i++)
        {
            PlayerPrefs.SetFloat("CurQuestPositionX_" + i, m_PlayerPositions[i].x);
            PlayerPrefs.SetFloat("CurQuestPositionY_" + i, m_PlayerPositions[i].y);
        }

     //   float weight = float.Parse(m_SelectedPin.m_Weight);
      //  PlayerPrefs.SetFloat("CurQuestWeight", weight);
        PlayerPrefs.SetInt("CurQuestReached", 1);

        string startquesttime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        PlayerPrefs.SetString("CurQuestStartQuestTime", startquesttime);


        int nrquestsdone = 0;
        if (PlayerPrefs.HasKey("NrQuestsDone"))
        {
            nrquestsdone = PlayerPrefs.GetInt("NrQuestsDone");
        }
        else
        {
            nrquestsdone = 0;
        }


        PlayerPrefs.SetString("Quest_" + nrquestsdone + "_Id", "-1");
        PlayerPrefs.SetInt("Quest_" + nrquestsdone + "_TrainingPoint", 1);
        PlayerPrefs.SetInt("Quest_" + nrquestsdone + "_TrainingPoint_Public", 0);
        PlayerPrefs.SetFloat("Quest_" + nrquestsdone + "_TrainingPoint_Lat", m_AddingPinPosition.y);
        PlayerPrefs.SetFloat("Quest_" + nrquestsdone + "_TrainingPoint_Lng", m_AddingPinPosition.x);


        PlayerPrefs.SetInt("Quest_" + nrquestsdone + "_PointReached", 1);

        PlayerPrefs.SetInt("DynQuestionsToLastState", 0);

        PlayerPrefs.Save();

        Application.targetFrameRate = 30;
        Application.LoadLevel("Validation");//"NotInReach");
    }

    public void OnStartQuest()
    {
        if (m_bLocationGPSDisabled && !m_bDebug)
        {
            return;
        }

        if (!m_bLocationEnabled && !m_bDebug)
        {
            return;
        }



      //  PlayerPrefs.SetInt("PickedPuzzle", 0);
        PlayerPrefs.SetString("CurQuestId", m_SelectedPin.m_Id);
        PlayerPrefs.SetFloat("CurQuestLat", (float)m_SelectedPin.m_Lat);
        PlayerPrefs.SetFloat("CurQuestLng", (float)m_SelectedPin.m_Lng);
        PlayerPrefs.SetString("CurQuestSelectedTime", m_QuestSelectedTime);
        PlayerPrefs.SetFloat("CurQuestStartPositionX", m_PlayerPositionStart.x);
        PlayerPrefs.SetFloat("CurQuestStartPositionY", m_PlayerPositionStart.y);
        PlayerPrefs.SetFloat("CurQuestEndPositionX", m_PlayerPosition.x);
        PlayerPrefs.SetFloat("CurQuestEndPositionY", m_PlayerPosition.y);
    PlayerPrefs.SetFloat("CurDistanceWalked", m_DistanceWalked);
        PlayerPrefs.SetString("CurQuestValidationId", m_SelectedPin.m_ValidationId);

        PlayerPrefs.SetInt("CurQuestNrPositions", m_NrPlayerPositions);
        for (int i = 0; i < m_NrPlayerPositions; i++)
        {
            PlayerPrefs.SetFloat("CurQuestPositionX_" + i, m_PlayerPositions[i].x);
            PlayerPrefs.SetFloat("CurQuestPositionY_" + i, m_PlayerPositions[i].y);
        }

      //  float weight = float.Parse(m_SelectedPin.m_Weight);
      //  PlayerPrefs.SetFloat("CurQuestWeight", weight);
        PlayerPrefs.SetInt("CurQuestReached", 1);

        string startquesttime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        PlayerPrefs.SetString("CurQuestStartQuestTime", startquesttime);


        PlayerPrefs.SetInt("DynQuestionsToLastState", 0);
        PlayerPrefs.Save();

        saveQuestStats();
        Application.targetFrameRate = 30;
        //      Application.LoadLevel ("Minigram");
        Application.LoadLevel("Validation");
        /*
        if (PlayerPrefs.HasKey ("MadeQuest") == false) {
            PlayerPrefs.SetInt ("MadeQuest", 1);


            PlayerPrefs.Save ();

            Application.targetFrameRate = 30;

        //  Application.LoadLevel ("DebugSelectPinType");
            //Application.LoadLevel ("ExplainQuests");
            Application.LoadLevel ("TestCamera");
        } else {
            PlayerPrefs.SetInt ("CameraStartLastStep", 0);

            PlayerPrefs.Save ();
            Application.targetFrameRate = 30;
        //  Application.LoadLevel ("DebugSelectPinType");
            Application.LoadLevel ("TestCamera");
            //Application.LoadLevel ("DynamicQuestions");
        }*/

    }




    public void OnPointNotReachable()
    {
        if (m_bLocationGPSDisabled && !m_bDebug)
        {
            return;
        }
        if (!m_bLocationEnabled && !m_bDebug)
        {
            return;
        }

       // PlayerPrefs.SetInt("PickedPuzzle", 0);
        PlayerPrefs.SetString("CurQuestId", m_SelectedPin.m_Id);
        PlayerPrefs.SetFloat("CurQuestLat", (float)m_SelectedPin.m_Lat);
        PlayerPrefs.SetFloat("CurQuestLng", (float)m_SelectedPin.m_Lng);
        PlayerPrefs.SetString("CurQuestSelectedTime", m_QuestSelectedTime);
        PlayerPrefs.SetFloat("CurQuestStartPositionX", m_PlayerPositionStart.x);
        PlayerPrefs.SetFloat("CurQuestStartPositionY", m_PlayerPositionStart.y);
        PlayerPrefs.SetFloat("CurQuestEndPositionX", m_PlayerPosition.x);
        PlayerPrefs.SetFloat("CurQuestEndPositionY", m_PlayerPosition.y);
    PlayerPrefs.SetFloat("CurDistanceWalked", m_DistanceWalked);
        PlayerPrefs.SetString("CurQuestValidationId", m_SelectedPin.m_ValidationId);
       // float weight = float.Parse(m_SelectedPin.m_Weight);
       // PlayerPrefs.SetFloat("CurQuestWeight", weight);
        PlayerPrefs.SetInt("CurQuestReached", 0);

        PlayerPrefs.SetInt("CurQuestNrPositions", m_NrPlayerPositions);
        for (int i = 0; i < m_NrPlayerPositions; i++)
        {
            PlayerPrefs.SetFloat("CurQuestPositionX_" + i, m_PlayerPositions[i].x);
            PlayerPrefs.SetFloat("CurQuestPositionY_" + i, m_PlayerPositions[i].y);
        }

        string startquesttime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        PlayerPrefs.SetString("CurQuestStartQuestTime", startquesttime);
        PlayerPrefs.Save();

        saveQuestStats();

        Application.targetFrameRate = 30;



        //      Application.LoadLevel ("Minigram");
        Application.LoadLevel("Validation");
        //  Application.LoadLevel ("DebugSelectPinType");
    }

    void internetEnabled(bool bEnabled)
    {
       // Debug.Log("internetEnabled: " + bEnabled);
        if (bEnabled == false)
        {
            m_TextDebug.SetActive(true);
            m_BackDebug.SetActive(true);
            UnityEngine.UI.Text textdebug;
            textdebug = m_TextDebug.GetComponent<UnityEngine.UI.Text>();
            textdebug.text = LocalizationSupport.GetString("CheckInternet");/*
            if (Application.systemLanguage == SystemLanguage.German) {
                textdebug.text = "Bitte überprüfe deine Internetverbindung.";
            } else {
                textdebug.text = "Please check your internet connection.";
            }*/
            m_BackDebug.GetComponent<Image>().color = new Color32(255, 255, 255, 240);//new Color32(243,26,26,240);//new Color32(219,32,32,240);// new Color32(255,20,20,240);

            m_bCheckInternet = true;
            m_CheckInternetTimer = 0.0f;
        }
        else
        {
            m_bCheckInternet = false;
            m_TextDebug.SetActive(false);
            m_BackDebug.SetActive(false);
        }
    }

    IEnumerator checkInternetConnection()
    {
        WWW www = new WWW("https://google.com");
        yield return www;
        if (www.error != null)
        {
            internetEnabled(false);
        }
        else
        {
            internetEnabled(true);
        }
    }
    void startCheckingInternet()
    {
        StartCoroutine(checkInternetConnection());
    }









    public static string ComputeHash(string s)
    {
        // Form hash
        System.Security.Cryptography.MD5 h = System.Security.Cryptography.MD5.Create();
        byte[] data = h.ComputeHash(System.Text.Encoding.Default.GetBytes(s));
        // Create string representation
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < data.Length; ++i)
        {
            sb.Append(data[i].ToString("x2"));
        }
        return sb.ToString();
    }






    private const string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";
    private const string TWEET_LANGUAGE = "en";

    public void OnNewsClicked()
    {
        Application.OpenURL("http://www.laco-wiki.net");
    }

    public void OpenTwitterPage()
    {
        Application.OpenURL("https://twitter.com/fotoquest_go");
    }
    public void OpenFacebookPage()
    {
        Application.OpenURL("https://www.facebook.com/GeoWiki");
    }
    public void SendEmail(string email, string subject, string body)
    {
        subject = MyEscapeURL(subject);
        //body = MyEscapeURL(body);
        Application.OpenURL("mailto:" + email + "?subject=" + subject);// + "&body=" + body);
    }
    public string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }

    public void OnContactUsClicked()
    {
        if (m_bIgnoreClick) return;
        SendEmail("info@laco-wiki.net", "LACO-Wiki", "Text");
    }

    public void ShareLevelReachedTwitter()
    {

    }
    public void ShareLevelReachedFB()
    {

    }


    void OnMsgBoxClicked(string param)
    {

    }

    void CloseMenu()
    {
        SlidingMenu menu = (SlidingMenu)m_MenuSlidin.GetComponent(typeof(SlidingMenu));
        m_bMenuOpened = false;

        menu.SlideToValue(-0.1f/*0.27f/*0.28f*/);
        m_MenuToggleButton.SetActive(false);
    }

    public void ToggleMenu()
    {
        SlidingMenu menu = (SlidingMenu)m_MenuSlidin.GetComponent(typeof(SlidingMenu));
        m_bMenuOpened = !m_bMenuOpened;

        if (m_bMenuOpened)
        {
            menu.SlideToValue(1.0f/*1.3f*//*1.1f*//*0.27f/*0.28f*/);
            m_MenuToggleButton.SetActive(true);
            m_MenuSlidin.SetActive(true);
        }
        else
        {
            menu.SlideToValue(-0.1f/*0.27f/*0.28f*/);
            m_MenuToggleButton.SetActive(false);
        }



    }





    public void deselectPoint()
    {
        m_bSelectedPin = false;
        m_bForceUpdate = true;

        m_ButtonStartQuest.SetActive(false);
        m_ButtonMoveCloser.SetActive(false);
        m_TextMoveCloser.SetActive(false);
        m_TextAlreadyRated.SetActive(false);
        //  m_ButtonStartQuestHighlighted.SetActive (false);
        //      m_ButtonNearlyStartQuest.SetActive (false);
      //  m_ButtonPointNotReachable.SetActive(false);

        m_StartQuestBackground.SetActive(false);
        m_StartQuestBackgroundLine.SetActive(false);

        hideLine();

        updateToPositionButtons(true);


        removePins();
        addPins();
    }

    public void closePin()
    {
        deselectPoint();

       /* if (m_SelectedActivity == -1)
        {
            showActivity(true, true);
        }*/
    }

    int m_SelectedActivity = -1;
    bool m_bMenuSelectedActivity = false;
    bool m_bIgnoreClick = false;
    int m_IgnoreClickIter = 0;
    List<int> m_FilterActivites = null;



    int m_PinInfoClosedIter = 0;
    bool m_bPinInfoClosed = false;
    public void closePinInfo()
    {
     //   Debug.Log(">> closePinInfo");
        m_SelectedPinTimer = 10000000.0f;
        m_bPinInfoClosed = true;
        m_PinInfoClosedIter = 0;
        m_bForceUpdate = true;
    }

    public void to2dMap()
    {
        m_bIn2dMap = true;
        m_bTo2dMap = true;
        m_To2dMapTimer = 0.0f;
        m_bTo3dMap = false;
        m_To2dMapTimer = 0.0f;
        m_bHasZoomed = true;
        m_CameraAngleTransition = m_CameraAngleMove;
      //  m_BtnTo2dMap.SetActive(false);
      //  m_BtnTo3dMap.SetActive(true);
        closePinInfo();

/*        m_DistanceBack.SetActive(false);
        m_DistanceBackHorizon.SetActive(false);
        m_DistanceText.SetActive(false);*//*
        m_DistanceText2.SetActive (true);
        m_DistanceBack2d.SetActive (true);*/
        /*
        m_DistanceText2.SetActive(false);
        m_DistanceBack2d.SetActive(false);*/

      //  m_Sky.SetActive(false);
      //  m_Sky2.SetActive(false);


        OnlineMapsControlBase3D control2 = GetComponent<OnlineMapsControlBase3D>();
        //  control2.setUpdateControl (true);
        //  control2.setAlwaysUpdateControl (true);
        control2.allowUserControl = true;
    }

    public void to3dMap()
    {
        m_bIn2dMap = false;
        m_bTo2dMap = false;
        m_To2dMapTimer = 0.0f;
        m_bTo3dMap = true;
        m_To3dMapTimer = 0.0f;
        //      m_CameraPitch = 37.0f;
        m_bHasZoomed = true;
        closePinInfo();

      //  m_BtnTo2dMap.SetActive(true);
      //  m_BtnTo3dMap.SetActive(false);
        /*
        m_DistanceBack.SetActive(true);
        m_DistanceBackHorizon.SetActive(true);
        m_DistanceText.SetActive(true);
        m_DistanceText2.SetActive(false);

      //  m_Sky.SetActive(true);
      //  m_Sky2.SetActive(true);
        m_DistanceBack2d.SetActive(false);*/

        toPlayerPosition();

        if (api.zoom < 13)
        {
            api.zoom = 13;
        }
        enableZoomButtons();


        //updatePins ();
        //Debug.Log ("Zoom: " + api.zoom);
        //loadPins ();
        showPins();
        OnlineMapsControlBase3D control2 = GetComponent<OnlineMapsControlBase3D>();
        /*      control2.setUpdateControl (true);
                control2.setAlwaysUpdateControl (false);*/
        control2.allowUserControl = false;
    }


    bool m_bSelectedNearestPinOnStart = false;
    int m_SelectedNearestPin = -1;
    void selectNearestPinOnStart()
    {/*
        Debug.Log ("selectNearestPinOnStart: " + m_bSelectedNearestPinOnStart + " nrpins: " + m_NrPins);
        if (m_bSelectedNearestPinOnStart) {
            return;
        }
        m_bSelectedNearestPinOnStart = true;
        m_SelectedNearestPin = -1;

        bool bFound = false;
        int which = -1;
        float smallestDist = 0.0f;
        for (int i = 0; i < m_NrPins; i++) {
            if (!pinAlreadyDone (m_Pins [i].m_Id)) {

                float lat = (float)m_Pins [i].m_Lat;
                float lng = (float)m_Pins [i].m_Lng;
                //Debug.Log ("playerx " + m_PlayerPosition.x + " y: " + m_PlayerPosition.y + " lat: " + lat + " lng: " + lng);

                float distance = OnlineMapsUtils.DistanceBetweenPoints(m_PlayerPosition, new Vector2(lng, lat)).magnitude;

                if (!bFound) {
                    which = i;
                    bFound = true;
                    smallestDist = distance;
                } else if (distance < smallestDist) {
                    which = i;
                    smallestDist = distance;
                }
            }
        }

        Debug.Log ("selectNearestPinOnStart found: " + bFound + " which: " + which);
        if (bFound) {
            m_SelectedNearestPin = which;
        }*/
    }

    void clickOnNearestPin()
    {
        /*if (m_SelectedNearestPin != -1) {
            if (m_Pins [m_SelectedNearestPin].m_Marker != null) {
                OnGOClick (m_Pins [m_SelectedNearestPin].m_Marker.instance);
            }
        }*/
    }

    void updateToPositionButtons(bool bBottom)
    {
        if (!bBottom)
        {
            if (!m_bBlackGui)
            {
                m_ButtonToPosBrightBottom.SetActive(false);
                m_ButtonToPosDarkBottom.SetActive(false);
                m_ButtonToPosBright.SetActive(true);
                m_ButtonToPosDark.SetActive(false);

            if (m_bAddingNewPoint || m_bAddingNewPointsEnabled == false)
                {
                     m_ButtonAddPinBottom.SetActive(false);
                    m_ButtonAddPin.SetActive(false);
                }
                else
                {
                    m_ButtonAddPinBottom.SetActive(false);
                    m_ButtonAddPin.SetActive(true);
                }
            }
            else
            {
                m_ButtonToPosBrightBottom.SetActive(false);
                m_ButtonToPosDarkBottom.SetActive(false);
                m_ButtonToPosBright.SetActive(false);
                m_ButtonToPosDark.SetActive(true);
            if (m_bAddingNewPoint || m_bAddingNewPointsEnabled == false)
                {
                    m_ButtonAddPinBottom.SetActive(false);
                    m_ButtonAddPin.SetActive(false);
                }
                else
                {
                    m_ButtonAddPinBottom.SetActive(false);
                    m_ButtonAddPin.SetActive(true);
                }
            }
        }
        else
        {
            if (!m_bBlackGui)
            {
                m_ButtonToPosBrightBottom.SetActive(true);
                m_ButtonToPosDarkBottom.SetActive(false);
                m_ButtonToPosBright.SetActive(false);
                m_ButtonToPosDark.SetActive(false);
            if (m_bAddingNewPoint || m_bAddingNewPointsEnabled == false)
                {
                    m_ButtonAddPinBottom.SetActive(false);
                    m_ButtonAddPin.SetActive(false);
                }
                else
                {
                    m_ButtonAddPinBottom.SetActive(true);
                    m_ButtonAddPin.SetActive(false);
                }
            }
            else
            {
                m_ButtonToPosBrightBottom.SetActive(false);
                m_ButtonToPosDarkBottom.SetActive(true);
                m_ButtonToPosBright.SetActive(false);
                 m_ButtonToPosDark.SetActive(false);
            if (m_bAddingNewPoint || m_bAddingNewPointsEnabled == false)
                {
                    m_ButtonAddPinBottom.SetActive(false);
                    m_ButtonAddPin.SetActive(false);
                }
                else
                {
                    m_ButtonAddPinBottom.SetActive(true);
                    m_ButtonAddPin.SetActive(false);
                }
            }
        }
    }


    public void SlidingMenu()
    {
        m_bIgnoreClick = true;
        m_IgnoreClickIter = 0;
        //Debug.Log("###SlidingMenu");
    }

    public void OpenMenu(int id)
    {
        m_bIgnoreClick = true;
        m_IgnoreClickIter = 0;

    }

    public void CloseMenu(int id)
    {
        m_bIgnoreClick = true;
        m_IgnoreClickIter = 0;

     //   Debug.Log("Close menu " + id);
        if (id == 1)// Pin Info
        {
                closePin();
        }
        else if (id == 2)
        {
            CloseAddingNewPoint();
        }
        else if (id == 3)
        {
            
        }
        else if (id == 4)
        {
            CloseMenu();
        }
        else if (id == 12)
        {
            
        }
    }

    public void SlidingMenuFinished(int id) 
    {
        if(id == 1) {
            
        } else if(id == 3) {
           
        } else if(id == 10) {
/*            if(!m_bActivityShown) {
                m_MenuDoActivity.SetActive(false);
            }*/
        }
        else if (id == 4)
        {
            if (!m_bMenuOpened)
            {
                m_MenuSlidin.SetActive(false);
            }
        }
        else if (id == 2)
        {
            if(!m_bAddingNewPoint) {
                m_MenuAddNewPoint.SetActive(false);
            }
        }
    }


    //------------------------------
    // Sample points

    int m_LoadSamplePointsIter = 0;
    public void loadSamplePoints(int iter)
    {
        Debug.Log("#### loadSamplePoints: " + iter + "####");
        m_LoadSamplePointsIter = iter;
        string sessions = PlayerPrefs.GetString("ActiveSessions");
        Debug.Log("Active sessions: " + sessions);
        string newsessions = "";
        string[] splitArray = sessions.Split(char.Parse(" "));
        Debug.Log("Nr sessions: " + splitArray.Length);
        if(m_LoadSamplePointsIter >= splitArray.Length) {
            m_TextLocationOutside.SetActive(false);
            m_BackLocationOutside.SetActive(false);

            createPins();
        } else {
            if (splitArray[m_LoadSamplePointsIter] != " " && splitArray[m_LoadSamplePointsIter] != "")
            {
                loadSamplePointsForValidation(splitArray[m_LoadSamplePointsIter]);
            } else {
                loadSamplePoints(m_LoadSamplePointsIter + 1);
            }
        }
    }


    string m_ValidationId;
    public void loadSamplePointsForValidation(string validation)
    {
        Debug.Log(">> loadSamplePointsForValidation validation: " + validation);
        m_ValidationId = validation;
       /* string url = "http://dev.laco-wiki.net/api/mobile/validationsessions/" + m_ValidationId + "/sampleItems";

        string token = PlayerPrefs.GetString("Token");
        Debug.Log("token: " + token);

        WWWForm form = new WWWForm();
        form.AddField("param", "param");


        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Authorization", "Bearer " + token);

        WWW www = new WWW(url, form.data, headers);

        StartCoroutine(WaitForSamplePoints(www));*/
    StartCoroutine(StartLoadingSamples());
    }


    IEnumerator StartLoadingSamples()     {
        //string url = "https://laco-wiki.net/api/mobile/validationsessions/" + m_ValidationId + "/sampleItems";
        string url = "https://dev.laco-wiki.net/api/mobile/validationsessions/" + m_ValidationId + "/sampleItems";
         UnityWebRequest www = UnityWebRequest.Get(url);         string token = PlayerPrefs.GetString("Token");         www.SetRequestHeader("Authorization", "Bearer " + token);

        Debug.Log("Url: " + url);         Debug.Log("header: " + "Bearer " + token);          yield return www.Send();;          if (www.isNetworkError || www.isHttpError)         {             Debug.Log("Error: " + www.error);         if (www.error == "HTTP/1.1 401 Unauthorized"/* && false*/) // Todo: comment false out for release             {                 Debug.Log("Unauthorized access");                 Application.LoadLevel("LoginLacoWiki");             }         }         else         {
            m_Samples = new ArrayList();

            JSONObject j = new JSONObject(www.downloadHandler.text);
       // Debug.Log("Sample points data: " + www.downloadHandler.text);
            m_ReadingWhich = -1;
            accessSamplePoints(j);

            int nrsamples = m_Samples.Count;
            Debug.Log("nrsamples: " + nrsamples);



            PlayerPrefs.SetInt("Session_" + m_ValidationId + "_NrSamples", nrsamples);
            for (int i = 0; i < nrsamples; i++)
            {
                SamplePoint point = (SamplePoint)m_Samples[i];
                PlayerPrefs.SetInt("Session_" + m_ValidationId + "_Sample_" + i + "_Id", point.m_SampleId);
                PlayerPrefs.SetInt("Session_" + m_ValidationId + "_Sample_" + i + "_LegendId", point.m_SampleLegendId);
                PlayerPrefs.SetInt("Session_" + m_ValidationId + "_Sample_" + i + "_Validated", point.m_SampleValidated);
                PlayerPrefs.SetString("Session_" + m_ValidationId + "_Sample_" + i + "_Lat", point.m_SampleLat);
                PlayerPrefs.SetString("Session_" + m_ValidationId + "_Sample_" + i + "_Lng", point.m_SampleLng);
            }

            PlayerPrefs.Save();


            loadSamplePoints(m_LoadSamplePointsIter + 1);
         }     }


    ArrayList m_Samples;
   /* IEnumerator WaitForSamplePoints(WWW www)
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
            for (int i = 0; i < nrsamples; i++)
            {
                SamplePoint point = (SamplePoint)m_Samples[i];
                PlayerPrefs.SetInt("Session_" + m_ValidationId + "_Sample_" + i + "_Id", point.m_SampleId);
                PlayerPrefs.SetInt("Session_" + m_ValidationId + "_Sample_" + i + "_LegendId", point.m_SampleLegendId);
                PlayerPrefs.SetInt("Session_" + m_ValidationId + "_Sample_" + i + "_Validated", point.m_SampleValidated);
                PlayerPrefs.SetString("Session_" + m_ValidationId + "_Sample_" + i + "_Lat", point.m_SampleLat);
                PlayerPrefs.SetString("Session_" + m_ValidationId + "_Sample_" + i + "_Lng", point.m_SampleLng);
            }

            PlayerPrefs.Save();


            loadSamplePoints(m_LoadSamplePointsIter + 1);
        }
        else
        {
            Debug.Log("Could not load sample points");
            Debug.Log("WWW Error: " + www.error);
            Debug.Log("WWW Error 2: " + www.text);

            if (www.error == "401 Unauthorized" && false) // Todo: Comment false out again for release
            {
                Debug.Log("Unauthorized access");
                Application.LoadLevel("LoginLacoWiki");
            } else {
                createPins();

               m_TextLocationOutside.SetActive(false);
                m_BackLocationOutside.SetActive(false);
            }
        }
    }*/

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

                if(m_SampleId >= 460 && m_SampleId < 480) {
                        Debug.Log("sample " + m_SampleId + " validated: " + m_SampleValidated + " legendid: " + m_SampleLegendId);
                }

                if (m_SampleId >= 555 && m_SampleId < 570)
                    {
                        Debug.Log("sample " + m_SampleId + " validated: " + m_SampleValidated + " legendid: " + m_SampleLegendId);
                    }
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

    void createPins()
    {
        Debug.Log("###### createPins #######");

        string sessions = PlayerPrefs.GetString("ActiveSessions");
        string newsessions = "";
        string[] splitArray = sessions.Split(char.Parse(" "));

        m_CurrentPin = 0;

        for (int i = 0; i < splitArray.Length; i++) {
            string valid = splitArray[i];
            if (valid != "" && valid != " ")
            {
                int nrsamples = PlayerPrefs.GetInt("Session_" + valid + "_NrSamples");
                Debug.Log("> nr samples: " + nrsamples);
                for (int sample = 0; sample < nrsamples && m_CurrentPin < 2000; sample++)
                {
                    m_Pins[m_CurrentPin].m_Id = "" + PlayerPrefs.GetInt("Session_" + valid + "_Sample_" + sample + "_Id");
                    m_Pins[m_CurrentPin].m_Lat = double.Parse(PlayerPrefs.GetString("Session_" + valid + "_Sample_" + sample + "_Lng"));
                    m_Pins[m_CurrentPin].m_Lng = double.Parse(PlayerPrefs.GetString("Session_" + valid + "_Sample_" + sample + "_Lat"));

                    m_Pins[m_CurrentPin].m_NrVisits = PlayerPrefs.GetInt("Session_" + valid + "_Sample_" + sample + "_Validated");
                    m_Pins[m_CurrentPin].m_LegendId = "" + PlayerPrefs.GetInt("Session_" + valid + "_Sample_" + sample + "_LegendId");
                    m_Pins[m_CurrentPin].m_ValidationId = valid;

                    m_Pins[m_CurrentPin].InitValues();
                    m_CurrentPin++;
                }
            }
        }

        Debug.Log("Create nr pins: " + m_CurrentPin);

        m_NrPins = m_CurrentPin;
        if(m_NrPins > 100) {
            m_TextLocationOutside.SetActive(false);
            m_BackLocationOutside.SetActive(false);
        }

        showPins();
    }
}
