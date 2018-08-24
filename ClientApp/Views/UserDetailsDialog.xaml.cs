using ClientApp.Models;
using ClientApp.ViewModels;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClientApp.Views {
    public sealed partial class UserDetailsDialog : ContentDialog {

        UserViewModel User;
        ICollection<List> Lists;

        public UserDetailsDialog(UserViewModel user) {
            Initialize(user);
            this.InitializeComponent();
        }

        private async void Initialize(UserViewModel user) {
            User temp = await App.dataService.GetUser(user.Id);
            User = new UserViewModel(temp);
            Lists = temp.OwningLists;
        }

        private void Request(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            App.dataService.RequestAccess(User.Email);
        }

        private void Close(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            Hide();
        }
    }
}
