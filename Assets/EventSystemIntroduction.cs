using UnityEngine;
using System.Collections;
using UI.Pagination;
//using PartaGames.Android;

public class EventSystemIntroduction : MonoBehaviour
{

    public GameObject m_Text;
    public GameObject m_Text2;
   
    public GameObject m_PageText3;
    public GameObject m_PageText4;
    public GameObject m_PageText5;
    public GameObject m_PageText6;

    public GameObject m_Button;

    public GameObject m_ButtonPoint1;
    public GameObject m_ButtonPoint2;
    public GameObject m_ButtonPoint3;
    public GameObject m_ButtonPoint4;
    public GameObject m_ButtonPoint5;

    public GameObject m_Point1;
    public GameObject m_Point2;
    public GameObject m_Point3;
    public GameObject m_Point4;
    public GameObject m_Point5;

    public GameObject m_BackgroundImage;

    public UI.Pagination.PagedRect_ScrollRect m_ScrollRect;
    public GameObject m_Page;


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

    // Use this for initialization
    void Start()
    {
        Debug.Log("Intro1");
        StartCoroutine(changeFramerate());
        ForceLandscapeLeft();
        /*
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToLandscapeLeft = false;*/

        Debug.Log("Intro2");

        if ((!LocalizationSupport.StringsLoaded))
            LocalizationSupport.LoadStrings();


        Debug.Log("Intro3");
        m_CurState = 0;
        updateStates();


        Debug.Log("Intro4");
        bool bApple = false;
       


        Debug.Log("Intro5");
        UnityEngine.UI.Text text;
        if (Application.systemLanguage == SystemLanguage.German || true)
        {
            
            text = m_Text.GetComponent<UnityEngine.UI.Text>();
            //  text.text = "Versuche Zielpunkte zu erreichen und hilf so beim Umweltschutz in Österreich!";
            text.text = LocalizationSupport.GetString("IntroText");//"Join the green side!\nHelp #FAOUN in the monitoring process of the world’s forests.";


            /*text = m_Page2Text.GetComponent<UnityEngine.UI.Text>();
            text.text = "Jeden Tag werden in Österreich 150000 m² Land in Geschäfts-, Verkehrs-, Freizeit- und Wohnflächen umgewandelt. Dabei müssen fruchtbare Böden, Artenvielfalt und natürliche CO2-Speicher Asphalt und Beton weichen. Dies führt unter anderem dazu, dass das Risiko für Überschwemmungen steigt, landwirtschaftliche Flächen unproduktiv werden, Hitzewellen in Städten steigen und zur Erderwärmung beitragen.\n\n" +
            "Mit deiner Hilfe kann FotoQuest Go die Folgen der Veränderung unserer Landschaft aufzeichnen und dabei helfen, Österreichs Natur für zukünftige Generationen zu erhalten.\n\n" +
            "Wir haben 9000 Punkte auf ganz Österreich verteilt. Schaue auf die Karte, finde einen Punkt in deiner Nähe und folge dann den Anweisungen um eine Quest durchzufüren. Dabei musst du Bilder von der Landschaft machen und ein paar kurze Fragen beantworten.";
*/
            /*RectTransform rectTransform2 = m_Page2Text.GetComponent<RectTransform> ();
            float scalex = rectTransform2.sizeDelta.x;
            float heightentry = 1000.0f;
            rectTransform2.sizeDelta = new Vector2 (scalex, heightentry);


            float posx = rectTransform2.position.x;
            float posy = rectTransform2.position.y;
            float posz = rectTransform2.position.z;
            rectTransform2.position = new Vector3 (posx, -287.0f, posz);*/


            //  text = m_Page2Title.GetComponent<UnityEngine.UI.Text>();
            //      text.text = "Warum?";


            text = m_PageText3.GetComponent<UnityEngine.UI.Text>();
            text.text = LocalizationSupport.GetString("IntroText3");
            text = m_PageText4.GetComponent<UnityEngine.UI.Text>();
            text.text = LocalizationSupport.GetString("IntroText4");
            text = m_PageText5.GetComponent<UnityEngine.UI.Text>();
            text.text = LocalizationSupport.GetString("IntroText5");
            text = m_PageText6.GetComponent<UnityEngine.UI.Text>();
            text.text = LocalizationSupport.GetString("IntroText6");

            m_Button.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Next");//"WEITER";
        }
        else
        {


            //text = m_Page2Text.GetComponent<UnityEngine.UI.Text>();
            //  text.text = "In order to track land changes and to better evaluate the effects these changes have, the scientists at IIASA would like to run a careful examination and need your help!";
            /*  text.text = "Every day around 100.000 m² land are soil sealed in Austria. In order to track these land changes and to better evaluate the effects these changes have more data is urgently needed.\n\n" +
                    "Therefore the scientists at IIASA would like to run a careful examination and need your help to visit around 8000 points in Austria which are located on a regular 2 km grid!\n\n" +
                    "All data collected (after having gone through an anonymization process by removing all personal information like username or e-mail) will be made completely free and thus can serve as a very valuable source for science!";
    */
            /*  text.text = "Every day around 150,000 m² of soil are being turned into roads, businesses, homes and recreational areas in Austria, which leads to soil degradation and an increasing risk of flooding, water scarcity, unproductive agricultural land and heat waves in cities, contributing to global warming.\n\n" +
                    "With your help, FotoQuest Go can track the effect of these changes in our landscape and help to conserve Austria’s nature for future generations.\n\n" +
                    "We have placed around 9,000 points at a range of locations across Austria. Using the map, find a point near you and then follow the instructions to complete each quest. A quest requires you to take photographs of the landscape and answer a few short questions.";

                //, the scientists at IIASA would like to run a careful examination and need your help! In order to track land changes and to better evaluate the effects these changes have, the scientists at IIASA would like to run a careful examination and need your help!";


                text = m_Page2Title.GetComponent<UnityEngine.UI.Text>();
                text.text = "Why help?";

                */
            //Any attempt to cheat or hack will result in a ban and no money will be transferred.

            m_Button.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Next");//"NEXT";
        }

        Debug.Log("Intro6");
/*

#if UNITY_ANDROID
       
        string[] names = { "android.permission.WRITE_EXTERNAL_STORAGE", "android.permission.CAMERA", "android.permission.ACCESS_FINE_LOCATION" };
            // PermissionGranterUnity.GrantPermission("android.permission.CAMERA", PermissionGrantedCallback);
            PermissionGranterUnity.GrantPermissions(names, PermissionGrantedCallback);
#endif*/

#if UNITY_ANDROID
        //AndroidRuntimePermissions.Permission[] result = AndroidRuntimePermissions.RequestPermissions("android.permission.ACCESS_FINE_LOCATION", "android.permission.ACCESS_COARSE_LOCATION", "android.permission.CAMERA");
      /*  AndroidRuntimePermissions.Permission[] result = AndroidRuntimePermissions.RequestPermissions("android.permission.ACCESS_FINE_LOCATION", "android.permission.CAMERA");
        if(result[0] == AndroidRuntimePermissions.Permission.Granted && result[1] == AndroidRuntimePermissions.Permission.Granted)
            Debug.Log( "We have all the permissions!" );
        else
          Debug.Log( "Some permission(s) are not granted..." );

        /**/
/*        AndroidRuntimePermissions.Permission result = AndroidRuntimePermissions.RequestPermission("android.permission.CAMERA");
        if (result == AndroidRuntimePermissions.Permission.Granted)
            Debug.Log("We have permission to access external storage!");
        else
            Debug.Log("Permission state: " + result);*/
/**/

#if ASDFASDFASFD
        Debug.Log("NativeEssentials 1");
        NativeEssentials.Instance.Initialize();
        PermissionsHelper.StatusResponse sr;
        PermissionsHelper.StatusResponse sr2;
        PermissionsHelper.StatusResponse sr3;// = PermissionsHelper.StatusResponse.;//NativeEssentials.Instance.GetAndroidPermissionStatus(PermissionsHelper.Permissions.CAMERA);
        sr =NativeEssentials.Instance.GetAndroidPermissionStatus(PermissionsHelper.Permissions.ACCESS_FINE_LOCATION);
        sr2 =NativeEssentials.Instance.GetAndroidPermissionStatus(PermissionsHelper.Permissions.ACCESS_COARSE_LOCATION);
        sr3 =NativeEssentials.Instance.GetAndroidPermissionStatus(PermissionsHelper.Permissions.CAMERA);

        Debug.Log("NativeEssentials 1_1");
        if (sr == PermissionsHelper.StatusResponse.PERMISSION_RESPONSE_GRANTED && sr2 == PermissionsHelper.StatusResponse.PERMISSION_RESPONSE_GRANTED) {

        Debug.Log("NativeEssentials 2");
        } else {
            if (sr == PermissionsHelper.StatusResponse.PERMISSION_RESPONSE_GRANTED && sr2 == PermissionsHelper.StatusResponse.PERMISSION_RESPONSE_GRANTED) {

        Debug.Log("NativeEssentials 3");
            } else {

        Debug.Log("NativeEssentials 4");
                NativeEssentials.Instance.RequestAndroidPermissions(new string[] {PermissionsHelper.Permissions.ACCESS_FINE_LOCATION, PermissionsHelper.Permissions.ACCESS_COARSE_LOCATION, PermissionsHelper.Permissions.CAMERA
                });
            }
        }
#endif
#endif
    }

