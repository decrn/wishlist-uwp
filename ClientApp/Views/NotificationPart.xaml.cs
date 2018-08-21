using ClientApp.Models;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace ClientApp.Views {
    public sealed partial class NotificationPart : StackPanel {

        private ObservableCollection<Notification> Notifications = new ObservableCollection<Notification>();

        public NotificationPart() {
            this.InitializeComponent();
            this.Refresh(null, null);
        }

        public bool CannotActOnNotif(Notification notif) {
            if (notif.Type == NotificationType.JoinRequest)
                return true;
            return false;
        }

        public void MarkAll(object sender, RoutedEventArgs e) {
            App.dataService.MarkAllNotificationsAsRead();
            foreach (Notification n in Notifications) {
                n.IsUnread = false;
            }
        }

        public void Mark(object sender, RoutedEventArgs e) {
            Notification notif = (Notification) ((Button)sender).Tag;
            App.dataService.ExecuteOrMarkNotification(notif);
        }

        public void Refresh(object sender, RoutedEventArgs e) {
            Notifications.Clear();
            foreach (Notification n in App.dataService.GetNotifications()) {
                // TODO: probably needed to use a ViewModel for this
                Notifications.Add(n);
            }
        }

        public void Act(object sender, TappedRoutedEventArgs e) {
            // TODO: Implement act on notification (open list or user? in popup)
            Notification notif = (Notification)((StackPanel)sender).Tag;
        }
    }
}
