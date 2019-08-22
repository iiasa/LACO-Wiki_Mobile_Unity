using UnityEngine;
using System.Collections;

public class EventSystemQuestFinished : MonoBehaviour {
	


    public GameObject m_Button;
    public GameObject m_ButtonShare;
	public GameObject m_Title;
	public GameObject m_TextCompetition;
	public GameObject m_TextDidYouKnow;


	public void ForceLandscapeLeft()
	{
		StartCoroutine(ForceAndFixLandscape());
	}

	IEnumerator ForceAndFixLandscape()
	{
		yield return new WaitForSeconds (0.01f);
		/*for (int i = 0; i < 3; i++) {
			if (i == 0) {
				Screen.orientation = ScreenOrientation.Portrait;
			}  else if (i == 1) {
				Screen.orientation = ScreenOrientation.LandscapeLeft;
			}  else {*/
		//	Screen.autorotateToPortrait = true;
		Screen.autorotateToPortraitUpsideDown = false;
		Screen.autorotateToLandscapeRight = false;
		Screen.autorotateToLandscapeLeft = false;
		Screen.orientation = ScreenOrientation.Portrait;
		Screen.autorotateToPortrait = true;
		//	}
		yield return new WaitForSeconds (0.5f);
		//}
	}

	// Use this for initialization
	void Start () {
		ForceLandscapeLeft ();
		if ((!LocalizationSupport.StringsLoaded))
			LocalizationSupport.LoadStrings();



		int nrquestsdone = 0;
		if (PlayerPrefs.HasKey ("NrQuestsDone")) {
			nrquestsdone = PlayerPrefs.GetInt ("NrQuestsDone");
		} else {
			nrquestsdone = 0;
		}


		nrquestsdone++;
		PlayerPrefs.SetInt ("NrQuestsDone", nrquestsdone);
		PlayerPrefs.Save ();



        updateStates(nrquestsdone-1);

	}
	
	// Update is called once per frame
	void Update () {
	}

	int m_CurState = 0;
	public void updateStates(int questid) {
		float money = PlayerPrefs.GetFloat ("CurQuestWeight") / 100.0f;
		string strtotal = money.ToString ("F2") + "€";//4F9F56FF//20A0DABF	

		m_Title.GetComponentInChildren<UnityEngine.UI.Text> ().text =LocalizationSupport.GetString("QuestSuccessful");// "Quest successful!";
	//	m_Text.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Your support is very important to improve nature protection!\nThank you very much!";

		/*int nrquestsdone = 0;
		if (PlayerPrefs.HasKey ("NrQuestsDone")) {
			nrquestsdone = PlayerPrefs.GetInt ("NrQuestsDone");
		} else {
			nrquestsdone = 0;
		}
		if (nrquestsdone > 4) {
			nrquestsdone = 4;
		}

		if (nrquestsdone < 4) {
			m_TextCompetition.GetComponentInChildren<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString("QuestCompetition" + nrquestsdone);//"You have successfully completed your quest. You can upload the results of your quest now or later.";
			m_Button.GetComponentInChildren<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString ("BtnContinue");
		} else {
			if (checkLoggedIn ()) {
				m_TextCompetition.GetComponentInChildren<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString("QuestCompetition" + nrquestsdone);//"You have successfully completed your quest. You can upload the results of your quest now or later.";
				m_Button.GetComponentInChildren<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString ("Upload");
			} else {
				m_TextCompetition.GetComponentInChildren<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString("QuestCompetition" + nrquestsdone + "Register");//"You have successfully completed your quest. You can upload the results of your quest now or later.";
				m_Button.GetComponentInChildren<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString ("BtnRegister");
			}
		}*/

        int trainingpoint = PlayerPrefs.GetInt("Quest_" + questid + "_TrainingPoint");
        if (trainingpoint == 0)
        {
            m_TextCompetition.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("QuestCompleted");//"You have successfully completed your quest. You can upload the results of your quest now or later.";
        } else {
            m_TextCompetition.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("AddedNewPoint");//"You have successfully completed your quest. You can upload the results of your quest now or later.";
        }
        m_Button.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("BtnContinue");
        //m_ButtonShare.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("BtnShare");
        /*
        if(PlayerPrefs.HasKey("LastImageTaken") == false) {
            m_ButtonShare.SetActive(false);
        }*/
        m_ButtonShare.SetActive(false);
		m_TextDidYouKnow.GetComponentInChildren<UnityEngine.UI.Text> ().text = "";
		//m_TextDidYouKnow.GetComponentInChildren<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString("QuestDidYouKnow" + nrquestsdone);//"You have successfully completed your quest. You can upload the results of your quest now or later.";

	}

	bool checkLoggedIn() {
		if (PlayerPrefs.HasKey ("PlayerMail") == false) {
			/*UnityEngine.Events.UnityAction<string> ua = new UnityEngine.Events.UnityAction<string> (OnMsgBoxLoginClicked);
			if (Application.systemLanguage == SystemLanguage.German ) {
				string[] options = { "Später", "Login" };
				messageBoxSmall.Show ("", "Du musst dich anmelden, um auf deine Profilseite zu gelangen.", ua, options);
			} else {
				string[] options = { "Cancel", "Login" };
				messageBoxSmall.Show ("", "You need to login to get to your profile site.", ua, options);
			}*/
			return false;
		}
		return true;
	}

	public void OnBackClicked() {
		/*PlayerPrefs.SetInt ("Questions_FromQuestFinished", 1);
		PlayerPrefs.Save ();

		Application.LoadLevel ("DynamicQuestions");*/
	}

	public void NextClicked () {
		// When clicking next quest is actually saved

		PlayerPrefs.SetInt ("QuestsBtnContinue", 1);
		PlayerPrefs.Save ();

		Application.LoadLevel ("Quests");/*
		int nrquestsdone = 0;
		if (PlayerPrefs.HasKey ("NrQuestsDone")) {
			nrquestsdone = PlayerPrefs.GetInt ("NrQuestsDone");
		} else {
			nrquestsdone = 0;
		}

		nrquestsdone++;
		PlayerPrefs.SetInt ("NrQuestsDone", nrquestsdone);
		PlayerPrefs.Save ();


		if(nrquestsdone >= 5) {
			if (checkLoggedIn ()) {
				Application.LoadLevel ("Quests");
			} else {
				PlayerPrefs.SetInt ("LoginReturnToQuests", 1);
				PlayerPrefs.Save ();
				Application.LoadLevel ("StartScreen");
			}
		} else {
			Application.LoadLevel ("DemoMap");
		}*/
    }



}
