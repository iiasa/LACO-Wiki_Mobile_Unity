using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;
using Unitycoding.UIWidgets;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using System.Text;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EventSystemQuests : MonoBehaviour
{



    public GameObject m_Button;
    public GameObject m_TextInfo;
    public GameObject m_Info;

    public GameObject m_TextNoQuests;

    public GameObject m_Content;
    public GameObject m_Rank;
    public GameObject m_LName;
    public GameObject m_Score;
    public GameObject m_RankS;
    public GameObject m_NameS;
    public GameObject m_Desc;
    public GameObject m_ScoreS;
    public GameObject m_Image;
    public GameObject m_ImageNoPicSmall;

    public GameObject m_UploadQuest;
    public GameObject m_ShowQuest;
    public GameObject m_ShowQuestPhotos;


    public GameObject m_UploadingBack;
    public GameObject m_UploadingText;
    public GameObject m_UploadingText2;
    public GameObject m_UploadingTextWait;
    public GameObject m_UploadingImage;

    public GameObject m_DebugText;
    public GameObject m_DebugInputField;

    public GameObject m_UploadingAll;


    public GameObject m_ImageQuest;
    public GameObject m_SkyQuestBack;
    public GameObject m_QuestTitle;
    public GameObject m_QuestText;
    public GameObject m_BtnDeleteQuest;
    public GameObject m_BtnCloseQuest;
    public GameObject m_Image1Quest;
    public GameObject m_Image2Quest;
    public GameObject m_Image3Quest;
    public GameObject m_Image4Quest;
    public GameObject m_Image5Quest;

    public GameObject m_BtnEditImage1;
    public GameObject m_BtnEditImage2;
    public GameObject m_BtnEditImage3;
    public GameObject m_BtnEditImage4;
    public GameObject m_BtnEditImage5;

    public GameObject m_TextQuestExplanation;


    private MessageBox messageBox;
    private MessageBox verticalMessageBox;

    //public Sprite m_Sprite;

    ArrayList m_AddedTexts;

    int m_NrQuestsDone;
    //	int m_NrPoints;
    //	int m_LastLevel;

    bool m_bShowQuest;

    //---------------------
    // Edit image

    public GameObject m_EditImageBack;
    public GameObject m_EditImage;
    public GameObject m_EditImageClose;
    public GameObject m_EditImageClose2;
    public GameObject m_EditImageExpl;
    public GameObject m_EditImageExplBack;

    string m_StrImg1;
    bool m_bImg1Set;
    string m_StrImg2;
    bool m_bImg2Set;
    string m_StrImg3;
    bool m_bImg3Set;
    string m_StrImg4;
    bool m_bImg4Set;
    string m_StrImg5;
    bool m_bImg5Set;


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
        Debug.Log("OpenQuests");
        StartCoroutine(changeFramerate());
        ForceLandscapeLeft();

        if ((!LocalizationSupport.StringsLoaded))
            LocalizationSupport.LoadStrings();
        /*
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToLandscapeLeft = false;*/

        m_bShowMessageBluring = true;
        m_ShowMessageBluringIter = 0;

        PlayerPrefs.SetInt("BackToUpload", 0);
        PlayerPrefs.Save();


        m_EditImageExpl.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("BlurHelp");//"Touch the parts you want to blur.";
        m_TextQuestExplanation.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("BlurHint");//"Please blur out all parts in the pictures that could identify a person or property!";
        m_TextNoQuests.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("NoQuestsDone");//"You have not made any quests yet.";


        m_bHideEditImageExplanation = false;
        m_bHiddenEditImageExplanation = false;

        //		m_NrPoints = -1;
        //		m_LastLevel = -1;

        m_CurSelection = 0;
        m_AddedTexts = new ArrayList();

        m_UploadingBack.SetActive(false);
        m_UploadingText.SetActive(false);
        m_UploadingText2.SetActive(false);
        m_UploadingTextWait.SetActive(false);
        m_UploadingImage.SetActive(false);

        m_BtnEditImage1.SetActive(false);
        m_BtnEditImage2.SetActive(false);
        m_BtnEditImage3.SetActive(false);
        m_BtnEditImage4.SetActive(false);
        m_BtnEditImage5.SetActive(false);
        m_TextQuestExplanation.SetActive(false);

        m_bShowQuest = false;
        m_BtnCloseQuest.SetActive(false);
        m_BtnDeleteQuest.SetActive(false);
        m_ImageQuest.SetActive(false);
        m_SkyQuestBack.SetActive(false);
        m_QuestTitle.SetActive(false);
        m_QuestText.SetActive(false);
        m_Image1Quest.SetActive(false);
        m_Image2Quest.SetActive(false);
        m_Image3Quest.SetActive(false);
        m_Image4Quest.SetActive(false);
        m_Image5Quest.SetActive(false);

        m_EditImageBack.SetActive(false);
        m_EditImage.SetActive(false);
        m_EditImageClose.SetActive(false);
        m_EditImageClose2.SetActive(false);
        m_EditImageExpl.SetActive(false);
        m_EditImageExplBack.SetActive(false);

        //	PlayerPrefs.DeleteAll ();

        if (PlayerPrefs.HasKey("NrQuestsDone"))
        {
            m_NrQuestsDone = PlayerPrefs.GetInt("NrQuestsDone");
        }
        else
        {
            m_NrQuestsDone = 0;
        }

        if (m_NrQuestsDone > 0)
        {
            m_TextNoQuests.SetActive(false);
        }

        bool bOneQuestNotUploaded = false;
        for (int i = 0; i < m_NrQuestsDone && !bOneQuestNotUploaded; i++)
        {
            string stralreadyuploaded = "Quest_" + i + "_Uploaded";
            if (PlayerPrefs.HasKey(stralreadyuploaded) == false)
            {
                string strdeleted = "Quest_" + i + "_Del";
                int deleted = 0;
                if (PlayerPrefs.HasKey(strdeleted))
                {
                    deleted = PlayerPrefs.GetInt(strdeleted);
                }
                if (deleted == 0)
                {
                    bOneQuestNotUploaded = true;
                }
            }
        }
        if (bOneQuestNotUploaded)
        {
            m_UploadingAll.SetActive(true);
        }
        else
        {
            m_UploadingAll.SetActive(false);
        }

        messageBox = UIUtility.Find<MessageBox>("MessageBox");



        PlayerPrefs.SetInt("LoginReturnToQuests", 0);
        PlayerPrefs.Save();

        UpdateText();
        updateStates();

        m_bCreateQuestsList = true;

        //createQuestList ();
        //loadLeaderboard ();


    }

    bool m_bCreateQuestsList = false;
    int m_IterCreateQuestsList = 0;

    float m_TouchPosX;
    float m_TouchPosY;

    bool m_bHideEditImageExplanation;
    bool m_bHiddenEditImageExplanation;
    float m_HideEditImageExplanationTime;

    bool m_bShowMessageBluring;
    int m_ShowMessageBluringIter;

    bool m_bTest = false;
    int m_TestIter = 0;



    //int m_DebugIter = 0;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.LoadLevel("DemoMap");

        /*m_DebugIter++;
		if (m_DebugIter == 20) {
			Application.LoadLevel ("Quests");// Test if creating memory leak
		}*/
        /*
		if (!m_bTest) {
			m_TestIter++;
			if (m_TestIter > 10) {
				UnityEngine.Events.UnityAction<string> ua = new UnityEngine.Events.UnityAction<string> (OnMsgBoxAllUploadedClicked);
				string[] options = { "Ok"};
				messageBox.Show ("", LocalizationSupport.GetString("QuestUploadSuccessfulPrize"), ua, options);
				m_bTest = true;
			}
		}*/
        /*
		Screen.orientation = ScreenOrientation.Portrait;
		Screen.autorotateToPortrait = true;
		Screen.autorotateToPortraitUpsideDown = false;
		Screen.autorotateToLandscapeRight = false;
		Screen.autorotateToLandscapeLeft = false;
*/
        /*if (m_bShowMessageBluring) {
			m_ShowMessageBluringIter++;
			if (m_ShowMessageBluringIter > 3) {
				m_bShowMessageBluring = false;
				
					string[] options2 = { "OK" };
					messageBox.Show ("", LocalizationSupport.GetString("PleaseBlur"), options2);
			}
		}*/

        if (m_bCreateQuestsList) {
			m_IterCreateQuestsList++;
			if (m_IterCreateQuestsList > 3) {
				m_bCreateQuestsList = false;
				createQuestList ();
			//	Debug.Log ("start loading leaderboard");
		//		loadLeaderboard ();
			}
		}
        /*
		if (m_bLoadImagesForQuest && m_LoadImagesForQuestIter > 3) {
			m_bLoadImagesForQuest = false;
			StartCoroutine (loadQuestImages (m_LoadImagesForQuestInReach, m_LoadImagesForQuestId));
		} else if(m_bLoadImagesForQuest) {
			m_LoadImagesForQuestIter++;
		}


		if (m_bBluring) {
			blurImage (m_BluringPosX, m_BluringPosY);
		}*/
		/*
		if (m_bHideEditImageExplanation) {
			m_HideEditImageExplanationTime += Time.deltaTime * 1000.0f;

			float proc = (m_HideEditImageExplanationTime) / 500.0f;
			Debug.Log ("m_HideEditImageExplanationTime: " + m_HideEditImageExplanationTime);
			if (proc > 1.0f) {
				m_bHideEditImageExplanation = false;
				m_EditImageExplBack.SetActive (false);
				m_EditImageExpl.SetActive (false);
			} else {
				if (proc > 1.0f)
					proc = 1.0f;
				proc = 1.0f - proc;

				byte alpha = (byte)(208 * proc);
				m_EditImageExplBack.GetComponent<UnityEngine.UI.Image> ().color = new Color32 (255, 255, 255, alpha);
				alpha = (byte)(255 * proc);
				m_EditImageExpl.GetComponent<UnityEngine.UI.Text> ().color = new Color32 (0, 0, 0, alpha);


				m_EditImageExplBack.SetActive (true);
				m_EditImageExpl.SetActive (true);
			}
		}*/
	}

	int m_CurSelection = 0;
	void UpdateText() {
		/*if (Application.systemLanguage == SystemLanguage.German && false ) {
			

			//m_UploadingAll.GetComponentInChildren<UnityEngine.UI.Text>().text = "Alle hochladen";
			m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Quests hochgeladen: ";
		} else {
		//	m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text = "This week: " + "\nLast week: \nTotal: ";
			m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString("QuestsUploaded") + " ";//"Quests uploaded: ";
		}
*/
		m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString("QuestsUploaded") + " ";//"Quests uploaded: ";

		m_UploadingAll.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("UploadAll");//"Alle hochladen";

		/*if (m_CurSelection == 0) {
			m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text>().text = "Total Photos:\nTotal Quests:\nLocations visited:";
		} else if (m_CurSelection == 1) {
			m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text>().text = "Photos this week:\nQuests this week:\nLocations visited:";
		} else if (m_CurSelection == 2) {
			m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text>().text = "Photos last week:\nQuests last week:\nLocations visited:";
		} */
	}

	int m_CurState = 0;
	public void updateStates() {
		
		if (Application.systemLanguage == SystemLanguage.German && false) {
			m_Button.GetComponentInChildren<UnityEngine.UI.Text>().text = "ZURÜCK";

			/*UnityEngine.UI.Dropdown.OptionData list = new UnityEngine.UI.Dropdown.OptionData("Insgesamt");
			UnityEngine.UI.Dropdown.OptionData list2 = new UnityEngine.UI.Dropdown.OptionData("Diese Woche");
			UnityEngine.UI.Dropdown.OptionData list3 = new UnityEngine.UI.Dropdown.OptionData("Letzte Woche");*/
			m_BtnCloseQuest.GetComponentInChildren<UnityEngine.UI.Text>().text = "ZURÜCK";
			m_BtnDeleteQuest.GetComponentInChildren<UnityEngine.UI.Text>().text = "LÖSCHE QUEST";

		} else {
			int btncontinue = 0;
			if(PlayerPrefs.HasKey("QuestsBtnContinue")) {
				btncontinue = PlayerPrefs.GetInt("QuestsBtnContinue");
				PlayerPrefs.SetInt("QuestsBtnContinue", 0);
				PlayerPrefs.Save();
			}

			if(btncontinue == 1) {
				m_Button.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("BtnContinue");//"CLOSE";
			} else {
				m_Button.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("BtnBack");//"CLOSE";
			}

			m_BtnCloseQuest.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("Close");//"CLOSE";
			m_BtnDeleteQuest.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("DeleteQuest");//"DELETE QUEST";
			/*UnityEngine.UI.Dropdown.OptionData list = new UnityEngine.UI.Dropdown.OptionData("Total");
			UnityEngine.UI.Dropdown.OptionData list2 = new UnityEngine.UI.Dropdown.OptionData("This week");
			UnityEngine.UI.Dropdown.OptionData list3 = new UnityEngine.UI.Dropdown.OptionData("Last week");*/

		}

    }

    public void NextClicked()
    {
        Debug.Log("Nextclicked: Load demp map");
        Application.LoadLevel("DemoMap");
        //  Debug.Log ("OnSelected: " );
    }


    public void NextClicked2()
    {
        Debug.Log("Nextclicked: Load demp map");
        //  Debug.Log ("OnSelected: " );
    }


    public void NextClicked3()
    {
        Debug.Log("Nextclicked: Load demp map");
        //  Debug.Log ("OnSelected: " );
        Application.LoadLevel("DemoMap");
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

	int m_WhichLeaderboard = 0;

	void stardLoading() {
        Debug.Log("####### startLoading");
		m_WhichLeaderboard = 0;
		m_NrQuests = "";
		m_NrPhotos = "";
		m_PercDone = "";
		loadLeaderboard ();
	}

	void loadLeaderboard()
	{
		Debug.Log ("##### loadLeaderboard");
		if (PlayerPrefs.HasKey ("PlayerPassword") == false || PlayerPrefs.HasKey ("PlayerMail") == false) {
			Debug.Log ("Points: Did not login yet");
			if (Application.systemLanguage == SystemLanguage.German && false) {
				//m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Diese Woche:" + "\nLetzte Woche:\nInsgesamt:";
				m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Quests hochgeladen: 0";
			} else {
				//m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text = "This week:" + "\nLast week:\nTotal:";
				m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString("QuestsUploaded") + " 0";//Quests uploaded: 0";
			}
			return;
		}


		string url = "https://geo-wiki.org/Application/api/Campaign/FotoQuestStats";
		string param = "";

		string email = PlayerPrefs.GetString ("PlayerMail");
		string password = PlayerPrefs.GetString ("PlayerPassword");
		string passwordmd5 = ComputeHash (password);



	//	if (m_WhichLeaderboard == 0) {
			param += "{\"email\":" + "\"" + email + "\",\"md5password\":" + "\"" + passwordmd5 + "\"" + ",\"scope\":" + "\"total\",\"limit\":\"" + "0" + "\""+ ",\"platform\":{" + "\""+ "app"+ "\":" + "\"23\"" + "}";
	/*	} else if (m_WhichLeaderboard == 1) {
			param += "{\"email\":" + "\"" + email + "\",\"md5password\":" + "\""+ passwordmd5 + "\"" + ",\"scope\":" + "\"thisweek\",\"limit\":\"" + "0" + "\""+ ",\"platform\":{" + "\""+"app"+ "\":"  + "\"14\"" + "}";
		} else if (m_WhichLeaderboard == 2) {
			param += "{\"email\":" + "\"" + email + "\",\"md5password\":" + "\""+ passwordmd5 + "\"" + ",\"scope\":" + "\"lastweek\",\"limit\":\"" + "0" + "\""+ ",\"platform\":{" + "\""+"app"+ "\":"  + "\"14\"" + "}";
			
		//	param += "{\"email\":" + "\"" + email + "\",\"md5password\":" + "\"" + passwordmd5 + "\"" + ",\"scope\":" + "\"lastweek\",\"limit\":\"" + "0" + "\""+ ",\"platform\":{" + "\""+ "app"+ "\":" + "\"11\"" + "}";
		}*/

		/*if (m_WhichLeaderboard == 0) {
			param += "{\"email\":" + "\"" + email + "\",\"md5password\":" + "\"" + passwordmd5 + "\"" + ",\"scope\":" + "\"total\",\"limit\":\"" + "0" + "\""+ ",\"platform\":{" + "\""+ "app"+ "\":" + "\"11\"" + "}";
		} else if (m_WhichLeaderboard == 1) {
			param += "{\"email\":" + "\"" + email + "\",\"md5password\":" + "\""+ passwordmd5 + "\"" + ",\"scope\":" + "\"thisweek\",\"limit\":\"" + "0" + "\""+ ",\"platform\":{" + "\""+"app"+ "\":"  + "\"11\"" + "}";
		} else if (m_WhichLeaderboard == 2) {
			param += "{\"email\":" + "\"" + email + "\",\"md5password\":" + "\""+ passwordmd5 + "\"" + ",\"scope\":" + "\"lastweek\",\"limit\":\"" + "0" + "\""+ ",\"platform\":{" + "\""+"app"+ "\":"  + "\"11\"" + "}";

			//	param += "{\"email\":" + "\"" + email + "\",\"md5password\":" + "\"" + passwordmd5 + "\"" + ",\"scope\":" + "\"lastweek\",\"limit\":\"" + "0" + "\""+ ",\"platform\":{" + "\""+ "app"+ "\":" + "\"11\"" + "}";
		}*/

		param += "}";


		Debug.Log ("login param: " + param);


		WWWForm form = new WWWForm();
		form.AddField ("parameter", param);

		//Debug.Log ("Url data: " + System.Text.Encoding.UTF8.GetString(form.data));
		WWW www = new WWW(url, form);


		StartCoroutine(WaitForData(www));





		return;
/*		string url = "https://geo-wiki.org/Application/api/Campaign/FotoQuestStats";
		string param = "";

				if (m_CurSelection == 0) {
			param += "{\"scope\":\"" + "total" + "\",\"limit\":\"" + "-1" + "\""+ ",\"platform\":{" + "\""+ "app"+ "\":" + "\"11\"" + "}";
				} else if (m_CurSelection == 1) {
			param += "{\"scope\":\"" + "thisweek" + "\",\"limit\":\"" + "-1" + "\""+ ",\"platform\":{" + "\""+"app"+ "\":"  + "\"11\"" + "}";
				} else if (m_CurSelection == 2) {
			param += "{\"scope\":\"" + "lastweek" + "\",\"limit\":\"" + "-1" + "\""+ ",\"platform\":{" + "\""+ "app"+ "\":" + "\"11\"" + "}";
				}

		param += "}";


		Debug.Log ("login param: " + param);


		WWWForm form = new WWWForm();
		form.AddField ("parameter", param);

		//Debug.Log ("Url data: " + System.Text.Encoding.UTF8.GetString(form.data));
		WWW www = new WWW(url, form);

		StartCoroutine(WaitForData(www));*/
	}


	bool m_bQuestsRead;
	bool m_bPhotosRead;
	bool m_bPercRead;
	string m_NrQuests;
	string m_NrPhotos;
	string m_PercDone;
	ArrayList m_Names;
	ArrayList m_Scores;


	ArrayList m_UploadButtons;
	ArrayList m_EditButtons;
	ArrayList m_UploadQuestUploaded;
	ArrayList m_UploadLabels;
	ArrayList m_UploadButtonsQuestId;
	ArrayList m_UploadShowPictures;


	float m_MoneyEarned;
	float m_MoneyPending;

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

			if (m_WhichLeaderboard == 0) {
				m_PercDone = "0";
			} else if (m_WhichLeaderboard == 1) {
				m_NrQuests = "0";
			} else {
				m_NrPhotos = "0";
			}


		m_MoneyEarned = 0;
		m_MoneyPending = 0;

			accessPinData(j);


		/*	if (m_CurSelection == 0) {
				m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Points this week: " + "\nPoints last week: 0\nPoints total: 0";
				
				//m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text>().text = "Total Photos: "+ m_NrPhotos+"\nTotal Quests: " + m_NrQuests +  "\nLocations visited: " + m_PercDone +"%";
			} else if (m_CurSelection == 1) {
				m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text>().text = "Photos this week: "+ m_NrPhotos+"\nQuests this week: " + m_NrQuests +  "\nLocations visited: " + m_PercDone +"%";
			} else if (m_CurSelection == 2) {
				m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text>().text = "Photos last week: "+ m_NrPhotos+"\nQuests last week: " + m_NrQuests + "\nLocations visited: " + m_PercDone  +"%";
			} 
*/			
			Debug.Log ("Thisweek: " + m_NrQuests);
			Debug.Log ("LastWeek: " + m_NrPhotos);
			Debug.Log ("Total: " + m_PercDone);

			string strthisweek = "";
			string strlastweek = "";
		//	string strtotal = "";

			m_MoneyEarned /= 100.0f;
			m_MoneyPending /= 100.0f;

			strthisweek = m_NrQuests;//m_MoneyEarned + "";

			/*//if(m_NrQuests != "" && m_NrQuests != null) {
			//	float thisweek = float.Parse (m_NrQuests);
			strthisweek = m_MoneyEarned.ToString ("F2") + "€";
		//	}
		//	if(m_NrPhotos != "" && m_NrPhotos != null) {
		//		float lastweek = float.Parse (m_NrPhotos);
				strlastweek = m_MoneyPending.ToString ("F2") + "€";
		//	}
		*/
			/*if(m_PercDone != "" && m_PercDone != null) {
				float total = float.Parse (m_PercDone);
				strtotal = total.ToString ("F2") + "€";
			}*/


			if (Application.systemLanguage == SystemLanguage.German) {
				string strtext;// = "Diese Woche: " + strthisweek + "\nLetzte Woche: " + strlastweek + "\nInsgesamt: " + strtotal;
				strtext = "Quests hochgeladen: " + strthisweek;
					m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text = strtext;
			} else {
			string strtext;// = "This week: " + strthisweek + "\nLast week: " + strlastweek + "\nTotal: " + strtotal;

				strtext = "Quests uploaded: " + strthisweek;

				m_TextInfo.GetComponentInChildren<UnityEngine.UI.Text> ().text = strtext;
			}
		/*
			m_WhichLeaderboard++;
			if (m_WhichLeaderboard <= 2) {
				loadLeaderboard ();
			} */

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
					Debug.Log("key: " + key);
				/*if (key == "quests") {
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
				}*/
			if (key == "quests") {
				m_ReadingWhich = 6;
			} else if (key == "moneypending") {
				m_ReadingWhich = 7;
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
			if (m_ReadingWhich == 6) {
				/*if (m_WhichLeaderboard == 0) {
					m_PercDone = obj.str;
				} else if (m_WhichLeaderboard == 1) {
					m_NrQuests = obj.str;
				} else {
					m_NrPhotos = obj.str;
				}*/
				m_NrQuests = obj.str;
			} else if (m_ReadingWhich == 7) {
				m_NrPhotos = obj.str;
			}
			/*
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
			}*/
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
			Debug.Log ("number: " + obj.n);
			/*
			if (m_ReadingWhich == 3) {
				m_PercDone = obj.n + "";//"asdf";//obj.n + "";
			}*/
	/*		if (m_ReadingWhich == 4) {
				m_Pins [m_CurrentPin].m_Weight = "" + obj.n;
			}*/
			if (m_ReadingWhich == 6) {
				Debug.Log ("Money earned number: " + obj.n);
				m_MoneyEarned = obj.n;
		} else if (m_ReadingWhich == 7) {
			Debug.Log ("Money pending number: " + obj.n);
				m_MoneyPending = obj.n;
			}

	m_ReadingWhich = -1;
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


	}

	public void createQuestList()
	{
//		return;
		int nrentries = 0;// m_Names.Count;

		nrentries = m_NrQuestsDone;

		Debug.Log ("createQuestLit nrquests: " + m_NrQuestsDone);

		int nrentriesactive = 0;
		//for (int i = 0; i < nrentries; i++) {
		for (int i = nrentries-1; i >= 0; i--) {

			string strdeleted = "Quest_" + i + "_Del";
			int deleted = 0;
			if (PlayerPrefs.HasKey (strdeleted)) {
				deleted = PlayerPrefs.GetInt (strdeleted);
			}

			string stralreadyuploaded = "Quest_" + i + "_Uploaded";
			bool bUploaded = false;
			if (PlayerPrefs.HasKey (stralreadyuploaded)) {
				bUploaded = true;
			}


			if (deleted == 0 && (!bUploaded || nrentriesactive < 10)) { // First 10 entries should be shown no matter if they have already been uploaded
				nrentriesactive++;
			}
		}



		m_UploadButtons = new ArrayList ();
		m_EditButtons = new ArrayList ();
		m_UploadQuestUploaded = new ArrayList ();

		m_UploadLabels = new ArrayList ();
		m_UploadButtonsQuestId = new ArrayList ();
		m_UploadShowPictures = new ArrayList ();



		//nrentriesactive = 10;//For test, comment this out again

		RectTransform rectTransform2 = m_Content.GetComponent<RectTransform> ();
		//rectTransform2.sizeDelta.
		//rt.sizeDelta = new Vector2 (100, 100);
		float scalex = rectTransform2.sizeDelta.x;
		float scaley = rectTransform2.sizeDelta.y;
        float heightentry = 230.0f;//350.0f;//300.0f;//240.0f;//250.0f;//200.0f;
		//rectTransform2.sizeDelta = new Vector2 (scalex, heightentry * nrentries + 100.0f);
		rectTransform2.sizeDelta = new Vector2 (scalex, heightentry * nrentriesactive + 100.0f);

		float posoffset = 0;
		int nrentriesadded = 0;
		int curreport = 1;
		for (int i = nrentries-1; i >= 0; i--) {
		//for(int testi=0; testi<10; testi++) { // For test
		//	int i = nrentries - 1; // For test
			Debug.Log("Create quest entry: " + i);

			GameObject copy;
			RectTransform rectTransform;
			float curpos;
			float curposx;
			int currank;
			string text;

			/* = (GameObject)GameObject.Instantiate (m_RankS);

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
*/

		//Debug.Log("Quest_1");
			string strdeleted = "Quest_" + i + "_Del";
			int deleted = 0;
			if (PlayerPrefs.HasKey (strdeleted)) {
				deleted = PlayerPrefs.GetInt (strdeleted);
			}

			string stralreadyuploaded = "Quest_" + i + "_Uploaded";
			bool bUploaded = false;
			if (PlayerPrefs.HasKey (stralreadyuploaded)) {
				bUploaded = true;
			}

			if (deleted == 0 && (!bUploaded || nrentriesadded < 10)) {
				nrentriesadded++;
				//Debug.Log("Quest_2");

				copy = (GameObject)GameObject.Instantiate (m_NameS);
				copy.transform.SetParent (m_Content.transform, false);
				copy.SetActive (true);
				rectTransform = copy.GetComponent<RectTransform> ();
				curpos = rectTransform.localPosition.y;
				curposx = rectTransform.localPosition.x;
				curpos -= posoffset;//i * heightentry;
				rectTransform.localPosition = new Vector2 (curposx, curpos);
				m_AddedTexts.Add (copy);


			//	Debug.Log("Quest_3");

                int inreach = 1;// PlayerPrefs.GetInt (strinreach);

				text = "inr: " + inreach + " h: ";
                int classification = i + 1;
                text = classification + ". " + LocalizationSupport.GetString("Classification");

				string questtype = "Quest_" + i + "_" + "Report";
				bool bIsReport = false;
			if(PlayerPrefs.HasKey(questtype)) {
				int questtypei = PlayerPrefs.GetInt (questtype);
				if(questtypei == 1) {
					text = "Report";//+ curreport;
					curreport++;
						bIsReport = true;
				}
			}


/*			//float heading = 120.0f;
			if (inreach == 1) {
				for (int photo = 1; photo < 6; photo++) {
					string strheading = "Quest_" + i + "_" + photo + "_Heading";
					float heading = PlayerPrefs.GetFloat (strheading);
					text += heading + " ";
				}
			} else {
				for (int photo = 2; photo < 7; photo++) {
					string strheading = "Quest_" + i + "_" + photo + "_Heading";
					float heading = PlayerPrefs.GetFloat (strheading);
					text += heading + " ";
				}
			}*/


			//	Debug.Log("Quest_4");
				//text = (string)m_Names [i];
				copy.GetComponentInChildren<UnityEngine.UI.Text> ().text = text;


			//------------------

			copy = (GameObject)GameObject.Instantiate (m_Desc);
			copy.transform.SetParent (m_Content.transform, false);
			copy.SetActive (true);
			rectTransform = copy.GetComponent<RectTransform> ();
			curpos = rectTransform.localPosition.y;
			curposx = rectTransform.localPosition.x;
			curpos -= posoffset;//i * heightentry;
			rectTransform.localPosition = new Vector2 (curposx, curpos);
			m_AddedTexts.Add (copy);

			/*
			//	Debug.Log("Quest_3");

			int curquestid = i + 1;
			string strinreach = "Quest_" + i + "_PointReached";
			int inreach = PlayerPrefs.GetInt (strinreach);

			text = "inr: " + inreach + " h: ";*/
				text = "";//Date: ";//"Done on ";

			string starttime = "Quest_" + i + "_" + "StartQuestTime";
				Debug.Log ("Starttime: " + starttime);
				if (PlayerPrefs.HasKey (starttime)) {
					string date = PlayerPrefs.GetString (starttime);
					string[] strArr;
					strArr = date.Split(new string[] { " " }, System.StringSplitOptions.None);
					text += strArr [0];
				} else {
					text += "No date";
				}
				/*if (bIsReport) {
					int reportanswer = PlayerPrefs.GetInt ("Quest_" + i + "_" + "Report_Type");
					if (reportanswer == 1) {
						text += "\nBuilding has been destroyed.";
					} else {
						text += "\nA Building has been built.";
					}

				} else {

					if (PlayerPrefs.HasKey ("Quest_" + i + "_" + "CampaignType")) {
						int campaigntype = PlayerPrefs.GetInt ("Quest_" + i + "_" + "CampaignType");
						if (campaigntype == 0) {
							text += "\nBuilding campaign";
						} else if (campaigntype == 1) {
							text += "\nFlash campaign";
						} else if (campaigntype == 2) {
							text += "\nLULC campaign";
						}
					} else {
					//	int campaigntype = PlayerPrefs.GetInt ("Quest_" + i + "_" + "CampaignType");
				//		text += "\nOther campaign " + campaigntype;
					}
				}*/


			/*if(PlayerPrefs.HasKey("Quest_" + i + "_" + "EndPositionX")) {
				text += "\nCoordinate: " + PlayerPrefs.GetFloat("Quest_" + i + "_" + "EndPositionX") + ", " + PlayerPrefs.GetFloat("Quest_" + i + "_" + "EndPositionY");
			} else {
				text += "\nCoordinate: No coordinate";
			}*/
			/*			//float heading = 120.0f;
			if (inreach == 1) {
				for (int photo = 1; photo < 6; photo++) {
					string strheading = "Quest_" + i + "_" + photo + "_Heading";
					float heading = PlayerPrefs.GetFloat (strheading);
					text += heading + " ";
				}
			} else {
				for (int photo = 2; photo < 7; photo++) {
					string strheading = "Quest_" + i + "_" + photo + "_Heading";
					float heading = PlayerPrefs.GetFloat (strheading);
					text += heading + " ";
				}
			}*/


			//	Debug.Log("Quest_4");
			//text = (string)m_Names [i];
			copy.GetComponentInChildren<UnityEngine.UI.Text> ().text = text;


			//------------------


                /*
				copy = (GameObject)GameObject.Instantiate (m_Image);
				copy.transform.SetParent (m_Content.transform, false);
				copy.SetActive (true);
				rectTransform = copy.GetComponent<RectTransform> ();
				curpos = rectTransform.localPosition.y;
				curposx = rectTransform.localPosition.x;
				curpos -= posoffset + 71;// - 80;
				rectTransform.localPosition = new Vector2 (curposx, curpos);
				m_AddedTexts.Add (copy);

				UnityEngine.UI.Image image = copy.GetComponent<UnityEngine.UI.Image> ();


                //	Debug.Log("Quest_5");

                bool bFoundImage = false;
                int indeximage = 1;
                while (bFoundImage == false && indeximage <= 5)
                {
                    string name = Application.persistentDataPath + "/" + "Quest_Img_" + i + "_" + indeximage + ".jpg";
                    if (File.Exists(name))
                    {
                        byte[] bytes = File.ReadAllBytes(name);

                        if (bytes != null)
                        {
                            Texture2D texture = new Texture2D(1, 1);
                            texture.LoadImage(bytes);
                            //			text.LoadImage(Convert.FromBase64String(PlayerPrefs.GetString("PrimaryImage_ByteString")));

                            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));


                            image.sprite = sprite;//m_Sprite;
                        }
                        bFoundImage = true;
                    }
                    indeximage++;
                }
*/

			//	Debug.Log("Quest_6");

				
				//	bUploaded = true;

				//	if (bUploaded) {
				copy = (GameObject)GameObject.Instantiate (m_Info);
				copy.transform.SetParent (m_Content.transform, false);
				if (bUploaded) {
					copy.SetActive (true);
				} else {
					copy.SetActive (false);
				}
				rectTransform = copy.GetComponent<RectTransform> ();
				curpos = rectTransform.localPosition.y;
				curposx = rectTransform.localPosition.x;
				curpos -= posoffset;//i * heightentry;
				rectTransform.localPosition = new Vector2 (curposx, curpos);
				m_AddedTexts.Add (copy);

				m_UploadQuestUploaded.Add (copy);

				string strcurquestidparam = "Quest_" + i + "_Id";
				string strcurquestid = PlayerPrefs.GetString(strcurquestidparam);

				

				/*int curstatus = 2;
				if (!bAlreadyAdded) {
					curstatus = getStatusOfQuest (strcurquestid);

					if (curstatus == 2) {
						// Quest has been rejected -> see if quest has already been made before if yes check that quest 
						int howmanystatuses = howManyStatusesForQuest(strcurquestid);
						int howmanyquests = howManyTimesMadeQuest (strcurquestid);

						if (howmanyquests > howmanystatuses) {
							curstatus = 0;
						}
					}
				}
				if (curstatus == 0) {*/
				/*	if (Application.systemLanguage == SystemLanguage.German) {
						text = "Quest hochgeladen.";
					} else {
						text = "Quest uploaded.";
					}*/
					text = LocalizationSupport.GetString("QuestUploaded");
				/*} else if (curstatus == 1) {
					if (Application.systemLanguage == SystemLanguage.German) {
						text = "Quest wurde akzeptiert.";
					} else {
					text = "Quest accepted.";
					}
				} else if (curstatus == 2) {
					if (Application.systemLanguage == SystemLanguage.German) {
						text = "Quest wurde abgelehnt.";
					} else {
					text = "Quest rejected.";
					}
				}*/

				copy.GetComponentInChildren<UnityEngine.UI.Text> ().text = text;
				//} else {


			//	Debug.Log("Quest_7");

				copy = (GameObject)GameObject.Instantiate (m_UploadQuest);
				copy.transform.SetParent (m_Content.transform, false);
				if (bUploaded) {
					copy.SetActive (false);
				} else {
					copy.SetActive (true);
				}
				rectTransform = copy.GetComponent<RectTransform> ();
				curpos = rectTransform.localPosition.y;
				curposx = rectTransform.localPosition.x;
				curpos -= posoffset;//i * heightentry;
				rectTransform.localPosition = new Vector2 (curposx, curpos);
				m_AddedTexts.Add (copy);

				m_UploadButtons.Add (copy);
				m_UploadButtonsQuestId.Add (i + "");


				if (Application.systemLanguage == SystemLanguage.German && false/*|| true*/) {
					copy.GetComponentInChildren<UnityEngine.UI.Text> ().text = "HOCHLADEN";
				} else {
					copy.GetComponentInChildren<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString("Upload");//"UPLOAD";
				}

				UnityEngine.UI.Button b = copy.GetComponent<UnityEngine.UI.Button> ();
				//	b.onClick.AddListener(() => AddListener(b, i+""));
				AddListener (b, i + "");



                /*
				//Debug.Log("Quest_8");
		
				copy = (GameObject)GameObject.Instantiate (m_ShowQuestPhotos);
				copy.transform.SetParent (m_Content.transform, false);

				if (bUploaded) {
					copy.SetActive (false);
				} else {
					copy.SetActive (true);
				}

				rectTransform = copy.GetComponent<RectTransform> ();
				curpos = rectTransform.localPosition.y;
				curposx = rectTransform.localPosition.x;
				curpos -= posoffset;//i * heightentry;
				rectTransform.localPosition = new Vector2 (curposx, curpos);
				//m_AddedTexts.Add (copy);

				m_EditButtons.Add (copy);
				//m_UploadButtons.Add (copy);
				//m_UploadButtonsQuestId.Add (i + "");


				if (Application.systemLanguage == SystemLanguage.German && false) {
					copy.GetComponentInChildren<UnityEngine.UI.Text> ().text = "EDITIEREN";
				} else {
			copy.GetComponentInChildren<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString("Edit");// "EDIT";
				}

				UnityEngine.UI.Button b2 = copy.GetComponent<UnityEngine.UI.Button> ();
				//AddListener (b2, i + "");
				AddListenerShow (b2, i + "");
                */



				//----------------------------
				// View quest hallohallo


		/*	Debug.Log("Quest_9");

				copy = (GameObject)GameObject.Instantiate (m_ShowQuest);
				copy.transform.SetParent (m_Content.transform, false);
				copy.SetActive (true);
		
				rectTransform = copy.GetComponent<RectTransform> ();
				curpos = rectTransform.localPosition.y;
				curposx = rectTransform.localPosition.x;
				curpos -= posoffset;//i * heightentry;
				rectTransform.localPosition = new Vector2 (curposx, curpos);
				


				if (Application.systemLanguage == SystemLanguage.German) {
					copy.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Quest hochladen";
				}

				UnityEngine.UI.Button bquest = copy.GetComponent<UnityEngine.UI.Button> ();
				//	b.onClick.AddListener(() => AddListener(b, i+""));
				AddListenerImage (bquest, i + "");
*/
				//	}


				posoffset += heightentry;

				/*
			copy = (GameObject)GameObject.Instantiate (m_ScoreS);
			copy.transform.SetParent (m_Content.transform, false);
			copy.SetActive (true);
			rectTransform = copy.GetComponent<RectTransform> ();
			curpos = rectTransform.localPosition.y;
			curposx = rectTransform.localPosition.x;
			curpos -= i * 30.0f;
			rectTransform.localPosition = new Vector2 (curposx, curpos);
			m_AddedTexts.Add (copy);
			string textscore = "23422";//(string)m_Scores [i];
			copy.GetComponentInChildren<UnityEngine.UI.Text> ().text = textscore;//"2323";
			*/

			//	Debug.Log("Quest_10");

			}

		}

		Debug.Log ("Quest list created");

	}


	void AddListener(UnityEngine.UI.Button b, string value) 
	{
		b.onClick.AddListener(() => OnQuestUploadClickedValue(value));
	}


	void AddListenerImage(UnityEngine.UI.Button b, string value) 
	{
		b.onClick.AddListener(() => OnShowQuestClickedValue(value));
	}


	void AddListenerShow(UnityEngine.UI.Button b, string value) 
	{
		b.onClick.AddListener(() => OnShowQuestClickedValue(value));
	}

	bool checkLoggedIn() {
		if (PlayerPrefs.HasKey ("PlayerMail") == false) {
			UnityEngine.Events.UnityAction<string> ua = new UnityEngine.Events.UnityAction<string> (OnMsgBoxClicked);
			if (Application.systemLanguage == SystemLanguage.German && false) {
				string[] options = { "Abbrechen", "Login" };
				messageBox.Show ("", "Du musst dich anmelden um Quests hochladen zu können.", ua, options);
			} else {
				string[] options = { LocalizationSupport.GetString("Cancel"), LocalizationSupport.GetString("Login") };
				messageBox.Show ("", LocalizationSupport.GetString("UploadLoginFirst"), ua, options);
			}
			return false;
		}
		return true;
	}


	public void OnShowQuestClickedValue(string param) {
		Debug.Log ("OnShowQuestClickedValue: " + param);
		openQuest (param);
	}

	public void OnQuestUploadClickedValue(string param) {
		Debug.Log ("OnQuestUploadClickedValue: " + param);

		/*if (checkLoggedIn () == false) {
			return;
		}*/


		m_UploadingBack.SetActive (true);
        m_UploadingText.SetActive (true);
        m_UploadingText2.SetActive(true);
		m_UploadingTextWait.SetActive (true);
		m_UploadingImage.SetActive (true);

		if (Application.systemLanguage == SystemLanguage.German && false) {
			m_UploadingTextWait.GetComponentInChildren<UnityEngine.UI.Text> ().text =  "Bitte warten...";
			m_UploadingText.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Quests werden hochgeladen.";
		} else {
		m_UploadingTextWait.GetComponentInChildren<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString("PleaseWait");//"Please wait...";
		m_UploadingText.GetComponentInChildren<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString("UploadingQuests");//"Uploading all quests.";
		}

        m_UploadingText2.GetComponentInChildren<UnityEngine.UI.Text>().text = "";
		m_bUploadAll = false;
		m_UploadAllIter = int.Parse (param);

		uploadQuest(int.Parse (param));
	} 

	bool m_bUploadAll = false;
	int m_UploadAllIter = 0;

	public void OnQuestUploadAllClicked() {
		Debug.Log ("OnQuestUploadAllClicked");
        /*
		if (checkLoggedIn () == false) {
			return;
		}*/


		if (m_NrQuestsDone <= 0) {
			return;
		}

/*
	string[] options2 = { "Ok" };
	messageBox.Show ("", LocalizationSupport.GetString("QuestsUploadSuccessful"), options2);
		return;*/

		m_bUploadAll = true;
		m_UploadAllIter = 0;

		m_UploadingBack.SetActive (true);
        m_UploadingText.SetActive (true);
        m_UploadingText2.SetActive(true);
		m_UploadingTextWait.SetActive (true);
		m_UploadingImage.SetActive (true);

		if (Application.systemLanguage == SystemLanguage.German && false) {
			m_UploadingTextWait.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Bitte warten...";
			m_UploadingText.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Quests werden hochgeladen.";
		} else {
			m_UploadingTextWait.GetComponentInChildren<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString("PleaseWait");//"Please wait...";
			m_UploadingText.GetComponentInChildren<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString("UploadingQuests");//"Uploading all quests.";
        }
        m_UploadingText2.GetComponentInChildren<UnityEngine.UI.Text>().text = "";

		uploadQuest (0);
	}

	public void onCloseQuestClicked() {
		m_bShowQuest = false;
		m_BtnCloseQuest.SetActive (false);
		m_BtnDeleteQuest.SetActive (false);
		m_ImageQuest.SetActive (false);

		m_SkyQuestBack.SetActive (false);

		m_QuestTitle.SetActive (false);
		m_QuestText.SetActive (false);

		m_Image1Quest.SetActive (false);
		m_Image2Quest.SetActive (false);
		m_Image3Quest.SetActive (false);
		m_Image4Quest.SetActive (false);
		m_Image5Quest.SetActive (false);

		m_BtnEditImage1.SetActive (false);
		m_BtnEditImage2.SetActive (false);
		m_BtnEditImage3.SetActive (false);
		m_BtnEditImage4.SetActive (false);
		m_BtnEditImage5.SetActive (false);
		m_TextQuestExplanation.SetActive (false);
	}

	public void onDeleteQuestClicked() {
		UnityEngine.Events.UnityAction<string> ua = new UnityEngine.Events.UnityAction<string> (OnDeleteClicked);
		if (Application.systemLanguage == SystemLanguage.German && false) {
			string[] options = { "Abbrechen", "Löschen" };
			messageBox.Show ("", "Willst du wirklich die Quest löschen?", ua, options);
		} else {
		string[] options = { LocalizationSupport.GetString("Cancel"), LocalizationSupport.GetString("Delete") };
		messageBox.Show ("", LocalizationSupport.GetString("ReallyDelete"), ua, options);
		}
	}

	void OnDeleteClicked(string result) {
		Debug.Log ("OnMsgBoxClicked: " + result);
		if (result == "Löschen" || result == "Delete") {
			Debug.Log ("Quest deleted!");

			string strdeleted = "Quest_" + m_OpenQuestId + "_Del";
			PlayerPrefs.SetInt (strdeleted, 1);

			Application.LoadLevel ("Quests");
		} else {
			Debug.Log ("Quest not deleted");
		}
	}

	IEnumerator loadQuestImages(int inreach, int questid) {
		string name;

		//if (inreach == 1) {
			name = Application.persistentDataPath + "/" + "Quest_Img_" + questid + "_" + 1 + ".jpg";
		/*} else {
			name = Application.persistentDataPath + "/" + "Quest_Img_" + questid + "_" + 2 + ".jpg";
		}*/
		m_bImg1Set = false;
		UnityEngine.UI.Image image = m_Image1Quest.GetComponent<UnityEngine.UI.Image>();
		if(File.Exists(name)) {
			m_StrImg1 = name;
			m_bImg1Set = true;
			byte[] bytes = File.ReadAllBytes (name);
			if(bytes != null) {
				Texture2D texture = new Texture2D(1, 1);
				texture.LoadImage (bytes);
				Sprite sprite = Sprite.Create (texture, new Rect(0,0,texture.width,texture.height), new Vector2(.5f,.5f));
				image.sprite = sprite;//m_Sprite;
            }
            m_Image1Quest.SetActive(true);
        } else {
            UnityEngine.UI.Image imagenopic = m_ImageNoPicSmall.GetComponent<UnityEngine.UI.Image>();
            image.sprite = imagenopic.sprite;
            m_Image1Quest.SetActive(true);
        }


		//if (inreach == 1) {
			name = Application.persistentDataPath + "/" + "Quest_Img_" + questid + "_" + 2 + ".jpg";
		/*} else {
			name = Application.persistentDataPath + "/" + "Quest_Img_" + questid + "_" + 3 + ".jpg";
		}*/
		m_bImg2Set = false;
		image = m_Image2Quest.GetComponent<UnityEngine.UI.Image>();
		if(File.Exists(name)) {
			m_StrImg2 = name;
			m_bImg2Set = true;
			byte[] bytes = File.ReadAllBytes (name);
			if(bytes != null) {
				Texture2D texture = new Texture2D(1, 1);
				texture.LoadImage (bytes);
				Sprite sprite = Sprite.Create (texture, new Rect(0,0,texture.width,texture.height), new Vector2(.5f,.5f));
				image.sprite = sprite;//m_Sprite;
            }
            m_Image2Quest.SetActive(true);
        }
        else
        {
            UnityEngine.UI.Image imagenopic = m_ImageNoPicSmall.GetComponent<UnityEngine.UI.Image>();
            image.sprite = imagenopic.sprite;
            m_Image2Quest.SetActive(true);
        }


		//if (inreach == 1) {
			name = Application.persistentDataPath + "/" + "Quest_Img_" + questid + "_" + 3 + ".jpg";
		/*} else {
			name = Application.persistentDataPath + "/" + "Quest_Img_" + questid + "_" + 4 + ".jpg";
		}*/
		m_bImg3Set = false;
		image = m_Image3Quest.GetComponent<UnityEngine.UI.Image>();
		if(File.Exists(name)) {
			m_StrImg3 = name;
			m_bImg3Set = true;
			byte[] bytes = File.ReadAllBytes (name);
			if(bytes != null) {
				Texture2D texture = new Texture2D(1, 1);
				texture.LoadImage (bytes);
				Sprite sprite = Sprite.Create (texture, new Rect(0,0,texture.width,texture.height), new Vector2(.5f,.5f));
				image.sprite = sprite;//m_Sprite;
            }
            m_Image3Quest.SetActive(true);
        }
        else
        {
            UnityEngine.UI.Image imagenopic = m_ImageNoPicSmall.GetComponent<UnityEngine.UI.Image>();
            image.sprite = imagenopic.sprite;
            m_Image3Quest.SetActive(true);
        }


		//if (inreach == 1) {
			name = Application.persistentDataPath + "/" + "Quest_Img_" + questid + "_" + 4 + ".jpg";
		/*} else {
			name = Application.persistentDataPath + "/" + "Quest_Img_" + questid + "_" + 5 + ".jpg";
		}*/
		image = m_Image4Quest.GetComponent<UnityEngine.UI.Image>();
		m_bImg4Set = false;
		if(File.Exists(name)) {
			m_StrImg4 = name;
			m_bImg4Set = true;
			byte[] bytes = File.ReadAllBytes (name);
			if(bytes != null) {
				Texture2D texture = new Texture2D(1, 1);
				texture.LoadImage (bytes);
				Sprite sprite = Sprite.Create (texture, new Rect(0,0,texture.width,texture.height), new Vector2(.5f,.5f));
				image.sprite = sprite;//m_Sprite;
            }
            m_Image4Quest.SetActive(true);
        }
        else
        {
            UnityEngine.UI.Image imagenopic = m_ImageNoPicSmall.GetComponent<UnityEngine.UI.Image>();
            image.sprite = imagenopic.sprite;
            m_Image4Quest.SetActive(true);
        }


	//	if (inreach == 1) {
			name = Application.persistentDataPath + "/" + "Quest_Img_" + questid + "_" + 5 + ".jpg";
		/*} else {
			name = Application.persistentDataPath + "/" + "Quest_Img_" + questid + "_" + 6 + ".jpg";
		}*/
		image = m_Image5Quest.GetComponent<UnityEngine.UI.Image>();
		m_bImg5Set = false;
		if(File.Exists(name)) {
			m_StrImg5 = name;
			m_bImg5Set = true;
			byte[] bytes = File.ReadAllBytes (name);
			if(bytes != null) {
				Texture2D texture = new Texture2D(1, 1);
				texture.LoadImage (bytes);
				Sprite sprite = Sprite.Create (texture, new Rect(0,0,texture.width,texture.height), new Vector2(.5f,.5f));
				image.sprite = sprite;//m_Sprite;
            }
            m_Image5Quest.SetActive(true);
        }
        else
        {
            UnityEngine.UI.Image imagenopic = m_ImageNoPicSmall.GetComponent<UnityEngine.UI.Image>();
            image.sprite = imagenopic.sprite;

            m_Image5Quest.SetActive(true);
        }

        /*
		m_Image1Quest.SetActive (true);
		m_Image2Quest.SetActive (true);
		m_Image3Quest.SetActive (true);
		m_Image4Quest.SetActive (true);
		m_Image5Quest.SetActive (true);*/

		m_BtnEditImage1.SetActive (true);
		m_BtnEditImage2.SetActive (true);
		m_BtnEditImage3.SetActive (true);
		m_BtnEditImage4.SetActive (true);
		m_BtnEditImage5.SetActive (true);
		m_TextQuestExplanation.SetActive (true);
		m_QuestText.SetActive (false);

		yield return new WaitForSeconds(0);
	}


	int m_OpenQuestId = -1;
	string m_CurrentOpenQuest;
	bool m_bLoadImagesForQuest = false;
	int m_LoadImagesForQuestInReach = 0;
	int m_LoadImagesForQuestId = 0;
	int m_LoadImagesForQuestIter = 0;
	void openQuest(string whichquest) {

		string stralreadyuploaded = "Quest_" + whichquest + "_Uploaded";
	if(PlayerPrefs.HasKey(stralreadyuploaded)) {
			return; // Dont open uploaded quests anymore
	}


		int iquest = int.Parse(whichquest);

		int questtitlenr = iquest + 1;
		m_QuestTitle.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Quest " + questtitlenr;

		m_ImageQuest.SetActive (true);
		m_QuestTitle.SetActive (true);

		m_OpenQuestId = iquest;
		m_CurrentOpenQuest = whichquest;

		int questid = iquest;
		int inreach = 1;
		/*if (PlayerPrefs.HasKey ("Quest_" + questid + "_PointReached")) {
			inreach = PlayerPrefs.GetInt ("Quest_" + questid + "_PointReached");
			if (inreach == 0) {
			}
		}*/

		float lat = 0.0f;
		float lng = 0.0f;

		lat = PlayerPrefs.GetFloat ("Quest_" + questid + "_" + "LandCover" + "_Lat");
		lng = PlayerPrefs.GetFloat ("Quest_" + questid + "_" + "LandCover" + "_Lng");
		string date = PlayerPrefs.GetString ("Quest_" + questid + "_" + "LandCover" + "_Timestamp");



		







		//Debug.Log("Quest coord: " + PlayerPrefs.GetFloat ("Quest_" + questid + "_" + "LandCover" + "_Lat"));
		iquest++;
		m_bShowQuest = true;
		m_BtnCloseQuest.SetActive (true);
		m_BtnDeleteQuest.SetActive (true);
		m_ImageQuest.SetActive (true);
		m_SkyQuestBack.SetActive (false);
		m_QuestTitle.SetActive (true);
	//	m_QuestText.SetActive (true);

	/*
		m_Image1Quest.SetActive (true);
		m_Image2Quest.SetActive (true);
		m_Image3Quest.SetActive (true);
		m_Image4Quest.SetActive (true);
		m_Image5Quest.SetActive (true);

		m_BtnEditImage1.SetActive (true);
		m_BtnEditImage2.SetActive (true);
		m_BtnEditImage3.SetActive (true);
		m_BtnEditImage4.SetActive (true);
		m_BtnEditImage5.SetActive (true);
		m_TextQuestExplanation.SetActive (true);*/

		//m_QuestTitle.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Quest " + iquest;
	/*
		if (inreach == 1) {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_QuestText.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Datum: " + date + "\nPunkt erreicht.\nKoordinate: " + lat + ", " + lng;
			} else {
				m_QuestText.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Date: " + date + "\nPoint reached.\nCoordinate: " + lat + ", " + lng;
			}
		} else {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_QuestText.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Datum: " + date + "\nPunkt nicht erreicht.\nKoordinate: " + lat + ", " + lng;
			} else {
				m_QuestText.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Date: " + date + "\nPoint not reached.\nCoordinate: " + lat + ", " + lng;
			}
		}
*/
		if (Application.systemLanguage == SystemLanguage.German && false) {
			m_QuestText.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Wird geladen...";
		} else {
		m_QuestText.GetComponentInChildren<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString("Loading");//"Loading...";
		}
		m_QuestText.SetActive (true);
		m_LoadImagesForQuestIter = 0;
		m_bLoadImagesForQuest = true;
		m_LoadImagesForQuestInReach = inreach;
		m_LoadImagesForQuestId = questid;
		//StartCoroutine(loadQuestImages(inreach, questid));

	}

	void OnMsgBoxClicked(string result) {
		Debug.Log ("OnMsgBoxClicked: " + result);
		if (result.CompareTo(LocalizationSupport.GetString("Login")) == 0) {
			PlayerPrefs.SetInt ("LoginReturnToQuests", 1);
			PlayerPrefs.SetInt ("RegisterMsgShown", 0);
			PlayerPrefs.Save ();

			Application.LoadLevel ("StartScreen");
		}
	}

    int m_QuestCurrentlyUploading = -1;
    void uploadQuest(int questid) {
        m_QuestCurrentlyUploading = questid;

		Debug.Log ("Upload quest: " + questid);

		int whichquest = questid + 1;
		if (Application.systemLanguage == SystemLanguage.German && false) {
			m_UploadingText.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Quest " + whichquest + " wird hochgeladen.";
		} else {
		m_UploadingText.GetComponentInChildren<UnityEngine.UI.Text> ().text = LocalizationSupport.GetString("UploadingQuest1") + " " + whichquest +" " + LocalizationSupport.GetString("UploadingQuest2");
		}

		string strdeleted = "Quest_" + questid + "_Del";
		int deleted = 0;
		if (PlayerPrefs.HasKey (strdeleted)) {
			deleted = PlayerPrefs.GetInt (strdeleted);
		}



		string stralreadyuploaded = "Quest_" + questid + "_Uploaded";
		bool bUploaded = false;
		if (PlayerPrefs.HasKey (stralreadyuploaded) || deleted==1) {

			if (!m_bUploadAll) {
				m_UploadingBack.SetActive (false);
                m_UploadingText.SetActive (false);
                m_UploadingText2.SetActive(false);
				m_UploadingTextWait.SetActive (false);
				m_UploadingImage.SetActive (false);
			} else {
				m_UploadAllIter++;
				if (m_UploadAllIter < m_NrQuestsDone) {
					uploadQuest (m_UploadAllIter);
				} else {
					m_UploadingBack.SetActive (false);
                    m_UploadingText.SetActive (false);
                    m_UploadingText2.SetActive(false);
					m_UploadingTextWait.SetActive (false);
					m_UploadingImage.SetActive (false);
				}
			}

			return;
		}



        m_UploadingText2.GetComponentInChildren<UnityEngine.UI.Text>().text = "";

        //---------------------------
        // Upload pictures

        string pictureurls = "";
        int nrpicturestoupload = 0;
        for (int blurphoto = 1; blurphoto < 6; blurphoto++)
        {
            if (PlayerPrefs.GetInt("Quest_" + questid + "_" + "PhotoTaken" + "_" + blurphoto) == 1)
            {
                if (!PlayerPrefs.HasKey("Quest_" + questid + "_" + "PhotoUploaded" + "_" + blurphoto))
                {
                    string filenamename = Application.persistentDataPath + "/" + "Quest_Img_" + questid + "_" + blurphoto + ".jpg";
                    if (File.Exists(filenamename))
                    {
                        m_UploadingText2.GetComponentInChildren<UnityEngine.UI.Text>().text = LocalizationSupport.GetString("UploadingPhoto" + blurphoto);

                        uploadPhoto(questid, blurphoto);
                        return;
                    }
                } else {
                    if (blurphoto == 1)
                    {
                        pictureurls = "\"" + PlayerPrefs.GetString("Quest_" + questid + "_" + "PhotoUploaded" + "_" + blurphoto) + "\"";
                    }
                    else
                    {
                        pictureurls = pictureurls + "," + "\"" + PlayerPrefs.GetString("Quest_" + questid + "_" + "PhotoUploaded" + "_" + blurphoto) + "\"";
                    }
                    nrpicturestoupload++;

                    Debug.Log("Photo " + blurphoto + " already uploaded. Url: " + PlayerPrefs.GetString("Quest_" + questid + "_" + "PhotoUploaded" + "_" + blurphoto));
                }
            }
        }

        Debug.Log("All photos uploaded");
		string param = "";

		Debug.Log ("Upload quest -> 3");

/*        param += "{\"email\":" + "\"" + email + "\",\"md5password\":" + "\"" + passwordmd5 + "\"";

		int trainingpoint = PlayerPrefs.GetInt ("Quest_" + questid + "_TrainingPoint");
		if (trainingpoint == 1) {
			float flat = PlayerPrefs.GetFloat ("Quest_" + questid + "_TrainingPoint_Lat");
			float flng = PlayerPrefs.GetFloat ("Quest_" + questid + "_TrainingPoint_Lng");

			Debug.Log ("Upload training point lat: " + flat + " lng. " + flng);

            param += ",\"" + "latitude" + "\":" + flat +
                "," + "\"" + "longitude" + "\":" + flng;
        } else {
            param += ",\"id\":" + "\"" + strquestid + "\"";
        }*/




        int trainingpoint = PlayerPrefs.GetInt("Quest_" + questid + "_TrainingPoint");

        string valid = PlayerPrefs.GetString("Quest_" + questid + "_ValidationId");
        int classificationid = PlayerPrefs.GetInt("Quest_" + questid + "_ClassificationId");
        string sampleid = PlayerPrefs.GetString("Quest_" + questid + "_Id");

        Debug.Log("Upload quest trainingpoint: " + trainingpoint + " valid: " + valid + " sampleid: " + sampleid + " classid: " + classificationid);



        if(trainingpoint == 1) {
           /* m_UploadingBack.SetActive(false);
            m_UploadingText.SetActive(false);
            m_UploadingTextWait.SetActive(false);
            m_UploadingImage.SetActive(false);

            string[] options2 = { "Ok" };
            messageBox.Show("", "Can't upload opportunistic samples yet.", options2);*/
            /// api / mobile / validationsessions /{ validationsessionid}/ opportunistic / validations

            string url = "https://dev.laco-wiki.net/api/mobile/validationsessions/" +
                valid + "/opportunistic/validations";

            string token = PlayerPrefs.GetString("Token");
            Debug.Log("token: " + token);

            param = "{";


            float strlat = PlayerPrefs.GetFloat("Quest_" + questid + "_TrainingPoint_Lat");
            float strlng = PlayerPrefs.GetFloat("Quest_" + questid + "_TrainingPoint_Lng");

            param += "\"legendItemID\":" + classificationid + ",";
            param += "\"Latitude\":" + strlat + ",";
            param += "\"Longitude\":" + strlng;

            if(nrpicturestoupload > 0) {
                param += ",";
                param += "\"ValidationFilePaths\":[" + pictureurls + "]";
            }

            param += "}";


            Debug.Log("Upload Parameter: " + param);
        //return;
            StartCoroutine(StartUploadingQuest(url, param));
        } else {
            string url = "https://dev.laco-wiki.net/api/mobile/validationsessions/" +
                valid + "/sampleItems/" + sampleid + "/validations";

            string token = PlayerPrefs.GetString("Token");
            Debug.Log("token: " + token);

            param = "{";
               
            param += "\"legendItemID\":" + classificationid; 

            if (nrpicturestoupload > 0)
            {
                param += ",";
                param += "\"ValidationFilePaths\":[" + pictureurls + "]";
            }

            param += "}";

            Debug.Log("Upload Parameter: " + param);
        //return;
            StartCoroutine(StartUploadingQuest(url, param));
        }
        /*
        string strcomment = PlayerPrefs.GetString("Quest_" + questid + "_" + "Comment");
        param += ",\"comment\":\"" + strcomment + "\"";

		param += "}";*/

		//Debug.Log("UploadQuest param: " + param);


        //----------------------
        // Comment this out for release -> just for debugging
      /*  m_UploadingText.GetComponentInChildren<UnityEngine.UI.Text>().text = "";
        m_DebugText.GetComponentInChildren<UnityEngine.UI.Text>().text = param;
        m_DebugText.SetActive(true);
        //----------------------*/

		//string url = "https://geo-wiki.org/Application/api/Campaign/CityOasesUpload";
        /*

		WWWForm form = new WWWForm();
		form.AddField ("parameter", param);

		//Debug.Log ("Url data: " + System.Text.Encoding.UTF8.GetString(form.data));
		WWW www = new WWW(url, form);

		// TODO: COMMENT THIS IN AGAIN FOR FINAL VERSION!!!
		StartCoroutine(WaitForDataUpload(www));*/
	}

    IEnumerator StartUploadingQuest(string url, string param)     {       //  UnityWebRequest www = UnityWebRequest.Get(url);         string token = PlayerPrefs.GetString("Token");        // www.SetRequestHeader("Authorization", "Bearer " + token);

        string[] options3 = { LocalizationSupport.GetString("Login") }; 
        Debug.Log("header: " + "Bearer " + token);
        Debug.Log("Body: " + param);
        Debug.Log("Url: " + url);

        // yield return www.Send();//www.SendWebRequest();
      //  using (UnityWebRequest www = UnityWebRequest.Post(url, param))//UnityWebRequest.Post(url, param))
        using (UnityWebRequest www = UnityWebRequest.Post(url, "POST"))//UnityWebRequest.Post(url, param))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(param);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");


            www.SetRequestHeader("Authorization", "Bearer " + token);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("Error: " + www.error);
                if (www.error == "HTTP/1.1 401 Unauthorized")
                {
                    UnityEngine.Events.UnityAction<string> ua = new UnityEngine.Events.UnityAction<string>(OnLoginAgainClicked);

                    messageBox.Show("", LocalizationSupport.GetString("LoginExpired"), ua, options3);
                }
                else
                {

                    if (!m_bUploadAll)
                    {
                        m_UploadingBack.SetActive(false);
                        m_UploadingText.SetActive(false);
                        m_UploadingText2.SetActive(false);
                        m_UploadingTextWait.SetActive(false);
                        m_UploadingImage.SetActive(false);

                        string[] options2 = { "Ok" };
                        string errormsg = www.error;
                        if (errormsg.Length <= 0)
                        {
                            messageBox.Show("", "Upload failed.", options2);
                        }
                        else
                        {
                            string data = www.downloadHandler.text;
                            if (data.Contains("Location is outside dataset extent."))
                            {
                                messageBox.Show("", "Upload failed: Location is outside dataset extent.", options2);
                            }
                            else
                            {
                                messageBox.Show("", "Upload failed: " + www.error, options2);
                            }
                        }
                    }
                    else
                    {
                        m_UploadAllIter++;
                        if (m_UploadAllIter < m_NrQuestsDone)
                        {
                            uploadQuest(m_UploadAllIter);
                        }
                        else
                        {
                            m_UploadingBack.SetActive(false);
                            m_UploadingText.SetActive(false);
                            m_UploadingText2.SetActive(false);
                            m_UploadingTextWait.SetActive(false);
                            m_UploadingImage.SetActive(false);

                            string[] options2 = { "Ok" };
                            //messageBox.Show ("", "Upload failed: " + www.data , options2);

                            string errormsg = www.error;
                            if (errormsg.Length <= 0)
                            {
                                messageBox.Show("", "Upload failed.", options2);
                            }
                            else
                            {
                                string data = www.downloadHandler.text;
                                if (data.Contains("Location is outside dataset extent."))
                                {
                                    messageBox.Show("", "Upload failed: Location is outside dataset extent.", options2);
                                }
                                else
                                {
                                    messageBox.Show("", "Upload failed: " + www.error, options2);
                                }
                            }

                        }
                    }
                }
            }
            else
            {
                string data = www.downloadHandler.text;
                //string[] parts = data.Split (":", 2);

                Debug.Log("Upload successful result: " + data + " iter: " + m_UploadAllIter);

                questSuccessfullyUploaded();
            }
        }     }
    /*
	IEnumerator WaitForDataUpload(WWW www)
	{
		yield return www;

		string[] options = { "Ok" };
        string[] options3 = { LocalizationSupport.GetString("Login") };

		// check for errors
		if (www.error == null)
		{
			string data = www.text;
			//string[] parts = data.Split (":", 2);

			Debug.Log ("Upload successful result: " + data + " iter: " + m_UploadAllIter);

			questSuccessfullyUploaded ();
		} else {
            Debug.Log("#### Uploading quest error ####");
			Debug.Log("WWW Error: "+ www.error);
			Debug.Log("WWW Error 2: "+ www.text);

            if (www.error == "401 Unauthorized")
            {
                UnityEngine.Events.UnityAction<string> ua = new UnityEngine.Events.UnityAction<string>(OnLoginAgainClicked);

                messageBox.Show("", LocalizationSupport.GetString("LoginExpired"), ua, options3);
            }
            else
            {

                if (!m_bUploadAll)
                {
                    m_UploadingBack.SetActive(false);
                    m_UploadingText.SetActive(false);
                    m_UploadingTextWait.SetActive(false);
                    m_UploadingImage.SetActive(false);

                    string[] options2 = { "Ok" };
                    string errormsg = www.text;
                    if (errormsg.Length <= 0)
                    {
                        messageBox.Show("", "Upload failed.", options2);
                    }
                    else
                    {
                        messageBox.Show("", "Upload failed: " + www.text, options2);
                    }
                }
                else
                {
                    m_UploadAllIter++;
                    if (m_UploadAllIter < m_NrQuestsDone)
                    {
                        uploadQuest(m_UploadAllIter);
                    }
                    else
                    {
                        m_UploadingBack.SetActive(false);
                        m_UploadingText.SetActive(false);
                        m_UploadingTextWait.SetActive(false);
                        m_UploadingImage.SetActive(false);

                        string[] options2 = { "Ok" };
                        //messageBox.Show ("", "Upload failed: " + www.data , options2);

                        string errormsg = www.text;
                        if (errormsg.Length <= 0)
                        {
                            messageBox.Show("", "Upload failed.", options2);
                        }
                        else
                        {
                            messageBox.Show("", "Upload failed: " + www.text, options2);
                        }

                    }
                }
            }
		}   
	} */


	public void questSuccessfullyUploaded()
	{
		string stralreadyuploaded = "Quest_" + m_UploadAllIter + "_Uploaded";
		PlayerPrefs.SetInt (stralreadyuploaded, 1);
		PlayerPrefs.Save ();

		Debug.Log ("=== Disable upload buttons nr buttons: " + m_UploadButtons.Count + " ===");
		for (int i = 0; i < m_UploadButtons.Count; i++) {
			string thequestid = (string)m_UploadButtonsQuestId [i];
			int ithequestid = int.Parse (thequestid);
			Debug.Log (i + " quest id: " + ithequestid);
			if (ithequestid == m_UploadAllIter) {
				Debug.Log ("Found entry -> disable upload button");
				GameObject obj = (GameObject)m_UploadQuestUploaded [i];
				obj.SetActive (true);
				GameObject obj2 = (GameObject)m_UploadButtons [i];
				obj2.SetActive (false);
				/*GameObject obj3 = (GameObject)m_EditButtons [i];
				obj3.SetActive (false);*/
			}
		}



		if (!m_bUploadAll) {
			m_UploadingBack.SetActive (false);
            m_UploadingText.SetActive (false);
            m_UploadingText2.SetActive(false);
			m_UploadingTextWait.SetActive (false);
			m_UploadingImage.SetActive (false);

			bool bOneQuestNotUploaded = false;
			for (int i = 0; i < m_NrQuestsDone && !bOneQuestNotUploaded; i++) {
				string stralreadyuploadedquest = "Quest_" + i + "_Uploaded";
				if (PlayerPrefs.HasKey (stralreadyuploadedquest) == false) {
					bOneQuestNotUploaded = true;
				}
			}
			if (bOneQuestNotUploaded) {
				m_UploadingAll.SetActive (true);
			} else {
				m_UploadingAll.SetActive (false);
			}


		/*	if (m_NrQuestsDone >= 5 && bOneQuestNotUploaded == false) {
				UnityEngine.Events.UnityAction<string> ua = new UnityEngine.Events.UnityAction<string> (OnMsgBoxAllUploadedClicked);
				string[] options = { "Ok"} ;
				messageBox.Show ("", LocalizationSupport.GetString("QuestUploadSuccessfulPrize"), ua, options);
			} else {*/
				string[] options2 = { "Ok" };
				messageBox.Show ("", LocalizationSupport.GetString ("QuestUploadSuccessful"), options2);
		//	}

			//stardLoading ();
		} else {
			m_UploadAllIter++;
			if (m_UploadAllIter < m_NrQuestsDone) {
				uploadQuest (m_UploadAllIter);
			} else {
				m_UploadingBack.SetActive (false);
                m_UploadingText.SetActive (false);
                m_UploadingText2.SetActive(false);
				m_UploadingTextWait.SetActive (false);
				m_UploadingImage.SetActive (false);

				bool bOneQuestNotUploaded = false;
				for (int i = 0; i < m_NrQuestsDone && !bOneQuestNotUploaded; i++) {
					string stralreadyuploadedquest = "Quest_" + i + "_Uploaded";
					if (PlayerPrefs.HasKey (stralreadyuploadedquest) == false) {
						bOneQuestNotUploaded = true;
					}
				}
				if (bOneQuestNotUploaded) {
					m_UploadingAll.SetActive (true);
				} else {
					m_UploadingAll.SetActive (false);
				}

			/*if (m_NrQuestsDone >= 5) {
				UnityEngine.Events.UnityAction<string> ua = new UnityEngine.Events.UnityAction<string> (OnMsgBoxAllUploadedClicked);
				string[] options = { "Ok"} ;
				messageBox.Show ("", LocalizationSupport.GetString("QuestUploadSuccessfulPrize"), ua, options);
			} else {*/
				string[] options2 = { "Ok" };
				messageBox.Show ("", LocalizationSupport.GetString ("QuestsUploadSuccessful"), options2);
		//	}


			//	stardLoading ();
			}
		}
	}

	string m_EditImageFile;
	public void EditImage1()
	{
        if(m_bImg1Set == false) {
            return;
        }
		m_EditImageFile = m_StrImg1;
		openEditImage ();
	}
	public void EditImage2()
    {
        if (m_bImg2Set == false)
        {
            return;
        }
		m_EditImageFile = m_StrImg2;
		openEditImage ();
	}
	public void EditImage3()
    {
        if (m_bImg3Set == false)
        {
            return;
        }
		m_EditImageFile = m_StrImg3;
		openEditImage ();
	}
	public void EditImage4()
    {
        if (m_bImg4Set == false)
        {
            return;
        }
		m_EditImageFile = m_StrImg4;
		openEditImage ();
	}
	public void EditImage5()
    {
        if (m_bImg5Set == false)
        {
            return;
        }
		m_EditImageFile = m_StrImg5;
		openEditImage ();
	}
	void openEditImage()
	{
		UnityEngine.UI.Image image = m_EditImage.GetComponent<UnityEngine.UI.Image>();
		if(File.Exists(m_EditImageFile)) {
			byte[] bytes = File.ReadAllBytes (m_EditImageFile);
			if(bytes != null) {
				Texture2D texture = new Texture2D(1, 1);
				texture.LoadImage (bytes);
				Sprite sprite = Sprite.Create (texture, new Rect(0,0,texture.width,texture.height), new Vector2(.5f,.5f));
				image.sprite = sprite;//m_Sprite;

				image.preserveAspect = true;
				Debug.Log ("image width: " + texture.width + " height: " + texture.height);
			}
		}


		Debug.Log ("openEditImage");
		m_EditImageBack.SetActive (true);
		m_EditImage.SetActive (true);
		m_EditImageClose.SetActive (true);
		m_EditImageClose2.SetActive (true);
		m_EditImageExpl.SetActive (true);
		m_EditImageExplBack.SetActive (true);

		m_bHideEditImageExplanation = false;
		m_bHiddenEditImageExplanation = false;
		float proc = 1.0f;
		byte alpha = (byte)(208 * proc);
		m_EditImageExplBack.GetComponent<UnityEngine.UI.Image> ().color = new Color32 (255, 255, 255, alpha);
		alpha = (byte)(255 * proc);
		m_EditImageExpl.GetComponent<UnityEngine.UI.Text> ().color = new Color32 (0, 0, 0, alpha);
	}

	public void CloseEditImage()
	{
		m_bBluring = false;
		m_EditImageBack.SetActive (false);
		m_EditImage.SetActive (false);
		m_EditImageClose.SetActive (false);
		m_EditImageClose2.SetActive (false);
		m_EditImageExpl.SetActive (false);
		m_EditImageExplBack.SetActive (false);
	}

	public void SaveEditImage() 
	{

		UnityEngine.UI.Image image = m_EditImage.GetComponent<UnityEngine.UI.Image>();
		Sprite sprite = image.sprite;
		Texture2D tex = sprite.texture;


		byte[] bytes = tex.EncodeToJPG ();
		//string name = "Quest_Img_" + m_NrQuestsDone + "_" + m_CurrentStep;


		if (Application.platform == RuntimePlatform.Android) { 
		//	File.WriteAllBytes( Application.persistentDataPath+"/"+name+".jpg",bytes );
			File.WriteAllBytes( m_EditImageFile,bytes );
		} else {
		//	File.WriteAllBytes( Application.persistentDataPath+"/"+name+".jpg",bytes );
			File.WriteAllBytes( m_EditImageFile,bytes );
		}

		openQuest (m_CurrentOpenQuest);
		CloseEditImage ();

	}


