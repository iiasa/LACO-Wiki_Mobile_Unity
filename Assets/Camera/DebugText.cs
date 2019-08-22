using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour
{
    //private string m_String ;
    private Text m_Text ;
    private static readonly int VarWidth = 15 ;

    // Use this for initialization
    private void Start ()
    {
        m_Text = GetComponent<Text>();
        //m_String = GetComponent<Text>().text ;
        //GetComponent<Text>().text = "Test1\nTest2\nTest3" ;
    }

    private void PrintSpaces ( string var )
    {
        //m_Text.text += "\tab";
        int NumSpaces = VarWidth - var.Length;
        for ( int i = 0; i < NumSpaces ; i++ )
        {
            m_Text.text += ' ' ;
        }
    }

    public void Clear ()
    {
        //m_String = "" ;
        m_Text.text = "" ;
        //m_Text.text = "\tqr\tqx20\tqx25";
    }

    public void PrintText ( string pre , int v )
    {
        PrintSpaces ( pre ) ;
        m_Text.text += pre ;
        m_Text.text += "\t : ";
        m_Text.text += v.ToString() ;
        m_Text.text += "\n" ;
    }

    public void PrintText ( string pre , float v , int numdigits = 3 )
    {
        PrintSpaces ( pre ) ;
        m_Text.text += pre ;
        m_Text.text += "\t : ";
        m_Text.text += Div.getRounded ( v , numdigits ) . ToString () ;
        m_Text.text += "\n" ;
    }

    public void PrintText ( Vector3 v , int numdigits = 3 )
    {
        PrintText ( "x" , v.x , numdigits ) ;
        PrintText ( "y" , v.y , numdigits ) ;
        PrintText ( "z" , v.z , numdigits ) ;
        PrintText ( "m" , v.magnitude , numdigits ) ;
    }

    public void PrintText ( Quaternion q , int numdigits = 3 )
    {
        PrintText ( SOrientation.FromRotation ( q ) ) ;
    }

    public void PrintText ( SOrientation o , int numdigits = 3 )
    {
        PrintText ( "Yaw" , o.Yaw   , numdigits ) ;
        PrintText ( "Pitch" , o.Pitch , numdigits ) ;
        PrintText ( "Roll" , o.Roll  , numdigits ) ;
    }

    public void NewLine()
    {
        m_Text.text += "\n";
    }
}
