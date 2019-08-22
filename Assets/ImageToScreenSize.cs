using UnityEngine;
using System.Collections;

public class ImageToScreenSize : MonoBehaviour {

	// Use this for initialization
	private void Start()
	{
		RectTransform rect = GetComponent<RectTransform> ();

		GetComponent<RectTransform> ().sizeDelta = new Vector2(Screen.width*0.5f, Screen.height*2.0f);

		/*#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
		GUITexture gt = guiTexture;
		Rect pi = guiTexture.pixelInset;
		#else
		GUITexture gt = GetComponent<GUITexture>();
		Rect pi = gt.pixelInset;
		#endif
		float sw = Screen.width / (float) gt.texture.width;
		float sh = Screen.height / (float) gt.texture.height;

		if (sw > sh)
		{
			pi.width = Screen.width;
			pi.height = sw * gt.texture.height;
		}
		else
		{
			pi.height = Screen.height;
			pi.width = sh * gt.texture.width;
		}

		pi.x = pi.width / -2;
		pi.y = pi.height / -2;


		gt.pixelInset = pi;*/
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<RectTransform> ().sizeDelta = new Vector2(Screen.width, Screen.height);
	}
}

