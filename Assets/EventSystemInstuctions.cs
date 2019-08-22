using UnityEngine;
using System.Collections;

public class EventSystemInstuctions : MonoBehaviour {

	public GameObject m_Text;
	public GameObject m_Text2;
	public GameObject m_Logo;


	public GameObject m_Button;

	public GameObject m_ButtonPoint1;
	public GameObject m_ButtonPoint2;
	public GameObject m_ButtonPoint3;

	public GameObject m_Point1;
	public GameObject m_Point2;
	public GameObject m_Point3;


	public GameObject m_Link;

	// Use this for initialization
	void Start () {
		m_CurState = 0;
		updateStates ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) 
			Application.LoadLevel ("DemoMap");
	}

	int m_CurState = 0;
	public void updateStates() {
		if (m_CurState == 0) {
			m_Point1.SetActive (true);
			m_Point2.SetActive (false);
			m_Point3.SetActive (false);

			m_Logo.SetActive (true);

			m_Link.SetActive (false);

			m_ButtonPoint1.SetActive (false);
			m_ButtonPoint2.SetActive (true);
			m_ButtonPoint3.SetActive (true);


			m_Text.SetActive (true);
			m_Text2.SetActive (false);


			UnityEngine.UI.Text text;
			text = m_Text.GetComponent<UnityEngine.UI.Text>();
			if (Application.systemLanguage == SystemLanguage.German) {
				text.text = "Wähle einen Punkt auf der Karte aus und versuche ihn dann zu erreichen.";

				m_Button.GetComponentInChildren<UnityEngine.UI.Text>().text = "Weiter";
			} else {
				text.text = "Select a point on the map and try to reach it.";

				m_Button.GetComponentInChildren<UnityEngine.UI.Text>().text = "Next";
			}  
		} else if (m_CurState == 1) {
			m_Point1.SetActive (false);
			m_Point2.SetActive (true);
			m_Point3.SetActive (false);

			m_Logo.SetActive (false);

			m_Link.SetActive (false);


			m_Text.SetActive (false);
			m_Text2.SetActive (true);
			UnityEngine.UI.Text text;
			text = m_Text2.GetComponent<UnityEngine.UI.Text>();
			if (Application.systemLanguage == SystemLanguage.German) {
				text.text = "Sobald du den Zielpunkt erreicht hast, kannst du die Quest starten! Sollte der Punkt nicht erreichbar sein, wähle \"Punkt nicht erreichbar\" aus.";


				m_Button.GetComponentInChildren<UnityEngine.UI.Text>().text = "Weiter";
			} else {
				text.text = "As soon as you reached the point you can start the quest! If the point is not reachable, please select \"Point not reachable\".";
			

				m_Button.GetComponentInChildren<UnityEngine.UI.Text>().text = "Next";
			}  

			m_ButtonPoint1.SetActive (true);
			m_ButtonPoint2.SetActive (false);
			m_ButtonPoint3.SetActive (true);
		} else if (m_CurState == 2) {
			m_Point1.SetActive (false);
			m_Point2.SetActive (false);
			m_Point3.SetActive (true);

			m_Logo.SetActive (false);

			m_Text.SetActive (false);
			m_Text2.SetActive (true);
			m_Link.SetActive (true);

			UnityEngine.UI.Text text;
			text = m_Text2.GetComponent<UnityEngine.UI.Text>();
			if (Application.systemLanguage == SystemLanguage.German) {
				text.text = "Weitere Informationen erhälst du hier: ";


				m_Button.GetComponentInChildren<UnityEngine.UI.Text>().text = "Ok";
			} else {
				text.text = "You can find additional information here:";
		

				m_Button.GetComponentInChildren<UnityEngine.UI.Text>().text = "Ok";
			}  

			m_ButtonPoint1.SetActive (true);
			m_ButtonPoint2.SetActive (true);
			m_ButtonPoint3.SetActive (false);
		}
	}

	public void NextClicked () {
		m_CurState++;
		if (m_CurState > 2) {
			m_CurState = 2;
			Application.LoadLevel ("DemoMap");
		}
		updateStates ();
	}
	public void Point1Clicked () {
		m_CurState = 0;
		updateStates ();
	}
	public void Point2Clicked () {
		m_CurState = 1;
		updateStates ();
	}
	public void Point3Clicked () {
		m_CurState = 2;
		updateStates ();
	}

	public void OnHomepageClicked()
	{
		Application.OpenURL("http://www.fotoquest-go.org");
	}
}
