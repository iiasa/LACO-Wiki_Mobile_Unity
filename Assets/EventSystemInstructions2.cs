using UnityEngine;
using System.Collections;

#if DONTUSESIGNAL
using Signalphire;
#endif
using UI.Pagination;

public class EventSystemInstructions2 : MonoBehaviour {

	public GameObject m_Text;
	public GameObject m_Text2;
	public GameObject m_Logo;


	public GameObject m_Page1Text;
	public GameObject m_Page2Text;
	public GameObject m_Page3Text;
	public GameObject m_Page4Text;
	public GameObject m_Page5Text;

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

	public UI.Pagination.PagedRect_ScrollRect m_ScrollRect;
	public GameObject m_Page;


	IEnumerator changeFramerate() {
		yield return new WaitForSeconds(1);
		Application.targetFrameRate = 30;
	}


    // Use this for initialization
    void Start()
    {
        StartCoroutine(changeFramerate());

        if ((!LocalizationSupport.StringsLoaded))
            LocalizationSupport.LoadStrings();

        Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToLandscapeLeft = false;


        m_CurState = 0;
        updateStates();

        UnityEngine.UI.Text text;
        if (Application.systemLanguage == SystemLanguage.German)
        {

            text = m_Page1Text.GetComponent<UnityEngine.UI.Text>();
            text.text = LocalizationSupport.GetString("Instructions1");

            text = m_Page2Text.GetComponent<UnityEngine.UI.Text>();
            text.text = LocalizationSupport.GetString("Instructions2");

            text = m_Page3Text.GetComponent<UnityEngine.UI.Text>();
            text.text = LocalizationSupport.GetString("Instructions3");

            text = m_Page4Text.GetComponent<UnityEngine.UI.Text>();
            text.text = LocalizationSupport.GetString("Instructions4");

            text = m_Page5Text.GetComponent<UnityEngine.UI.Text>();
            text.text = LocalizationSupport.GetString("Instructions5");




            m_Button.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Next");
        }
        else
        {

            text = m_Page1Text.GetComponent<UnityEngine.UI.Text>();
            text.text = LocalizationSupport.GetString("Instructions1");

            text = m_Page2Text.GetComponent<UnityEngine.UI.Text>();
            text.text = LocalizationSupport.GetString("Instructions2");

            text = m_Page3Text.GetComponent<UnityEngine.UI.Text>();
            text.text = LocalizationSupport.GetString("Instructions3");

            text = m_Page4Text.GetComponent<UnityEngine.UI.Text>();
            text.text = LocalizationSupport.GetString("Instructions4");

            text = m_Page5Text.GetComponent<UnityEngine.UI.Text>();
            text.text = LocalizationSupport.GetString("Instructions5");


            m_Button.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Next");
        }

        //PlayerPrefs.SetInt ("InstructionShown", 1);
        //PlayerPrefs.Save ();

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
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			int instructionshown = PlayerPrefs.GetInt ("InstructionShown");
			if (instructionshown != 0) {
				Application.LoadLevel ("DemoMap");
			}
		}


		//===========================
		// Force app to portrait mode

