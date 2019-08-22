using UnityEngine;
using System.Collections;
using Unitycoding.UIWidgets;
using ImaginationOverflow.UniversalDeepLinking;
using System.Runtime.InteropServices;
using UnityEngine;

public class EventSystemLoginLacoWiki : MonoBehaviour
{
#if UNITY_IOS
    [DllImport("__Internal")]
    extern static void launchUrl(string url);
    [DllImport("__Internal")]
    extern static void dismissSafariView();
#endif

    public GameObject m_ButtonBack;
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

      //  loginSuccessful("laco-wiki-app:///#access_token=eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IndwWDRxblFtTzVOWG1kbExUUXd6Vk53WWlZMCIsImtpZCI6IndwWDRxblFtTzVOWG1kbExUUXd6Vk53WWlZMCJ9.eyJpc3MiOiJodHRwczovL2Rldi5sYWNvLXdpa2kubmV0L2lkZW50aXR5IiwiYXVkIjoiaHR0cHM6Ly9kZXYubGFjby13aWtpLm5ldC9pZGVudGl0eS9yZXNvdXJjZXMiLCJleHAiOjE1NjM0NDU5MzMsIm5iZiI6MTU2MzQ0MjMzMywiY2xpZW50X2lkIjoid2ViYXBpIiwic2NvcGUiOiJ3ZWJhcGkiLCJzdWIiOiIyIiwiYXV0aF90aW1lIjoxNTYzNDQyMzMzLCJpZHAiOiJHZW9XaWtpIiwibmFtZSI6IlRvYmlhcyBTdHVybiIsImVtYWlsIjoidG9iaWFzLnN0dXJuQHZvbC5hdCIsInJvbGUiOiJVc2VyIiwiYW1yIjpbImV4dGVybmFsIl19.MeiCRFLNJM70M_YI0M1TST51V3kKr8UKRBWyupdW7HVrHiXQkAE_qe_v9sBAFIbZs4aBSZDmjAYPcEMiPg97bwepFogQTioUWAjlcF2X4kvFJvkC-KZnP67HFGH0BmCfdYNaJgSeRUn0LNMI_7hz8WgcLrT4dMkVMgwVUi_iwtPG0FD2P8EnBEm_pzFXDlDOAypJ5dVKHnb3hH8HIkVqiF6HYrMmxPy2tLMcmAbuIDybPqMD4VLA5R0vPvYaWlQCpkcuMBXrH_vXFoeGk1S45yU8e7N6kQgnOomtIL66iCu9N_xIpXZmNWzz3Os3-SWZ7-keBFcxcnDTT2D3WvtooQ&token_type=Bearer&expires_in=3600&scope=webapi");
        loginSuccessful("laco-wiki-app:///#access_token=eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IndwWDRxblFtTzVOWG1kbExUUXd6Vk53WWlZMCIsImtpZCI6IndwWDRxblFtTzVOWG1kbExUUXd6Vk53WWlZMCJ9.eyJpc3MiOiJodHRwczovL2Rldi5sYWNvLXdpa2kubmV0L2lkZW50aXR5IiwiYXVkIjoiaHR0cHM6Ly9kZXYubGFjby13aWtpLm5ldC9pZGVudGl0eS9yZXNvdXJjZXMiLCJleHAiOjE1NjU3ODMwNDAsIm5iZiI6MTU2NTc3OTQ0MCwiY2xpZW50X2lkIjoid2ViYXBpIiwic2NvcGUiOiJ3ZWJhcGkiLCJzdWIiOiI2IiwiYXV0aF90aW1lIjoxNTY1Nzc5MTIzLCJpZHAiOiJHZW9XaWtpIiwibmFtZSI6IlRvYmlhcyBTdHVybiIsImVtYWlsIjoidG9iaWFzLnN0dXJuQHZvbC5hdCIsInJvbGUiOiJVc2VyIiwiYW1yIjpbImV4dGVybmFsIl19.tf7y-AyazB_NaHhYe41yxTL32XtgdZKLR77vrU4l1cJ4Tr96ESxrZqEBS98bsqnnJGQ6YyCJBr7Tn65pdrQiXuQeK0q635QIdJhm5EzbpurDBYB6hEVWt0bOlJeIFbdkl7uFvFi5hUzQZICAPdDLdJEdXvnDdGXQ_vj9Ny3M2BRyPneqQ5RoQbHl-uh3Tf88HLsiDwM2vQ632UCbMCXhe-vU_Q3MU9D1Rgm0iB7R2fH249WhuYyerAA129PUHptAv7QysLm8WhgEBgGghFmuweGfHs7GFQeicUDv5mzUJlPU-tHjz5UoDPP5oODKF4TmQPaKAwk3IPYQS88XFwwNBQ&token_type=Bearer&expires_in=3600&scope=webapi");

