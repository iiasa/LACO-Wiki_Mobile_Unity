using UnityEngine;
using System.Collections;

public class EventSystemContact : MonoBehaviour {
	


	public GameObject m_Button;
	public GameObject m_ButtonContact;
	public GameObject m_ButtonHomepage;
	public GameObject m_TextCopyright;


	IEnumerator changeFramerate() {
		yield return new WaitForSeconds(1);
		Application.targetFrameRate = 30;
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(changeFramerate());
		m_DebugIter = 0;

		m_ButtonDebug.SetActive (false);
		if (PlayerPrefs.HasKey ("DebugEnabled")) {
			if (PlayerPrefs.GetInt ("DebugEnabled") == 1) {
				m_ButtonDebug.SetActive (true);
			}
		}

		if ((!LocalizationSupport.StringsLoaded))
			LocalizationSupport.LoadStrings();

		updateStates ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) 
			Application.LoadLevel ("DemoMap");
	}

	int m_CurState = 0;
	public void updateStates() {
		

		if (Application.systemLanguage == SystemLanguage.German) {
			m_Button.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("BtnBack");
			m_TextCopyright.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("About");//"FRA Quest ist ein Projekt des Internationalen Instituts für Angewandte Systemanalyse (IIASA) in Laxenburg bei Wien.\n\n©2017 IIASA.";
		} else {
			m_Button.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("BtnBack");
			m_TextCopyright.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("About");//"FRA Quest is developed by the International Institute for Applied Systems Analysis (IIASA) based in Laxenburg, Austria.\n\n©2017 IIASA.";
		}

	}

	public void NextClicked () {
		Application.LoadLevel ("DemoMap");
	}

	public void onHomepageClicked() {
		Application.OpenURL("http://www.fotoquest-go.org");
	}

	public void SendEmail (string email,string subject,string body)
	{
		subject = MyEscapeURL(subject);
		//body = MyEscapeURL(body);
		Application.OpenURL ("mailto:" + email + "?subject=" + subject);// + "&body=" + body);
	}
	public string MyEscapeURL (string url)
	{
		return WWW.EscapeURL(url).Replace("+","%20");
	}

	public void onContactUsClicked() {
		SendEmail ("info@fotoquest.at", "FotoQuestGo", "Text");
	}

	int m_DebugIter = 0;
	public GameObject m_ButtonDebug;

	public void onDebugClicked() {
		Debug.Log ("onDebugClicked");
		m_DebugIter++;
		if (m_DebugIter >= 15) {
			m_DebugIter = 0;
			PlayerPrefs.SetInt ("DebugEnabled", 1);
			PlayerPrefs.Save ();
			m_ButtonDebug.SetActive (true);
		}
	}

	public void onDisableDebug() {
		PlayerPrefs.SetInt ("DebugEnabled", 0);
		PlayerPrefs.Save ();
		m_ButtonDebug.SetActive (false);
	}

}