    // Update is called once per frame
    void Update()
    {

        //   Debug.Log("IntroUpdate1");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            int instructionshown = PlayerPrefs.GetInt("IntroductionShown");
            if (instructionshown != 0)
            {
                Application.LoadLevel("DemoMap");
            }
        }


        //   Debug.Log("IntroUpdate2");
        //========================
        // Move background image

        //#if ASDFASDFASD
        UI.Pagination.PagedRect rect;
        rect = m_Page.GetComponent<UI.Pagination.PagedRect>();
        //float procpage = rect.ScrollRect.GetOffset () / rect.ScrollRect.GetPageSize ();//rect.ScrollRect.GetTotalSize ();
        float procpage = rect.ScrollRect.GetOffset() / rect.ScrollRect.GetTotalSize();
        //  Debug.Log("proc: " + procpage + " Offset: " + rect.ScrollRect.GetOffset () + " total: " + rect.ScrollRect.GetTotalSize ());
        if (procpage < 0.0f)
            procpage = 0.0f;
        RectTransform rt;
        rt = m_BackgroundImage.GetComponent<RectTransform>();
        rt.position = new Vector3(Screen.width * -procpage * 1.0f/*2.0f*/, Screen.height * 0.5f, 0);
        //rt.position = new Vector3 (0.0f, Screen.height*0.5f, 0);
        //rt.sizeDelta = new Vector2 (Screen.width * 1.0f, Screen.width * menuheight);


