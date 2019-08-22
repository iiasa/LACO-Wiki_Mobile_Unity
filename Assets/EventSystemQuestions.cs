using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Unitycoding.UIWidgets;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using System.Text;
using System.Collections.Generic;

public class Question
{
	public int m_Id = 0;
	public string m_Question;
	public int m_PrevId = -1;
	public int m_NextIdYes = -1;
	public int m_NextIdNo = -1;
	public int m_NextSelectLandCoverYes = -1;
	public int m_NextSelectLandCoverNo = -1;
	public int m_NextSelectedLandCoverYes = -1;
	public int m_NextSelectedLandCoverNo = -1;



	public Question(int id)
	{
		m_Id = id;
		m_NextIdYes = -1;
		m_NextIdNo = -1;
		m_PrevId = -1;
		m_NextSelectLandCoverYes = -1;
		m_NextSelectLandCoverNo = -1;
		m_NextSelectedLandCoverYes = -1;
		m_NextSelectedLandCoverNo = -1;

		if (id == 0) {
			if (Application.systemLanguage == SystemLanguage.German) {
				//m_Question = "Befindet sich der Punkt auf einem Gebäude, Straße, Parkplatz, Brücke oder einer anderen künstlichen Fläche?";
				m_Question = "Befindet sich der Punkt auf einer künstlichen Fläche?";
			} else {
				//m_Question = "Is the point located on a building, street, road, parking lot, industrial yard, greenhouse, dam, bridge or other artificial feature?";
				m_Question = "Is the point located on an artificial feature?";
			}
			m_NextIdYes = 1;
			m_NextIdNo = 4;
		} else if (id == 1) {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_Question = "Befindet sich der Punkt auf einem Fußweg, Straße oder Gleise?";
			} else {
				m_Question = "Is the point located on a road, street, railway or runway?";
			}
			m_PrevId = 0;


			m_NextSelectedLandCoverYes = 3049;//3080;
			m_NextIdNo = 2;
		} else if (id == 2) {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_Question = "Befindet sich der Punkt auf einem Gebäude?";
			} else {
				m_Question = "Is the point located on a building?";
			}
			m_PrevId = 1;
			m_NextSelectedLandCoverYes = 3048;//15; // To dropbox
			m_NextIdNo = 3;
		} else if (id == 3) {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_Question = "Befindet sich der Punkt auf einer Brücke oder Pipeline?";
			} else {
				m_Question = "Is the point located on a bridge or pipeline?";
			}
			m_PrevId = 2;

			m_NextSelectedLandCoverYes = 3050; // To dropbox
			m_NextSelectedLandCoverNo = 3040;//0;
		} else if (id == 4) {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_Question = "Befindet sich der Punkt auf Ackerland?";
			} else {
				m_Question = "Is the point located in an agricultural field?";
			}
			m_PrevId = 0;

			m_NextIdYes = 5;
			m_NextIdNo = 8;
		} else if (id == 5) {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_Question = "Kannst du die Feldfrucht sehen, Pflanzenüberreste oder Samen um die Pflanze identifizieren zu können?";
			} else {
				m_Question = "Can you see crops, crop residues or seeds in the soil and identify the crop?";
			}
			m_PrevId = 4;

			m_NextSelectLandCoverYes = 4; // To dropbox
			m_NextIdNo = 6;
		} else if (id == 6) {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_Question = "Wurde die Erdoberfläche neu kultiviert (Zum Beispiel ohne Pflanzen)? ";
			} else {
				m_Question = "Has the soil surface been freshly cultivated (i.e. without plants)?";
			}
			m_PrevId = 5;

			m_NextSelectedLandCoverYes = 3069; // To dropbox
			m_NextIdNo = 7;
		}else if (id == 7) {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_Question = "Kannst du Unkraut im Feld sehen aber keine Nutzpflanzen?";
			} else {
				m_Question = "Can you see weeds growing in the field but without crops present?";
			}
			m_PrevId = 6;

			m_NextSelectedLandCoverYes = 3065; // Combobox nothing selected
			m_NextSelectedLandCoverNo = 3041;//0 // Landcover 0
		}else if (id == 8) {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_Question = "Befindet sich der Punkt unter Bäumen?";
			} else {
				m_Question = "Is the point located under the trees?";
			}
			m_PrevId = 4;

			m_NextIdYes = 10;
			m_NextIdNo = 14;
		}else if (id ==9) {
			/*if (Application.systemLanguage == SystemLanguage.German) {
				m_Question = "Befindet sich der Punkt unter Bäumen in einem Wald, Park, Obstgarten, Hecke?";
			} else {
				m_Question = "Is the point in the forest, park, orchard, alley or hedgerow?";
			}
			m_PrevId = 8;

			m_NextIdYes = 10;
			m_NextIdNo = 13;*/
		}else if (id ==10) {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_Question = "Sind die meisten der Bäume um den Punkt Laubbäme?";
			} else {
				m_Question = "Are broad-leaved trees the dominant trees?";
			}
			m_PrevId = 8;

			m_NextSelectedLandCoverYes = 3058;// 10; // Choose land cover
			m_NextIdNo = 11;
		}else if (id ==11) {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_Question = "Sind die meisten Bäume Nadelbäume?";
			} else {
				m_Question = "Are needle-leaved trees the dominant trees?";
			}
			m_PrevId = 10;

			m_NextSelectedLandCoverYes = 3059; // Choose land cover
			m_NextSelectedLandCoverNo = 3060; // Choose land cover
		}else if (id ==12) {
			/*if (Application.systemLanguage == SystemLanguage.German) {
				m_Question = "Sind weder Nadel- noch Laubbäume die dominierenden Bäume?";
			} else {
				m_Question = "Do neither broad-leaved nor needle-leaved trees prevail in the tree cover?";
			}
			m_PrevId = 11;

			m_NextSelectedLandCoverYes = 3059; // Choose land cover
			m_NextSelectedLandCoverNo = 0;*/
		}else if (id == 13) {
			/*if (Application.systemLanguage == SystemLanguage.German) {
				m_Question = "Ist der Punkt in einem Obstgarten mit Obst oder Olivenbäumen?";
			} else {
				m_Question = "Is the point in an orchard with fruit/olive trees?";
			}
			m_PrevId = 9;

			m_NextSelectedLandCoverYes = 3056;
			m_NextSelectedLandCoverNo = 0;*/
		} else if (id == 14) {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_Question = "Befindet sich der Punkt auf einer Wiese, Weide oder Grünfläche?";
			} else {
				m_Question = "Is the point located in a meadow, pasture, or lawn?";
			}
			m_PrevId = 8;

			m_NextIdYes = 15;
			m_NextIdNo = 16;
		} else if (id == 15) {
			if (Application.systemLanguage == SystemLanguage.German ) {
				m_Question = "Kannst du einige Bäume oder Büsche sehen die in der Wiese, Weide oder Grünfläche wachsen?";
			} else {
				m_Question = "Can you see some trees or shrubs growing in the meadow, pasture or lawn close to the point?";
			}
			m_PrevId = 14;

			m_NextSelectedLandCoverYes = 3063;
			m_NextSelectedLandCoverNo = 3064;
		} else if (id == 16) {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_Question = "Befinden sich am Punkt vorherschend Büsche?";
			} else {
				m_Question = "Are bushes at the point?";
			}
			m_PrevId = 14;

			m_NextIdYes = 17;
			m_NextIdNo = 20;
		}else if (id == 17) {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_Question = "Ist der Punkt in einem Weingut oder in einer Obstbaumplantage?";
			} else {
				m_Question = "Is the point in the vineyard or in a soft or small fruit plantation?";
			}
			m_PrevId = 16;

			m_NextSelectedLandCoverYes = 3056; // Select land cover
			m_NextIdNo = 18;
		}else if (id == 18) {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_Question = "Kannst du ein paar Bäume sehen die in der Nähe des Punktes wachsen?";
			} else {
				m_Question = "Can you see some trees growing in the shrub landscape close to the point?";
			}
			m_PrevId = 17;

			m_NextSelectedLandCoverYes = 3061;
			m_NextSelectedLandCoverNo = 3062;
		}else if (id == 20) {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_Question = "Wächst Unkraut oder andere krautartige Pflanzen außer Gras?";
			} else {
				m_Question = "Is the point located in an area with weeds or herbaceous vegetation other then grass growing on it?";
			}
			m_PrevId = 16;

			m_NextSelectedLandCoverYes = 3065; // Select land cover
			m_NextIdNo = 21;
		}
		if (id == 21) {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_Question = "Befindet sich der Punkt in Wasser (z.B. Fluss, See, Eis oder Schnee) oder befand er sich kürzlich unter Wasser (z.B. ungeschützes Flussufer)?";
			} else {
				m_Question = "Is the point located in water (e.g. river, lake, glacier or snow) or was he recently under water (e.g. tidal zone or bare river bank)?";
			}
			m_PrevId = 20;
			m_NextIdYes = 22;
			m_NextIdNo = 23;
		} else if (id == 22) {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_Question = "Ist das Wasser seicht oder momentan kein Wasser sichtbar aber offensichtlich kürzlich überflutet worden?";
			} else {
				m_Question = "Is the water either shallow or is there no water visible but there are obvious signs of recent flooding?";
			}
			m_PrevId = 21;

			m_NextSelectLandCoverYes = 2;
			m_NextSelectLandCoverNo = 1;
		}else if (id == 23) {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_Question = "Befindet sich der Punkt auf bracher Erde (aber keine landwirtschaftlichen Flächen) mit Steinen, Sand oder anderen Flächen auf denen keine Pflanzen (außer vielleicht Flechten oder Moos) wachsen?";
			} else {
				m_Question = "Is the point located on bare soil (but not an agricultural field) with rocks, sand, or other surface with no plants growing there, perhaps only lichens and moss?";
			}
			m_PrevId = 21;
			m_NextSelectLandCoverYes = 3;
			m_NextSelectedLandCoverNo = 3034;//0;
		} 

		/*  else if (id == 4) {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_Question = "Befindet sich der Punkt in einem Gewächshaus?";
			} else {
				m_Question = "Is the point located in a greenhouse?";
			}
			m_PrevId = 1;
//			m_NextIdYes = 14;
			m_NextSelectedLandCoverYes = 3078;
			m_NextIdNo = 5;
		}  else if (id == 7) {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_Question = "Befindet sich der Punkt in einem Hof, Parkplatz oder anderer nicht gebauter, künstlicher Fläche?";
			} else {
				m_Question = "Is the point located in a yard, parking lot, dam or other non-built up area with artificial cover?";
			}
			m_PrevId = 6;


			m_NextSelectedLandCoverYes = 3079;
			m_NextIdNo = 8;
		}     else if (id == 13) {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_Question = "Befindet sich der Punkt in einem Gebiet mit anderen Pflanzen wie Unkraut oder vorherschend krautiger Vegetation?";
			} else {
				m_Question = "Is the point located in the area with other plants such as weeds or other herbaceous vegetation prevailing?";
			}
			m_PrevId = 12;

			m_NextSelectedLandCoverYes = 3065;
			m_NextSelectedLandCoverNo = 0;
		} else if (id == 14) {
			m_Question = "undefined question 14!!!";


			m_PrevId = 0;
			m_NextSelectedLandCoverYes = 0;
			m_NextSelectedLandCoverNo = 0;
		} else if (id == 15) {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_Question = "Hat das Gebäude mehr als 3 Stockwerke?";
			} else {
				m_Question = "Does the building have more than three floors?";
			}
			m_PrevId = 5;

			m_NextSelectedLandCoverYes = 3077;
			m_NextSelectedLandCoverNo = 3076;
		}else if (id == 21) {
			m_Question = "undefined!!!";
			m_PrevId = 0;

			m_NextSelectedLandCoverYes = 0;
			m_NextSelectedLandCoverNo = 0;
		} else {
			m_Question = "undefined!!!";
			m_PrevId = 0;


			m_NextSelectedLandCoverYes = 0;
			m_NextSelectedLandCoverNo = 0;
		}*/
	}
}

