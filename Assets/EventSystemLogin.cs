using UnityEngine;
using System.Collections;
using Unitycoding.UIWidgets;

public class EventSystemLogin : MonoBehaviour {


	public GameObject m_ButtonBack;
	public GameObject m_TextTitle;
	public GameObject m_TextEMail;
	public GameObject m_TextPassword;
	public GameObject m_TextForgot;

	public GameObject m_ButtonLogin;


	public GameObject m_InputLogin;
	public GameObject m_InputPassword;
	public GameObject m_InputFieldLogin;
	public GameObject m_InputFieldPassword;

	private Rect windowRect = new Rect (20, 20, 120, 50);


	private MessageBox messageBox;
	private MessageBox verticalMessageBox;

	private int m_Show = 0;

	public GameObject m_TermsBack;
	public GameObject m_TermsTitle;
	public GameObject m_TermsTextBack;
	public GameObject m_TermsScrollbarAT;
	public GameObject m_TermsImageAT;
	public GameObject m_TermsScrollbarEN;
	public GameObject m_TermsImageEN;
	public GameObject m_TermsBtnAccept;
	public GameObject m_TermsBtnDecline;


	public GameObject m_LoadingBack;
	public GameObject m_LoadingText;

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
        ForceLandscapeLeft();/*
        Screen.orientation = ScreenOrientation.Portrait;

		Screen.autorotateToPortrait = true;
		Screen.autorotateToPortraitUpsideDown = false;
		Screen.autorotateToLandscapeRight = false;
		Screen.autorotateToLandscapeLeft = false;


		Screen.orientation = ScreenOrientation.Portrait;
		Screen.autorotateToPortrait = true;
		Screen.autorotateToPortraitUpsideDown = false;
		Screen.autorotateToLandscapeRight = false;
		Screen.autorotateToLandscapeLeft = false;*/


		if ((!LocalizationSupport.StringsLoaded))
			LocalizationSupport.LoadStrings();

		updateStates ();


		m_LoadingText.SetActive (false);
		m_LoadingBack.SetActive (false);
			messageBox = UIUtility.Find<MessageBox> ("MessageBox");
		//	verticalMessageBox = UIUtility.Find<MessageBox> ("VerticalMessageBox");

		/*
			messageBox.Show(title,message,icon,null,options);
		*/


		if (messageBox == null) {
			Debug.Log ("No message box set");
		} else {
			Debug.Log ("Message set");
		}