bool m_bBluring = false;
int m_BluringPosX = 0;
int m_BluringPosY = 0;

void blurImage(int curposx, int curposy)
{

	UnityEngine.UI.Image image = m_EditImage.GetComponent<UnityEngine.UI.Image>();
	Sprite sprite = image.sprite;
	Texture2D tex = sprite.texture;
	int width = tex.width;
	int height = tex.height;
	Debug.Log("texturewidth: " + width + " height: " + height);

	int border = 30;//40;
	int core = 3;//3;//6;


	int corex;
	int corey;

	float r = 0;
	float g = 0;
	float b = 0;
	int nrcores = 0;
	int _x;
	int _y;
	Color newcol;

	for (int x = -border; x < border; x++) {
		for (int y = -border; y < border; y++) {
			corex = x + curposx;
			corey = y + curposy;

			r = 0;
			g = 0;
			b = 0;
			nrcores = 0;

			for (_x = corex - core; _x <= corex + core; _x++) {
				for (_y = corey - core; _y <= corey + core; _y++) {
					nrcores++;

					Color col = tex.GetPixel (_x, _y);
					r += col.r;
					g += col.g;
					b += col.b;
				}
			}

			r /= nrcores;
			g /= nrcores;
			b /= nrcores;

			newcol = new Color (r, g, b);

			tex.SetPixel (corex, corey, newcol);
		}
	}
	tex.Apply();

}

