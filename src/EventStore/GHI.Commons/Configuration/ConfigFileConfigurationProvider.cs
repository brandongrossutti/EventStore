using System.Configuration;

namespace GHI.Commons.Configuration
{
    public class ConfigFileConfigurationProvider : IConfigurationProvider
    {
        public string this[string settingName]
        {
            get
            {
                string settingValue = ConfigurationManager.AppSettings[settingName];
                if (settingValue == null)
                {
                    throw new SettingNotFoundException(settingName);
                }
                return settingValue;
            }
        }

        public bool HasSetting(string settingName)
        {
            return !string.IsNullOrEmpty(ConfigurationManager.AppSettings[settingName]);
        }
    }
}