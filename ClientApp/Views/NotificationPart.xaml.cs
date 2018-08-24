using ClientApp.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace ClientApp.Views {
    public sealed partial class NotificationPart : StackPanel {

        private ObservableCollection<Notification> Notifications = new ObservableCollection<Notification>();

        public MainPage MainPage { get; set; }

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
            this.Refresh(null,null);
        }

        public void Mark(object sender, RoutedEventArgs e) {
            Notification notif = (Notification) ((Button)sender).Tag;
            App.dataService.MarkNotification(notif);
            this.Refresh(null,null);
        }

        public async void Refresh(object sender, RoutedEventArgs e) {
            Notifications.Clear();
            List<Notification> notifs = await App.dataService.GetNotifications();
            foreach (Notification n in notifs) {
                // TODO: probably needed to use a ViewModel for this
                Notifications.Add(n);
            }
        }

        public void Act(object sender, TappedRoutedEventArgs e) {

            Notification notif = (Notification)((StackPanel)sender).Tag;

            if (notif.Type == NotificationType.ListJoinSuccess) {
                MainPage.GoToOwnedList(notif.SubjectList);

            } else if (notif.Type == NotificationType.JoinRequest) {
                MainPage.GoToOwnedList(null);

            } else if (notif.Type == NotificationType.ListInvitation) {
                App.dataService.ActOnNotification(notif);

            } else if (notif.Type == NotificationType.DeadlineReminder) {
                MainPage.GoToSubscribedList(notif.SubjectList);

            }

        }
    }
}
