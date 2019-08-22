using UnityEngine;
using System.Collections;
using Unitycoding.UIWidgets;

public class EventSystemStartScreen : MonoBehaviour {


	public GameObject m_ButtonLogin;
	public GameObject m_ButtonRegister;

	private MessageBox messageBox;
	private MessageBox verticalMessageBox;
	public GameObject m_ButtonBack;

	bool m_bShown = false;
	private int m_Show = 0;


	IEnumerator changeFramerate() {
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
	void Start () {
		StartCoroutine(changeFramerate());
        ForceAndFixLandscape();

		if ((!LocalizationSupport.StringsLoaded))
			LocalizationSupport.LoadStrings();

		//Screen.fullScreen = false;
		/*Screen.orientation = ScreenOrientation.Portrait;

		Screen.autorotateToPortrait = true;
		Screen.autorotateToPortraitUpsideDown = false;
		Screen.autorotateToLandscapeRight = false;
		Screen.autorotateToLandscapeLeft = false;*/

		m_bShown = false;
		m_Show = 0;

		if (PlayerPrefs.HasKey ("RegisterMsgShown")) {
			int hasshown = PlayerPrefs.GetInt ("RegisterMsgShown");
			Debug.Log ("RegisterMsgShown: " + hasshown);
			if (hasshown == 1) {
				m_bShown = true;
			} else {
				PlayerPrefs.SetInt ("RegisterMsgShown", 1);
				PlayerPrefs.Save ();
			}
		} else {
			Debug.Log ("RegisterMsgShown not set");
			PlayerPrefs.SetInt ("RegisterMsgShown", 1);
			PlayerPrefs.Save ();
		}
		m_bShown = false; // Comment this out again

		messageBox = UIUtility.Find<MessageBox> ("MessageBox");

		updateStates ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			//Application.Quit (); 
			Application.LoadLevel ("DemoMap");
		}

        /*
		Screen.orientation = ScreenOrientation.Portrait;
		Screen.autorotateToPortrait = true;
		Screen.autorotateToPortraitUpsideDown = false;
		Screen.autorotateToLandscapeRight = false;
		Screen.autorotateToLandscapeLeft = false;*/


		/*m_Show++;
		if (m_Show >= 3 && !m_bShown) {
			if (Application.systemLanguage == SystemLanguage.German ) {
				string[] options = { "Ok" };
				messageBox.Show ("", "Verwende deinen existierenden Geo-Wiki Account, oder erzeuge einen neuen, um dich bei FotoQuest einzuloggen.", options);
			} else {
				string[] options = { "Ok" };
				messageBox.Show ("", "Use your existing Geo-Wiki Account (www.geo-wiki.org) or create a new one to login.", options);
			}
			m_bShown = true;
		}*/
		//	Application.LoadLevel ("DemoMap");
	}

	public void updateStates() {
		if (Application.systemLanguage == SystemLanguage.German && false) {

			m_ButtonLogin.GetComponentInChildren<UnityEngine.UI.Text>().text = "LOGIN";
			m_ButtonRegister.GetComponentInChildren<UnityEngine.UI.Text>().text = "REGISTRIEREN";
			m_ButtonBack.GetComponentInChildren<UnityEngine.UI.Text>().text = "Zurück";
			} else {

			m_ButtonLogin.GetComponentInChildren<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString("BtnLogin");//"LOGIN";
			m_ButtonRegister.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("BtnRegister");//"REGISTER";
			m_ButtonBack.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Back");//"Back";
			}

	}

	public void LoginClicked () {

		Application.LoadLevel ("Login");
	}
	public void RegisterClicked () {
		//Application.LoadLevel ("LoginLandSense");
		Application.LoadLevel ("Register");
	}

	public void OnBackClicked() {
		Application.LoadLevel ("DemoMap");
	}
}
