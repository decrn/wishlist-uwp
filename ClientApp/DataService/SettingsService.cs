using System;
using Windows.ApplicationModel.Background;

namespace ClientApp.DataService {

    public class SettingsService {

        private static Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public SettingsService() {

            var FirstBoot = localSettings.Values["FirstBoot"];
            if (FirstBoot == null) {
                RegisterBackgroundTaskAsync();
                localSettings.Values["FirstBoot"] = false;
            }

            _ThemeSetting = ParseEnum<Theme>(localSettings.Values["ThemeSetting"]);

            foreach (var task in BackgroundTaskRegistration.AllTasks) {
                if (task.Value.Name == "NotificationBackgroundTask") {
                    _BackgroundTaskEnabled = true;
                    break;
                }
            }

            var fake = localSettings.Values["UseFakeDataService"];
            if (fake != null) {
                UseFakeDataService = (bool) fake;
            }
        }


        // Theme Setting
        private Theme _ThemeSetting;

        public Theme GetThemeSetting() {
            return _ThemeSetting;
        }

        public void SetThemeSetting(object value) {
            _ThemeSetting = ParseEnum<Theme>(value);
            localSettings.Values["ThemeSetting"] = _ThemeSetting.ToString();
        }

        // Background Task

        private bool _BackgroundTaskEnabled = false;

        public bool GetBackgroundTaskEnabledSetting() {
            return _BackgroundTaskEnabled;
        }

        public void SetBackgroundTaskEnabledSetting(bool value) {
            _BackgroundTaskEnabled = value;

            if (value) {
                // Register backgroundtask
                var taskRegistered = false;
                foreach (var task in BackgroundTaskRegistration.AllTasks) {
                    if (task.Value.Name == "NotificationBackgroundTask") {
                        taskRegistered = true;
                        break;
                    }
                }

                if (!taskRegistered) {
                    RegisterBackgroundTaskAsync();
                }
            } else {
                // Unregister backgroundtask
                foreach (var task in BackgroundTaskRegistration.AllTasks) {
                    if (task.Value.Name == "NotificationBackgroundTask") {
                        task.Value.Unregister(true);
                    }
                }
            }
        }

        // Use fake data service

        private bool _UseFakeDataService = true;

        public bool UseFakeDataService {
            get { return _UseFakeDataService; }
            set { _UseFakeDataService = value; localSettings.Values["UseFakeDataService"] = value; }
        }

        // Helpers

        public static async void RegisterBackgroundTaskAsync() {
            var builder = new BackgroundTaskBuilder();
            builder.Name = "NotificationBackgroundTask";
            builder.SetTrigger(new TimeTrigger(60, false));
            builder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));
            var requestStatus = await BackgroundExecutionManager.RequestAccessAsync();
            if (requestStatus != BackgroundAccessStatus.AlwaysAllowed) {
                BackgroundTaskRegistration task = builder.Register();
            }
        }

        public static T ParseEnum<T>(object value) {
            // return first enum value if value is null
            if (value == null) return ParseEnum<T>(0);

            return (T)Enum.Parse(typeof(T), value.ToString());
        }
    }

}

public enum Theme { System, Light, Dark }