        ImaginationOverflow.UniversalDeepLinking.DeepLinkManager.Instance.LinkActivated += Instance_LinkActivated;
    }

    private void Instance_LinkActivated(ImaginationOverflow.UniversalDeepLinking.LinkActivation s)
    {
        m_bLoggedInAndroid = true;
        /* m_LoadingText.GetComponent<UnityEngine.UI.Text>().text = s.Uri;// "Link opened";
         m_LoadingBack.SetActive(true);
         m_LoadingText.SetActive(true);*/

        loginSuccessful(s.Uri);
        //Debug.Log("Url: " + s.Uri);
    }


    void OnGUI()
    {
        //		windowRect = GUI.Window (0, windowRect, WindowFunction, "My Window");
    }



    void WindowFunction(int windowID)
    {
        // Draw any Controls inside the window here
    }

    bool m_bShown = false;
    bool m_bLoggedInAndroid = false;

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

    public void loginSuccessful(string data)
    {
        m_bLoggedInAndroid = true;
        m_TextResult.GetComponent<UnityEngine.UI.Text>().text = data;
        m_LoadingBack.SetActive(true);
        m_TextResult.SetActive(true);
        Debug.Log("Data: " + data);

        string token = getBetween(data, "access_token=", "&token_type");
        if (token.Length > 0)
        {
            Debug.Log("token: " + token);
            PlayerPrefs.SetString("Token", token);
            PlayerPrefs.Save();
            m_TextResult.GetComponent<UnityEngine.UI.Text>().text = "Login successful";

#if UNITY_IOS
            dismissSafariView();
#endif

            if (PlayerPrefs.GetInt("BackToUpload") == 1)
            {
                Application.LoadLevel("Quests");
            }
            else
            {
                Application.LoadLevel("Validations");
            }

        }
        //data.
        //access_token =
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.LoadLevel ("StartScreen");
		}*/

#if ADSFASDFASDF
        m_LoadingText.SetActive(true);
        m_LoadingText.GetComponent<UnityEngine.UI.Text>().text = "asf";
        string strurl = AndroidDeepLink.GetURL();
        if (m_bLoggedInAndroid == false)
        {
            /*if (strurl.CompareTo("null") != 0)
            {
                m_bLoggedInAndroid = true;
                OnOpenWithUrl(strurl);
            }
            else
            {*/
            if (strurl.CompareTo("null") != 0)
            {
                m_bLoggedInAndroid = true;
                m_LoadingText.GetComponent<UnityEngine.UI.Text>().text = strurl;
                /*  m_LoadingBack.SetActive(true);
                  m_TextResult.SetActive(true);
  */
                m_LoadingBack.SetActive(true);
                m_LoadingText.SetActive(true);
                // m_TextResult;
            }
            /*  else
              {
                  //m_TextResult.SetActive(true);
                  //m_TextResult.GetComponent<UnityEngine.UI.Text>().text = strurl;

                  m_LoadingBack.SetActive(true);
                  m_LoadingText.SetActive(true);
              }*/
            //  }
        }
#endif

#if ADSFASDFASFASFD
        string strurl = AndroidDeepLink.GetURL();
        if (m_bLoggedInAndroid == false)
        {
            /*if (strurl.CompareTo("null") != 0)
            {
                m_bLoggedInAndroid = true;
                OnOpenWithUrl(strurl);
            }
            else
            {*/
            if (strurl.CompareTo("null") != 0)
            {
                m_bLoggedInAndroid = true;
                m_TextResult.GetComponent<UnityEngine.UI.Text>().text = strurl;
                m_LoadingBack.SetActive(true);
                m_TextResult.SetActive(true);
              //  OnOpenWithUrl(strurl);
            }
            else
            {
                m_TextResult.SetActive(true);
                m_TextResult.GetComponent<UnityEngine.UI.Text>().text = strurl;
            }
          //  }
        }