        //#endif
        //    Debug.Log("IntroUpdate3");

        //===========================
        // Force app to portrait mode
        /*
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToLandscapeLeft = false;
*/
        //  Debug.Log("IntroUpdate4");
    }

    int m_CurState = 0;
    public void updateStates()
    {
        //  m_Logo.SetActive (false);
        //m_Text.SetActive (false);
        m_Text2.SetActive(false);


        if (m_CurState == 0)
        {
            m_Point1.SetActive(true);
            m_Point2.SetActive(false);
            m_Point3.SetActive(false);
            m_Point4.SetActive(false);
            m_Point5.SetActive(false);

            m_ButtonPoint1.SetActive(false);
            m_ButtonPoint2.SetActive(true);
            m_ButtonPoint3.SetActive(true);
            m_ButtonPoint4.SetActive(true);
            m_ButtonPoint5.SetActive(true);


        }
        else if (m_CurState == 1)
        {
            m_Point1.SetActive(false);
            m_Point2.SetActive(true);
            m_Point3.SetActive(false);
            m_Point4.SetActive(false);
            m_Point5.SetActive(false);


            m_ButtonPoint1.SetActive(true);
            m_ButtonPoint2.SetActive(false);
            m_ButtonPoint3.SetActive(true);
            m_ButtonPoint4.SetActive(true);
            m_ButtonPoint5.SetActive(true);
        }
        else if (m_CurState == 2)
        {
            m_Point1.SetActive(false);
            m_Point2.SetActive(false);
            m_Point3.SetActive(true);
            m_Point4.SetActive(false);
            m_Point5.SetActive(false);


            m_ButtonPoint1.SetActive(true);
            m_ButtonPoint2.SetActive(true);
            m_ButtonPoint3.SetActive(false);
            m_ButtonPoint4.SetActive(true);
            m_ButtonPoint5.SetActive(true);
        }
        else if (m_CurState == 3)
        {
            m_Point1.SetActive(false);
            m_Point2.SetActive(false);
            m_Point3.SetActive(false);
            m_Point4.SetActive(true);
            m_Point5.SetActive(false);


            m_ButtonPoint1.SetActive(true);
            m_ButtonPoint2.SetActive(true);
            m_ButtonPoint3.SetActive(true);
            m_ButtonPoint4.SetActive(false);
            m_ButtonPoint5.SetActive(true);
        }
        else if (m_CurState == 4)
        {
            m_Point1.SetActive(false);
            m_Point2.SetActive(false);
            m_Point3.SetActive(false);
            m_Point4.SetActive(false);
            m_Point5.SetActive(true);


            m_ButtonPoint1.SetActive(true);
            m_ButtonPoint2.SetActive(true);
            m_ButtonPoint3.SetActive(true);
            m_ButtonPoint4.SetActive(true);
            m_ButtonPoint5.SetActive(false);
        }
        else
        {
            m_Point1.SetActive(false);
            m_Point2.SetActive(false);
            m_Point3.SetActive(false);
            m_Point4.SetActive(false);
            m_Point5.SetActive(false);


            m_ButtonPoint1.SetActive(true);
            m_ButtonPoint2.SetActive(true);
            m_ButtonPoint3.SetActive(true);
            m_ButtonPoint4.SetActive(true);
            m_ButtonPoint5.SetActive(true);
        }
    }

    public void NextClicked()
    {
        /*   #if UNITY_ANDROID
           AndroidRuntimePermissions.Permission result = AndroidRuntimePermissions.RequestPermission("android.permission.ACCESS_FINE_LOCATION");
           if (result == AndroidRuntimePermissions.Permission.Granted)
               Debug.Log("We have permission to access external storage!");
           else
               Debug.Log("Permission state: " + result);
   #endif*/

        /*m_CurState++;
        if (m_CurState > 2) {
            m_CurState = 2;
            PlayerPrefs.SetInt ("IntroductionShown", 1);
            PlayerPrefs.Save ();
            Application.LoadLevel ("DemoMap");
        }
        updateStates ();*/
        m_CurState++;

        if (m_CurState >= 5)
        {
            m_CurState = 5;
            /*m_CurState = 2;
            */

           // int introductionshown = PlayerPrefs.GetInt("IntroductionShown");

            PlayerPrefs.SetInt("IntroductionShown", 1);
            PlayerPrefs.Save();

            /*if (instructionshown == 0) {
                Application.LoadLevel ("Instructions2");
            } else {*/
            if(PlayerPrefs.HasKey("Token")) {
                Application.LoadLevel("DemoMap");
            } else {
                Application.LoadLevel("LoginLacoWiki");
            }
                                  
           /* if (introductionshown == 1)
            {
                Application.LoadLevel("DemoMap");
            } else {
                Application.LoadLevel("LoginLacoWiki");
            }*/
            //  }
            /**/
        }
        else
        {
            UI.Pagination.PagedRect rect;
            rect = m_Page.GetComponent<UI.Pagination.PagedRect>();
            rect.SetCurrentPage(m_CurState + 1, false);
        }
        updateButtonText();

        if (m_CurState == 1)
        {
            Debug.Log("Camera permission");
        }/*
#if UNITY_ANDROID
        if (m_CurState == 1)
        {
            string[] names = { "android.permission.CAMERA", "android.permission.ACCESS_FINE_LOCATION"};
           // PermissionGranterUnity.GrantPermission("android.permission.CAMERA", PermissionGrantedCallback);
            PermissionGranterUnity.GrantPermissions(names, PermissionGrantedCallback);
        }
#endif*/
    }

    public void PermissionGrantedCallback(string permission, bool isGranted)
    {

    }

    void updateButtonText()
    {
        if (m_CurState < 4)
        {
            if (Application.systemLanguage == SystemLanguage.German)
            {
                m_Button.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Next");//"WEITER";
            }
            else
            {
                m_Button.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Next");//"NEXT";
            }
        }
        else
        {
            if (Application.systemLanguage == SystemLanguage.German)
            {
                m_Button.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("LetsGo");//"LOS GEHTS!";
            }
            else
            {
                m_Button.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("LetsGo");//"LET'S GO!";
            }
        }
    }
    public void Point1Clicked()
    {
        UI.Pagination.PagedRect rect;
        rect = m_Page.GetComponent<UI.Pagination.PagedRect>();
        rect.SetCurrentPage(1, false);
        m_CurState = 0;
        updateButtonText();
    }
    public void Point2Clicked()
    {
        UI.Pagination.PagedRect rect;
        rect = m_Page.GetComponent<UI.Pagination.PagedRect>();
        rect.SetCurrentPage(2, false);
        m_CurState = 1;
        updateButtonText();
    }
    public void Point3Clicked()
    {
        UI.Pagination.PagedRect rect;
        rect = m_Page.GetComponent<UI.Pagination.PagedRect>();
        rect.SetCurrentPage(3, false);
        m_CurState = 2;
        updateButtonText();
    }
    public void Point4Clicked()
    {
        UI.Pagination.PagedRect rect;
        rect = m_Page.GetComponent<UI.Pagination.PagedRect>();
        rect.SetCurrentPage(4, false);
        m_CurState = 3;
        updateButtonText();
    }
    public void Point5Clicked()
    {
        UI.Pagination.PagedRect rect;
        rect = m_Page.GetComponent<UI.Pagination.PagedRect>();
        rect.SetCurrentPage(5, false);
        m_CurState = 4;
        updateButtonText();
    }

    public void OnPageChanged(Page newpage, Page lastpage)
    {
        Debug.Log("> OnPageChanged: " + newpage.PageNumber);

        if (newpage.PageNumber == 1)
        {
            m_ButtonPoint1.SetActive(false);
            m_ButtonPoint2.SetActive(true);
            m_ButtonPoint3.SetActive(true);
            m_ButtonPoint4.SetActive(true);
            m_ButtonPoint5.SetActive(true);

            m_Point1.SetActive(true);
            m_Point2.SetActive(false);
            m_Point3.SetActive(false);
            m_Point4.SetActive(false);
            m_Point5.SetActive(false);
            m_CurState = 0;
        }
        else if (newpage.PageNumber == 2)
        {
            m_ButtonPoint1.SetActive(true);
            m_ButtonPoint2.SetActive(false);
            m_ButtonPoint3.SetActive(true);
            m_ButtonPoint4.SetActive(true);
            m_ButtonPoint5.SetActive(true);

            m_Point1.SetActive(false);
            m_Point2.SetActive(true);
            m_Point3.SetActive(false);
            m_Point4.SetActive(false);
            m_Point5.SetActive(false);
            m_CurState = 1;
        }
        else if (newpage.PageNumber == 3)
        {
            m_ButtonPoint1.SetActive(true);
            m_ButtonPoint2.SetActive(true);
            m_ButtonPoint3.SetActive(false);
            m_ButtonPoint4.SetActive(true);
            m_ButtonPoint5.SetActive(true);

            m_Point1.SetActive(false);
            m_Point2.SetActive(false);
            m_Point3.SetActive(true);
            m_Point4.SetActive(false);
            m_Point5.SetActive(false);
            m_CurState = 2;
        }
        else if (newpage.PageNumber == 4)
        {
            m_ButtonPoint1.SetActive(true);
            m_ButtonPoint2.SetActive(true);
            m_ButtonPoint3.SetActive(true);
            m_ButtonPoint4.SetActive(false);
            m_ButtonPoint5.SetActive(true);

            m_Point1.SetActive(false);
            m_Point2.SetActive(false);
            m_Point3.SetActive(false);
            m_Point4.SetActive(true);
            m_Point5.SetActive(false);
            m_CurState = 3;
        }
        else if (newpage.PageNumber == 5)
        {
            m_ButtonPoint1.SetActive(true);
            m_ButtonPoint2.SetActive(true);
            m_ButtonPoint3.SetActive(true);
            m_ButtonPoint4.SetActive(true);
            m_ButtonPoint5.SetActive(false);

            m_Point1.SetActive(false);
            m_Point2.SetActive(false);
            m_Point3.SetActive(false);
            m_Point4.SetActive(false);
            m_Point5.SetActive(true);
            m_CurState = 4;
        }

        updateButtonText();
    }
}
