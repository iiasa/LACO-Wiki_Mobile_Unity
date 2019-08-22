using UnityEngine;
using System.Collections;

public class EventSystemLeaderboard : MonoBehaviour {
	


	public GameObject m_Button;
	public GameObject m_Dropdown;
	public GameObject m_TextInfo;

	public GameObject m_Loading;

	public GameObject m_Content;
	public GameObject m_Rank;
	public GameObject m_LName;
	public GameObject m_Score;
	public GameObject m_RankS;
	public GameObject m_NameS;
	public GameObject m_ScoreS;

	ArrayList m_AddedTexts;


	IEnumerator changeFramerate() {
		yield return new WaitForSeconds(1);
		Application.targetFrameRate = 30;
	}


	// Use this for initialization
	void Start () {
		StartCoroutine(changeFramerate());


		if ((!LocalizationSupport.StringsLoaded))
			LocalizationSupport.LoadStrings();
		

		m_CurSelection = 0;
		m_AddedTexts = new ArrayList();
		UpdateText ();
		updateStates ();

		loadLeaderboard ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) 
			Application.LoadLevel ("DemoMap");
	}

	int m_CurSelection = 0;
	void UpdateText() {
		/*if (Application.systemLanguage == SystemLanguage.German ) {
			if (m_CurSelection == 0) {
				m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Fotos insgesamt:\nQuests insgesamt:\nOrte besucht:";
			} else if (m_CurSelection == 1) {
				m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Fotos diese Woche:\nQuests diese Woche:\nOrte besucht:";
			} else if (m_CurSelection == 2) {
				m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Fotos letzte Woche:\nQuests letzte Woche:\nOrte besucht:";
			} 

			m_Loading.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Wird geladen...";
		} else {
			if (m_CurSelection == 0) {
				m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Total photos:\nTotal quests:\nLocations visited:";
			} else if (m_CurSelection == 1) {
				m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Photos this week:\nQuests this week:\nLocations visited:";
			} else if (m_CurSelection == 2) {
				m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Photos last week:\nQuests last week:\nLocations visited:";
			} 
			m_Loading.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Loading...";
		}*/

		/*m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text =  LocalizationSupport.GetString("LeaderboardTotalPhotos") +
			"\n" + LocalizationSupport.GetString("LeaderboardTotalQuests") + "\n" +  LocalizationSupport.GetString("LeaderboardTotalLocations");
		*/m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text =  LocalizationSupport.GetString("LeaderboardTotalQuests") + "\n" +  LocalizationSupport.GetString("LeaderboardTotalLocations");

		m_Loading.GetComponentInChildren<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString("Loading");
	}

	int m_CurState = 0;
	public void updateStates() {
		
		if (Application.systemLanguage == SystemLanguage.German ) {
			m_Button.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("BtnBack");//"ZURÜCK";

			m_Rank.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("LeaderboardRank");//"Rang";
			m_LName.GetComponentInChildren<UnityEngine.UI.Text>().text =  LocalizationSupport.GetString("LeaderboardName");//"Name";
			m_Score.GetComponentInChildren<UnityEngine.UI.Text>().text =  LocalizationSupport.GetString("LeaderboardNrQuests");//"Quests gemacht";

			m_Dropdown.GetComponentInChildren<UnityEngine.UI.Text>().text = "Insgesamt";

			UnityEngine.UI.Dropdown.OptionData list = new UnityEngine.UI.Dropdown.OptionData("Insgesamt");
			UnityEngine.UI.Dropdown.OptionData list2 = new UnityEngine.UI.Dropdown.OptionData("Diese Woche");
			UnityEngine.UI.Dropdown.OptionData list3 = new UnityEngine.UI.Dropdown.OptionData("Letzte Woche");

			UnityEngine.UI.Dropdown dropdown;
			dropdown = m_Dropdown.GetComponent<UnityEngine.UI.Dropdown>();
			dropdown.options.Clear ();
			dropdown.options.Add (list);
			dropdown.options.Add (list2);
			dropdown.options.Add (list3);
		} else {
			//m_Score.GetComponentInChildren<UnityEngine.UI.Text>().text = "Quests done";

			m_Rank.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("LeaderboardRank");//"Rang";
			m_LName.GetComponentInChildren<UnityEngine.UI.Text>().text =  LocalizationSupport.GetString("LeaderboardName");//"Name";
			m_Score.GetComponentInChildren<UnityEngine.UI.Text>().text =  LocalizationSupport.GetString("LeaderboardNrQuests");//"Quests gemacht";


			m_Button.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("BtnBack");//"CLOSE";

			m_Dropdown.GetComponentInChildren<UnityEngine.UI.Text>().text = "Total";

			UnityEngine.UI.Dropdown.OptionData list = new UnityEngine.UI.Dropdown.OptionData("Total");
			UnityEngine.UI.Dropdown.OptionData list2 = new UnityEngine.UI.Dropdown.OptionData("This week");
			UnityEngine.UI.Dropdown.OptionData list3 = new UnityEngine.UI.Dropdown.OptionData("Last week");

			UnityEngine.UI.Dropdown dropdown;
			dropdown = m_Dropdown.GetComponent<UnityEngine.UI.Dropdown>();
			dropdown.options.Clear ();
			dropdown.options.Add (list);
			dropdown.options.Add (list2);
			dropdown.options.Add (list3);
		}

	}

	public void NextClicked () {
		Application.LoadLevel ("DemoMap");
	//	Debug.Log ("OnSelected: " );
	}

	void loadLeaderboard()
	{
		string url = "https://geo-wiki.org/Application/api/Campaign/FotoQuestStats";
		string param = "";
		//param += "{\"email\":\"" + mail + "\",\"md5password\":\"" + passwordmd5 + "\",\"username\":\"" + user + "\"";

		//"platform":{"app":"11"}


	/*	if (m_CurSelection == 0) {
			param += "{\"scope\":\"" + "total" + "\",\"limit\":\"" + "-1" + "\"" + "\"platform\";"" +
				"//:{\"app\":\" + "11" + "\"";
		} else if (m_CurSelection == 1) {
			param += "{\"scope\":\"" + "thisweek" + "\",\"limit\":\"" + "-1" + "\"" + ",\"platform\"":{\"app\":\"" + "11" + "\""
		} else if (m_CurSelection == 2) {
			param += "{\"scope\":\"" + "lastweek" + "\",\"limit\":\"" + "-1" + "\"" + ",\"platform\":{\"app\":\"" + "11" + "\""
		}*/

	/*	if (m_CurSelection == 0) {
			param += "{\"scope\":\"" + "total" + "\",\"limit\":\"" + "-1" + "\""+ ",\"platform\":{" + "\""+ "app"+ "\":" + "\"11\"" + "}";
		} else if (m_CurSelection == 1) {
			param += "{\"scope\":\"" + "thisweek" + "\",\"limit\":\"" + "-1" + "\""+ ",\"platform\":{" + "\""+"app"+ "\":"  + "\"11\"" + "}";
		} else if (m_CurSelection == 2) {
			param += "{\"scope\":\"" + "lastweek" + "\",\"limit\":\"" + "-1" + "\""+ ",\"platform\":{" + "\""+ "app"+ "\":" + "\"11\"" + "}";
		}*/
		if (m_CurSelection == 0) {
			param += "{\"scope\":\"" + "total" + "\",\"limit\":\"" + "-1" + "\""+ ",\"platform\":{" + "\""+ "app"+ "\":" + "\"23\"" + "}";
		} else if (m_CurSelection == 1) {
			param += "{\"scope\":\"" + "thisweek" + "\",\"limit\":\"" + "-1" + "\""+ ",\"platform\":{" + "\""+"app"+ "\":"  + "\"23\"" + "}";
		} else if (m_CurSelection == 2) {
			param += "{\"scope\":\"" + "lastweek" + "\",\"limit\":\"" + "-1" + "\""+ ",\"platform\":{" + "\""+ "app"+ "\":" + "\"23\"" + "}";
		}
					
		/*
		 * 
		if (m_CurSelection == 0) {
			param += "{\"scope\":\"" + "total" + "\",\"limit\":\"" + "-1" + "\"" + ",\"platform\":{\"app\":\"" + "11" + "\"";
		} else if (m_CurSelection == 1) {
			param += "{\"scope\":\"" + "thisweek" + "\",\"limit\":\"" + "-1" + "\"" + ",\"platform\":{\"app\":\"" + "11" + "\""
		} else if (m_CurSelection == 2) {
			param += "{\"scope\":\"" + "lastweek" + "\",\"limit\":\"" + "-1" + "\"" + ",\"platform\":{\"app\":\"" + "11" + "\""
		}*/
		param += "}";


		Debug.Log ("login param: " + param);


		WWWForm form = new WWWForm();
		form.AddField ("parameter", param);

		//Debug.Log ("Url data: " + System.Text.Encoding.UTF8.GetString(form.data));
		WWW www = new WWW(url, form);

		StartCoroutine(WaitForData(www));
	}


	bool m_bQuestsRead;
	bool m_bPhotosRead;
	bool m_bPercRead;
	string m_NrQuests;
	string m_NrPhotos;
	string m_PercDone;
	ArrayList m_Names;
	ArrayList m_Scores;

	IEnumerator WaitForData(WWW www)
	{
		yield return www;

		string[] options = { "Ok" };


		m_Names = new ArrayList();
		m_Scores = new ArrayList();

		// check for errors
		if (www.error == null)
		{
			string data = www.text;
			//string[] parts = data.Split (":", 2);

			Debug.Log ("Leaderboard result: " + data);

			JSONObject j = new JSONObject(www.text);
			m_ReadingWhich = -1;

			m_bQuestsRead = false;
			 m_bPhotosRead = false;
			 m_bPercRead = false;

			accessPinData(j);

			if (m_PercDone.Length > 0) {
				float percdone = float.Parse (m_PercDone);
				m_PercDone = percdone.ToString ("F1");
			}

			/*if (Application.systemLanguage == SystemLanguage.German ) {
			if (m_CurSelection == 0) {
				m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Fotos insgesamt: " + m_NrPhotos + "\nQuests insgesamt: " + m_NrQuests + "\nOrte besucht: " + m_PercDone + "%";
			} else if (m_CurSelection == 1) {
				m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Fotos diese Woche: " + m_NrPhotos + "\nQuests diese Woche: " + m_NrQuests + "\nOrte besucht: " + m_PercDone + "%";
			} else if (m_CurSelection == 2) {
				m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Fotos letzte Woche: " + m_NrPhotos + "\nQuests letzte Woche: " + m_NrQuests + "\nOrte besucht: " + m_PercDone + "%";
			}
			} else {
				if (m_CurSelection == 0) {
					m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Total photos: " + m_NrPhotos + "\nTotal quests: " + m_NrQuests + "\nLocations visited: " + m_PercDone + "%";
				} else if (m_CurSelection == 1) {
					m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Photos this week: " + m_NrPhotos + "\nQuests this week: " + m_NrQuests + "\nLocations visited: " + m_PercDone + "%";
				} else if (m_CurSelection == 2) {
					m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Photos last week: " + m_NrPhotos + "\nQuests last week: " + m_NrQuests + "\nLocations visited: " + m_PercDone + "%";
				}
			}*/
	//	m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text =  LocalizationSupport.GetString("LeaderboardTotalPhotos") + " " + m_NrPhotos +
	//		"\n" + LocalizationSupport.GetString("LeaderboardTotalQuests") + " " + m_NrQuests +  "\n" +  LocalizationSupport.GetString("LeaderboardTotalLocations") + " " + m_PercDone + "%";
		m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text =  LocalizationSupport.GetString("LeaderboardTotalQuests") + " " + m_NrQuests +  "\n" +  LocalizationSupport.GetString("LeaderboardTotalLocations") + " " + m_PercDone + "%";
		

			m_Loading.SetActive (false);
			createLeaderboard ();

			/*
			string[] parts = data.Split(new string[] { ":" }, 0);
			string[] parts2 = parts[1].Split(new string[] { "," }, 0);
			string part3 = parts2 [0];

			Debug.Log("WWW Ok!: " + www.data);
			Debug.Log("part1: " + parts[0]);
			Debug.Log("part2: " + parts[1]);
			Debug.Log("part3: " + part3);

			part3 = part3.Replace ("\"", "");
			part3 = part3.Replace ("}", "");


			if (part3.Equals ("null")) {
				if (Application.systemLanguage == SystemLanguage.German) {
					messageBox.Show ("", "Registrierung fehlgeschlagen. Versuchen sie es bitte erneut.", options);
				} else {
					messageBox.Show ("", "Registration failed. Please try again.", options);
				}
				yield return www;
			} else {
				PlayerPrefs.SetString("PlayerId",part3);
				PlayerPrefs.Save ();

				Application.LoadLevel ("DemoMap");
			}*/


		} else {
			Debug.Log("WWW Error: "+ www.error);
			Debug.Log("WWW Error 2: "+ www.text);
		}   
	} 

	int m_ReadingWhich;

	void accessPinData(JSONObject obj){
		switch(obj.type){
		case JSONObject.Type.OBJECT:
			for(int i = 0; i < obj.list.Count; i++){
				string key = (string)obj.keys[i];
				JSONObject j = (JSONObject)obj.list[i];
				//	Debug.Log("key: " + key);
				if (key == "quests") {
					if (m_bQuestsRead == false) {
						m_ReadingWhich = 1;
					}
					m_bQuestsRead = true;
				}if (key == "photos") {
					if (m_bPhotosRead == false) {
						m_ReadingWhich = 2;
					}
					m_bPhotosRead = true;

				}if (key == "percentageTotal") {
					if (m_bPercRead == false) {
						m_ReadingWhich = 3;
					}
					m_bPercRead = true;
				}
				if (key == "highscore") {
					m_ReadingWhich = 4;

				}

				if (key == "user") {
					m_ReadingWhich = 5;
				}
				if (key == "score") {
					m_ReadingWhich = 6;
				}
			/*	if (key == "id") {
					m_CurrentPin++;
					if (m_CurrentPin >= 1000) {
						m_CurrentPin = 999;
					}
					m_ReadingWhich = 1;
				} else if (key == "lat") {
					m_ReadingWhich = 2;
				} else if (key == "lon") {
					m_ReadingWhich = 3;
				} else if (key == "weight") {
					m_ReadingWhich = 4;
				} else if (key == "color") {
					m_ReadingWhich = 5;
				} else {
					m_ReadingWhich = 0;
				}*/
				accessPinData(j);
			}
			break;
		case JSONObject.Type.ARRAY:
			//	Debug.Log ("Array");
			foreach(JSONObject j in obj.list){
				accessPinData(j);
			}
			break;
		case JSONObject.Type.STRING:
			//Debug.Log ("string: " + obj.str);
			if (m_ReadingWhich == 1) {
				m_NrQuests = obj.str;
			}
			if (m_ReadingWhich == 2) {
				m_NrPhotos = obj.str;
			}
			if (m_ReadingWhich == 5) {
				m_Names.Add (obj.str);
			}
			if (m_ReadingWhich == 6) {
				m_Scores.Add (obj.str);
			}
			m_ReadingWhich = -1;
			/*if (m_ReadingWhich == 1) {

				//Debug.Log ("m_CurrentPin: " + m_CurrentPin);
				m_Pins [m_CurrentPin].m_Id = obj.str;
				//Debug.Log ("Read pin id: " + obj.str);
			} else if (m_ReadingWhich == 2) {
				m_Pins [m_CurrentPin].m_Lat = double.Parse(obj.str);
			} else if (m_ReadingWhich == 3) {
				m_Pins [m_CurrentPin].m_Lng = double.Parse(obj.str);
			} else if (m_ReadingWhich == 5) {
				m_Pins [m_CurrentPin].m_Color = obj.str;
			}*/
			break;
		case JSONObject.Type.NUMBER:
		//	Debug.Log ("number: " + obj.n);

			if (m_ReadingWhich == 3) {
				m_PercDone = obj.n + "";//"asdf";//obj.n + "";
			}
			m_ReadingWhich = -1;
	/*		if (m_ReadingWhich == 4) {
				m_Pins [m_CurrentPin].m_Weight = "" + obj.n;
			}*/
			break;
		case JSONObject.Type.BOOL:
			//		Debug.Log("bool: " + obj.b);
			break;
		case JSONObject.Type.NULL:
			//	Debug.Log("NULL");
			break;

		}
	}

	public void OnSelectedRange( int value) {

		for (int i = 0; i < m_AddedTexts.Count; i++) {
			GameObject go = (GameObject)m_AddedTexts [i];
			Destroy (go);
		}
		m_AddedTexts = new ArrayList();

		UnityEngine.UI.Dropdown dropdown;
		dropdown = m_Dropdown.GetComponent<UnityEngine.UI.Dropdown>();

	//	Debug.Log ("OnSelected: " + value + "," + dropdown.value);

		m_CurSelection = dropdown.value;
		UpdateText ();

		loadLeaderboard ();

		/*
		int nrentries = 5;

		RectTransform rectTransform2 = m_Content.GetComponent<RectTransform> ();
		//rectTransform2.sizeDelta.
		//rt.sizeDelta = new Vector2 (100, 100);
		float scalex = rectTransform2.sizeDelta.x;
		float scaley = rectTransform2.sizeDelta.y;
		rectTransform2.sizeDelta = new Vector2 (scalex, 30.0f * nrentries);



		for (int i = 0; i < nrentries; i++) {
			GameObject copy = (GameObject)GameObject.Instantiate (m_RankS);

			copy.transform.SetParent (m_Content.transform, false);
			copy.SetActive (true);
			RectTransform rectTransform = copy.GetComponent<RectTransform> ();
			float curpos = rectTransform.localPosition.y;
			float curposx = rectTransform.localPosition.x;
			curpos -= i * 30.0f;
			rectTransform.localPosition = new Vector2 (curposx, curpos);
			m_AddedTexts.Add (copy);
			int currank = i + 1;
			string text = "" + currank;
			copy.GetComponentInChildren<UnityEngine.UI.Text> ().text = text;


			copy = (GameObject)GameObject.Instantiate (m_NameS);
			copy.transform.SetParent (m_Content.transform, false);
			copy.SetActive (true);
			 rectTransform = copy.GetComponent<RectTransform> ();
			 curpos = rectTransform.localPosition.y;
			 curposx = rectTransform.localPosition.x;
			curpos -= i * 30.0f;
			rectTransform.localPosition = new Vector2 (curposx, curpos);
			m_AddedTexts.Add (copy);
			 text = "Tobias";
			copy.GetComponentInChildren<UnityEngine.UI.Text> ().text = text;

			copy = (GameObject)GameObject.Instantiate (m_ScoreS);
			copy.transform.SetParent (m_Content.transform, false);
			copy.SetActive (true);
			rectTransform = copy.GetComponent<RectTransform> ();
			curpos = rectTransform.localPosition.y;
			curposx = rectTransform.localPosition.x;
			curpos -= i * 30.0f;
			rectTransform.localPosition = new Vector2 (curposx, curpos);
			m_AddedTexts.Add (copy);
			copy.GetComponentInChildren<UnityEngine.UI.Text> ().text = "2323";

		}*/
	}

	public void createLeaderboard()
	{
		int nrentries = m_Names.Count;

		float sizeentry = 40.0f;
		RectTransform rectTransform2 = m_Content.GetComponent<RectTransform> ();
		//rectTransform2.sizeDelta.
		//rt.sizeDelta = new Vector2 (100, 100);
		float scalex = rectTransform2.sizeDelta.x;
		float scaley = rectTransform2.sizeDelta.y;
		rectTransform2.sizeDelta = new Vector2 (scalex, sizeentry * nrentries + 100.0f);



		for (int i = 0; i < nrentries; i++) {
			GameObject copy = (GameObject)GameObject.Instantiate (m_RankS);

			copy.transform.SetParent (m_Content.transform, false);
			copy.SetActive (true);
			RectTransform rectTransform = copy.GetComponent<RectTransform> ();
			float curpos = rectTransform.localPosition.y;
			float curposx = rectTransform.localPosition.x;
			curpos -= i * sizeentry;
			rectTransform.localPosition = new Vector2 (curposx, curpos);
			m_AddedTexts.Add (copy);
			int currank = i + 1;
			string text = "" + currank;
			copy.GetComponentInChildren<UnityEngine.UI.Text> ().text = text;


			copy = (GameObject)GameObject.Instantiate (m_NameS);
			copy.transform.SetParent (m_Content.transform, false);
			copy.SetActive (true);
			rectTransform = copy.GetComponent<RectTransform> ();
			curpos = rectTransform.localPosition.y;
			curposx = rectTransform.localPosition.x;
			curpos -= i * sizeentry;
			rectTransform.localPosition = new Vector2 (curposx, curpos);
			m_AddedTexts.Add (copy);
			//text = "Tobias";
			text = (string)m_Names [i];
			text = System.Text.RegularExpressions.Regex.Unescape (text);
			if (text.Length > 15) {
				text = text.Substring (0, 15);
			}
		//hallihallo
			copy.GetComponentInChildren<UnityEngine.UI.Text> ().text = text;

			copy = (GameObject)GameObject.Instantiate (m_ScoreS);
			copy.transform.SetParent (m_Content.transform, false);
			copy.SetActive (true);
			rectTransform = copy.GetComponent<RectTransform> ();
			curpos = rectTransform.localPosition.y;
			curposx = rectTransform.localPosition.x;
			curpos -= i * sizeentry;
			rectTransform.localPosition = new Vector2 (curposx, curpos);
			m_AddedTexts.Add (copy);
			string textscore = (string)m_Scores [i];

			if(textscore != "" && textscore != null) {
				float money = float.Parse (textscore);
			//	money /= 100;
				string strtotal = money.ToString ();//(( ("F2");
				textscore = strtotal;
			}

				copy.GetComponentInChildren<UnityEngine.UI.Text> ().text = textscore;// + "€";//"2323";

		}
	}

}
