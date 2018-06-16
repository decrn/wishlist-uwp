using System;
using System.Collections.Generic;

namespace ClientApp.DataService {

    public class SettingsService {

        private static Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public SettingsService() {
            _ThemeSetting = ParseEnum<Theme>(localSettings.Values["ThemeSetting"]);
        }


        // Theme Setting
        private Theme _ThemeSetting;

        public Theme GetThemeSetting() {
            return _ThemeSetting;
        }

        public void SetThemeSetting(object value) {
            _ThemeSetting = ParseEnum<Theme>(value);
            bool added = localSettings.Values.TryAdd("ThemeSetting", _ThemeSetting.ToString());
            if (!added) localSettings.Values["ThemeSetting"] = _ThemeSetting.ToString();
        }


        // Helpers

        public static T ParseEnum<T>(object value) {
            // return first enum value if value is null
            if (value == null) return ParseEnum<T>(0);

            return (T) Enum.Parse(typeof(T), value.ToString());
        }
    }

}

public enum Theme { System, Light, Dark }
