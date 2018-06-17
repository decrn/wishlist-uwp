using ClientApp.Models;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ClientApp.Views {
    public sealed partial class NotificationPart : RelativePanel {

        private ObservableCollection<Notification> notifications = new ObservableCollection<Notification>();

        public NotificationPart() {
            this.InitializeComponent();
            foreach (Notification n in App.dataService.GetNotifications()) {
                // TODO: probably needed to use a ViewModel for this
                notifications.Add(n);
            } 
        }

        public bool CannotActOnNotif(Notification notif) {
            if (notif.Type == NotificationType.JoinRequest)
                return true;
            return false;
        }

        public void MarkAll(object sender, RoutedEventArgs e) {
            // TODO: Implement Mark all notifications as read
        }

        public void Mark(object sender, RoutedEventArgs e) {
            // TODO: Implement Mark notification as read
        }

        public void Act(object sender, RoutedEventArgs e) {
            // TODO: Implement act on notification (open list or perhaps user?)
        }
    }
}
