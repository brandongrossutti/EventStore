namespace GHI.Commons.Configuration
{
    public interface IConfigurationProvider
    {
        string this[string settingName] { get; }
        bool HasSetting(string settingName);
    }
}
