using ClientApp.Models;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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
            App.dataService.MarkAllNotificationsAsRead();
            foreach (Notification n in notifications) {
                n.IsUnread = false;
            }
        }

        public void Mark(object sender, RoutedEventArgs e) {
            Notification notif = (Notification) ((Button)sender).Tag;
            App.dataService.MarkNotificationAsRead(notif);
        }


        public void Act(object sender, TappedRoutedEventArgs e) {
            // TODO: Implement act on notification (open list or user? in popup)
            Notification notif = (Notification)((StackPanel)sender).Tag;
        }
    }
}
