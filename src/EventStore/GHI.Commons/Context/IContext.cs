namespace GHI.Commons.Context
{
    public interface ILocalContext
    {
        object Get(string key);
        void Set(string key, object value);
    }
}
