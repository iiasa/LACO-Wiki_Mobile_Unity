using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class TaskPanel : MonoBehaviour
{
    //public class CTimer
    //{
    //    private bool m_bRunning = false;
    //    private float m_Elapsed = 0.0f;
    //    private float m_BlinkingFrequency = 6.0f;
    //    public void Start() { m_bRunning = true; }
    //    public void Stop() { m_bRunning = false; m_Elapsed = 0.0f; }
    //    public void Update(float timestep) { if (m_bRunning) m_Elapsed += timestep; }
    //    public float Elapsed { get { return m_Elapsed; } }
    //    public bool BlinkStatus
    //    {
    //        get
    //        {
    //            if (!m_bRunning) return true;
    //            return 0 == (1 & Mathf.RoundToInt(m_Elapsed * m_BlinkingFrequency * 2.0f));
    //        }
    //    }
    //}

    public enum ETaskStatus
    {
        TS_Pending  ,
        TS_Focus    ,
        TS_Finished ,
    }

    public enum EColorStatus
    {
        CS_NotDefined ,
        CS_Pending  ,
        CS_Focus    ,
        CS_Finished ,
    }

    public float Heading ;
    public float Pitch   ;
    public string Name   ;

    private ETaskStatus TaskStatus = ETaskStatus.TS_Pending ;
    private EColorStatus ColorStatus = EColorStatus.CS_NotDefined ;
    private CTimer m_Timer = new CTimer() ;

    public bool IsFinished () { return ETaskStatus.TS_Finished == TaskStatus ; }
    public void SetFinished() { SetStatus ( ETaskStatus.TS_Finished ) ; }
    public float GetFocusTime() { return m_Timer.Elapsed ; }
    private static EColorStatus GetColorStatus ( ETaskStatus taskstatus , bool flashon )
    {
        switch ( taskstatus )
        {
            case ETaskStatus.TS_Pending  : return EColorStatus.CS_Pending;
            case ETaskStatus.TS_Focus    : return flashon ? EColorStatus.CS_Focus : EColorStatus.CS_Pending;
            case ETaskStatus.TS_Finished : return EColorStatus.CS_Finished;
            default : return EColorStatus.CS_Pending ;
        }
    }

    private void SetColor ( Color color )
    {
        GetComponent<Image>().color = color ;
    }

    private void SetColor ( EColorStatus cs )
    {
        if ( ColorStatus != cs )
        {
            SetColor ( CompassStrip.getPanelColor ( cs ) ) ;
            ColorStatus = cs ;
        }
    }

    private void SetColor ( ETaskStatus status )
    {
        SetColor ( GetColorStatus ( status , m_Timer.BlinkStatus ) ) ;
    }

    public void SetStatus ( ETaskStatus status , bool flashingfocus = false )
    {
        TaskStatus = status ;
        if ( ETaskStatus.TS_Focus == status )
        {
            m_Timer.Start ( flashingfocus ) ;
        }
        else
        {
            m_Timer.Stop () ;
        }
    }

    public float UpdateAngles ( float heading , float pitch )
    {
        float LabelFactorX = CompassStrip.LabelDistance / 90.0f ;
        float HeadingDist = Cyclic.getAngleDiff ( heading , Heading ) ;
        float PitchDist = pitch - Pitch ;
        float PosX = HeadingDist * LabelFactorX ;

        //float PosX = (Cyclic.getNearestAngle(heading, Heading) - heading) * LabelFactorX;
        float PosY = Div.getClamped ( -10.0f * ( PitchDist ) , -200.0f , 200.0f ) ;

        transform.localPosition = new Vector3 ( PosX , PosY , 0.0f ) ;
        return Mathf.Max ( Mathf.Abs ( HeadingDist ) , Mathf.Abs ( PitchDist ) ) ;
    }

    

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_Timer.Update ( Time.deltaTime ) ;
        SetColor ( TaskStatus ) ;
    }
}