public void OnImageReleased(BaseEventData data)
{
		m_bBluring = false;
}

	public void OnImageClicked(BaseEventData data)
	{
		m_bBluring = true;
		m_EditImageExplBack.SetActive (false);
		m_EditImageExpl.SetActive (false);




		PointerEventData pointerData = data as PointerEventData;
		Debug.Log ("OnImageClicked x: " + pointerData.position.x + " y: " + pointerData.position.y);

		UnityEngine.UI.Image image = m_EditImage.GetComponent<UnityEngine.UI.Image>();
		Sprite sprite = image.sprite;
		Texture2D tex = sprite.texture;
		int width = tex.width;
		int height = tex.height;
		Debug.Log("texturewidth: " + width + " height: " + height);



	float screenwidth = Screen.width;
	float screenheight = Screen.height;
	Debug.Log ("Screenwidth: " + screenwidth + " height: " + screenheight);

	float procx = ((float)pointerData.position.y / (float)screenheight);
	float procy = 1.0f - ((float)pointerData.position.x / (float)screenwidth);
	Debug.Log ("procx: " + procx + " y: " + procy);


	float ratio = screenwidth / screenheight;
	float heightshown = width * ratio;
	Debug.Log ("Height shown: " + heightshown);



	float heightdif = heightshown - height;
	heightdif *= 0.5f;

	int curposx = (int)(procx * width);
	//int curposy = (int)(procy * height);
	int curposy = (int)(procy * heightshown);
	curposy -= (int)heightdif;

	Debug.Log ("paint posx: " + curposx + " posy: " + curposy);

		m_bBluring = true;
		m_BluringPosX = curposx;
		m_BluringPosY = curposy;

		blurImage (curposx, curposy);


	/**/
		/*
		//---------------------
		// For test
		border = 6;
		for (int x = -border; x < border; x++) {
			for (int y = -border; y < border; y++) {
				corex = x + curposx;
				corey = y + curposy;


				newcol = new Color (1.0f, 0.0f, 0.0f);

				tex.SetPixel (corex, corey, newcol);
			}
		}
		tex.Apply();*/

	}



	void OnMsgBoxAllUploadedClicked(string result) {
		/*if (PlayerPrefs.HasKey ("ProfileSaved") == false) {
			UnityEngine.Events.UnityAction<string> ua = new UnityEngine.Events.UnityAction<string> (OnMsgBoxProfileClicked);
			string[] options = { LocalizationSupport.GetString("QuestionProfileTimeNo"), LocalizationSupport.GetString("QuestionProfileTimeYes")};
			messageBox.Show ("", LocalizationSupport.GetString("QuestionProfileTime"), ua, options);
		}*/
	}

	void OnMsgBoxProfileClicked(string result) {
		if (result.CompareTo( LocalizationSupport.GetString("QuestionProfileTimeYes")) == 0) {
			Application.targetFrameRate = 30;
			//	Application.LoadLevel ("DynamicQuestionsPark");
			Application.LoadLevel ("Profile");
		}
	}


    void OnLoginAgainClicked(string result)
    {
        PlayerPrefs.SetInt("BackToUpload", 1);
        PlayerPrefs.Save();

        Debug.Log("Unauthorized access");
        Application.LoadLevel("LoginLacoWiki");
    }



    int m_BlurPhotoQuestId;
    int m_BlurPhotoWhich;
    void uploadPhoto(int questid, int which)
    {
        m_BlurPhotoQuestId = questid;
        m_BlurPhotoWhich = which;

        string name = Application.persistentDataPath + "/" + "Quest_Img_" + questid + "_" + which + ".jpg";
        Debug.Log("uploadTempPhoto questid: " + questid + " which: " + which);
        if (File.Exists(name))
        {
            Debug.Log("uploadPhoto: " + name);
           /* string param = "";
            param += "{\"photo\":\"";

            byte[] bytes = File.ReadAllBytes(name);
            string encodedText = Convert.ToBase64String(bytes);
            string replacedText = encodedText.Replace("+", "%2B");
            param += encodedText;


            param += "\"}";

            string url = "https://geo-wiki.org/Application/api/Game/uploadTempImage";


            WWWForm form = new WWWForm();
            form.AddField("parameter", param);

            //Debug.Log ("Url data: " + System.Text.Encoding.UTF8.GetString(form.data));
            WWW www = new WWW(url, form);

            StartCoroutine(WaitForPhotoUpload(www));*/


            //------------------------
            // Create bundle
          /*  string url = "https://content.api.geo-wiki.org/v1/Bundles";

            string param = "";
            param = "{";

//            param += "\"legendItemID\":" + classificationid;

            param += "}";


            Debug.Log("Create bundle url: " + url);
            Debug.Log("Create bundle param: " + param);

            WWWForm form = new WWWForm();
            form.AddField("parameter", param);

            //Debug.Log ("Url data: " + System.Text.Encoding.UTF8.GetString(form.data));
            WWW www = new WWW(url, form);

            StartCoroutine(WaitForBundleCreation(www));
            */
            StartCoroutine(CreateBundle());


        }
    }

    IEnumerator CreateBundle()
    {
        string url = "https://content.api.geo-wiki.org/v1/Bundles";

        string param = "";
        param = "{";

        //            param += "\"legendItemID\":" + classificationid;

        param += "}";


           Debug.Log("Creating bundle url: " + url);
        Debug.Log("Creating bundle param: " + param);

        // yield return www.Send();//www.SendWebRequest();
        //  using (UnityWebRequest www = UnityWebRequest.Post(url, param))//UnityWebRequest.Post(url, param))
        using (UnityWebRequest www = UnityWebRequest.Post(url, "POST"))//UnityWebRequest.Post(url, param))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(param);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
           

          //  www.SetRequestHeader("Authorization", "Bearer " + token);
            yield return www.SendWebRequest();


            string[] options2 = { "Ok" };

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("Error: " + www.error);


                  m_UploadingBack.SetActive(false);
                m_UploadingText.SetActive(false);
                m_UploadingText2.SetActive(false);
                m_UploadingTextWait.SetActive(false);
                m_UploadingImage.SetActive(false);

                messageBox.Show("", "Upload failed: " + www.error, options2);
            }
            else
            {
                string data = www.downloadHandler.text;
                //string[] parts = data.Split (":", 2);

                Debug.Log("Creating bundle result: " + data);


                JSONObject j = new JSONObject(data);
                m_ReadingWhichBundle = -1;
                m_BundleId = "";
                accessBundle(j);
                Debug.Log("Bundle Id: " + m_BundleId);


                 StartCoroutine(CreateContentItem());
            }

        }
    }

    int m_ReadingWhichBundle;
    string m_BundleId;

    void accessBundle(JSONObject obj)
    {
        switch (obj.type)
        {
            case JSONObject.Type.OBJECT:
                for (int i = 0; i < obj.list.Count; i++)
                {
                    string key = (string)obj.keys[i];
                    JSONObject j = (JSONObject)obj.list[i];

                    Debug.Log("Accesss bundle: " + key);
                    if (key == "id")
                    {
                        m_ReadingWhichBundle = 1;
                    }
                    else
                    {
                        m_ReadingWhichBundle = 0;
                    }
                    accessBundle(j);
                }
                break;
            case JSONObject.Type.ARRAY:
                //  Debug.Log ("Array");
                foreach (JSONObject j in obj.list)
                {
                    accessBundle(j);
                }
                break;
            case JSONObject.Type.STRING:
                if (m_ReadingWhichBundle == 1)
                {
                    m_BundleId = obj.str;
                    m_ReadingWhichBundle = 0;
                }
                break;

        }
    }

    IEnumerator CreateContentItem()
    {
        string url = "https://content.api.geo-wiki.org/v1/Bundles/" + m_BundleId + "/Items";

        string param = "";
        param = "{";

        //            param += "\"legendItemID\":" + classificationid;

        param += "}";


        Debug.Log("Creating content item url: " + url);
        Debug.Log("Creating content item param: " + param);

        // yield return www.Send();//www.SendWebRequest();
        //  using (UnityWebRequest www = UnityWebRequest.Post(url, param))//UnityWebRequest.Post(url, param))
        using (UnityWebRequest www = UnityWebRequest.Post(url, "POST"))//UnityWebRequest.Post(url, param))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(param);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");


            //  www.SetRequestHeader("Authorization", "Bearer " + token);
            yield return www.SendWebRequest();


            string[] options2 = { "Ok" };

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("Error: " + www.error);


                m_UploadingBack.SetActive(false);
                m_UploadingText.SetActive(false);
                m_UploadingText2.SetActive(false);
                m_UploadingTextWait.SetActive(false);
                m_UploadingImage.SetActive(false);

                messageBox.Show("", "Upload failed creating item: " + www.error, options2);
            }
            else
            {
                string data = www.downloadHandler.text;
                //string[] parts = data.Split (":", 2);

                Debug.Log("Creating item result: " + data);


                JSONObject j = new JSONObject(data);
                m_ReadingWhichItem = -1;
                m_ItemId = "";
                accessContentItem(j);
                Debug.Log("m_ItemId: " + m_ItemId);


                StartCoroutine(UploadPhoto());
            }
        }
    }

    int m_ReadingWhichItem;
    string m_ItemId;

    void accessContentItem(JSONObject obj)
    {
        switch (obj.type)
        {
            case JSONObject.Type.OBJECT:
                for (int i = 0; i < obj.list.Count; i++)
                {
                    string key = (string)obj.keys[i];
                    JSONObject j = (JSONObject)obj.list[i];

                    Debug.Log("Accesss bundle: " + key);
                    if (key == "id")
                    {
                        m_ReadingWhichItem = 1;
                    }
                    else
                    {
                        m_ReadingWhichItem = 0;
                    }
                    accessContentItem(j);
                }
                break;
            case JSONObject.Type.ARRAY:
                //  Debug.Log ("Array");
                foreach (JSONObject j in obj.list)
                {
                    accessContentItem(j);
                }
                break;
            case JSONObject.Type.STRING:
                if (m_ReadingWhichItem == 1)
                {
                    m_ItemId = obj.str;
                    m_ReadingWhichItem = 0;
                }
                break;

        }
    }



    IEnumerator UploadPhoto()
    {
        string url = "https://content.api.geo-wiki.org/v1/Bundles/" + m_BundleId + "/Items/" + m_ItemId + "/File";

        string param = "";
        param = "{";

        //            param += "\"legendItemID\":" + classificationid;

        param += "}";



        string filename = Application.persistentDataPath + "/" + "Quest_Img_" + m_BlurPhotoQuestId + "_" + m_BlurPhotoWhich + ".jpg";
        Debug.Log("UploadPhoto questid: " + m_BlurPhotoQuestId + " which: " + m_BlurPhotoWhich);
       


        Debug.Log("UploadPhoto url: " + url);
        //Debug.Log("UploadPhoto param: " + param);

        // yield return www.Send();//www.SendWebRequest();
        //  using (UnityWebRequest www = UnityWebRequest.Post(url, param))//UnityWebRequest.Post(url, param))
        using (UnityWebRequest www = UnityWebRequest.Post(url, "POST"))//UnityWebRequest.Post(url, param))
        {
            byte[] bytes = File.ReadAllBytes(filename);

           /* byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(param);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
*/

            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bytes);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "image/jpeg");


            //  www.SetRequestHeader("Authorization", "Bearer " + token);
            yield return www.SendWebRequest();


            string[] options2 = { "Ok" };

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("Error: " + www.error);


                m_UploadingBack.SetActive(false);
                m_UploadingText.SetActive(false);
                m_UploadingText2.SetActive(false);
                m_UploadingTextWait.SetActive(false);
                m_UploadingImage.SetActive(false);

                messageBox.Show("", "Upload of photo failed: " + www.error, options2);
            }
            else
            {
                string data = www.downloadHandler.text;
                //string[] parts = data.Split (":", 2);

                Debug.Log("Upload Photo result: " + data);

                PlayerPrefs.SetString("Quest_" + m_BlurPhotoQuestId + "_" + "PhotoUploaded" + "_" + m_BlurPhotoWhich, url);

                uploadQuest(m_QuestCurrentlyUploading);
            }

        }
    }
}