		hideTerms ();

	}


	void OnGUI () {
//		windowRect = GUI.Window (0, windowRect, WindowFunction, "My Window");
	}



	void WindowFunction (int windowID) {
		// Draw any Controls inside the window here
	}

	bool m_bShown = false;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.LoadLevel ("StartScreen");
		}
		/*
		if (Input.GetKeyDown(KeyCode.Escape)) 
			Application.LoadLevel ("DemoMap");*/
		
		/*m_Show++;
		if (m_Show >= 3 && !m_bShown) {
			if (Application.systemLanguage == SystemLanguage.German) {
				string[] options = { "Ok" };
				messageBox.Show ("", "Verwende deinen Geo-Wiki Account (www.geo-wiki.org) um dich einzuloggen.", options);
			} else {
				string[] options = { "Ok" };
				messageBox.Show ("", "Use your excisting Geo-Wiki Account (www.geo-wiki.org) to login.", options);
			}
			m_bShown = true;
		}*/
	}

	public void updateStates() {
		if (Application.systemLanguage == SystemLanguage.German) {
			m_TextTitle.GetComponentInChildren<UnityEngine.UI.Text>().text = "Login";

			m_ButtonLogin.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("LoginLogin");//"EINLOGGEN";
			m_TextForgot.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("LoginForgotPassword");//"Passwort vergessen?";
			m_TextEMail.GetComponent<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString("LoginMail");//"E-Mail:";
			m_TextPassword.GetComponent<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString("LoginPassword");//"Passwort:";

			m_ButtonBack.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Back");//"Zurück";

			m_LoadingText.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Loading");//"Laden...";
		} else {
			m_TextTitle.GetComponentInChildren<UnityEngine.UI.Text>().text = "Login";
		
			m_ButtonLogin.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("LoginLogin");//"LOGIN";
			m_TextForgot.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("LoginForgotPassword");//"Forgot password?";
			m_TextEMail.GetComponent<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString("LoginMail");//"E-Mail:";
			m_TextPassword.GetComponent<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString("LoginPassword");//"Password:";
			m_ButtonBack.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Back");//"Back";
			m_LoadingText.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Loading");//"Loading...";
		}

		if (Application.systemLanguage == SystemLanguage.German ) {
			m_TermsTitle.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("MenuTerms");//"Teilnahmebedingungen";


			m_TermsBtnAccept.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Accept");//"Annehmen";
			m_TermsBtnDecline.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Decline");//"Ablehnen";
		} else {
			m_TermsTitle.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("MenuTerms");//"Terms and Conditions";

			m_TermsBtnAccept.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Accept");//"Accept";
			m_TermsBtnDecline.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Decline");//"Decline";
		}
	}

	public static string ComputeHash(string s){
		// Form hash
		System.Security.Cryptography.MD5 h = System.Security.Cryptography.MD5.Create();
		byte[] data = h.ComputeHash(System.Text.Encoding.Default.GetBytes(s));
		// Create string representation
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		for (int i = 0; i < data.Length; ++i) {
			sb.Append(data[i].ToString("x2"));
		}
		return sb.ToString();
	}

	public void LoginClicked () {
	/*	string[] options = { "OK" };
	//	messageBox.Show("Title","Message",null,null,options);
		messageBox.Show("Title","Message",options);
*/

		//messageBox.Show(

		Debug.Log("LoginClicked");

		//string user = m_InputLogin.GetComponent<UnityEngine.UI.Text> ().text;
	//	string password = m_InputPassword.GetComponent<UnityEngine.UI.InputField> ().text;
		UnityEngine.UI.InputField inputfield = m_InputFieldLogin.GetComponent<UnityEngine.UI.InputField> ();
		string user = inputfield.text;
		Debug.Log ("user2: " + user);


		UnityEngine.UI.InputField textinput;
		textinput = m_InputPassword.GetComponent<UnityEngine.UI.InputField>();
		string password = textinput.text;

		string value = user + "," + password;
		string[] options = { "OK" };
		//messageBox.Show ("", value, options);

		if (user.Length <= 0) {
			if (Application.systemLanguage == SystemLanguage.German) {
				messageBox.Show ("", LocalizationSupport.GetString("LoginNoMail"), options);
			} else {
				messageBox.Show ("", LocalizationSupport.GetString("LoginNoMail"), options);
			}
			return;
		}

		if (password.Length <= 0) {
			if (Application.systemLanguage == SystemLanguage.German) {
				messageBox.Show ("", LocalizationSupport.GetString("LoginNoPassword"), options);
			} else {
				messageBox.Show ("", LocalizationSupport.GetString("LoginNoPassword"), options);
			}
			return;
		}

//		showTerms();
		acceptedTerms ();


	/*
		if (Application.systemLanguage == SystemLanguage.German) {
			string[] options = { "OK" };
			messageBox.Show ("", "erwende deinen Geo-Wiki (www.geo-wiki.org) Account um dich einzuloggen.", options);
		} else {
			string[] options = { "OK" };
			messageBox.Show ("", "Use your excisting Geo-Wiki Account (www.geo-wiki.org) to login.", options);
		}*/

	}


	IEnumerator WaitForData(WWW www)
	{
		yield return www;

		string[] options = { "OK" };


		// check for errors
		if (www.error == null)
		{
			string data = www.text;
			//string[] parts = data.Split (":", 2);

			string[] parts = data.Split(new string[] { ":" }, 0);
			string[] parts2 = parts[1].Split(new string[] { "," }, 0);
			string part3 = parts2 [0];

			Debug.Log("WWW Ok!: " + www.text);
			Debug.Log("part1: " + parts[0]);
			Debug.Log("part2: " + parts[1]);
			Debug.Log("part3: " + part3);

			part3 = part3.Replace ("\"", "");
			part3 = part3.Replace ("}", "");

			Debug.Log ("parts2 len: " + parts2.Length);
			if (parts.Length > 2) {
				string part4 = parts [2];
				Debug.Log ("Part4: " + part4);


				part4 = part4.Replace ("\"", "");
				part4 = part4.Replace ("}", "");


				Debug.Log ("Part4: " + part4);

				PlayerPrefs.SetString("PlayerName",part4);
			}

			if (part3.Equals ("null")) {

				m_LoadingText.SetActive (false);
				m_LoadingBack.SetActive (false);

				if (Application.systemLanguage == SystemLanguage.German) {
					messageBox.Show ("", LocalizationSupport.GetString("LoginFailed"), options);
				} else {
					messageBox.Show ("", LocalizationSupport.GetString("LoginFailed"), options);
				}
				yield return www;
			} else {

				/*if (Application.systemLanguage == SystemLanguage.German) {
					messageBox.Show ("", "Einloggen erfolgreich.", options);
				} else {
					messageBox.Show ("", "Login successful.", options);
				}*/

				PlayerPrefs.SetString("PlayerId",part3);


				UnityEngine.UI.InputField textinput;
				textinput = m_InputPassword.GetComponent<UnityEngine.UI.InputField>();
				string password = textinput.text;

				PlayerPrefs.SetString("PlayerPassword",password);

				UnityEngine.UI.InputField inputfield = m_InputFieldLogin.GetComponent<UnityEngine.UI.InputField> ();
				string mail = inputfield.text;
				//string mail = m_InputLogin.GetComponent<UnityEngine.UI.Text> ().text;
				PlayerPrefs.SetString("PlayerMail",mail);


				PlayerPrefs.SetInt ("LoggedOut", 0);

				PlayerPrefs.Save ();

				Debug.Log ("Saved Mail: " + mail + " password: " + password);

				bool bDontGoToQuestsPage = false;
				if (PlayerPrefs.HasKey ("LoginReturnToQuests")) {
					int returntoquests = PlayerPrefs.GetInt ("LoginReturnToQuests");
					if (returntoquests == 1) {
						Application.LoadLevel ("Quests");
						bDontGoToQuestsPage = true;
						yield return www;
					}
				}

				if (bDontGoToQuestsPage == false) {
					Application.LoadLevel ("DemoMap");
				}
			}


		} else {
			Debug.Log("WWW Error: "+ www.error);
			Debug.Log("WWW Error 2: "+ www.text);


			m_LoadingText.SetActive (false);
			m_LoadingBack.SetActive (false);

			if (Application.systemLanguage == SystemLanguage.German) {
				messageBox.Show ("", LocalizationSupport.GetString("LoginFailedNoInternet"), options);
			} else {
				messageBox.Show ("", LocalizationSupport.GetString("LoginFailedNoInternet"), options);
			}
		}   
	} 


	public void RegisterClicked () {
	}
	public void OnBackClicked () {
		Application.LoadLevel ("StartScreen");
	}
	public void OnForgotClicked () {
        Application.OpenURL("https://application.geo-wiki.org/Security/lostpassword");
	}


	public void hideTerms()
	{
		m_TermsBack.SetActive (false);
		m_TermsTitle.SetActive (false);
		m_TermsScrollbarAT.SetActive (false);
		m_TermsTextBack.SetActive (false);
		m_TermsImageAT.SetActive (false);
		m_TermsBtnAccept.SetActive (false);
		m_TermsBtnDecline.SetActive (false);
		m_TermsScrollbarEN.SetActive (false);
		m_TermsImageEN.SetActive (false);
	}
	void showTerms()
	{
		m_TermsBack.SetActive (true);
		m_TermsTitle.SetActive (true);

		if (Application.systemLanguage == SystemLanguage.German ) {
			m_TermsScrollbarAT.SetActive (true);
			m_TermsImageAT.SetActive (true);
			m_TermsScrollbarEN.SetActive (false);
			m_TermsImageEN.SetActive (false);
		} else {
			m_TermsScrollbarAT.SetActive (false);
			m_TermsImageAT.SetActive (false);
			m_TermsScrollbarEN.SetActive (true);
			m_TermsImageEN.SetActive (true);
		}

		m_TermsTextBack.SetActive (true);
		m_TermsBtnAccept.SetActive (true);
		m_TermsBtnDecline.SetActive (true);
	}

	public void acceptedTerms()
	{
		UnityEngine.UI.InputField inputfield = m_InputFieldLogin.GetComponent<UnityEngine.UI.InputField> ();
		string user = inputfield.text;
		Debug.Log ("user2: " + user);


		UnityEngine.UI.InputField textinput;
		textinput = m_InputPassword.GetComponent<UnityEngine.UI.InputField>();
		string password = textinput.text;

		string value = user + "," + password;
		string[] options = { "OK" };
		//messageBox.Show ("", value, options);

		if (user.Length <= 0) {
			if (Application.systemLanguage == SystemLanguage.German) {
				messageBox.Show ("", LocalizationSupport.GetString("LoginNoMail"), options);
			} else {
				messageBox.Show ("", LocalizationSupport.GetString("LoginNoMail"), options);
			}
			return;
		}

		if (password.Length <= 0) {
			if (Application.systemLanguage == SystemLanguage.German) {
				messageBox.Show ("", LocalizationSupport.GetString("LoginNoPassword"), options);
			} else {
				messageBox.Show ("", LocalizationSupport.GetString("LoginNoPassword"), options);
			}
			return;
		}


		m_LoadingText.SetActive (true);
		m_LoadingBack.SetActive (true);
		hideTerms ();

		string passwordmd5 = ComputeHash (password);

		string url = "https://geo-wiki.org/Application/api/User/checkCredentials";
		string param = "";
		param += "{\"username\":\"" + user + "\",\"passwordMD5\":\"" + passwordmd5 + "\"";
		param += "}";



		Debug.Log ("login param: " + param);


		WWWForm form = new WWWForm();
		form.AddField ("parameter", param);

		//Debug.Log ("Url data: " + System.Text.Encoding.UTF8.GetString(form.data));
		WWW www = new WWW(url, form);

		StartCoroutine(WaitForData(www));
	}

	public void declineTerms()
	{
		hideTerms ();
	}
}
