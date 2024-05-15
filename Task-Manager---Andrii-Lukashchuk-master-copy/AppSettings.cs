using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Task_Manager___Andrii_Lukashchuk
{
    public static class AppSettings
    {
        private static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public static void SaveSetting(string key, object value)
        {
            // Store the setting in local settings
            localSettings.Values[key] = value;
        }

        public static T LoadSetting<T>(string key, T defaultValue)
        {
            // Try to get the setting from local settings
            if (localSettings.Values.TryGetValue(key, out object value))
            {
                // Return the stored value if found
                return (T)value;
            }
            else
            {
                // Return the default value if the setting is not found
                return defaultValue;
            }
        }

        public static bool SettingExists(string key)
        {
            return localSettings.Values.ContainsKey(key);
        }
    }
}
