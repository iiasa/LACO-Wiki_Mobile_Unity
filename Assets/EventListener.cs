using UnityEngine;
using System.Collections;

public class EventListener : MonoBehaviour {

	// Use this for initialization
	public void ClickedButtonChangeScene () {
		Debug.Log ("Clicked button change scene");

		//Application.LoadLevel ("MapScene");
		//Application.LoadLevel ("TilesetTest");
		Application.LoadLevel ("DemoMap");
	}

	public void ClickedCamera () {
		Debug.Log ("Clicked camera");
		Application.LoadLevel ("TestCamera");
	}

}
