using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TaskSequential : MonoBehaviour , IPhotoTask
{
    public PhotoTask m_PhotoTask;
    public Text StatusText;
    public Text GuidanceText;

    //private GameObject m_Buttons = new GameObject[4];
    private int m_ActualTask = 0 ;
    private readonly string GuidanceTextValue = "Mache ein Foto Richtung " ;
    private readonly string[] DirNames = new string[] { "Norden" , "Osten" , "Süden" , "Westen" } ;

    public void Reset ()
    {
        SetActualTask ( 0 ) ;
    }

    public void Back ()
    {
        if (IsAnyFinished())
        {
            SetActualTask ( m_ActualTask - 1 ) ;
        } else {

            PlayerPrefs.SetInt("DynQuestionsToLastState", 1);
            PlayerPrefs.Save();
            Application.LoadLevel("Validation");
            //Application.LoadLevel("DynamicQuestions");
        }
    }

    public void SetFinished ( int n )
    {
        if ( n == m_ActualTask ) SetActualTask ( m_ActualTask + 1 ) ;
    }

    public bool IsAllFinished()
    {
        return m_ActualTask > 3 ;
    }

    public bool IsAnyFinished()
    {
        return m_ActualTask > 0 ;
    }

    private void TakePicture ( int direction )
    {
        m_PhotoTask.TakePicture ( direction ) ;
    }

    public void TakePhoto () { TakePicture ( m_ActualTask ) ; }

    private string GetGuidanceText ( int dir )
    {
        if ( dir > 3 ) return "Sehr gut, Aufgabe erledigt!";
        return GuidanceTextValue + DirNames [ dir ] ;
    }

    void SetActualTask ( int dir )
    {
        m_ActualTask = dir ;
        GuidanceText.text = GetGuidanceText ( dir ) ;
        StatusText.text   = GetGuidanceText ( dir ) ;
    }

    // Use this for initialization
 //   void Start ()
 //   {
		
	//}

    // Update is called once per frame
    public void UpdateTask() { }


    public void Next() {}
}
