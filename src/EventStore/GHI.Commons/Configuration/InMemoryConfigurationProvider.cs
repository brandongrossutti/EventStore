using System.Collections.Generic;

namespace GHI.Commons.Configuration
{
    public class InMemoryConfigurationProvider : IConfigurationProvider
    {
        private readonly IDictionary<string, string> _configurationValues = new Dictionary<string, string>();

        public string this[string settingName]
        {
            get
            {
                if (HasSetting(settingName))
                {
                    return _configurationValues[settingName];
                }
                else
                {
                    throw new SettingNotFoundException(settingName);
                }
            }
            set { _configurationValues[settingName] = value; }
        }

        public bool HasSetting(string settingName)
        {
            return _configurationValues.ContainsKey(settingName);
        }   
    }
}