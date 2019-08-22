using System;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class OnlineMapsElevationManagerBase : MonoBehaviour
{
    #region Variables

    protected static OnlineMapsElevationManagerBase _instance;

    public Action<string> OnElevationFails;
    public Action OnElevationRequested;
    public Action OnElevationUpdated;
    public Action<double, double, double, double> OnGetElevation;
    public Action<Vector2, Vector2> OnGetElevationLegacy;

    public OnlineMapsElevationBottomMode bottomMode = OnlineMapsElevationBottomMode.zero;
    public float scale = 1;
    public OnlineMapsRange zoomRange = new OnlineMapsRange(11, OnlineMaps.MAXZOOM);

    /// <summary>
    /// The minimum elevation value.
    /// </summary>
    [NonSerialized]
    public short minValue;

    /// <summary>
    /// The maximum elevation value.
    /// </summary>
    [NonSerialized]
    public short maxValue;

    public bool lockYScale = false;

    public float yScaleValue = 1;

    protected OnlineMapsVector2i elevationBufferPosition;

    #endregion

    #region Properties

    #region Static

    protected static OnlineMapsVector2i bufferPosition
    {
        get { return control.bufferPosition; }
    }

    protected static OnlineMapsControlBaseDynamicMesh control
    {
        get { return OnlineMapsControlBaseDynamicMesh.instance; }
    }

    public static OnlineMapsElevationManagerBase instance
    {
        get
        {
            return _instance;
        }
    }

    protected static OnlineMaps map
    {
        get { return OnlineMaps.instance; }
    }

    protected static Vector2 sizeInScene
    {
        get { return control.sizeInScene; }
    }

    public static bool isActive
    {
        get { return _instance != null && _instance.enabled; }
    }

    public static bool useElevation
    {
        get { return isActive && _instance.zoomRange.InRange(map.zoom) && _instance.hasData; }
    }

    #endregion

    public abstract bool hasData { get; }

    #endregion

    #region Methods

    public abstract void CancelCurrentElevationRequest();

    protected abstract float GetElevationValue(double x, double z, float yScale, double tlx, double tly, double brx, double bry);

    public static float GetElevation(double x, double z, float yScale, double tlx, double tly, double brx, double bry)
    {
        if (_instance != null) return _instance.GetElevationValue(x, z, yScale, tlx, tly, brx, bry);
        return 0;
    }

    public static float GetBestElevationYScale(double tlx, double tly, double brx, double bry)
    {
        if (_instance != null)
        {
            if (_instance.lockYScale) return _instance.yScaleValue;
        }
        else if (control == null)
        {
            return 0;
        }

        double dx, dy;
        OnlineMapsUtils.DistanceBetweenPoints(tlx, tly, brx, bry, out dx, out dy);
        dx = dx / sizeInScene.x * map.width;
        dy = dy / sizeInScene.y * map.height;
        return (float)Math.Min(map.width / dx, map.height / dy) / 1000;
    }

    public float GetMaxElevation(float yScale)
    {
        return hasData ? maxValue * yScale * scale: 0;
    }

    public float GetMinElevation(float yScale)
    {
        return hasData ? minValue * yScale * scale: 0;
    }

    public abstract void RequestNewElevationData();

    protected virtual void UpdateMinMax()
    {
        
    }

    #endregion
}