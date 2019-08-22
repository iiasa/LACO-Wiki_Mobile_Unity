public abstract class OnlineMapsMarkerDrawerBase
{
    protected static OnlineMaps map
    {
        get { return OnlineMaps.instance; }
    }

    public virtual void Dispose()
    {
        
    }
}