#endif
    }

    public void updateStates()
    {

        m_TextTitle.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("ChooseProvider");
        m_ButtonBack.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Back");//"Back";
        m_LoadingText.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Loading");//"Loading...";

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

    public void LoginClickedFacebook()
    {
        string url = "http://dev.laco-wiki.net/identity/connect/authorize?client_id=webapi&scope=webapi&response_type=token&redirect_uri=laco-wiki-app://&acr_values=idp:Facebook";
        // string url = "https://laco-wiki.net/identity/connect/authorize?client_id=webapi&scope=webapi&response_type=token&redirect_uri=laco-wiki-app://&acr_values=idp:Facebook";

        Application.OpenURL(url);
    }
    public void LoginClickedGoogle()
    {
        /*   InAppBrowser.DisplayOptions options = new InAppBrowser.DisplayOptions();
           options.displayURLAsPageTitle = false;
           options.pageTitle = "Login";
           options.hidesHistoryButtons = true;
          // options.hidesTopBar = true;
           string url = "http://dev.laco-wiki.net/identity/connect/authorize?client_id=webapi&scope=webapi&response_type=token&redirect_uri=laco-wiki-app://&acr_values=idp:Google";

           InAppBrowser.OpenURL(url, options);
   */

        string url = "http://dev.laco-wiki.net/identity/connect/authorize?client_id=webapi&scope=webapi&response_type=token&redirect_uri=laco-wiki-app://&acr_values=idp:Google";
        // string url = "https://laco-wiki.net/identity/connect/authorize?client_id=webapi&scope=webapi&response_type=token&redirect_uri=laco-wiki-app://&acr_values=idp:Google";
        Application.OpenURL(url);
    }

    public void LoginClickedGeoWiki()
    {
        Debug.Log("LoginClickedGeoWiki");
        /*  InAppBrowser.DisplayOptions options = new InAppBrowser.DisplayOptions();
          options.displayURLAsPageTitle = false;
          options.pageTitle = "Login";
          options.hidesHistoryButtons = true;
         // options.hidesTopBar = true;
          string url = "http://dev.laco-wiki.net/identity/connect/authorize?client_id=webapi&scope=webapi&response_type=token&redirect_uri=laco-wiki-app://&acr_values=idp:GeoWiki";

          InAppBrowser.OpenURL(url, options);*/
        /**/

        //string url = "https://dev.laco-wiki.net/identity/connect/authorize?client_id=webapi&scope=webapi&response_type=token&redirect_uri=laco-wiki-app://&acr_values=idp:GeoWiki";
        string url = "https://dev.laco-wiki.net/identity/connect/authorize?client_id=webapi&scope=webapi&response_type=token&redirect_uri=laco-wiki-app://&acr_values=idp:GeoWiki";
        // string url = "https://laco-wiki.net/identity/connect/authorize?client_id=webapi&scope=webapi&response_type=token&redirect_uri=laco-wiki-app://&acr_values=idp:GeoWiki";

        Application.OpenURL(url);
#if UNITY_IOS
        Debug.Log("open embeded safari browser with geo-wiki");
        //launchUrl(url);
#else
        Debug.Log("open browser");
        Application.OpenURL(url);
#endif
        /**/

	/*	string[] options = { "OK" };
	//	messageBox.Show("Title","Message",null,null,options);
		messageBox.Show("Title","Message",options);
*/

		//messageBox.Show(

	/*	Debug.Log("LoginClicked");

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
*/

	/*
		if (Application.systemLanguage == SystemLanguage.German) {
			string[] options = { "OK" };
			messageBox.Show ("", "erwende deinen Geo-Wiki (www.geo-wiki.org) Account um dich einzuloggen.", options);
		} else {
			string[] options = { "OK" };
			messageBox.Show ("", "Use your excisting Geo-Wiki Account (www.geo-wiki.org) to login.", options);
		}*/

	}

    /*
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
	} */


}
