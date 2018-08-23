using ClientApp.ViewModels;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClientApp.Views {
    public sealed partial class SubscribersList : ContentDialog {

        private List<UserViewModel> Users;

        public SubscribersList(List<UserViewModel> subscribedUsers) {
            Users = subscribedUsers;
            this.InitializeComponent();
        }

        private void Close(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            sender.Hide();
        }

        public void ShowAsync() {
            base.ShowAsync();
        }

        private async void OpenDetails(object sender, TappedRoutedEventArgs e) {
            UserViewModel vm = (UserViewModel) ((StackPanel)sender).Tag;
            UserDetailsDialog dialog = new UserDetailsDialog(vm);
            Hide();
            dialog.ShowAsync();
        }
    }
}