public class EventSystemQuestions : MonoBehaviour {

	public GameObject m_TextQuestion;
	public GameObject m_ButtonYes;
	public GameObject m_ButtonNo;
	public GameObject m_ButtonBack;
	public GameObject m_ButtonNext;
	public GameObject m_DropdownText;
	public GameObject m_Dropdown;
	public GameObject m_Dropdown2;
	public GameObject m_ButtonBackWhite;
	public GameObject m_ImageBackWhite;
	public GameObject m_ButtonBackBlack;
	public GameObject m_ImageBackBlack;

	public GameObject m_TextLandUse;



	public GameObject m_LabelResidential;
	public GameObject m_LabelAmenities;
	public GameObject m_LabelRecreation;
	public GameObject m_LabelCommerce;
	public GameObject m_LabelConstruction;
	public GameObject m_LabelTransport;
	public GameObject m_LabelIndustry;
	public GameObject m_LabelAgriculture;
	public GameObject m_LabelForestry;



	public GameObject m_CheckResidential;
	public GameObject m_CheckAmenities;
	public GameObject m_CheckRecreation;
	public GameObject m_CheckCommerce;
	public GameObject m_CheckConstruction;
	public GameObject m_CheckTransport;
	public GameObject m_CheckIndustry;
	public GameObject m_CheckAgriculture;
	public GameObject m_CheckForestry;

	public GameObject m_ImageUrban;
	public GameObject m_ImageRoad;
	public GameObject m_ImageBuilding;
	public GameObject m_ImageBridge;
	public GameObject m_ImageAgrar;
	public GameObject m_ImageTree;
	public GameObject m_ImageCrop;
	public GameObject m_ImageWeed;
	public GameObject m_ImageCultivated;
	public GameObject m_ImageLeaf1;
	public GameObject m_ImageLeaf2;
	public GameObject m_ImagePasture;
	public GameObject m_ImageBushes;
	public GameObject m_ImageHerb;
	public GameObject m_ImageRock;
	public GameObject m_ImageSlope;
	public GameObject m_ImageVineyard;
	public GameObject m_ImageWater;
	public GameObject m_ImageWaterShallow;

	public GameObject m_ImageBackUrban;
	public GameObject m_ImageBackBuilding;
	public GameObject m_ImageBackBridge;
	public GameObject m_ImageBackLandUse;
	public GameObject m_ImageBackField;
	public GameObject m_ImageBackTrees;
	public GameObject m_ImageBackTrees2;
	public GameObject m_ImageBackGrass;
	public GameObject m_ImageBackBush;
	public GameObject m_ImageBackWeed;
	public GameObject m_ImageBackWater;
	public GameObject m_ImageBackRock;

	public GameObject m_ImageBack;


	private MessageBox messageBox;
	private MessageBox verticalMessageBox;

	int m_CurQuestion = 0;
	int m_CurLandCover = 0;
	int m_CurLandCoverId = 0;
	int m_CurLandUse = -1;
	int m_CurLandUseId = 0;
	int m_CurNrLandUses = 0;

	//int m_CollectingPuzzles = 0;

	public Question[] m_Questions;

	// Use this for initialization
	void Start () {
		/*Screen.orientation = ScreenOrientation.AutoRotation;
		Screen.autorotateToPortrait = true;
		Screen.autorotateToPortraitUpsideDown = true;
		Screen.autorotateToLandscapeRight = true;
		Screen.autorotateToLandscapeLeft = true;*/

		Screen.orientation = ScreenOrientation.Portrait;

		Screen.autorotateToPortrait = true;
		Screen.autorotateToPortraitUpsideDown = false;
		Screen.autorotateToLandscapeRight = false;
		Screen.autorotateToLandscapeLeft = false;


		m_ImageBack.GetComponent<Image>().color = new Color32(255,0,0,50);

		m_ButtonBackWhite.SetActive (false);
		m_ImageBackWhite.SetActive (false);
		m_ButtonBackBlack.SetActive(true);
		m_ImageBackBlack.SetActive(true);



		/*m_CollectingPuzzles = PlayerPrefs.GetInt("PickedPuzzle");
		Debug.Log ("m_CollectingPuzzles: " + m_CollectingPuzzles);
*/
		m_ImageBackLandUse.SetActive (false);
		m_Questions = new Question[30];
		for (int i = 0; i < 30; i++) {
			m_Questions[i] = new Question(i);
		}

		m_ButtonNext.SetActive (false);
		m_Dropdown.SetActive (false);
		m_Dropdown2.SetActive (false);
		m_DropdownText.SetActive (false);
		m_TextLandUse.SetActive (false);


		m_CheckResidential.SetActive (false);
		m_CheckAmenities.SetActive (false);
		m_CheckRecreation.SetActive (false);
		m_CheckCommerce.SetActive (false);
		m_CheckConstruction.SetActive (false);
		m_CheckTransport.SetActive (false);
		m_CheckIndustry.SetActive (false);
		m_CheckAgriculture.SetActive (false);
		m_CheckForestry.SetActive (false);

		m_CheckResidential.GetComponent<Toggle>().isOn = false;
		m_CheckAmenities.GetComponent<Toggle>().isOn = false;
		m_CheckRecreation.GetComponent<Toggle>().isOn = false;
		m_CheckCommerce.GetComponent<Toggle>().isOn = false;
		m_CheckConstruction.GetComponent<Toggle>().isOn = false;
		m_CheckTransport.GetComponent<Toggle>().isOn = false;
		m_CheckIndustry.GetComponent<Toggle>().isOn = false;
		m_CheckAgriculture.GetComponent<Toggle>().isOn = false;
		m_CheckForestry.GetComponent<Toggle>().isOn = false;

		if (Application.systemLanguage == SystemLanguage.German){
			m_ButtonYes.GetComponentInChildren<UnityEngine.UI.Text>().text = "Ja";
			m_ButtonNo.GetComponentInChildren<UnityEngine.UI.Text>().text = "Nein";
			//if (m_CollectingPuzzles == 0) {
			m_ButtonBack.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Zurück zur Kamera";
			m_ButtonBackWhite.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Zurück zur Kamera";
			/*} else {
				m_ButtonBack.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Zurück";
			}*/
			m_ButtonNext.GetComponentInChildren<UnityEngine.UI.Text>().text = "Weiter";

			m_TextQuestion.GetComponent<UnityEngine.UI.Text>().text = "Wähle die Landbedeckung aus:";

			m_TextLandUse.GetComponent<UnityEngine.UI.Text>().text = "Wie wird das Land verwendet?";

			m_LabelResidential.GetComponent<UnityEngine.UI.Text>().text = "Wohngebiet";
			m_LabelAmenities.GetComponent<UnityEngine.UI.Text>().text = "Vergnügen (Museum, Kino...)";
			m_LabelRecreation.GetComponent<UnityEngine.UI.Text>().text = "Erholung, Sport";
			m_LabelCommerce.GetComponent<UnityEngine.UI.Text>().text = "Gewerbe";
			m_LabelConstruction.GetComponent<UnityEngine.UI.Text>().text = "Bau";
			m_LabelTransport.GetComponent<UnityEngine.UI.Text>().text = "Transport (Straßen, Schienen...)";
			m_LabelIndustry.GetComponent<UnityEngine.UI.Text>().text = "Industrie und Herstellung";
			m_LabelAgriculture.GetComponent<UnityEngine.UI.Text>().text = "Landwirtschaft";
			m_LabelForestry.GetComponent<UnityEngine.UI.Text>().text = "Forstwirtschaft";
		} else {
			m_ButtonYes.GetComponentInChildren<UnityEngine.UI.Text>().text = "Yes";
			m_ButtonNo.GetComponentInChildren<UnityEngine.UI.Text>().text = "No";
			//if (m_CollectingPuzzles == 0) {
				m_ButtonBack.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Back to Camera";
			m_ButtonBackWhite.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Back to Camera";
			/*} else {
				m_ButtonBack.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Back";
			}*/
			m_ButtonNext.GetComponentInChildren<UnityEngine.UI.Text>().text = "Next";

			m_TextQuestion.GetComponent<UnityEngine.UI.Text>().text = "Select the landcover:";


			m_TextLandUse.GetComponent<UnityEngine.UI.Text>().text = "How is the land used?";

			m_LabelResidential.GetComponent<UnityEngine.UI.Text>().text = "Residential";
			m_LabelAmenities.GetComponent<UnityEngine.UI.Text>().text = "Amenities (museums, cinema...)";
			m_LabelRecreation.GetComponent<UnityEngine.UI.Text>().text = "Recreation, Sport";
			m_LabelCommerce.GetComponent<UnityEngine.UI.Text>().text = "Commerce";
			m_LabelConstruction.GetComponent<UnityEngine.UI.Text>().text = "Construction";
			m_LabelTransport.GetComponent<UnityEngine.UI.Text>().text = "Transport (Streets, Railroads...)";
			m_LabelIndustry.GetComponent<UnityEngine.UI.Text>().text = "Industry and manufacturing";
			m_LabelAgriculture.GetComponent<UnityEngine.UI.Text>().text = "Agriculture";
			m_LabelForestry.GetComponent<UnityEngine.UI.Text>().text = "Forestry";
		} 


		messageBox = UIUtility.Find<MessageBox> ("MessageBox");

		//m_bShowPuzzleMessage = false;
	/*	if (m_CollectingPuzzles == 1) {
			m_bShowPuzzleMessage = true;
			m_PuzzleMessageIter = 0;
		}*/

		string startquestionstime = System.DateTime.Now.ToString ("yyyy/MM/dd HH:mm:ss");
		PlayerPrefs.SetString ("CurQuestStartQuestionsTime", startquestionstime);
		PlayerPrefs.Save ();

		int fromQuestEnd = PlayerPrefs.GetInt ("Questions_FromQuestFinished");
		if (fromQuestEnd == 1) {

			if(PlayerPrefs.GetInt ("CheckResidential") == 1) {
				m_CheckResidential.GetComponent<Toggle>().isOn = true;
			}
			if(PlayerPrefs.GetInt ("CheckAmenities") == 1) {
				m_CheckAmenities.GetComponent<Toggle>().isOn = true;
			}
			if(PlayerPrefs.GetInt ("CheckRecreation") == 1) {
				m_CheckRecreation.GetComponent<Toggle>().isOn = true;
			}
			if(PlayerPrefs.GetInt ("CheckCommerce") == 1) {
				m_CheckCommerce.GetComponent<Toggle>().isOn = true;
			}
			if(PlayerPrefs.GetInt ("CheckConstruction") == 1) {
				m_CheckConstruction.GetComponent<Toggle>().isOn = true;
			}
			if(PlayerPrefs.GetInt ("CheckTransport") == 1) {
				m_CheckTransport.GetComponent<Toggle>().isOn = true;
			}
			if(PlayerPrefs.GetInt ("CheckIndustry") == 1) {
				m_CheckIndustry.GetComponent<Toggle>().isOn = true;
			}
			if(PlayerPrefs.GetInt ("CheckAgriculture") == 1) {
				m_CheckAgriculture.GetComponent<Toggle>().isOn = true;
			}
			if(PlayerPrefs.GetInt ("CheckForestry") == 1) {
				m_CheckForestry.GetComponent<Toggle>().isOn = true;
			}





			m_CurQuestion = PlayerPrefs.GetInt ("CurQuestions_QuestionId");
			updateQuestion ();
			openLandUse ();
		} else {
			PlayerPrefs.SetInt ("CheckResidential", 0);
			PlayerPrefs.SetInt ("CheckAmenities", 0);
			PlayerPrefs.SetInt ("CheckRecreation", 0);
			PlayerPrefs.SetInt ("CheckCommerce", 0);
			PlayerPrefs.SetInt ("CheckConstruction", 0);
			PlayerPrefs.SetInt ("CheckTransport", 0);
			PlayerPrefs.SetInt ("CheckIndustry", 0);
			PlayerPrefs.SetInt ("CheckAgriculture", 0);
			PlayerPrefs.SetInt ("CheckForestry", 0);
			PlayerPrefs.Save ();


			m_CurQuestion = 0;
			updateQuestion ();
		}
	}
	/*
	bool m_bShowPuzzleMessage;
	int m_PuzzleMessageIter;
*/
	ArrayList m_Landcovers;
	ArrayList m_LandcoverIds;

