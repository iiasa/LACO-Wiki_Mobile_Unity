using UnityEngine;
using System.Collections;

public class EventSystemEntry : MonoBehaviour {

	public GameObject m_Instructions;

	// Use this for initialization
	void Start () {
		if ((!LocalizationSupport.StringsLoaded))
			LocalizationSupport.LoadStrings();
		
		Screen.orientation = ScreenOrientation.Portrait;
		Screen.autorotateToPortrait = true;
		Screen.autorotateToPortraitUpsideDown = false;
		Screen.autorotateToLandscapeRight = false;
		Screen.autorotateToLandscapeLeft = false;

        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();

        int introshown = PlayerPrefs.GetInt("IntroductionShown");
        if (introshown == 0)
        {
            Application.LoadLevel("Introduction");
            return;
        }

        if (PlayerPrefs.HasKey("Token"))
        {
            bool isSessionActive = false;
            string sessions = PlayerPrefs.GetString("ActiveSessions");
            string newsessions = "";
            string[] splitArray = sessions.Split(char.Parse(" "));
            for (int i = 0; i < splitArray.Length; i++)
            {
                string valid = splitArray[i];
                if (valid != "" && valid != " ")
                {
                    isSessionActive = true;
                }
            }

            if (isSessionActive == false)
            {
                Application.LoadLevel("Validations");
            }
            else
            {
                Application.LoadLevel("DemoMap");
            }
        }
        else
        {
            Application.LoadLevel("LoginLacoWiki");
        }

        /*
        if (PlayerPrefs.HasKey("LoggedOut"))
        {
            int loggedout = PlayerPrefs.GetInt("LoggedOut");
            if (loggedout == 1)
            {
                Application.LoadLevel("StartScreen");
                return;
            }
        }


        if (PlayerPrefs.HasKey("NrQuestsDone"))
        {
            int nrquestsdone = PlayerPrefs.GetInt("NrQuestsDone");
            Application.LoadLevel("DemoMap");
            return;
        }

        if (!PlayerPrefs.HasKey("IntroductionShown"))
        {
            Application.LoadLevel("Introduction");
        }
        else
        {
            Application.LoadLevel("DemoMap");
        }*/

	}

	void Update()
	{
		/*Screen.orientation = ScreenOrientation.Portrait;
		Screen.autorotateToPortrait = true;
		Screen.autorotateToPortraitUpsideDown = false;
		Screen.autorotateToLandscapeRight = false;
		Screen.autorotateToLandscapeLeft = false;*/
	}

	IEnumerator ShowSomeTime() {
		yield return new WaitForSeconds(3.0f);

      //  Application.LoadLevel("DemoMap");

		//Application.LoadLevel ("StartScreen");/**/
		if (PlayerPrefs.HasKey ("LoggedOut")) {
			int loggedout = PlayerPrefs.GetInt ("LoggedOut");
			if (loggedout == 1) {
				Application.LoadLevel ("StartScreen");
				//return;
				yield return null;
			}
		}



		if (PlayerPrefs.HasKey ("NrQuestsDone")) {
			int nrquestsdone = PlayerPrefs.GetInt ("NrQuestsDone");
			Application.LoadLevel ("DemoMap");
			//return;
			yield return null;
		} 

		if (!PlayerPrefs.HasKey ("IntroductionShown")) {
			Application.LoadLevel ("Introduction");
		} else {
			Application.LoadLevel ("DemoMap");
		}/**/
	}

}
