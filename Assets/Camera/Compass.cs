using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct Div
{
    public static Quaternion GyroToUnity ( Quaternion q )
    {
        return new Quaternion ( q.x , q.y , -q.z , -q.w ) ;
    }
    public static float getRounded ( float v , int n )
    {
        float Fact = Mathf.Pow ( 10.0f , ( float ) n ) ;
        return Mathf.Round ( v * Fact ) / Fact ;
    }
    public static float getClamped ( float v , float minv , float maxv )
    {
        if (v < minv) v = minv;
        if (v > maxv) v = maxv;
        return v;
    }

    public static float sqr ( float v ) { return v * v ; }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////
// class DeviceInput
// provides euler angles Yaw , Pitch , Roll ;
// 20.10.2018 by Rudi Weinacker
/////////////////////////////////////////////////////////////////////////////////////////////////////

public class CTimer
{
    private bool m_bRunning = false ;
    private bool m_Flashing = false;
    private float m_Elapsed = 0.0f ;
    private float m_BlinkingFrequency = 6.0f ;
    public void Start ( bool flashing = false ) { m_bRunning = true ; m_Flashing = flashing ; }
    public void Stop () { m_bRunning = false ; m_Flashing = false ; m_Elapsed = 0.0f ; }
    public void Update ( float timestep) { if ( m_bRunning ) m_Elapsed += timestep ; }
    public float Elapsed { get { return m_Elapsed; } }
    public bool BlinkStatus
    {
        get
        {
            if ( !m_bRunning ) return true ;
            if ( !m_Flashing ) return true ;
            return 0 == ( 1 & Mathf.RoundToInt ( m_Elapsed * m_BlinkingFrequency * 2.0f ) ) ;
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////
// class DeviceInput
// provides euler angles Yaw , Pitch , Roll ;
// 20.10.2018 by Rudi Weinacker
/////////////////////////////////////////////////////////////////////////////////////////////////////

public class DeviceInput
{
    private static bool m_UseGyro = false ;
    private static bool m_UseGPS = false ;
    private static bool m_Inited = false;
    private static Vector3 m_Gravity ;
    private static readonly Vector3 VRad2Deg = new Vector3 ( Mathf.Rad2Deg , -Mathf.Rad2Deg , Mathf.Rad2Deg ) ;

    public static void Init()
    {
        if ( m_Inited ) return ;
        m_UseGyro = SystemInfo.supportsGyroscope ;
        m_UseGPS = SystemInfo.supportsLocationService && Input.location.isEnabledByUser ;

        Input.compass.enabled = true ;
        if ( m_UseGPS ) Input.location.Start() ;
        Input.gyro.enabled = m_UseGyro ;
        m_Inited = true ;
    }
    public static void Update ()
    {
        if ( m_UseGyro ) m_Gravity = Input.gyro.gravity ;
        else
        {
            Vector3 Acc = Input.acceleration ;
            if ( Acc.sqrMagnitude < 1.3f ) m_Gravity = Acc.normalized ;
        }
    }
    public static bool HasGyro
    {
        get
        {
            return m_UseGyro ;
        }
    }
    public static Vector3 Gravity
    {
        get
        {
            return m_Gravity ;
        }
    }
    public static Vector3 RotationRate
    {
        get
        {
            return Vector3.Scale ( Input.gyro.rotationRate, VRad2Deg ) ;
        }
    }
    public static float RotationSpeed
    {
        get
        {
            return RotationRate.magnitude ;
        }
    }
    public static float Heading
    {
        get
        {
            return m_UseGPS ? Input.compass.trueHeading : Input.compass.magneticHeading ;
        }
    }
    public static SOrientation AccMagOrientation
    {
        get
        {
            return SOrientation.FromHeadingGravity ( Heading , Gravity ) ;
        }
    }
    public static Quaternion GetRotationChange ( float timestep )
    {
        return Quaternion.Euler ( RotationRate * timestep ) ;
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////
// struct SOrientation
// provides euler angles Yaw , Pitch , Roll ;
// 20.10.2018 by Rudi Weinacker
/////////////////////////////////////////////////////////////////////////////////////////////////////

public struct SOrientation
{
    public float Yaw, Pitch, Roll;
    public Quaternion Rotation
    {
        get
        {
            return Quaternion.Euler(Pitch, Yaw, Roll);
        }
    }

    private static float GetRoll(Vector3 gravity)
    {
        return Mathf.Atan2(-gravity.x, -gravity.y) * Mathf.Rad2Deg;
    }

    private static float GetPitch(Vector3 gravity)
    {
        return Mathf.Asin(gravity.z) * Mathf.Rad2Deg;
    }

    public void Rotate(Quaternion q)
    {
        Quaternion Me = Rotation;
        SOrientation Rotated = FromRotation(Me * q);
        Yaw = Rotated.Yaw;
        Pitch = Rotated.Pitch;
        Roll = Rotated.Roll;
    }

    public static SOrientation FromHeadingGravity(float heading, Vector3 gravity)
    {
        SOrientation R = new SOrientation
        {
            Yaw = heading,
            Pitch = GetPitch(gravity),
            Roll = GetRoll(gravity),
        };
        return R;
    }

    public static SOrientation FromRotation(Quaternion q1)
    {
        Vector3 Euler = q1.eulerAngles;
        float pitch = Euler.x; if (pitch > 180.0f) pitch -= 360.0f;
        float roll = Euler.z; if (roll > 180.0f) roll -= 360.0f;
        SOrientation R = new SOrientation
        {
            Yaw = Euler.y,
            Pitch = pitch,
            Roll = roll,
        };
        return R;
    }
    public static SOrientation FromQuaternion1(Quaternion q1)
    {
        // http://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToEuler/
        float heading, attitude, bank;
        float test = q1.x * q1.y + q1.z * q1.w;
        if (test > 0.499)
        { // singularity at north pole
            heading = 2.0f * Mathf.Atan2(q1.x, q1.w) * Mathf.Rad2Deg;
            attitude = 90.0f;
            bank = 0.0f;
        }
        else if (test < -0.499)
        { // singularity at south pole
            heading = -2.0f * Mathf.Atan2(q1.x, q1.w) * Mathf.Rad2Deg;
            attitude = -90.0f;
            bank = 0.0f;
        }
        else
        {
            float sqx = q1.x * q1.x;
            float sqy = q1.y * q1.y;
            float sqz = q1.z * q1.z;
            heading = Mathf.Rad2Deg * Mathf.Atan2(2.0f * q1.y * q1.w - 2.0f * q1.x * q1.z, 1.0f - 2.0f * sqy - 2.0f * sqz);
            attitude = Mathf.Rad2Deg * Mathf.Asin(2.0f * test);
            bank = Mathf.Rad2Deg * Mathf.Atan2(2.0f * q1.x * q1.w - 2.0f * q1.y * q1.z, 1.0f - 2.0f * sqx - 2.0f * sqz);
        }
        if (heading < 0.0f) heading += 360.0f;
        SOrientation R = new SOrientation
        {
            Yaw = heading,
            Pitch = attitude,
            Roll = bank,
        };
        return R;
    }

}

/////////////////////////////////////////////////////////////////////////////////////////////////////
// class Cyclic
// helps for cyclic interpolations, e.g. angles
// 20.10.2018 by Rudi Weinacker
/////////////////////////////////////////////////////////////////////////////////////////////////////

public class Cyclic
{
    public static float getDiff ( float v1 , float v2 , float cycle )
    {
        float half = cycle * 0.5f;
        float Diff = v2 - v1;
        while ( Diff >  half ) Diff -= cycle ;
        while ( Diff < -half ) Diff += cycle ;
        return Diff ;
    }
    public static float getNearest ( float v1 , float v2 , float cycle )
    {
        return v1 + getDiff ( v1 , v2 , cycle ) ;
    }
    public static float getAngleDiff ( float v1 , float v2 )
    {
        return getDiff ( v1 , v2 , 360.0f ) ;
    }
    public static float getNearestAngle ( float v1 , float v2 )
    {
        return getNearest ( v1 , v2 , 360.0f ) ;
    }
    public static float GetWithin ( float v , float cycle )
    {
        while ( v <   0.0f ) v += cycle ;
        while ( v >= cycle ) v -= cycle ;
        return v ;
    }
    public static int getOffset ( float v , float cycle )
    {
        int n = 0 ;
        while ( cycle * n + v <   0.0f ) ++n ;
        while ( cycle * n + v >= cycle ) --n ;
        return n ;
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////
// class Interpol
// supports linear and cyclic interpolations for smoothing
// 20.10.2018 by Rudi Weinacker
/////////////////////////////////////////////////////////////////////////////////////////////////////

public class Interpol
{
    public static float Lerp ( float v1 , float v2 , float t )
    {
        return v1 + ( v2 - v1 ) * t ;
    }
    public static float LerpCyclic ( float v1 , float v2 , float t , float cycle )
    {
        return Lerp ( v1 , Cyclic.getNearest ( v1 , v2 , cycle ) , t ) ;
    }
    public static SOrientation Lerp ( SOrientation o1 , SOrientation o2 , float t )
    {
        SOrientation R = new SOrientation
        {
            Yaw   = LerpCyclic ( o1.Yaw , o2.Yaw , t , 360.0f ) ,
            Pitch = Lerp ( o1.Pitch , o2.Pitch , t ) ,
            Roll  = Lerp ( o1.Roll , o2.Roll , t ) ,
        };
        return R ;
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////
// class Smoothed
// provides smoothed variable changes driven by independend time steps
// supports cyclic variables as angles
// 20.10.2018 by Rudi Weinacker
/////////////////////////////////////////////////////////////////////////////////////////////////////

public class Smoothed
{
    private static int m_SmoothInt;
    private static double m_SmoothD;
    private static float m_Lerp;

    public static void SetSmoothingStrength ( int v )
    {
        if ( v <  0 ) v =  0 ;
        if ( v > 10 ) v = 10 ;
        m_SmoothInt = v ;
        m_SmoothD = System.Math.Tanh ( ( double ) ( m_SmoothInt - 10 ) ) * 0.5 + 0.5 ; // between 0 and 1
    }
    public static void SetTimeStep ( float dt )
    {
        m_Lerp = ( float ) ( 1.0 - System.Math.Pow ( m_SmoothD , ( double ) dt ) ) ;
    }

    private float m_SmoothVal = 0.0f ;
    private float m_RawVal = 0.0f ;

    public void init ( float val ) { m_SmoothVal = val ; m_RawVal = val ; }
    public float getSmoothedValue() { return m_SmoothVal ; }
    public float getRawValue() { return m_RawVal ; }
    public float Update ( float RawVal )
    {
        m_RawVal = RawVal ;
        m_SmoothVal = Interpol.Lerp ( m_SmoothVal , RawVal , m_Lerp ) ;
        return m_SmoothVal ;
    }
    public float UpdateCyclic ( float rawval , float cycle )
    {
        m_RawVal    = Cyclic.getNearest ( m_RawVal , rawval, cycle ) ;
        m_SmoothVal = Interpol.LerpCyclic ( m_SmoothVal , m_RawVal , m_Lerp , cycle ) ;
        int Offset = Cyclic.getOffset ( m_SmoothVal , cycle ) ;
        if ( 0 != Offset )
        {
            m_SmoothVal += cycle * Offset ;
            m_RawVal    += cycle * Offset ;
        }
        return m_SmoothVal ;
    }
    public float UpdateAngle ( float rawangle )
    {
        return UpdateCyclic ( rawangle , 360.0f ) ;
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////
// class SmoothedOrientation
// 22.10.2018 by Rudi Weinacker
/////////////////////////////////////////////////////////////////////////////////////////////////////

public class SmoothedOrientation
{
    private Smoothed m_Yaw = new Smoothed();
    private Smoothed m_Pitch = new Smoothed();
    private Smoothed m_Roll = new Smoothed();

    public SOrientation Update ( SOrientation o )
    {
        m_Yaw.UpdateAngle ( o.Yaw ) ;
        m_Pitch.Update ( o.Pitch ) ;
        m_Roll.Update ( o.Roll ) ;
        return Orientation ;
    }
    public float Yaw   { get { return m_Yaw   . getSmoothedValue() ; } }
    public float Pitch { get { return m_Pitch . getSmoothedValue() ; } }
    public float Roll  { get { return m_Roll  . getSmoothedValue() ; } }

    public SOrientation Orientation
    {
        get
        {
            SOrientation R = new SOrientation
            {
                Yaw = m_Yaw.getSmoothedValue(),
                Pitch = m_Pitch.getSmoothedValue(),
                Roll = m_Roll.getSmoothedValue(),
            };
            return R;
        }
    }

    public SOrientation GetRawOrientation ()
    {
        SOrientation R = new SOrientation
        {
            Yaw   = m_Yaw   . getRawValue () ,
            Pitch = m_Pitch . getRawValue () ,
            Roll  = m_Roll  . getRawValue () ,
        };
        return R;
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////
// class Fusion
// 22.10.2018 by Rudi Weinacker
/////////////////////////////////////////////////////////////////////////////////////////////////////

public class SensorFusion
{
    //private Quaternion m_Orientation_AccMag = Quaternion.identity ; // maybe useful for smoothing
    private Quaternion m_FusedRotation = Quaternion.identity ;
    public SOrientation Update ( SOrientation or_acc_mag , Quaternion gyro_rotation_step , float filter )
    {
        Quaternion Rotation_AccMag = or_acc_mag.Rotation ;
        m_FusedRotation = Quaternion.Lerp ( Rotation_AccMag , m_FusedRotation * gyro_rotation_step , filter ) ;
        return SOrientation.FromRotation ( m_FusedRotation ) ;
    }
    public SOrientation Orientation
    {
        get
        {
            return SOrientation.FromRotation ( m_FusedRotation ) ;
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////
;/*
public class Compass : MonoBehaviour {

    public Camera AR_Camera ;
    public float LabelDistance ;
    //public bool UseFusion ;
    public float Pitch ;
    public bool CheckTilt ;
    public float Accuracy;
    public float AccuracyDuration ;
    public bool Flash ;
    public bool m_SnapIfAccurate ;
    public Color ColorBad ;
    public Color ColorGood ;
    public Color ColorFinished ;
    public Text DebugText ;
    public Text StatusText ;
    //public AR_Quad m_AR_Quad;
    public SensorImage m_SensorImage;
    public Toggle m_ToggleFlash ;
    public Toggle m_ToggleSnap ;
    public Toggle m_ToggleSensorFusion ;
    public Button m_ButtonFinish ;

    private bool m_UseSensorFusion ;
    private int Smooth = 7 ;
    private readonly CTimer m_Timer = new CTimer () ;
    //private bool m_bUseFusion;

    private float Accuracity2;
    private float SnapHorizontally2;
    private MaterialPropertyBlock blockGood;
    private MaterialPropertyBlock blockBad;
    private MaterialPropertyBlock blockFinished;

    private readonly string[] LabelNames = new string[] { "North", "East", "South" , "West" } ;
    private readonly GameObject[] Labels = new GameObject [ 4 ] ;
    private enum ELabelStatus
    {
        LS_Pending ,
        LS_Ready ,
        LS_Finished
    }
    private readonly ELabelStatus[] LabelStatus = new ELabelStatus[4] { ELabelStatus.LS_Pending , ELabelStatus.LS_Pending , ELabelStatus.LS_Pending , ELabelStatus.LS_Pending } ;

    private SmoothedOrientation m_SmoothedOrientation = new SmoothedOrientation();
    private SensorFusion m_SensorFusion = new SensorFusion () ;
    private int m_NumFramesGood;
    private int m_IndexGoodLabel;

    public void Cancel ()
    {
        Application.Quit() ;
    }

    public void UseSensorFusion ( bool b )
    {
        m_UseSensorFusion = b ;
    }
    public void SnapIfAccurate ( bool snap )
    {
        m_SnapIfAccurate = snap ;
    }
    public void FlashIfAccurate ( bool flash )
    {
        Flash = flash ;
    }

    public void Reset ()
    {
        for ( int i = 0; i < 4; i++ )
        {
            LabelStatus[i] = ELabelStatus.LS_Pending;
        }
    }

    private Transform InsertInHierarchy ( GameObject go )
    {
        go.transform.SetParent ( transform.parent , false ) ;
        transform.SetParent ( go.transform , false ) ;
        return go.transform ;
    }

    // Use this for initialization

    void Start ()
    {
        // let screen stay on
        Screen.sleepTimeout = SleepTimeout.NeverSleep ;
        DeviceInput.Init() ;

        // sensor fusion needs gyroscope
        m_UseSensorFusion = DeviceInput.HasGyro;

        m_ToggleFlash.isOn = Flash;
        m_ToggleSnap.isOn = m_SnapIfAccurate;
        m_ToggleSensorFusion.isOn = m_UseSensorFusion ;
        if ( !DeviceInput.HasGyro ) m_ToggleSensorFusion.interactable  = false ;
        m_ButtonFinish.interactable = false ;

        // setup QualitySettings.vSyncCount
        QualitySettings.vSyncCount = 2;

        // setup Accuracity variables
        SnapHorizontally2 = Div.sqr ( Accuracy / 90.0f ) ;
        Accuracity2       = Div.sqr ( Accuracy         ) ;

        // MaterialPropertyBlocks for coloring signs
        blockGood = new MaterialPropertyBlock() ;
        blockBad = new MaterialPropertyBlock() ;
        blockFinished = new MaterialPropertyBlock() ;
        blockGood.SetColor("_Color", ColorGood);
        blockBad.SetColor("_Color", ColorBad);
        blockFinished.SetColor("_Color", ColorFinished);

        // setup smoothing class
        Smoothed.SetSmoothingStrength ( Smooth ) ;

        // make compass child of camera
        transform.SetParent ( AR_Camera.transform , false ) ;

        // make frame child of camera
        transform.Find("Frame").SetParent ( AR_Camera.transform , false ) ;

        // setup label objects
        for ( int i = 0 ; i < 4 ; i++ )
        {
            Labels[i] = transform.Find ( LabelNames[i] ).gameObject ;
        }

        m_IndexGoodLabel = -1 ;
        m_NumFramesGood = 0 ;
        //SnapIfAccurate ( true ) ;
        //m_DropDown_Cameras.AddOptions ( m_AR_Quad.getCameras () ) ;
    }

    private static float getClamped ( float v , float minv , float maxv )
    {
        if (v < minv) v = minv;
        if (v > maxv) v = maxv;
        return v;
    }

    private int GetSnappedIndex ( float heading , float pitch , float roll )
    {
        float Heading90 = heading / 90.0f;
        float headingR = Mathf.Round ( Heading90 ) ;
        int Index = Mathf.RoundToInt ( Heading90 ) & 3 ;
        if ( LabelStatus[Index] == ELabelStatus.LS_Finished ) return -1 ;

        if ( Div.sqr ( Cyclic.getDiff ( headingR , Heading90 , 4.0f ) ) > SnapHorizontally2 ) return -1 ;
        if ( Div.sqr ( Cyclic.getAngleDiff ( pitch , Pitch ) ) > Accuracity2 ) return -1 ;
        if ( Div.sqr ( Cyclic.getAngleDiff ( roll, 0.0f ) ) > Accuracity2 ) return -1 ;

        return Index;
    }

    private float GetSnappedHeading ( int headingindex )
    {
        return headingindex * 90.0f ;
    }

    private void ClearString ()
    {
        DebugText.text = "" ;
    }

    private void PrintText ( string pre , int v )
    {
        DebugText.text += pre;
        DebugText.text += v . ToString() ;
        DebugText.text += "\n" ;
    }

    private void PrintText ( string pre , float v , int numdigits = 3 )
    {
        DebugText.text += pre;
        DebugText.text += Div.getRounded ( v , numdigits ) . ToString() ;
        DebugText.text += "\n" ;
    }

    private void PrintText ( Vector3 v , int numdigits = 3 )
    {
        PrintText ( "x: " , v.x , numdigits ) ;
        PrintText ( "y: " , v.y , numdigits ) ;
        PrintText ( "z: " , v.z , numdigits ) ;
        PrintText ( "m: " , v.magnitude , numdigits ) ;
    }

    private void PrintText ( Quaternion q , int numdigits = 3 )
    {
        PrintText ( SOrientation.FromRotation ( q ) ) ;
    }

    private void PrintText ( SOrientation o , int numdigits = 3 )
    {
        PrintText ( "Yaw   " , o.Yaw   , numdigits ) ;
        PrintText ( "Pitch " , o.Pitch , numdigits ) ;
        PrintText ( "Roll  " , o.Roll  , numdigits ) ;
    }

    private MaterialPropertyBlock GetPropertyBlock ( ELabelStatus ls )
    {
        switch ( ls )
        {
            case ELabelStatus.LS_Pending  : return blockBad ;
            case ELabelStatus.LS_Ready    :
                {
                    bool FlashOn = m_Timer.BlinkStatus ;
                    return FlashOn || !Flash ? blockGood : blockBad ;
                }
            case ELabelStatus.LS_Finished : return blockFinished ;
            default : return blockBad ;
        }
    }

    private void SetColorOfLabel ( GameObject label , ELabelStatus ls )
    {
        MaterialPropertyBlock PropertyBlock = GetPropertyBlock ( ls ) ;
        MeshRenderer meshRenderer = label.GetComponent < MeshRenderer > () ;
        meshRenderer.SetPropertyBlock ( PropertyBlock ) ;
    }

    private void SetupLabelStatus ( int GoodIndex )
    {
        for ( int i = 0 ; i < 4 ; i++ )
        {
            if ( LabelStatus[i] != ELabelStatus.LS_Finished )
            {
                LabelStatus[i] = ( i == GoodIndex ? ELabelStatus.LS_Ready : ELabelStatus.LS_Pending ) ;
            }
        }
    }

    private bool IsTaskFinished ()
    {
        for ( int i = 0 ; i < 4 ; i++ )
        {
            if (LabelStatus[i] != ELabelStatus.LS_Finished) return false ;
        }
        return true ;
    }

    private void SetColourOfLabels ( int GoodIndex = -1 )
    {
        for ( int i = 0 ; i < 4 ; i++ )
        {
            SetColorOfLabel ( Labels[i] , LabelStatus[i] ) ;
        }
    }

    private void SetupLabelPos ( GameObject label , float heading , float pitch , float angle )
    {
        float LabelFactorX = LabelDistance / 90.0f;
        float PosX = ( Cyclic.getNearestAngle ( heading , angle ) - heading ) * LabelFactorX ;
        float PosY = getClamped ( -0.1f * ( pitch - Pitch ) , -2.0f , 2.0f ) ;

        label.transform.localPosition = new Vector3 ( PosX , PosY , 4.0f ) ;
    }

    void Update ()
    {
        if ( Input.GetKeyDown ( KeyCode.Escape ) ) Application.Quit () ;
        float DeltaTime = Time.deltaTime ;
        m_Timer.Update ( DeltaTime ) ;
        DeviceInput.Update();
        Smoothed.SetTimeStep ( DeltaTime ) ;
        ClearString () ;

        SOrientation AccMagOrientation = DeviceInput.AccMagOrientation ;
        Quaternion RotationChange = DeviceInput.GetRotationChange ( DeltaTime ) ;

        SOrientation Orientation ;

        if ( DeviceInput.HasGyro ) m_SensorFusion.Update ( AccMagOrientation , RotationChange , 0.9f ) ;
        m_SmoothedOrientation.Update ( AccMagOrientation ) ;

        if ( m_UseSensorFusion )
        {
            Orientation = m_SensorFusion.Orientation ;
        }
        else
        {
            Orientation = m_SmoothedOrientation.Orientation ;
        }

        float heading = Orientation.Yaw ;
        float pitch = Orientation.Pitch ;
        float tilt = Orientation.Roll ;

        if ( !CheckTilt ) tilt = 0.0f ;

        PrintText ( Orientation , 0 ) ;

        if ( IsTaskFinished () )
        {
            StatusText.text = "Aufgabe erledigt!" ;
            SetColourOfLabels () ;
        }
        else
        {
            int GoodIndex = GetSnappedIndex ( heading , pitch , tilt ) ;
            if ( GoodIndex >= 0 )
            {
                if ( GoodIndex != m_IndexGoodLabel ) m_Timer.Start() ;
                if ( m_SnapIfAccurate )
                {
                    heading = GetSnappedHeading ( GoodIndex ) ;
                    pitch = Pitch ;
                    tilt = 0.0f ;
                }
                StatusText.text = "Halten Sie das Handy kruz ruhig" ;
            }
            else
            {
                m_Timer.Stop () ;
                if ( Mathf.Abs ( tilt ) > 20 )
                {
                    StatusText.text = "!! Hier ist unten !!" ;
                }
                else
                {
                    StatusText.text = "Bringen Sie ein rotes Quadrat in den Rahmen" ;
                }
            }
            m_IndexGoodLabel = GoodIndex ;

            PrintText ( "Index" , GoodIndex ) ;
            SetupLabelStatus  ( GoodIndex ) ;
            SetColourOfLabels ( GoodIndex ) ;

            PrintText ( "NumFrames" , m_NumFramesGood ) ;

            //float DeviceRotationSpeed = SOrientation.DeviceRotationSpeed ;
            float DeviceRotationSpeed = 0.0f ;

            PrintText ( "RotSpeed" , DeviceRotationSpeed ) ;

            if ( m_Timer.Elapsed > AccuracyDuration && DeviceRotationSpeed < 5.0f )
            {
                string Filename = "Pic_" + LabelNames [ m_IndexGoodLabel ] ;
                // m_AR_Quad.TakePicture ( Filename , Flash ) ;
                m_SensorImage.TakePicture ( Filename, true ) ;
                LabelStatus [ GoodIndex ] = ELabelStatus.LS_Finished ;
                m_Timer.Stop () ;
            }
            m_ButtonFinish.interactable = IsTaskFinished () ;
        }

        SetupLabelPos ( Labels[0] , heading , pitch ,   0.0f ) ;
        SetupLabelPos ( Labels[1] , heading , pitch ,  90.0f ) ;
        SetupLabelPos ( Labels[2] , heading , pitch , 180.0f ) ;
        SetupLabelPos ( Labels[3] , heading , pitch , 270.0f ) ;

        float RotZ = getClamped ( - 2.0f * tilt , -35.0f , 35.0f ) ;
        transform.localRotation = Quaternion.Euler ( 0.0f , 0.0f , RotZ ) ;
    }
}

// */
