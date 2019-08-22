using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

// https://answers.unity.com/questions/773464/webcamtexture-correct-resolution-and-ratio.html

public class SensorImage : MonoBehaviour {

    private AspectRatioFitter imageFitter ;

    private float m_FlashLength = 0.3f ;
    private float m_FlashCounter = 0.0f ;
    private float m_ScreenAspectX ;
    private float m_VideoAspectX = 0.0f ;
    private WebCamTexture webcamTexture ;
    private RawImage m_RawImage ;
    //private bool m_Portrait = false;

    // Image uvRect
    private Rect rectDefault = new Rect ( 0f , 0f , 1f ,  1f ) ;
    private Rect rectMirrorY = new Rect ( 0f , 1f , 1f , -1f ) ;

   // public GameObject m_DisabledImage;

    private void DoFlash ()
    {
        m_FlashCounter = m_FlashLength ;
    }

    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
        Color[] rpixels = result.GetPixels(0);
        float incX = ((float)1 / source.width) * ((float)source.width / targetWidth);
        float incY = ((float)1 / source.height) * ((float)source.height / targetHeight);
        for (int px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth),
                incY * ((float)Mathf.Floor(px / targetWidth)));
        }
        result.SetPixels(rpixels, 0);
        result.Apply();
        return result;
    }

    public void TakePicture ( string filename , bool flash )
    {
        Debug.Log("Take picture: " + filename);
        Texture2D snap = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.ARGB32, true);// false  ) ;
        snap.SetPixels ( webcamTexture.GetPixels() ) ;
        snap.Apply () ;


        int w = snap.width;
        int h = snap.height;

        int newwidth = 128;
        int newheight = 128;

        if (w > h)
        {
            float newscale = 1024.0f / w;
            newwidth = (int)((float)w * newscale);
            newheight = (int)((float)h * newscale);
        }
        else
        {
            float newscale = 1024.0f / h;
            newwidth = (int)((float)w * newscale);
            newheight = (int)((float)h * newscale);
        }

        Texture2D scaledTex = ScaleTexture(snap, newwidth, newheight);

        Destroy(snap);
        snap = null;




        byte[] bytes = scaledTex.EncodeToJPG () ;


        File.WriteAllBytes ( Application.persistentDataPath + "/" + filename + ".jpg" , bytes ) ;
        PlayerPrefs.SetString("LastImageTaken", filename);
        PlayerPrefs.Save();

        Destroy(scaledTex);
        scaledTex = null;

        if (flash) DoFlash();
    }

    public void SetPortrait ( bool b )
    {
        if ( b )
        {
            float videoRatio = ( float ) webcamTexture.width / ( float ) webcamTexture.height ;
            GetComponent<RawImage>().transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 270.0f);
            GetComponent<RawImage>().transform.localScale = new Vector3(videoRatio, videoRatio, 1.0f);
            GetComponent<RawImage>().uvRect = rectDefault;
        }
        else
        {
            //if ( Application.platform == RuntimePlatform.IPhonePlayer )
            //if ( webcamTexture.videoVerticallyMirrored )
            //{
            //    GetComponent<RawImage>().uvRect = rectMirrorY ;
            //}
            //webcamTexture.videoVerticallyMirrored
            //else
            //{
            //}
            GetComponent<RawImage>().transform.localRotation = Quaternion.Euler ( 0.0f , 0.0f , 0.0f ) ;
            GetComponent<RawImage>().transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            GetComponent<RawImage>().uvRect = Application.platform == RuntimePlatform.IPhonePlayer ? rectMirrorY : rectDefault ;
        }
    }

    private void SetTexture ( Texture texture )
    {
        m_RawImage.texture = texture ;
      //  m_RawImage.material.mainTexture = texture ;
    }

    //public void printDebugInfo ( DebugText debugText )
    //{
    //    debugText.PrintText("VideoAspect", m_VideoAspectX);
    //    debugText.PrintText("webcamTexture.width", webcamTexture.width);
    //    debugText.PrintText("webcamTexture.height", webcamTexture.height);
    //    webcamTexture.width webcamTexture.height
    //}

    public void StopSensor()
    {
        Debug.Log("#### SensorImage stop ####");
        SetTexture(Texture2D.blackTexture);
        webcamTexture.Stop();
    }

    // Use this for initialization
    void Start () {
        imageFitter = this.GetComponent<AspectRatioFitter>() ;
        m_ScreenAspectX = ( ( float ) Screen.width) / Screen.height ;
        m_RawImage = GetComponent<RawImage> () ;
        //webcamTexture = new WebCamTexture(1280, Mathf.RoundToInt(1280.0f / m_ScreenAspectX));
        //webcamTexture = new WebCamTexture();
        webcamTexture = new WebCamTexture(1280, 720);
        webcamTexture.filterMode = FilterMode.Bilinear ;

        SetTexture ( webcamTexture ) ;

        GetComponent<RawImage>().color = Color.white ;

        //if ( Application.platform == RuntimePlatform.IPhonePlayer )
        //{
        //    GetComponent<RawImage>().transform.localRotation = Quaternion.Euler ( 0.0f , 0.0f , 180.0f ) ;
        //}
        //SetPortrait(false);
        //GetComponent<RawImage>().uvRect = webcamTexture.videoVerticallyMirrored ? rectMirrorY : rectDefault;
        GetComponent<RawImage>().uvRect = Application.platform == RuntimePlatform.IPhonePlayer ? rectMirrorY : rectDefault;

        webcamTexture.Play() ;
    }

    int m_DisabledIter = 0;

    // Update is called once per frame
    private void Update()
    {
        if(webcamTexture.isPlaying == false) {
            m_DisabledIter++;
            if(m_DisabledIter > 50) {
                webcamTexture.Stop();
                webcamTexture = new WebCamTexture(1280, 720);
                webcamTexture.filterMode = FilterMode.Bilinear;

                SetTexture(webcamTexture);

                GetComponent<RawImage>().color = Color.white;

                GetComponent<RawImage>().uvRect = Application.platform == RuntimePlatform.IPhonePlayer ? rectMirrorY : rectDefault;

                webcamTexture.Play();
                m_DisabledIter = 0;
            }
          //  m_DisabledImage.SetActive(true);
        }/* else {
            m_DisabledImage.SetActive(false);
        }*/
        // check aspect
        if (webcamTexture.width >= 100)
        {
            float videoRatio = (float)webcamTexture.width / (float)webcamTexture.height;
            if (m_VideoAspectX != videoRatio)
            {
                m_VideoAspectX = videoRatio;
                imageFitter.aspectRatio = m_VideoAspectX;
            }
        }
        // flash
        if (0.0f < m_FlashCounter)
        {
            if (m_FlashLength == m_FlashCounter)
            {
                SetTexture(Texture2D.whiteTexture);
            }
            m_FlashCounter -= Time.deltaTime;
            if (m_FlashCounter <= 0.0f)
            {
                SetTexture(webcamTexture);
                m_FlashCounter = 0.0f;
            }
        }
    }
}
