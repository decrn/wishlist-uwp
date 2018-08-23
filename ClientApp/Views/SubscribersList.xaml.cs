using ClientApp.Models;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClientApp.Views {
    public sealed partial class SubscribersList : ContentDialog {

        private List<User> Users;

        public SubscribersList(List<User> subscribedUsers) {
            this.Users = subscribedUsers;
            this.InitializeComponent();
        }

        private void Close(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            sender.Hide();
        }

        public void ShowAsync() {
            base.ShowAsync();
        }

        private async void OpenDetails(object sender, TappedRoutedEventArgs e) {
            string id = (string) ((StackPanel)sender).Tag;
            User user = await App.dataService.GetUser(id);
            UserDetailsDialog dialog = new UserDetailsDialog(user);
            Hide();
            dialog.ShowAsync();
        }
    }
}
