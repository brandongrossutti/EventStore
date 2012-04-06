using System;

namespace GHI.Commons.Configuration
{
    public class SettingNotFoundException : Exception
    {
        public SettingNotFoundException(string settingName) : base(string.Format("There is not value for the setting '{0}'.", settingName)) {}
    }
}