		Screen.orientation = ScreenOrientation.Portrait;
		Screen.autorotateToPortrait = true;
		Screen.autorotateToPortraitUpsideDown = false;
		Screen.autorotateToLandscapeRight = false;
		Screen.autorotateToLandscapeLeft = false;
	}

	int m_CurState = 0;
	public void updateStates() {
		m_Logo.SetActive (false);
		m_Text.SetActive (false);
		m_Text2.SetActive (false);


		if (m_CurState == 0) {
			m_Point1.SetActive (true);
			m_Point2.SetActive (false);
			m_Point3.SetActive (false);
			m_Point4.SetActive (false);
			m_Point5.SetActive (false);

			//m_Logo.SetActive (true);

			m_ButtonPoint1.SetActive (false);
			m_ButtonPoint2.SetActive (true);
			m_ButtonPoint3.SetActive (true);
			m_ButtonPoint4.SetActive (true);
			m_ButtonPoint5.SetActive (true);


		/*	m_Text.SetActive (true);
			m_Text2.SetActive (false);


			UnityEngine.UI.Text text;
			text = m_Text.GetComponent<UnityEngine.UI.Text>();
			if (Application.systemLanguage == SystemLanguage.German ){
				text.text = "Unterstütze die Wissenschaft beim Umweltschutz mit deinem Smartphone!";

				m_Button.GetComponentInChildren<UnityEngine.UI.Text>().text = "Weiter";
			} else {
				text.text = "Support science with your smartphone and help to improve the landscape conservation!";

				m_Button.GetComponentInChildren<UnityEngine.UI.Text>().text = "Next";
			}  */
		} else if (m_CurState == 1) {
			m_Point1.SetActive (false);
			m_Point2.SetActive (true);
			m_Point3.SetActive (false);
			m_Point4.SetActive (false);
			m_Point5.SetActive (false);

			//m_Logo.SetActive (false);



			/*m_Text.SetActive (false);
			m_Text2.SetActive (true);
			UnityEngine.UI.Text text;
			text = m_Text2.GetComponent<UnityEngine.UI.Text>();
			if (Application.systemLanguage == SystemLanguage.German ) {
				text.text = "Um die Veränderungen von Landflächen und deren Auswirkungen auf die Umwelt besser nachverfolgen und verstehen zu können, möchten die Forscher des IIASA eine sorgfältige Bestandsaufnahme durchführen und benötigen dabei deine Hilfe!";


				m_Button.GetComponentInChildren<UnityEngine.UI.Text>().text = "Weiter";
			} else {
				text.text = "In order to track land changes and to better evaluate the effects these changes have, the scientists at IIASA would like to run a careful examination and need your help!";
			

				m_Button.GetComponentInChildren<UnityEngine.UI.Text>().text = "Next";
			}  */

			m_ButtonPoint1.SetActive (true);
			m_ButtonPoint2.SetActive (false);
			m_ButtonPoint3.SetActive (true);
			m_ButtonPoint4.SetActive (true);
			m_ButtonPoint5.SetActive (true);
		} else if (m_CurState == 2) {
			m_Point1.SetActive (false);
			m_Point2.SetActive (false);
			m_Point3.SetActive (true);
			m_Point4.SetActive (false);
			m_Point5.SetActive (false);

			m_ButtonPoint1.SetActive (true);
			m_ButtonPoint2.SetActive (true);
			m_ButtonPoint3.SetActive (false);
			m_ButtonPoint4.SetActive (true);
			m_ButtonPoint5.SetActive (true);
		} else if (m_CurState == 3) {
			m_Point1.SetActive (false);
			m_Point2.SetActive (false);
			m_Point3.SetActive (false);
			m_Point4.SetActive (true);
			m_Point5.SetActive (false);

			m_ButtonPoint1.SetActive (true);
			m_ButtonPoint2.SetActive (true);
			m_ButtonPoint3.SetActive (true);
			m_ButtonPoint4.SetActive (false);
			m_ButtonPoint5.SetActive (true);
		} else if (m_CurState == 4) {
			m_Point1.SetActive (false);
			m_Point2.SetActive (false);
			m_Point3.SetActive (false);
			m_Point4.SetActive (false);
			m_Point5.SetActive (true);

			m_ButtonPoint1.SetActive (true);
			m_ButtonPoint2.SetActive (true);
			m_ButtonPoint3.SetActive (true);
			m_ButtonPoint4.SetActive (true);
			m_ButtonPoint5.SetActive (false);
		}
	}

	public void NextClicked () {

		/*m_CurState++;
		if (m_CurState > 2) {
			m_CurState = 2;
			PlayerPrefs.SetInt ("IntroductionShown", 1);
			PlayerPrefs.Save ();
			Application.LoadLevel ("DemoMap");
		}
		updateStates ();*/
		m_CurState++;
		if (m_CurState > 4) {
			m_CurState = 4;

			PlayerPrefs.SetInt ("InstructionShown", 1);
			PlayerPrefs.Save ();

			Application.LoadLevel ("DemoMap");
		} else {
			UI.Pagination.PagedRect rect;
			rect = m_Page.GetComponent<UI.Pagination.PagedRect>();
			rect.SetCurrentPage (m_CurState+1, false);
		}
		updateButtonText ();
	}

	void updateButtonText()
	{
		if (m_CurState < 4) {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_Button.GetComponentInChildren<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString("Next");
			} else {
				m_Button.GetComponentInChildren<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString("Next");
			}
		} else {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_Button.GetComponentInChildren<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString("Ok");
			} else {
				m_Button.GetComponentInChildren<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString("Ok");
			}
		}
	}
	public void Point1Clicked () {
		UI.Pagination.PagedRect rect;
		rect = m_Page.GetComponent<UI.Pagination.PagedRect>();
		rect.SetCurrentPage (1, false);
		m_CurState = 0;
		updateButtonText ();
	}
	public void Point2Clicked () {
		UI.Pagination.PagedRect rect;
		rect = m_Page.GetComponent<UI.Pagination.PagedRect>();
		rect.SetCurrentPage (2, false);
		m_CurState = 1;
		updateButtonText ();
	}
	public void Point3Clicked () {
		UI.Pagination.PagedRect rect;
		rect = m_Page.GetComponent<UI.Pagination.PagedRect>();
		rect.SetCurrentPage (3, false);
		m_CurState = 2;
		updateButtonText ();
	}
	public void Point4Clicked () {
		UI.Pagination.PagedRect rect;
		rect = m_Page.GetComponent<UI.Pagination.PagedRect>();
		rect.SetCurrentPage (4, false);
		m_CurState = 3;
		updateButtonText ();
	}
	public void Point5Clicked () {
		UI.Pagination.PagedRect rect;
		rect = m_Page.GetComponent<UI.Pagination.PagedRect>();
		rect.SetCurrentPage (5, false);
		m_CurState = 4;
		updateButtonText ();
	}

	public void OnPageChanged (Page newpage, Page lastpage) {
		Debug.Log ("> OnPageChanged: " + newpage.PageNumber);

		if (newpage.PageNumber == 1) {
			m_ButtonPoint1.SetActive (false);
			m_ButtonPoint2.SetActive (true);
			m_ButtonPoint3.SetActive (true);
			m_ButtonPoint4.SetActive (true);
			m_ButtonPoint5.SetActive (true);

			m_Point1.SetActive (true);
			m_Point2.SetActive (false);
			m_Point3.SetActive (false);
			m_Point4.SetActive (false);
			m_Point5.SetActive (false);
			m_CurState = 0;
		} else if (newpage.PageNumber == 2) {
			m_ButtonPoint1.SetActive (true);
			m_ButtonPoint2.SetActive (false);
			m_ButtonPoint3.SetActive (true);
			m_ButtonPoint4.SetActive (true);
			m_ButtonPoint5.SetActive (true);

			m_Point1.SetActive (false);
			m_Point2.SetActive (true);
			m_Point3.SetActive (false);
			m_Point4.SetActive (false);
			m_Point5.SetActive (false);
			m_CurState = 1;
		} else if (newpage.PageNumber == 3) {
			m_ButtonPoint1.SetActive (true);
			m_ButtonPoint2.SetActive (true);
			m_ButtonPoint3.SetActive (false);
			m_ButtonPoint4.SetActive (true);
			m_ButtonPoint5.SetActive (true);

			m_Point1.SetActive (false);
			m_Point2.SetActive (false);
			m_Point3.SetActive (true);
			m_Point4.SetActive (false);
			m_Point5.SetActive (false);
			m_CurState = 2;
		} else if (newpage.PageNumber == 4) {
			m_ButtonPoint1.SetActive (true);
			m_ButtonPoint2.SetActive (true);
			m_ButtonPoint3.SetActive (true);
			m_ButtonPoint4.SetActive (false);
			m_ButtonPoint5.SetActive (true);

			m_Point1.SetActive (false);
			m_Point2.SetActive (false);
			m_Point3.SetActive (false);
			m_Point4.SetActive (true);
			m_Point5.SetActive (false);
			m_CurState = 3;
		} else if (newpage.PageNumber == 5) {
			m_ButtonPoint1.SetActive (true);
			m_ButtonPoint2.SetActive (true);
			m_ButtonPoint3.SetActive (true);
			m_ButtonPoint4.SetActive (true);
			m_ButtonPoint5.SetActive (false);

			m_Point1.SetActive (false);
			m_Point2.SetActive (false);
			m_Point3.SetActive (false);
			m_Point4.SetActive (false);
			m_Point5.SetActive (true);
			m_CurState = 4;
		}

		updateButtonText ();
	}
}
