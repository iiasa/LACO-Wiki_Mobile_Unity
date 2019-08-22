using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour {
	WebCamTexture back;

	public RawImage rawimage;  

	// Use this for initialization
	void Start () {
		back = new WebCamTexture ();

		rawimage.texture = back;
		rawimage.material.mainTexture = back;
		back.Play();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