	void updateLandCover() {
		m_Landcovers = new ArrayList();
		m_LandcoverIds = new ArrayList();


		m_LandcoverIds.Add ("0");

		if (Application.systemLanguage == SystemLanguage.German) {
			m_Dropdown.GetComponentInChildren<UnityEngine.UI.Text>().text = "Auswählen";
			m_Landcovers.Add ("Auswählen");
		} else {
			m_Dropdown.GetComponentInChildren<UnityEngine.UI.Text>().text = "Select";
			m_Landcovers.Add ("Select");
		}

		if (m_CurLandCover == 1) {
			m_LandcoverIds.Add ("3070");
			m_LandcoverIds.Add ("3071");
			m_LandcoverIds.Add ("3072");
			m_LandcoverIds.Add ("3073");

			m_ImageBackWater.SetActive (true);

			if (Application.systemLanguage == SystemLanguage.German) {
				m_Landcovers.Add ("Inland Gewässer");
				m_Landcovers.Add ("Inland fließendes Wasser");
				m_Landcovers.Add ("Übergangsgewässer");
				m_Landcovers.Add ("Schnee, Glätscher");
			} else {
				m_Landcovers.Add ("Inland water bodies");
				m_Landcovers.Add ("Inland running water");
				m_Landcovers.Add ("Transitional water bodies");
				m_Landcovers.Add ("Glaciers, snow");
			}
		} else if (m_CurLandCover == 2) {

			m_ImageBackWater.SetActive (true);

			m_LandcoverIds.Add ("3074");
			m_LandcoverIds.Add ("3075");
			m_LandcoverIds.Add ("3047");


			if (Application.systemLanguage == SystemLanguage.German) {
				m_Landcovers.Add ("Feuchtgebiet Inland");
				m_Landcovers.Add ("Feuchtgebiet Küste");
				m_Landcovers.Add ("Feuchtgebiet");
			} else {
				m_Landcovers.Add ("Inland wetlands");
				m_Landcovers.Add ("Coastal wetlands");
				m_Landcovers.Add ("Wetlands");
			}
		} else if (m_CurLandCover == 3) {
			m_LandcoverIds.Add ("3066");
			m_LandcoverIds.Add ("3067");
			m_LandcoverIds.Add ("3068");
			m_LandcoverIds.Add ("3069");

			m_ImageBackRock.SetActive (true);


			if (Application.systemLanguage == SystemLanguage.German) {
				m_Landcovers.Add ("Steine und Felsen");
				m_Landcovers.Add ("Sand");
				m_Landcovers.Add ("Moos");
				m_Landcovers.Add ("Andere brache Erde");
			} else {
				m_Landcovers.Add ("Rocks and Stone");
				m_Landcovers.Add ("Sand");
				m_Landcovers.Add ("Lichens and moss");
				m_Landcovers.Add ("Other bare soil");
			}
		} else if (m_CurLandCover == 4) {
			m_LandcoverIds.Add ("3051");
			m_LandcoverIds.Add ("3052");
			m_LandcoverIds.Add ("3054");
			m_LandcoverIds.Add ("3054");
			m_LandcoverIds.Add ("3054");
			m_LandcoverIds.Add ("3055");
			m_LandcoverIds.Add ("3053");

			m_ImageBackField.SetActive (true);

			if (Application.systemLanguage == SystemLanguage.German ) {
				m_Landcovers.Add ("Getreide");
				m_Landcovers.Add ("Wurzelgemüse");
				m_Landcovers.Add ("Hülsenfrüchte");
				m_Landcovers.Add ("Gemüse");
				m_Landcovers.Add ("Blumen");
				m_Landcovers.Add ("Futterpflanzen");
				m_Landcovers.Add ("Andere Pflanze");
			} else {
				m_Landcovers.Add ("Cereals");
				m_Landcovers.Add ("Root crops");
				m_Landcovers.Add ("Dry pulses");
				m_Landcovers.Add ("Vegetables");
				m_Landcovers.Add ("Flowers");
				m_Landcovers.Add ("Fodder crops");
				m_Landcovers.Add ("Other crops");
			}
		}

		UnityEngine.UI.Dropdown dropdown;
		dropdown = m_Dropdown.GetComponent<UnityEngine.UI.Dropdown>();
		dropdown.options.Clear ();

		for(int i=0; i<m_Landcovers.Count; i++) {
			string strlandcover = (string)m_Landcovers [i];
			UnityEngine.UI.Dropdown.OptionData list = new UnityEngine.UI.Dropdown.OptionData(strlandcover);
			dropdown.options.Add (list);
		}

		dropdown.value = 0;
	}


	ArrayList m_Landuses;
	ArrayList m_LandusesIds;

