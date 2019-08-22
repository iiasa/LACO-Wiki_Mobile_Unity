using UnityEngine;

public abstract class OnlineMapsElevationManager<T>: OnlineMapsElevationManagerBase
    where T: OnlineMapsElevationManager<T>
{
    public new static T instance
    {
        get
        {
            return _instance as T;
        }
    }

    public static bool isEnabled
    {
        get { return instance.enabled; }
    }

    public override bool hasData
    {
        get
        {
            //TODO: Implement hasData
            return false;
        }
    }

    public override void CancelCurrentElevationRequest()
    {
        //TODO: Remove this method
    }

    protected override float GetElevationValue(double x, double z, float yScale, double tlx, double tly, double brx, double bry)
    {
        //TODO: Remove this method
        return 0;
    }

    protected virtual void OnDisable()
    {
        if (map != null) map.Redraw();
    }

    protected virtual void OnDestroy()
    {
        
    }

    protected virtual void OnEnable()
    {
        _instance = (T)this;
        if (map != null) map.Redraw();
    }

    public override void RequestNewElevationData()
    {
        //TODO: Remove this method
    }

    public virtual void SetElevationData(short[,] data)
    {
        
    }
}