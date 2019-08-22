using UnityEngine;
using System.Collections;
using Unitycoding.UIWidgets;

public class EventSystemTerms : MonoBehaviour {


	public GameObject m_ButtonBack;








	public GameObject m_TermsTitle;
	public GameObject m_TermsTextBack;
	public GameObject m_TermsScrollbarAT;
	public GameObject m_TermsImageAT;
	public GameObject m_TermsScrollbarEN;
	public GameObject m_TermsImageEN;


	IEnumerator changeFramerate() {
		yield return new WaitForSeconds(1);
		Application.targetFrameRate = 30;
	}


	// Use this for initialization
	void Start () {
		StartCoroutine(changeFramerate());

		if ((!LocalizationSupport.StringsLoaded))
			LocalizationSupport.LoadStrings();

		Screen.orientation = ScreenOrientation.Portrait;

		Screen.autorotateToPortrait = true;
		Screen.autorotateToPortraitUpsideDown = false;
		Screen.autorotateToLandscapeRight = false;
		Screen.autorotateToLandscapeLeft = false;



		updateStates ();


	}




	void WindowFunction (int windowID) {
		// Draw any Controls inside the window here
	}

	bool m_bShown = false;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.LoadLevel ("DemoMap");
		}


		Screen.orientation = ScreenOrientation.Portrait;
		Screen.autorotateToPortrait = true;
		Screen.autorotateToPortraitUpsideDown = false;
		Screen.autorotateToLandscapeRight = false;
		Screen.autorotateToLandscapeLeft = false;
	}

	public void updateStates() {
		if (Application.systemLanguage == SystemLanguage.German && false) {
			
			m_ButtonBack.GetComponentInChildren<UnityEngine.UI.Text>().text = "Schließen";

			m_TermsTitle.GetComponentInChildren<UnityEngine.UI.Text>().text = "Teilnahmebedingungen";
		} else {
			m_ButtonBack.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Close");//"Close";

			m_TermsTitle.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("MenuTerms");//"Terms and Conditions";
		}

		showTerms ();
	}


	public void hideTerms()
	{
		m_TermsTitle.SetActive (false);
		m_TermsScrollbarAT.SetActive (false);
		m_TermsTextBack.SetActive (false);
		m_TermsImageAT.SetActive (false);
		m_TermsScrollbarEN.SetActive (false);
		m_TermsImageEN.SetActive (false);
	}
	void showTerms()
	{
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
	}

	public void OnBackClicked () {
		Application.LoadLevel ("DemoMap");
	}
}
