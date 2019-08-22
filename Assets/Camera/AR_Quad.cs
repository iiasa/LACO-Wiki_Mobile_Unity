using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AR_Quad : MonoBehaviour {

    // Use this for initialization
    public Camera m_MainCamera ;
    //public bool m_Flash;
    private float m_FlashLength = 0.3f ;
    private WebCamTexture webcamTexture ;
    //private Texture FlashTexture = Texture.
    private float m_FlashCounter = 0.0f ;

    public List < string > getCameras ()
    {
        List < string > Result = new List < string > () ;
        WebCamDevice [] cam_devices = WebCamTexture.devices ;
        for (int i = 0 ; i < cam_devices.Length ; i++ )
        {
            print ( "Webcam available: " + cam_devices[i].name ) ;
            Result.Add ( cam_devices[i].name ) ;
        }
        return Result ;
    }

    private void DoFlash ()
    {
        m_FlashCounter = m_FlashLength ;
    }

    public void TakePicture ( string filename , bool flash )
    {
        Texture2D snap = new Texture2D ( webcamTexture.width , webcamTexture.height ) ;
        snap.SetPixels ( webcamTexture.GetPixels() ) ;
        snap.Apply () ;
        byte [] bytes = snap.EncodeToJPG() ;
        if ( Application.platform == RuntimePlatform.Android )
        {
            File.WriteAllBytes ( Application.persistentDataPath + "/" + filename + ".jpg", bytes ) ;
        }
        if ( flash ) DoFlash() ;
    }

    private void SetupScaling()
    {
        float distance = transform.localPosition.z ;
        float frustumHeight = 2.0f * distance * Mathf.Tan ( m_MainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad ) ;
        float aspect = ( float ) webcamTexture.width / ( float ) webcamTexture.height ;
        //float frustumWidth = frustumHeight * m_MainCamera.aspect;
        float frustumWidth = frustumHeight * aspect ;
        transform.localScale = new Vector3 ( frustumWidth , frustumHeight, 1.0f ) ;
    }

    private void Start ()
    {
        //webcamTexture = new WebCamTexture ( Screen.width , Screen.height ) ;
        webcamTexture = new WebCamTexture ( 1280 , 1280 * Screen.height / Screen.width ) ;
        //webcamTexture = new WebCamTexture ( 640 , 640 * Screen.height / Screen.width) ;

        //GetComponent<Renderer>().material.mainTexture = webcamTexture ;
        //webcamTexture.Play();
        //SetupScaling();
    }

    // Update is called once per frame
    private void Update ()
    {
        // flash
        if ( 0.0f < m_FlashCounter )
        {
            if ( m_FlashLength == m_FlashCounter ) GetComponent<Renderer>().material.mainTexture = Texture2D.whiteTexture ;
            m_FlashCounter -= Time.deltaTime ;
            if ( m_FlashCounter <= 0.0f )
            {
                GetComponent < Renderer > () . material.mainTexture = webcamTexture ;
                m_FlashCounter = 0.0f ;
            }
        }
    }
}
