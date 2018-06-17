using ClientApp.Models;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace ClientApp.Views {
    public sealed partial class NotificationFlyout : Flyout {

        private ObservableCollection<Notification> notifications = new ObservableCollection<Notification>();
        private string NotifText = "Notification";

        public NotificationFlyout() {
            this.InitializeComponent();
            foreach (Notification n in App.dataService.GetNotifications()) {
                notifications.Add(n);
            } 
        }

    }
}
