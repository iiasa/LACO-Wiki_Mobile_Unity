using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

public class FotoQuestPinCluster
{
    public List<FotoQuestPin> m_Childs = new List<FotoQuestPin>();
    public OnlineMapsMarker3D m_Marker = null;
}

public class FotoQuestPin
{
    public string m_Id = "";
  ///  public string m_Weight = "";
   // public string m_Color = "";
    public double m_Lat = 0.0;
    public double m_Lng = 0.0;
    //public OnlineMapsMarker m_Marker;
    public OnlineMapsMarker3D m_Marker = null;
   // public OnlineMapsMarker m_Marker2;
    public bool m_bDone = false;
    public int m_NrVisits;
  //  public string m_Conquerer;
  //  public Vector3[] vertices;
    public float m_ScreenPositionX;
    public float m_ScreenPositionY;
    public FotoQuestPinCluster m_Cluster = null;
    public bool m_bVisible;

    public string m_LegendId = "";
    public string m_ValidationId = "";


    public FotoQuestPin()
    {
        InitValues();
    }

    public void InitValues()
    {
        m_bDone = false;
        m_NrVisits = 0;
        m_bVisible = false;
    }
}

public class MapTest : MonoBehaviour
{
   
}