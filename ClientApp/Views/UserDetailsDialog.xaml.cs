using ClientApp.ViewModels;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClientApp.Views {
    public sealed partial class UserDetailsDialog : ContentDialog {

        UserViewModel User;

        public UserDetailsDialog(UserViewModel user) {
            this.User = user;
            this.InitializeComponent();
        }

        private void Request(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            App.dataService.RequestAccess(User.Email);
        }

        private void Close(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            Hide();
        }
    }
}