	void updateLandUses() {
		m_Landuses = new ArrayList();
		m_LandusesIds = new ArrayList();


		m_LandusesIds.Add ("-1");

		if (Application.systemLanguage == SystemLanguage.German) {
			m_Dropdown2.GetComponentInChildren<UnityEngine.UI.Text>().text = "Auswählen";
			m_Landuses.Add ("Auswählen");
		} else {
			m_Dropdown2.GetComponentInChildren<UnityEngine.UI.Text>().text = "Select";
			m_Landuses.Add ("Select");
		}

		if (m_CurLandCoverId == 3070) {
		} else if (m_CurLandCoverId == 3071) {
		} else if (m_CurLandCoverId == 3072) {
			m_LandusesIds.Add ("3298");
			m_LandusesIds.Add ("3358");
			m_LandusesIds.Add ("3378");
			m_LandusesIds.Add ("3385");


			if (Application.systemLanguage == SystemLanguage.German) {
				m_Landuses.Add ("Inland Gewässer");
				m_Landuses.Add ("Inland fließendes Wasser");
				m_Landuses.Add ("Übergangsgewässer");
				m_Landuses.Add ("Schnee, Glätscher");
			} else {
				m_Landuses.Add ("Aquaculture and fishing");
				m_Landuses.Add ("Water transport");
				m_Landuses.Add ("Amenities, museums, leisure");
				m_Landuses.Add ("Not in use");
			}
		} else if (m_CurLandCoverId == 3073) {
			m_LandusesIds.Add ("3385");
			m_LandusesIds.Add ("3378");


			if (Application.systemLanguage == SystemLanguage.German) {
				m_Landuses.Add ("Vergnügen, Museum, Freizeit");
				m_Landuses.Add ("Nicht in verwendung");
			} else {
				m_Landuses.Add ("Amenities, museums, leisure");
				m_Landuses.Add ("Not in use");
			}
		} else if (m_CurLandCoverId == 3074) {

		} else if (m_CurLandCoverId == 3075) {

		} else if (m_CurLandCoverId == 3047) {

		} else if (m_CurLandCoverId == 3066) {

			m_LandusesIds.Add ("3301");
			m_LandusesIds.Add ("3305");
			m_LandusesIds.Add ("3307");
			m_LandusesIds.Add ("3355");
			m_LandusesIds.Add ("3370");
			m_LandusesIds.Add ("3372");
			m_LandusesIds.Add ("3375");
			m_LandusesIds.Add ("3377");
			m_LandusesIds.Add ("3380");
			m_LandusesIds.Add ("3382");

			if (Application.systemLanguage == SystemLanguage.German) {
				m_Landuses.Add ("Bergbau und Steinbruch");
				m_Landuses.Add ("Energieproduction");
				m_Landuses.Add ("Industrie und Produktion");
				m_Landuses.Add ("Transport, Kommunikationsnetzwerk, Lagerung");
				m_Landuses.Add ("Bau");
				m_Landuses.Add ("Gewerbe, Finanzen, Informationsservice");
				m_Landuses.Add ("Gemeinschaftsservice");
				m_Landuses.Add ("Erholung, Sport");
				m_Landuses.Add ("Wohngebiet");
				m_Landuses.Add ("Verlassenes Gebiet");
			} else {
				m_Landuses.Add ("Mining and quarrying");
				m_Landuses.Add ("Energy production");
				m_Landuses.Add ("Industry and manufacturing");
				m_Landuses.Add ("Transport, communication networks, storage");
				m_Landuses.Add ("Construction");
				m_Landuses.Add ("Commerce, financial, information services");
				m_Landuses.Add ("Community services");
				m_Landuses.Add ("Recreation, leisure, sport");
				m_Landuses.Add ("Residential");
				m_Landuses.Add ("Unused and abandoned areas");


			}
		} else if (m_CurLandCoverId == 3067) {
			m_LandusesIds.Add ("3241");
			m_LandusesIds.Add ("3301");
			m_LandusesIds.Add ("3305");
			m_LandusesIds.Add ("3307");
			m_LandusesIds.Add ("3355");
			m_LandusesIds.Add ("3370");
			m_LandusesIds.Add ("3372");
			m_LandusesIds.Add ("3375");
			m_LandusesIds.Add ("3377");
			m_LandusesIds.Add ("3380");
			m_LandusesIds.Add ("3382");


			if (Application.systemLanguage == SystemLanguage.German) {
				m_Landuses.Add ("Baumschule");
				m_Landuses.Add ("Bergbau und Steinbruch");
				m_Landuses.Add ("Energieproduktion");
				m_Landuses.Add ("Industrie und Produktion");
				m_Landuses.Add ("Transport, Kommunikationsnetzwerk, Lagerung");
				m_Landuses.Add ("Bau");
				m_Landuses.Add ("Gewerbe, Finanzen, Informationsservice");
				m_Landuses.Add ("Sozialer Dienst");
				m_Landuses.Add ("Erholung, Sport");
				m_Landuses.Add ("Wohngebiet");
				m_Landuses.Add ("Verlassenes Gebiet");
			} else {
				m_Landuses.Add ("Forest tree nurseries");
				m_Landuses.Add ("Mining and quarrying");
				m_Landuses.Add ("Energy production");
				m_Landuses.Add ("Industry and manufacturing");
				m_Landuses.Add ("Transport, communication networks, storage");
				m_Landuses.Add ("Construction");
				m_Landuses.Add ("Commerce, financial, information services");
				m_Landuses.Add ("Community services");
				m_Landuses.Add ("Recreation, leisure, sport");
				m_Landuses.Add ("Residential");
				m_Landuses.Add ("Unused and abandoned areas");


			}
		} else if (m_CurLandCoverId == 3068) {
			m_LandusesIds.Add ("3241");
			m_LandusesIds.Add ("3301");
			m_LandusesIds.Add ("3305");
			m_LandusesIds.Add ("3307");
			m_LandusesIds.Add ("3355");
			m_LandusesIds.Add ("3370");
			m_LandusesIds.Add ("3372");
			m_LandusesIds.Add ("3375");
			m_LandusesIds.Add ("3377");
			m_LandusesIds.Add ("3380");
			m_LandusesIds.Add ("3382");


			if (Application.systemLanguage == SystemLanguage.German) {
				m_Landuses.Add ("Baumschule");
				m_Landuses.Add ("Bergbau und Steinbruch");
				m_Landuses.Add ("Energieproduktion");
				m_Landuses.Add ("Industrie und Produktion");
				m_Landuses.Add ("Transport, Kommunikationsnetzwerk, Lagerung");
				m_Landuses.Add ("Bau");
				m_Landuses.Add ("Gewerbe, Finanzen, Informationsservice");
				m_Landuses.Add ("Sozialer Dienst");
				m_Landuses.Add ("Erholung, Sport");
				m_Landuses.Add ("Wohngebiet");
				m_Landuses.Add ("Verlassenes Gebiet");
			} else {
				m_Landuses.Add ("Forest tree nurseries");
				m_Landuses.Add ("Mining and quarrying");
				m_Landuses.Add ("Energy production");
				m_Landuses.Add ("Industry and manufacturing");
				m_Landuses.Add ("Transport, communication networks, storage");
				m_Landuses.Add ("Construction");
				m_Landuses.Add ("Commerce, financial, information services");
				m_Landuses.Add ("Community services");
				m_Landuses.Add ("Recreation, leisure, sport");
				m_Landuses.Add ("Residential");
				m_Landuses.Add ("Unused and abandoned areas");


			}
		} else if (m_CurLandCoverId == 3069) {
			m_LandusesIds.Add ("3293");
			m_LandusesIds.Add ("3294");
			m_LandusesIds.Add ("3241");
			m_LandusesIds.Add ("3301");
			m_LandusesIds.Add ("3305");
			m_LandusesIds.Add ("3307");
			m_LandusesIds.Add ("3355");
			m_LandusesIds.Add ("3370");
			m_LandusesIds.Add ("3372");
			m_LandusesIds.Add ("3375");
			m_LandusesIds.Add ("3377");
			m_LandusesIds.Add ("3380");
			m_LandusesIds.Add ("3382");



			if (Application.systemLanguage == SystemLanguage.German) {
				m_Landuses.Add ("Landwirtschaft");
				m_Landuses.Add ("Brachland");
				m_Landuses.Add ("Baumschule");
				m_Landuses.Add ("Bergbau und Steinbruch");
				m_Landuses.Add ("Energieproduktion");
				m_Landuses.Add ("Industrie und Produktion");
				m_Landuses.Add ("Transport, Kommunikationsnetzwerk, Lagerung");
				m_Landuses.Add ("Bau");
				m_Landuses.Add ("Gewerbe, Finanzen, Informationsservice");
				m_Landuses.Add ("Sozialer Dienst");
				m_Landuses.Add ("Erholung, Sport");
				m_Landuses.Add ("Wohngebiet");
				m_Landuses.Add ("Verlassenes Gebiet");
			} else {
				m_Landuses.Add ("Agriculture");
				m_Landuses.Add ("Fallow Land");
				m_Landuses.Add ("Forest tree nurseries");
				m_Landuses.Add ("Mining and quarrying");
				m_Landuses.Add ("Energy production");
				m_Landuses.Add ("Industry and manufacturing");
				m_Landuses.Add ("Transport, communication networks, storage");
				m_Landuses.Add ("Construction");
				m_Landuses.Add ("Commerce, financial, information services");
				m_Landuses.Add ("Community services");
				m_Landuses.Add ("Recreation, leisure, sport");
				m_Landuses.Add ("Residential");
				m_Landuses.Add ("Unused and abandoned areas");




			}
		} else if (m_CurLandCoverId == 3061) {
			// ["U111","U112","U120","U36x","U420"]
			m_LandusesIds.Add ("3293");
			m_LandusesIds.Add ("3294");
			m_LandusesIds.Add ("3296");
			m_LandusesIds.Add ("3378");
			m_LandusesIds.Add ("3385");



			if (Application.systemLanguage == SystemLanguage.German) {
				m_Landuses.Add ("Landwirtschaft");
				m_Landuses.Add ("Brachfläche");
				m_Landuses.Add ("Forstwirtschaft");
				m_Landuses.Add ("Vergnügen, Museum, Freizeit");
				m_Landuses.Add ("Nicht in gebrauch");
			} else {
				m_Landuses.Add ("Agriculture");
				m_Landuses.Add ("Fallow land");
				m_Landuses.Add ("Forestry");
				m_Landuses.Add ("Amenities, museums, leisure");
				m_Landuses.Add ("Not in use");



			}
		} else if (m_CurLandCoverId == 3062) {
			// ["U111","U112","U120","U36x","U420"]
			m_LandusesIds.Add ("3293");
			m_LandusesIds.Add ("3294");
			m_LandusesIds.Add ("3296");
			m_LandusesIds.Add ("3378");
			m_LandusesIds.Add ("3385");

			if (Application.systemLanguage == SystemLanguage.German) {
				m_Landuses.Add ("Landwirtschaft");
				m_Landuses.Add ("Brachfläche");
				m_Landuses.Add ("Forstwirtschaft");
				m_Landuses.Add ("Vergnügen, Museum, Freizeit");
				m_Landuses.Add ("Nicht in gebrauch");
			} else {
				m_Landuses.Add ("Agriculture");
				m_Landuses.Add ("Fallow land");
				m_Landuses.Add ("Forestry");
				m_Landuses.Add ("Amenities, museums, leisure");
				m_Landuses.Add ("Not in use");
			}
		}


		else if (m_CurLandCoverId == 3058) {
			//["U111","U120","U341","U350","U36x","U370","U420"]
			m_LandusesIds.Add ("3293");
			m_LandusesIds.Add ("3296");
			m_LandusesIds.Add ("3378");
			m_LandusesIds.Add ("3373");
			m_LandusesIds.Add ("3376");
			m_LandusesIds.Add ("3380");
			m_LandusesIds.Add ("3377");


			if (Application.systemLanguage == SystemLanguage.German) {
				m_Landuses.Add ("Landwirtschaft");
				m_Landuses.Add ("Forstwirtschaft");
				m_Landuses.Add ("Vergnügen, Museum, Freizeit");
				m_Landuses.Add ("Handelsgewerbe");
				m_Landuses.Add ("Sozialer Dienst");
				m_Landuses.Add ("Wohngebiet");
				m_Landuses.Add ("Erholung, Sport");
			} else {
				m_Landuses.Add ("Agriculture");
				m_Landuses.Add ("Forestry");
				m_Landuses.Add ("Amenities, museums, leisure");
				m_Landuses.Add ("Commerce");
				m_Landuses.Add ("Community services");
				m_Landuses.Add ("Residential");
				m_Landuses.Add ("Recreation, Sport");





			}
		}else if (m_CurLandCoverId == 3063) {
			//["U111","U120","U31X","U34X","U350","U36X","U370","U4X0"]
			m_LandusesIds.Add ("3293");
			m_LandusesIds.Add ("3296");
			m_LandusesIds.Add ("3378");
			m_LandusesIds.Add ("3385");
			m_LandusesIds.Add ("3373");
			m_LandusesIds.Add ("3376");
			m_LandusesIds.Add ("3380");
			m_LandusesIds.Add ("3377");

			if (Application.systemLanguage == SystemLanguage.German) {
				m_Landuses.Add ("Landwirtschaft");
				m_Landuses.Add ("Forstwirtschaft");
				m_Landuses.Add ("Vergnügen, Museum, Freizeit");
				m_Landuses.Add ("Nicht in gebrauch");
				m_Landuses.Add ("Handelsgewerbe");
				m_Landuses.Add ("Sozialer Dienst");
				m_Landuses.Add ("Wohngebiet");
				m_Landuses.Add ("Erholung, Sport");
			} else {
				m_Landuses.Add ("Agriculture");
				m_Landuses.Add ("Forestry");
				m_Landuses.Add ("Amenities, museums, leisure");
				m_Landuses.Add ("Not in use");
				m_Landuses.Add ("Commerce");
				m_Landuses.Add ("Community services");
				m_Landuses.Add ("Residential");
				m_Landuses.Add ("Recreation, Sport");
			}
		}


		else if (m_CurLandCoverId == 3064) {
			//["U111","U120","U31x","U34x","U350","U36x","U370","U410","U420"]
			m_LandusesIds.Add ("3293");
			m_LandusesIds.Add ("3296");
			m_LandusesIds.Add ("3378");
			m_LandusesIds.Add ("3385");
			m_LandusesIds.Add ("3373");
			m_LandusesIds.Add ("3376");
			m_LandusesIds.Add ("3380");
			m_LandusesIds.Add ("3377");
			m_LandusesIds.Add ("3384");

			if (Application.systemLanguage == SystemLanguage.German) {
				m_Landuses.Add ("Landwirtschaft");
				m_Landuses.Add ("Forstwirtschaft");
				m_Landuses.Add ("Vergnügen, Museum, Freizeit");
				m_Landuses.Add ("Nicht in gebrauch");
				m_Landuses.Add ("Handelsgewerbe");
				m_Landuses.Add ("Sozialer Dienst");
				m_Landuses.Add ("Wohngebiet");
				m_Landuses.Add ("Erholung, Sport");
				m_Landuses.Add ("Verlassenes Gebiet");
			} else {
				m_Landuses.Add ("Agriculture");
				m_Landuses.Add ("Forestry");
				m_Landuses.Add ("Amenities, museums, leisure");
				m_Landuses.Add ("Not in use");
				m_Landuses.Add ("Commerce");
				m_Landuses.Add ("Community services");
				m_Landuses.Add ("Residential");
				m_Landuses.Add ("Recreation, Sport");
				m_Landuses.Add ("Abandoned areas");

			}
		}

		else if (m_CurLandCoverId == 3065) {
			//["U112","U120","U140","U2XX","U31X","U420"]
			m_LandusesIds.Add ("3294");
			m_LandusesIds.Add ("3296");
			m_LandusesIds.Add ("3385");
			m_LandusesIds.Add ("3300");
			m_LandusesIds.Add ("3307");
			m_LandusesIds.Add ("3355");

			if (Application.systemLanguage == SystemLanguage.German) {
				m_Landuses.Add ("Brachfläche");
				m_Landuses.Add ("Forstwirtschaft");
				m_Landuses.Add ("Nicht in gebrauch");
				m_Landuses.Add ("Bergbau und Steinbruch");
				m_Landuses.Add ("Industrie und Herstellung");
				m_Landuses.Add ("Transport, Kommunikationsnetzwerke, Lagerung");
			} else {
				m_Landuses.Add ("Fallow land");
				m_Landuses.Add ("Forestry");
				m_Landuses.Add ("Not in use");
				m_Landuses.Add ("Mining and Quarrying");
				m_Landuses.Add ("Industry and manufacturing");
				m_Landuses.Add ("Transport, Communication networks, storage");


			}
		}



		else if (m_CurLandCoverId == 3069) {
			//["U111","U112","U120","U140","U21x","U22x","U31x","U330","U34x","U350","U36x","U370","U4x0"]
			m_LandusesIds.Add ("3293");
			m_LandusesIds.Add ("3296");
			m_LandusesIds.Add ("3378");
			m_LandusesIds.Add ("3385");
			m_LandusesIds.Add ("3373");
			m_LandusesIds.Add ("3376");
			m_LandusesIds.Add ("3380");
			m_LandusesIds.Add ("3377");
			m_LandusesIds.Add ("3384");
			m_LandusesIds.Add ("3300");
			m_LandusesIds.Add ("3307");
			m_LandusesIds.Add ("3355");
			m_LandusesIds.Add ("3370");

			if (Application.systemLanguage == SystemLanguage.German) {
				m_Landuses.Add ("Landwirtschaft");
				m_Landuses.Add ("Brachfläche");
				m_Landuses.Add ("Vergnügen, Museum, Freizeit");
				m_Landuses.Add ("Nicht in gebrauch");
				m_Landuses.Add ("Handelsgewerbe");
				m_Landuses.Add ("Sozialer Dienst");
				m_Landuses.Add ("Wohngebiet");
				m_Landuses.Add ("Erholung, Sport");
				m_Landuses.Add ("Verlassenes Gebiet");
				m_Landuses.Add ("Bergbau");
				m_Landuses.Add ("Industrie und Herstellung");
				m_Landuses.Add ("Transport, Kommunikationsnetzwerke, Lagerung");
				m_Landuses.Add ("Bau");
			} else {
				m_Landuses.Add ("Agriculture");
				m_Landuses.Add ("Forestry");
				m_Landuses.Add ("Amenities, museums, leisure");
				m_Landuses.Add ("Not in use");
				m_Landuses.Add ("Commerce");
				m_Landuses.Add ("Community services");
				m_Landuses.Add ("Residential");
				m_Landuses.Add ("Recreation, Sport");
				m_Landuses.Add ("Abandoned areas");
				m_Landuses.Add ("Mining and Quarrying");
				m_Landuses.Add ("Industry and manufacturing");
				m_Landuses.Add ("Transport, Communication networks, storage");
				m_Landuses.Add ("Construction");
			}
		}else if (m_CurLandCoverId == 3076) {
			//["U111","U120","U130","U140","U210","U22x","U31x","U32x","U330","U341","U342","U350","U36X","U370","U410"]
			m_LandusesIds.Add ("3293");
			m_LandusesIds.Add ("3296");
			m_LandusesIds.Add ("3296");
			m_LandusesIds.Add ("3378");
			m_LandusesIds.Add ("3373");
			m_LandusesIds.Add ("3376");
			m_LandusesIds.Add ("3380");
			m_LandusesIds.Add ("3377");
			m_LandusesIds.Add ("3300");
			m_LandusesIds.Add ("3307");
			m_LandusesIds.Add ("3355");
			m_LandusesIds.Add ("3370");
			m_LandusesIds.Add ("3367");
			m_LandusesIds.Add ("3384");

			if (Application.systemLanguage == SystemLanguage.German) {
				m_Landuses.Add ("Landwirtschaft");
				m_Landuses.Add ("Brachfläche");
				m_Landuses.Add ("Forstwirtschaft");
				m_Landuses.Add ("Vergnügen, Museum, Freizeit");
				m_Landuses.Add ("Handelsgewerbe");
				m_Landuses.Add ("Sozialer Dienst");
				m_Landuses.Add ("Wohngebiet");
				m_Landuses.Add ("Erholung, Sport");
				m_Landuses.Add ("Bergbau und Steinbruch");
				m_Landuses.Add ("Industrie und Herstellung");
				m_Landuses.Add ("Transport, Kommunikationsnetzwerke, Lagerung");
				m_Landuses.Add ("Bau");
				m_Landuses.Add ("Wasser und Wasserbehandlung");
				m_Landuses.Add ("Verlassenes Gebiet");
			} else {
				m_Landuses.Add ("Agriculture");
				m_Landuses.Add ("Fallow land");
				m_Landuses.Add ("Forestry");
				m_Landuses.Add ("Amenities, museums, leisure");
				m_Landuses.Add ("Commerce");
				m_Landuses.Add ("Community services");
				m_Landuses.Add ("Residential");
				m_Landuses.Add ("Recreation, Sport");
				m_Landuses.Add ("Mining and Quarrying");
				m_Landuses.Add ("Industry and manufacturing");
				m_Landuses.Add ("Transport, Communication networks, storage");
				m_Landuses.Add ("Construction");
				m_Landuses.Add ("Water and Waste Treatment");
				m_Landuses.Add ("Abandoned areas");
			}
		}else if (m_CurLandCoverId == 3077) {
			//["U111","U120","U130","U140","U210","U22X","U31X","U32X","U330","U341","U350","U36X","U370"]
			m_LandusesIds.Add ("3293");
			m_LandusesIds.Add ("3296");
			m_LandusesIds.Add ("3378");
			m_LandusesIds.Add ("3373");
			m_LandusesIds.Add ("3376");
			m_LandusesIds.Add ("3380");
			m_LandusesIds.Add ("3377");
			m_LandusesIds.Add ("3300");
			m_LandusesIds.Add ("3307");
			m_LandusesIds.Add ("3355");
			m_LandusesIds.Add ("3370");
			m_LandusesIds.Add ("3367");

			if (Application.systemLanguage == SystemLanguage.German) {
				m_Landuses.Add ("Landwirtschaft");
				m_Landuses.Add ("Forstwirtschaft");
				m_Landuses.Add ("Vergnügen, Museum, Freizeit");
				m_Landuses.Add ("Handelsgewerbe");
				m_Landuses.Add ("Sozialer Dienst");
				m_Landuses.Add ("Wohngebiet");
				m_Landuses.Add ("Erholung, Sport");
				m_Landuses.Add ("Bergbau und Steinbruch");
				m_Landuses.Add ("Industrie und Herstellung");
				m_Landuses.Add ("Transport, Kommunikationsnetzwerke, Lagerung");
				m_Landuses.Add ("Bau");
				m_Landuses.Add ("Wasser und Wasserbehandlung");
			} else {
				m_Landuses.Add ("Agriculture");
				m_Landuses.Add ("Forestry");
				m_Landuses.Add ("Amenities, museums, leisure");
				m_Landuses.Add ("Commerce");
				m_Landuses.Add ("Community services");
				m_Landuses.Add ("Residential");
				m_Landuses.Add ("Recreation, Sport");
				m_Landuses.Add ("Mining and Quarrying");
				m_Landuses.Add ("Industry and manufacturing");
				m_Landuses.Add ("Transport, Communication networks, storage");
				m_Landuses.Add ("Construction");
				m_Landuses.Add ("Water and Waste Treatment");
			}
		}else if (m_CurLandCoverId == 3050) {
			//["U210","U311","U312","U318","U321","U322","U410"]

			m_LandusesIds.Add ("3355");
			m_LandusesIds.Add ("3367");
			m_LandusesIds.Add ("3384");
			m_LandusesIds.Add ("3369");
			m_LandusesIds.Add ("3368");
			m_LandusesIds.Add ("3357");
			m_LandusesIds.Add ("3356");
			m_LandusesIds.Add ("3305");

			if (Application.systemLanguage == SystemLanguage.German) {
				m_Landuses.Add ("Transport, Kommunikationsnetzwerke, Lagerung");
				m_Landuses.Add ("Wasser und Wasserbehandlung");
				m_Landuses.Add ("Verlassenes Gebiet");
				m_Landuses.Add ("Müllverarbeitung");
				m_Landuses.Add ("Wasserversorung und behandlung");
				m_Landuses.Add ("Straße Transport");
				m_Landuses.Add ("Schiene transport");
				m_Landuses.Add ("Energieproduktion");
			} else {

				m_Landuses.Add ("Transport, Communication networks, storage");
				m_Landuses.Add ("Water and Waste Treatment");
				m_Landuses.Add ("Abandoned areas");
				m_Landuses.Add ("Waste Treatment");
				m_Landuses.Add ("Water supply and treatment");
				m_Landuses.Add ("Road transport");
				m_Landuses.Add ("Railway transport");
				m_Landuses.Add ("Energy production");



			}
		}else if (m_CurLandCoverId == 3079) {
			//["U111","U120","U130","U140","U210","U22X","U31X","U32X","U330","U34X","U350","U36x","U370","U410"]
			m_LandusesIds.Add ("3293");
			m_LandusesIds.Add ("3296");
			m_LandusesIds.Add ("3378");
			m_LandusesIds.Add ("3373");
			m_LandusesIds.Add ("3376");
			m_LandusesIds.Add ("3380");
			m_LandusesIds.Add ("3377");
			m_LandusesIds.Add ("3300");
			m_LandusesIds.Add ("3307");
			m_LandusesIds.Add ("3355");
			m_LandusesIds.Add ("3370");
			m_LandusesIds.Add ("3367");
			m_LandusesIds.Add ("3384");


			if (Application.systemLanguage == SystemLanguage.German) {
				m_Landuses.Add ("Landwirtschaft");
				m_Landuses.Add ("Forstwirtschaft");
				m_Landuses.Add ("Vergnügen, Museum, Freizeit");
				m_Landuses.Add ("Handelsgewerbe");
				m_Landuses.Add ("Sozialer Dienst");
				m_Landuses.Add ("Wohngebiet");
				m_Landuses.Add ("Erholung, Sport");
				m_Landuses.Add ("Bergbau und Steinbruch");
				m_Landuses.Add ("Industrie und Herstellung");
				m_Landuses.Add ("Transport, Kommunikationsnetzwerke, Lagerung");
				m_Landuses.Add ("Bau");
				m_Landuses.Add ("Wasser und Wasserbehandlung");
				m_Landuses.Add ("Verlassenes Gebiet");
			} else {
				m_Landuses.Add ("Agriculture");
				m_Landuses.Add ("Forestry");
				m_Landuses.Add ("Amenities, museums, leisure");
				m_Landuses.Add ("Commerce");
				m_Landuses.Add ("Community services");
				m_Landuses.Add ("Residential");
				m_Landuses.Add ("Recreation, Sport");
				m_Landuses.Add ("Mining and Quarrying");
				m_Landuses.Add ("Industry and manufacturing");
				m_Landuses.Add ("Transport, Communication networks, storage");
				m_Landuses.Add ("Construction");
				m_Landuses.Add ("Water and Waste Treatment");
				m_Landuses.Add ("Abandoned areas");
			}
		}else if (m_CurLandCoverId == 3078) {
			//["U111","U113","U120","U340","U361","U410"]
			m_LandusesIds.Add ("3293");
			m_LandusesIds.Add ("3296");
			m_LandusesIds.Add ("3378");
			m_LandusesIds.Add ("3373");
			m_LandusesIds.Add ("3377");
			m_LandusesIds.Add ("3384");

			if (Application.systemLanguage == SystemLanguage.German) {
				m_Landuses.Add ("Landwirtschaft");
				m_Landuses.Add ("Forstwirtschaft");
				m_Landuses.Add ("Vergnügen, Museum, Freizeit");
				m_Landuses.Add ("Handelsgewerbe");
				m_Landuses.Add ("Erholung, Sport");
				m_Landuses.Add ("Verlassenes Gebiet");
			} else {
				m_Landuses.Add ("Agriculture");
				m_Landuses.Add ("Forestry");
				m_Landuses.Add ("Amenities, museums, leisure");
				m_Landuses.Add ("Commerce");
				m_Landuses.Add ("Recreation, Sport");
				m_Landuses.Add ("Abandoned areas");
			}
		}


		m_LandusesIds.Add ("0");
		if (Application.systemLanguage == SystemLanguage.German) {
			m_Landuses.Add ("Anderer Landnutzen");
		} else {
			m_Landuses.Add ("Other Landuse");
		}

		m_LandusesIds.Add ("0");
		if (Application.systemLanguage == SystemLanguage.German) {
			m_Landuses.Add ("Unbekannt");
		} else {
			m_Landuses.Add ("Unknown");
		}







		UnityEngine.UI.Dropdown dropdown;
		dropdown = m_Dropdown2.GetComponent<UnityEngine.UI.Dropdown>();
		dropdown.options.Clear ();

		for(int i=0; i<m_Landuses.Count; i++) {
			string strlandcover = (string)m_Landuses [i];
			UnityEngine.UI.Dropdown.OptionData list = new UnityEngine.UI.Dropdown.OptionData(strlandcover);
			dropdown.options.Add (list);
		}

		dropdown.value = 0;
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Escape)) {
			OnBackClicked ();
		}

		/*if (m_CurLandUse == 1) {
			
		}*/
		/*
		if (m_bShowPuzzleMessage) {
			m_PuzzleMessageIter++;
			if (m_PuzzleMessageIter > 5) {
				if (Application.systemLanguage == SystemLanguage.German) {
					string[] options = { "Ok" };
					messageBox.Show ("", "Bitte beantworten Sie ein paar Fragen um das Puzzleteil aufzuheben.", options);
				} else {
					string[] options = { "Ok" };
					messageBox.Show ("", "Please answer some questions to pick the puzzle piece up.", options);
				}
				m_bShowPuzzleMessage = false;
			}
		}*/

	}

	void hideImages() {
		m_ImageUrban.SetActive (false);
		m_ImageRoad.SetActive (false);
		m_ImageBuilding.SetActive (false);
		m_ImageBridge.SetActive (false);
		m_ImageAgrar.SetActive (false);
		m_ImageTree.SetActive (false);
		m_ImageCrop.SetActive (false);
		m_ImageWeed.SetActive (false);
		m_ImageCultivated.SetActive (false);
		m_ImageLeaf1.SetActive (false);
		m_ImageLeaf2.SetActive (false);
		m_ImagePasture.SetActive (false);
		m_ImageBushes.SetActive (false);
		m_ImageHerb.SetActive (false);
		m_ImageRock.SetActive (false);
		m_ImageSlope.SetActive (false);
		m_ImageVineyard.SetActive (false);
		m_ImageWater.SetActive (false);
		m_ImageWaterShallow.SetActive (false);

		m_ImageBackUrban.SetActive (false);
		m_ImageBackBridge.SetActive (false);
		m_ImageBackBuilding.SetActive (false);
		m_ImageBackField.SetActive (false);
		m_ImageBackTrees.SetActive (false);
		m_ImageBackTrees2.SetActive (false);
		m_ImageBackGrass.SetActive (false);
		m_ImageBackBush.SetActive (false);
		m_ImageBackWeed.SetActive (false);
		m_ImageBackWater.SetActive (false);
		m_ImageBackRock.SetActive (false);
	}

	public void updateQuestion() {
		hideImages ();

		Debug.Log ("updateQuestion: " + m_CurQuestion);
		UnityEngine.UI.Text text;
		text = m_TextQuestion.GetComponent<UnityEngine.UI.Text>();

		text.text = m_Questions [m_CurQuestion].m_Question; 

		if (m_CurLandUse == 1) {
			m_ImageBack.GetComponent<Image> ().color = new Color32 (255, 255, 255, 0);


			m_ButtonBackWhite.SetActive (true);
			m_ImageBackWhite.SetActive (true);
			m_ButtonBackBlack.SetActive(false);
			m_ImageBackBlack.SetActive(false);
			m_ImageBackLandUse.SetActive (true);
		} else {

			m_ButtonBackWhite.SetActive (false);
			m_ImageBackWhite.SetActive (false);
			m_ButtonBackBlack.SetActive(true);
			m_ImageBackBlack.SetActive(true);

			m_ImageBackLandUse.SetActive (false);

			UnityEngine.Random.seed = (m_CurQuestion * 4312) + 120;
			float temp = 0.0f;
			for (int i = 0; i < 4; i++) {
				 temp = UnityEngine.Random.Range (0.0f, 1.0f);
			}
		/*	byte red = (byte)(255 * UnityEngine.Random.Range (0.1f, 1.0f));
			byte green = (byte)(255 * UnityEngine.Random.Range (0.1f, 1.0f));
			byte blue = (byte)(255 * UnityEngine.Random.Range (0.1f, 1.0f));*/
			byte red = (byte)(255 * UnityEngine.Random.Range (0.7f, 1.0f));
			byte green = (byte)(255 * UnityEngine.Random.Range (0.7f, 1.0f));
			byte blue = (byte)(255 * UnityEngine.Random.Range (0.7f, 1.0f));
			byte alpha = 140;
			/*
			if(m_CurQuestion == 0) {
				red = 239;
				green = 250;
				blue = 254;
				alpha = 255;
			} else if(m_CurQuestion == 4 || m_CurQuestion == 1) {
				red = 255;
				green = 250;
				blue = 239;
				alpha = 255;
			}*/
			m_ImageBack.GetComponent<Image> ().color = new Color32 (red, green, blue, alpha);

			if (m_CurQuestion == 0) {
				m_ImageUrban.SetActive (true);
				m_ImageBackUrban.SetActive (true);
			} else if (m_CurQuestion == 1) {
				m_ImageRoad.SetActive (true);
				m_ImageBackUrban.SetActive (true);
			} else if (m_CurQuestion == 2) {
				m_ImageBackBuilding.SetActive (true);
				m_ImageBuilding.SetActive (true);
			} else if (m_CurQuestion == 3) {
				m_ImageBackBridge.SetActive (true);
				m_ImageBridge.SetActive (true);
			} else if (m_CurQuestion == 4) {
				m_ImageBackField.SetActive (true);
				m_ImageAgrar.SetActive (true);
			} else if (m_CurQuestion == 5) {
				m_ImageBackField.SetActive (true);
				m_ImageCrop.SetActive (true);
			} else if (m_CurQuestion == 6) {
				m_ImageBackField.SetActive (true);
				m_ImageCultivated.SetActive (true);
			} else if (m_CurQuestion == 7) {
				m_ImageBackField.SetActive (true);
				m_ImageWeed.SetActive (true);
			} else if (m_CurQuestion == 8) {
				m_ImageBackTrees.SetActive (true);
				m_ImageBackTrees2.SetActive (true);
				m_ImageTree.SetActive (true);
			} else if (m_CurQuestion == 10) {
				m_ImageBackTrees.SetActive (true);
				m_ImageBackTrees2.SetActive (true);
				m_ImageLeaf1.SetActive (true);
			} else if (m_CurQuestion == 11) {
				m_ImageBackTrees.SetActive (true);
				m_ImageBackTrees2.SetActive (true);
				m_ImageLeaf2.SetActive (true);
			} else if (m_CurQuestion == 14) {
				m_ImagePasture.SetActive (true);
				m_ImageBackGrass.SetActive (true);
			} else if (m_CurQuestion == 15) {
				m_ImageBushes.SetActive (true);
				m_ImageBackGrass.SetActive (true);
			} else if (m_CurQuestion == 16) {
				m_ImageSlope.SetActive (true);
				m_ImageBackBush.SetActive (true);
			}  else if (m_CurQuestion == 17) {
				m_ImageVineyard.SetActive (true);
				m_ImageBackBush.SetActive (true);
			} else if (m_CurQuestion == 18) {
				m_ImageBushes.SetActive (true);
				m_ImageBackBush.SetActive (true);
			} else if (m_CurQuestion == 20) {
				m_ImageHerb.SetActive (true);
				m_ImageBackWeed.SetActive (true);
			} else if (m_CurQuestion == 21) {
				m_ImageWater.SetActive (true);
				m_ImageBackWater.SetActive (true);
			} else if (m_CurQuestion == 22) {
				m_ImageWaterShallow.SetActive (true);
				m_ImageBackWater.SetActive (true);
			} else if (m_CurQuestion == 23) {
				m_ImageRock.SetActive (true);
				m_ImageBackRock.SetActive (true);
			}
		}

		//if (m_CollectingPuzzles == 0) {
			if (m_CurQuestion == 0) {
			if (Application.systemLanguage == SystemLanguage.German) {
				m_ButtonBack.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Zurück zur Kamera";
				m_ButtonBackWhite.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Zurück zur Kamera";
			} else {
				m_ButtonBack.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Back to Camera";
				m_ButtonBackWhite.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Back to Camera";
				} 
			} else {
				if (m_CurLandUse == 1) {
				if (Application.systemLanguage == SystemLanguage.German) {
					m_ButtonBack.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Zurück";
					m_ButtonBackWhite.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Zurück";
					} else {
						m_ButtonBack.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Back";
					m_ButtonBackWhite.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Back";
					} 
				} else {
				if (Application.systemLanguage == SystemLanguage.German) {
					m_ButtonBack.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Eine Frage zurück";
					m_ButtonBackWhite.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Eine Frage zurück";
				} else {
					m_ButtonBack.GetComponentInChildren<UnityEngine.UI.Text> ().text = "One Question back";
					m_ButtonBackWhite.GetComponentInChildren<UnityEngine.UI.Text> ().text = "One Question back";
					} 
				} 
			}
		/*} else {
			if (m_CurQuestion == 0) {
				if (Application.systemLanguage == SystemLanguage.German) {
					m_ButtonBack.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Zurück";
				} else {
					m_ButtonBack.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Back";
				} 
			} else {
				if (m_CurLandUse == 1) {
					if (Application.systemLanguage == SystemLanguage.German) {
						m_ButtonBack.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Zurück";
					} else {
						m_ButtonBack.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Back";
					} 
				} else {
					if (Application.systemLanguage == SystemLanguage.German) {
						m_ButtonBack.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Eine Frage zurück";
					} else {
						m_ButtonBack.GetComponentInChildren<UnityEngine.UI.Text> ().text = "One Question back";
					} 
				}
			}
		}*/
	}

	public void OnYesClicked () {
		if (m_Questions [m_CurQuestion].m_NextSelectedLandCoverYes != -1) {
			m_CurLandCoverId = m_Questions [m_CurQuestion].m_NextSelectedLandCoverYes;
			openLandUse ();
			return;
		}

		//Random.seed = 0;

		/*byte red = (byte)(255 * UnityEngine.Random.Range (0.0f, 1.0f));
		byte green = (byte)(255 * UnityEngine.Random.Range (0.0f, 1.0f));
		byte blue = (byte)(255 * UnityEngine.Random.Range (0.0f, 1.0f));
		m_ImageBack.GetComponent<Image>().color = new Color32(red, green, blue,50);
*/

		if (m_Questions [m_CurQuestion].m_NextSelectLandCoverYes != -1) {
		//	m_ImageBack.GetComponent<Image> ().color = new Color32 (0, 0, 0, 0);


			m_CurLandCover = m_Questions [m_CurQuestion].m_NextSelectLandCoverYes;

			m_ButtonYes.SetActive (false);
			m_ButtonNo.SetActive (false);
			m_TextQuestion.SetActive (false);
			m_Dropdown.SetActive (true);
			m_DropdownText.SetActive (true);
			hideImages ();

			if (Application.systemLanguage == SystemLanguage.German){
				m_DropdownText.GetComponent<UnityEngine.UI.Text>().text = "Wähle die Landbedeckung aus:";
			} else {
				m_DropdownText.GetComponent<UnityEngine.UI.Text>().text = "Select the landcover:";
			} 


			updateLandCover ();

			return;
		}


		m_ButtonNext.SetActive (false);
		m_Dropdown.SetActive (false);
		m_DropdownText.SetActive (false);

		if (m_Questions [m_CurQuestion].m_NextIdYes != -1) {
			m_CurQuestion = m_Questions [m_CurQuestion].m_NextIdYes;
			updateQuestion();
		}
	}
	public void OnNoClicked () {
		if (m_Questions [m_CurQuestion].m_NextSelectedLandCoverNo != -1) {
			m_CurLandCoverId = m_Questions [m_CurQuestion].m_NextSelectedLandCoverNo;
			openLandUse ();
			return;
		}

		/*
		byte red = (byte)(255 * UnityEngine.Random.Range (0.0f, 1.0f));
		byte green = (byte)(255 * UnityEngine.Random.Range (0.0f, 1.0f));
		byte blue = (byte)(255 * UnityEngine.Random.Range (0.0f, 1.0f));
		m_ImageBack.GetComponent<Image>().color = new Color32(red, green, blue,50);
*/

		if (m_Questions [m_CurQuestion].m_NextSelectLandCoverNo != -1) {
		//	m_ImageBack.GetComponent<Image> ().color = new Color32 (0, 0, 0, 0);

			m_CurLandCover = m_Questions [m_CurQuestion].m_NextSelectLandCoverNo;

			m_ButtonYes.SetActive (false);
			m_ButtonNo.SetActive (false);
			m_TextQuestion.SetActive (false);
			m_Dropdown.SetActive (true);
			m_DropdownText.SetActive (true);
			hideImages ();

			if (Application.systemLanguage == SystemLanguage.German){
				m_DropdownText.GetComponent<UnityEngine.UI.Text>().text = "Wähle die Landbedeckung aus:";
			} else {
				m_DropdownText.GetComponent<UnityEngine.UI.Text>().text = "Select the landcover:";
			} 
			updateLandCover ();
			return;
		}

		m_ButtonNext.SetActive (false);
		m_Dropdown.SetActive (false);
		m_DropdownText.SetActive (false);

		if (m_Questions [m_CurQuestion].m_NextIdNo != -1) {
			m_CurQuestion = m_Questions [m_CurQuestion].m_NextIdNo;
			updateQuestion();
		}
	}
	public void OnBackClicked () {
		if (m_CurLandUse != -1) {


			m_Dropdown2.SetActive (false);
			m_CurLandUse = -1;
			m_CurLandUseId = 0;


			m_TextLandUse.SetActive (false);


			m_CheckResidential.SetActive (false);
			m_CheckAmenities.SetActive (false);
			m_CheckRecreation.SetActive (false);
			m_CheckCommerce.SetActive (false);
			m_CheckConstruction.SetActive (false);
			m_CheckTransport.SetActive (false);
			m_CheckIndustry.SetActive (false);
			m_CheckAgriculture.SetActive (false);
			m_CheckForestry.SetActive (false);

			updateQuestion ();

			Debug.Log ("OnBackClicked");

			if (m_CurLandCover != 0) {
				Debug.Log ("OnBackClicked m_CurLandCover != 0");
				hideImages ();
				m_ButtonYes.SetActive (false);
				m_ButtonNo.SetActive (false);
				m_TextQuestion.SetActive (false);


				m_ButtonNext.SetActive (true);
				m_Dropdown.SetActive (true);
				m_DropdownText.SetActive (true);

				if (Application.systemLanguage == SystemLanguage.German){
					m_DropdownText.GetComponent<UnityEngine.UI.Text>().text = "Wähle die Landbedeckung aus:";
				} else {
					m_DropdownText.GetComponent<UnityEngine.UI.Text>().text = "Select the landcover:";
				}

				return;
			}

			Debug.Log ("OnBackClicked Buttons active");

			m_ButtonYes.SetActive (true);
			m_ButtonNo.SetActive (true);
			m_TextQuestion.SetActive (true);

			m_ButtonNext.SetActive (false);
			m_Dropdown.SetActive (false);
			m_DropdownText.SetActive (false);


			return;
		}

		if (m_CurLandCover != 0) {
			m_CurLandCover = 0;
			updateQuestion ();
			m_ButtonYes.SetActive (true);
			m_ButtonNo.SetActive (true);
			m_TextQuestion.SetActive (true);

			m_ButtonNext.SetActive (false);
			m_Dropdown.SetActive (false);
			m_DropdownText.SetActive (false);
			return;
		}

		if (m_CurQuestion == 0) {
			Debug.Log ("On root -> switch to camera");
		/*	if (m_CollectingPuzzles == 1) {
				Application.LoadLevel ("DemoMap");
			} else {*/
				PlayerPrefs.SetInt ("CameraStartLastStep", 1);
				PlayerPrefs.Save ();
				Application.LoadLevel ("TestCamera");
		//	}
		} else {
			if (m_Questions [m_CurQuestion].m_PrevId != -1) {
				m_CurQuestion = m_Questions [m_CurQuestion].m_PrevId;
				updateQuestion ();
			}
		}
	}

	public void openLandUse() {
		hideImages ();

		Debug.Log ("openLandUse landcoverid: " + m_CurLandCoverId);
		m_CurLandUse = 1;

		m_CurLandUseId = 0;

		m_ImageBack.GetComponent<Image>().color = new Color32(255,0,0,0);

		m_ButtonYes.SetActive (false);
		m_ButtonNo.SetActive (false);
		m_TextQuestion.SetActive (false);

		m_ButtonNext.SetActive (false);
		m_Dropdown.SetActive (false);
		m_Dropdown2.SetActive (true);
		m_DropdownText.SetActive (true);


		m_Dropdown2.SetActive (false);
		m_DropdownText.SetActive (false);

		m_TextLandUse.SetActive (true);

		m_ButtonNext.SetActive (true);


		m_CheckResidential.SetActive (true);
		m_CheckAmenities.SetActive (true);
		m_CheckRecreation.SetActive (true);
		m_CheckCommerce.SetActive (true);
		m_CheckConstruction.SetActive (true);
		m_CheckTransport.SetActive (true);
		m_CheckIndustry.SetActive (true);
		m_CheckAgriculture.SetActive (true);
		m_CheckForestry.SetActive (true);

		updateQuestion ();
		/*if (Application.systemLanguage == SystemLanguage.German){
			m_DropdownText.GetComponent<UnityEngine.UI.Text>().text = "Wähle die Landnutzung aus:";
		} else {
			m_DropdownText.GetComponent<UnityEngine.UI.Text>().text = "Select the land use:";
		} 
		updateLandUses ();

		if (m_Landuses.Count <= 3) {
			Debug.Log ("No land use -> continue");
			QuestionareFinished ();
		}*/
	}

	public void LandcoverSelected() {
		UnityEngine.UI.Dropdown dropdown;
		dropdown = m_Dropdown.GetComponent<UnityEngine.UI.Dropdown>();


		int curselection = dropdown.value;

		if (curselection > 0) {
			string strlandcoverid = (string)m_LandcoverIds [curselection];
			Debug.Log ("OnSelected: " + curselection + " strlandcoverid: " + strlandcoverid);
			m_CurLandCoverId = int.Parse (strlandcoverid);
			m_ButtonNext.SetActive (true);
		} else {
			m_ButtonNext.SetActive (false);
		}

	}

	public void LanduseSelected() {
		UnityEngine.UI.Dropdown dropdown;
		dropdown = m_Dropdown2.GetComponent<UnityEngine.UI.Dropdown>();


		int curselection = dropdown.value;

		if (curselection > 0) {
			string strlandcoverid = (string)m_LandusesIds [curselection];
			Debug.Log ("OnSelected: " + curselection + " strlanduseid: " + strlandcoverid);
			m_CurLandUseId = int.Parse (strlandcoverid);
			m_ButtonNext.SetActive (true);
		} else {
			m_ButtonNext.SetActive (false);
		}

	}

	public void OnNextClicked () {
		if (m_CurLandUse == -1) {
			openLandUse ();
		} else {
			Debug.Log ("Landuse next clicked");
			QuestionareFinished ();
		}
	}

	public void QuestionareFinished() {
		int m_NrQuestsDone = 0;
		if (PlayerPrefs.HasKey ("NrQuestsDone")) {
			m_NrQuestsDone = PlayerPrefs.GetInt ("NrQuestsDone");
		} else {
			m_NrQuestsDone = 0;
		}

		m_CurLandUseId = 0;
		m_CurNrLandUses = 0;

		if (m_CheckResidential.GetComponent<Toggle> ().isOn) {
			Debug.Log ("Residential checked");
			m_CurLandUseId = 3380;
			PlayerPrefs.SetInt ("CheckResidential", 1);

			PlayerPrefs.SetInt ("Quest_" + m_NrQuestsDone + "_LandUseId_" + m_CurNrLandUses, m_CurLandUseId);
			m_CurNrLandUses++;

		} else {
			PlayerPrefs.SetInt ("CheckResidential", 0);
		}
		if (m_CheckAmenities.GetComponent<Toggle> ().isOn) {
			Debug.Log ("Amenities checked");
			m_CurLandUseId = 3378;
			PlayerPrefs.SetInt ("CheckAmenities", 1);

			PlayerPrefs.SetInt ("Quest_" + m_NrQuestsDone + "_LandUseId_" + m_CurNrLandUses, m_CurLandUseId);
			m_CurNrLandUses++;
		} else {
			PlayerPrefs.SetInt ("CheckAmenities", 0);
		}
		if (m_CheckRecreation.GetComponent<Toggle> ().isOn) {
			Debug.Log ("m_CheckRecreation checked");
			m_CurLandUseId = 3377;
			PlayerPrefs.SetInt ("CheckRecreation", 1);

			PlayerPrefs.SetInt ("Quest_" + m_NrQuestsDone + "_LandUseId_" + m_CurNrLandUses, m_CurLandUseId);
			m_CurNrLandUses++;
		} else {
			PlayerPrefs.SetInt ("CheckRecreation", 0);
		}
		if (m_CheckCommerce.GetComponent<Toggle> ().isOn) {
			Debug.Log ("m_CheckCommerce checked");
			m_CurLandUseId = 3373;
			PlayerPrefs.SetInt ("CheckCommerce", 1);

			PlayerPrefs.SetInt ("Quest_" + m_NrQuestsDone + "_LandUseId_" + m_CurNrLandUses, m_CurLandUseId);
			m_CurNrLandUses++;
		} else {
			PlayerPrefs.SetInt ("CheckCommerce", 0);
		}
		if (m_CheckConstruction.GetComponent<Toggle> ().isOn) {
			Debug.Log ("m_CheckConstruction checked");
			m_CurLandUseId = 3370;
			PlayerPrefs.SetInt ("CheckConstruction", 1);

			PlayerPrefs.SetInt ("Quest_" + m_NrQuestsDone + "_LandUseId_" + m_CurNrLandUses, m_CurLandUseId);
			m_CurNrLandUses++;
		} else {
			PlayerPrefs.SetInt ("CheckConstruction", 0);
		}
		if (m_CheckTransport.GetComponent<Toggle> ().isOn) {
			Debug.Log ("m_CheckTransport checked");
			m_CurLandUseId = 3355;
			PlayerPrefs.SetInt ("CheckTransport", 1);

			PlayerPrefs.SetInt ("Quest_" + m_NrQuestsDone + "_LandUseId_" + m_CurNrLandUses, m_CurLandUseId);
			m_CurNrLandUses++;
		} else {
			PlayerPrefs.SetInt ("CheckTransport", 0);
		}
		if (m_CheckIndustry.GetComponent<Toggle> ().isOn) {
			Debug.Log ("m_CheckIndustry checked");
			m_CurLandUseId = 3307;
			PlayerPrefs.SetInt ("CheckIndustry", 1);

			PlayerPrefs.SetInt ("Quest_" + m_NrQuestsDone + "_LandUseId_" + m_CurNrLandUses, m_CurLandUseId);
			m_CurNrLandUses++;
		} else {
			PlayerPrefs.SetInt ("CheckIndustry", 0);
		}
		if (m_CheckAgriculture.GetComponent<Toggle> ().isOn) {
			Debug.Log ("m_CheckAgriculture checked");
			m_CurLandUseId = 3293;
			PlayerPrefs.SetInt ("CheckAgriculture", 1);

			PlayerPrefs.SetInt ("Quest_" + m_NrQuestsDone + "_LandUseId_" + m_CurNrLandUses, m_CurLandUseId);
			m_CurNrLandUses++;
		} else {
			PlayerPrefs.SetInt ("CheckAgriculture", 0);
		}
		if (m_CheckForestry.GetComponent<Toggle> ().isOn) {
			Debug.Log ("m_CheckForestry checked");
			m_CurLandUseId = 3296;
			PlayerPrefs.SetInt ("CheckForestry", 1);

			PlayerPrefs.SetInt ("Quest_" + m_NrQuestsDone + "_LandUseId_" + m_CurNrLandUses, m_CurLandUseId);
			m_CurNrLandUses++;
		} else {
			PlayerPrefs.SetInt ("CheckForestry", 0);
		}


		PlayerPrefs.SetInt ("Quest_" + m_NrQuestsDone + "_NrLandUses", m_CurNrLandUses);
		m_CurNrLandUses++;

		Debug.Log ("Landcover id: " + m_CurLandCoverId);
		Debug.Log ("Landuse id: " + m_CurLandUseId);


		/*
		Screen.orientation = ScreenOrientation.AutoRotation;
		Screen.autorotateToPortrait = true;
		Screen.autorotateToPortraitUpsideDown = true;
		Screen.autorotateToLandscapeRight = true;
		Screen.autorotateToLandscapeLeft = true;*/


		Debug.Log ("QuestionareFinished m_CurLandCoverId: " + m_CurLandCoverId + " m_CurLandUseId: " + m_CurLandUseId);
		//if (m_CollectingPuzzles == 0) {
			
			

			PlayerPrefs.SetInt ("Quest_" + m_NrQuestsDone + "_LandCoverId", m_CurLandCoverId);
			//PlayerPrefs.SetInt ("Quest_" + m_NrQuestsDone + "_LandUseId", m_CurLandUseId);

			PlayerPrefs.SetFloat ("Quest_" + m_NrQuestsDone + "_" + "LandCover" + "_Heading", Input.compass.trueHeading);
			PlayerPrefs.SetFloat ("Quest_" + m_NrQuestsDone + "_" + "LandCover" + "_AccX", Input.acceleration.x);
			PlayerPrefs.SetFloat ("Quest_" + m_NrQuestsDone + "_" + "LandCover" + "_AccY", Input.acceleration.y);
			PlayerPrefs.SetFloat ("Quest_" + m_NrQuestsDone + "_" + "LandCover" + "_AccZ", Input.acceleration.z);

			// Save timings
			string strtime = PlayerPrefs.GetString("CurQuestSelectedTime");
			PlayerPrefs.SetString ("Quest_" + m_NrQuestsDone + "_" + "SelectedTime", strtime);

			strtime = PlayerPrefs.GetString("CurQuestStartQuestTime");
			PlayerPrefs.SetString ("Quest_" + m_NrQuestsDone + "_" + "StartQuestTime", strtime);

			strtime = PlayerPrefs.GetString("CurQuestStartCameraTime");
			PlayerPrefs.SetString ("Quest_" + m_NrQuestsDone + "_" + "StartCameraTime", strtime);

			strtime = PlayerPrefs.GetString("CurQuestEndCameraTime");
			PlayerPrefs.SetString ("Quest_" + m_NrQuestsDone + "_" + "EndCameraTime", strtime);

			strtime = PlayerPrefs.GetString("CurQuestStartQuestionsTime");
			PlayerPrefs.SetString ("Quest_" + m_NrQuestsDone + "_" + "StartQuestionsTime", strtime);

			string endquestionstime = System.DateTime.Now.ToString ("yyyy/MM/dd HH:mm:ss");
			PlayerPrefs.SetString ("Quest_" + m_NrQuestsDone + "_" + "EndQuestionsTime", endquestionstime);

			// Save player positions
			float fvalue = PlayerPrefs.GetFloat("CurQuestStartPositionX");
			PlayerPrefs.SetFloat ("Quest_" + m_NrQuestsDone + "_" + "StartPositionX", fvalue);
			fvalue = PlayerPrefs.GetFloat("CurQuestStartPositionY");
			PlayerPrefs.SetFloat ("Quest_" + m_NrQuestsDone + "_" + "StartPositionY", fvalue);
			fvalue = PlayerPrefs.GetFloat("CurQuestEndPositionX");
			PlayerPrefs.SetFloat ("Quest_" + m_NrQuestsDone + "_" + "EndPositionX", fvalue);
			fvalue = PlayerPrefs.GetFloat("CurQuestEndPositionY");
			PlayerPrefs.SetFloat ("Quest_" + m_NrQuestsDone + "_" + "EndPositionY", fvalue);

			fvalue = PlayerPrefs.GetFloat("CurDistanceWalked");
			PlayerPrefs.SetFloat ("Quest_" + m_NrQuestsDone + "_" + "DistanceWalked", fvalue);

		int nrpositions = PlayerPrefs.GetInt("CurQuestNrPositions");
		PlayerPrefs.SetInt ("Quest_" + m_NrQuestsDone + "_" + "NrPositions", nrpositions);
		for (int pos = 0; pos < nrpositions; pos++) {
			float posx = PlayerPrefs.GetFloat ("CurQuestPositionX_" + pos);
			float posy = PlayerPrefs.GetFloat ("CurQuestPositionY_" + pos);

			PlayerPrefs.SetFloat ("Quest_" + m_NrQuestsDone + "_" + "PositionX_" + pos, posx);
			PlayerPrefs.SetFloat ("Quest_" + m_NrQuestsDone + "_" + "PositionY_" + pos, posy);
		}






			float tilted = Input.acceleration.z;
			if (tilted > 1.0f) {
				tilted = 1.0f;
			} else if (tilted < -1.0f) {
				tilted = -1.0f;
			}
			tilted *= 90.0f;
			PlayerPrefs.SetFloat ("Quest_" + m_NrQuestsDone + "_" + "LandCover" + "_Tilt", tilted);

			float lat = Input.location.lastData.latitude;
			float lng = Input.location.lastData.longitude;

			PlayerPrefs.SetFloat ("Quest_" + m_NrQuestsDone + "_" + "LandCover" + "_Lat", lat);
			PlayerPrefs.SetFloat ("Quest_" + m_NrQuestsDone + "_" + "LandCover" + "_Lng", lng);

			PlayerPrefs.SetFloat ("Quest_" + m_NrQuestsDone + "_" + "LandCover" + "_Accuracy", Input.compass.headingAccuracy);

			PlayerPrefs.SetInt ("CurQuestions_QuestionId", m_CurQuestion);


			string theTime = System.DateTime.Now.ToString ("yyyy-MM-dd HH:mm:sszz");
			string theTime2 = theTime;//theTime.Replace ("+", "%2B");
			Debug.Log ("CurrentTimestamp: " + theTime2);
			PlayerPrefs.SetString ("Quest_" + m_NrQuestsDone + "_" + "LandCover" + "_Timestamp", theTime2);


			PlayerPrefs.Save ();


			Application.LoadLevel ("QuestFinished");
	/*	} else {

			PlayerPrefs.SetInt ("CurPuzzle_LandCoverId", m_CurLandCoverId);
			PlayerPrefs.SetInt ("CurPuzzle_LandUseId", m_CurLandUseId);

			PlayerPrefs.SetFloat ("CurPuzzle_" + "LandCover" + "_Heading", Input.compass.trueHeading);
			PlayerPrefs.SetFloat ("CurPuzzle_" + "LandCover" + "_AccX", Input.acceleration.x);
			PlayerPrefs.SetFloat ("CurPuzzle_" + "LandCover" + "_AccY", Input.acceleration.y);
			PlayerPrefs.SetFloat ("CurPuzzle_" + "LandCover" + "_AccZ", Input.acceleration.z);

			float tilted = Input.acceleration.z;
			if (tilted > 1.0f) {
				tilted = 1.0f;
			} else if (tilted < -1.0f) {
				tilted = -1.0f;
			}
			tilted *= 90.0f;
			PlayerPrefs.SetFloat ("CurPuzzle_" + "LandCover" + "_Tilt", tilted);

			float lat = Input.location.lastData.latitude;
			float lng = Input.location.lastData.longitude;

			PlayerPrefs.SetFloat ("CurPuzzle_" + "PlayerPos" + "_Lat", lat);
			PlayerPrefs.SetFloat ("CurPuzzle_" + "PlayerPos" + "_Lng", lng);

			PlayerPrefs.SetFloat ("CurPuzzle_Accuracy", Input.compass.headingAccuracy);


			string endquestionstime = System.DateTime.Now.ToString ("yyyy/MM/dd HH:mm:ss");
			PlayerPrefs.SetString ("CurQuestEndQuestionsTime", endquestionstime);



			PlayerPrefs.Save ();

			Application.LoadLevel ("PuzzleUpload");
		}*/
	}
}
