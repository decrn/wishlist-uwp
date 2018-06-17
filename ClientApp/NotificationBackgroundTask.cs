using ClientApp.DataService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace ClientApp {

    public sealed class NotificationBackgroundTask : IBackgroundTask {

        public void Run(IBackgroundTaskInstance taskInstance) {

            BackgroundTaskDeferral _deferral = taskInstance.GetDeferral();

            //IDataService dataService = new RealService();
            IDataService dataService = new FakeService();
            SettingsService settingsService = new SettingsService();

            if (dataService.IsLoggedIn() && settingsService.GetBackgroundTaskEnabledSetting()) {

                List<Models.Notification> notifications = dataService.GetNotifications();

                notifications.FindAll(n => n.IsUnread).ForEach(n => {
                    Debug.WriteLine(DateTime.Now +" - "+ n.Message);
                    ShowNotification("Title", n.Message);
                });

            }

            _deferral.Complete();
        }

        private void ShowNotification(string title, string message) {
            ToastNotifier ToastNotifier = ToastNotificationManager.CreateToastNotifier();
            Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            Windows.Data.Xml.Dom.XmlNodeList toastNodeList = toastXml.GetElementsByTagName("text");
            toastNodeList.Item(0).AppendChild(toastXml.CreateTextNode(title));
            toastNodeList.Item(1).AppendChild(toastXml.CreateTextNode(message));
            Windows.Data.Xml.Dom.IXmlNode toastNode = toastXml.SelectSingleNode("/toast");

            ToastNotification toast = new ToastNotification(toastXml);
            toast.ExpirationTime = DateTime.Now.AddSeconds(4);
            ToastNotifier.Show(toast);
        }


    }
}
