using Microsoft.Toolkit.Extensions;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClientApp.Views {
    public sealed partial class RequestListAccess : ContentDialog {
        public RequestListAccess() {
            this.InitializeComponent();
        }

        private void Request(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            string value = Email.Text;
            if (value.IsEmail()) {
                App.dataService.RequestAccess(value);
                sender.Hide();
            } else {
                args.Cancel = true;
                Error.Text = "Not a valid email address";
            }
        }

        private void Cancel(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            sender.Hide();
        }

        public void ShowAsync() {
            base.ShowAsync();
        }

        private void ErrorReset(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e) {
            Error.Text = "";

        }
    }
}
