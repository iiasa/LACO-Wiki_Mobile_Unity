public abstract class OnlineMapsGenericBase<T> where T : OnlineMapsGenericBase<T>
{
    protected static T _instance;

    public static T instance
    {
        get { return _instance; }
    }

    protected OnlineMapsGenericBase()
    {
        _instance = (T)this;
    }
}