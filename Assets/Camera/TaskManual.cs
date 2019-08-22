using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManual : MonoBehaviour , IPhotoTask
{
    public PhotoTask m_PhotoTask;
    public Text StatusText;

    private bool[] m_bDone = new bool[4] ;
    private GameObject[] m_Buttons = new GameObject[4];
    private readonly string[] DirNames = new string[] { "North", "East", "South", "West" };
    private Stack < int > m_Dones = new Stack < int > () ;

    public void Reset()
    {
      /*  for ( int i = 0; i < 4; i++ )
        {
            m_bDone [ i ] = false ;
            m_Buttons[i].GetComponent<Image>().color = Color.red ;
        }
        m_Dones.Clear();
        StatusText.text = "Bitte machen sie ein Foto in jede Himmelsrichtung" ;*/
    }
    public void Back ()
    {
      /*  if (IsAnyFinished())
        {
            int n = m_Dones.Pop();
            m_bDone[n] = false;
            m_Buttons[n].GetComponent<Image>().color = Color.red;
        }*/
    }

    public void SetFinished ( int n )
    {/*
        m_bDone [ n ] = true ;
        m_Buttons [ n ] . GetComponent<Image> () . color = Color.green ;
        m_Dones.Push(n);*/
    }

    public bool IsAllFinished ()
    {
        return true;/*
        for ( int i = 0 ; i < 4 ; i++ )
        {
            if ( !m_bDone[i] ) return false ;
        }
        return true;*/
    }

    public bool IsAnyFinished()
    {
        return true;/*
        return m_Dones.Count != 0;*/
    }


    private void TakePicture ( int direction )
    {
      //  m_PhotoTask.TakePicture ( direction ) ;
    }

    public void TakePhotoNorth () { TakePicture ( 0 ) ; }
    public void TakePhotoEast  () { TakePicture ( 1 ) ; }
    public void TakePhotoSouth () { TakePicture ( 2 ) ; }
    public void TakePhotoWest  () { TakePicture ( 3 ) ; }

    // Use this for initialization
    void Start ()
    {
       /* for ( int i = 0 ; i < 4 ; i++ )
        {
            string ButtonName = "Button" + DirNames [ i ] ;
            m_Buttons[i] = transform.Find(ButtonName).Find("Image").gameObject;
        }
        Reset () ;*/
    }

    public void UpdateTask() { }


    //// Update is called once per frame
    //void Update ()
    //   {

    //}


    public void Next() {}
}
