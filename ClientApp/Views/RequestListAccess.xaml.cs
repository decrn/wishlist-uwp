using ClientApp.Models;
using Microsoft.Toolkit.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClientApp.Views {
    public sealed partial class RequestListAccess : ContentDialog {
        public RequestListAccess() {
            this.InitializeComponent();
            Email.Focus(FocusState.Programmatic);
        }

        private async void Request(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            string value = Email.Text;
            if (value.IsEmail()) {
                base.Hide();
                User user = await App.dataService.GetUser(value);
                UserDetailsDialog dialog = new UserDetailsDialog(user);
                dialog.ShowAsync();
            } else {
                if (args != null)
                    args.Cancel = true;
                Error.Text = "Not a valid email address";
            }
        }

        private void Cancel(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            base.Hide();
        }

        private void OnTextBoxKeyDown(object sender, KeyRoutedEventArgs e) {
            Error.Text = "";
            if (e.Key == Windows.System.VirtualKey.Enter)
                Request(this, null);
        }

        public void ShowAsync() {
            base.ShowAsync();
        }

    }
}
