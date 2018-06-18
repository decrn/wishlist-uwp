using Newtonsoft.Json.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace ClientApp.Views {

    public sealed partial class ForgotPasswordDialog : ContentDialog {

        private bool resetMode = false;

        public ForgotPasswordDialog() {
            this.InitializeComponent();
        }

        private void Request(object sender, RoutedEventArgs e) {

            ErrorText.Visibility = Visibility.Collapsed;
            JObject result = App.dataService.ForgotPassword(EmailBox.Text);

            if (result["success"].ToString() == "True") {

                // Really, I tried to do it with x:Bind but the ui wouldn't update
                CodeBox.Visibility = Visibility.Visible;
                CodeBox.Focus(FocusState.Programmatic);
                Border1.Visibility = Visibility.Visible;
                Message.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Visible;
                ConfirmPasswordBox.Visibility = Visibility.Visible;
                ResetButton.Visibility = Visibility.Visible;
                RequestButton.Visibility = Visibility.Collapsed;
                ErrorText.Visibility = Visibility.Collapsed;

            } else {
                var test = result["errors"];
                if (result["errors"] == null)
                    ErrorText.Text = "Error";
                else
                    ErrorText.Text = result["errors"][0]["message"].ToString();
                ErrorText.Visibility = Visibility.Visible;
            }
        }

        private void Reset(object sender, RoutedEventArgs e) {
            ErrorText.Visibility = Visibility.Collapsed;

            JObject result = App.dataService.ResetPassword(EmailBox.Text, PasswordBox.Password, ConfirmPasswordBox.Password, CodeBox.Text);

            if (result["success"].ToString() == "True") {
                Hide();
                var messageDialog = new MessageDialog("You can now login with your new password.");
                messageDialog.ShowAsync();

            } else {
                var test = result["errors"];
                if (result["errors"] == null)
                    ErrorText.Text = "Error";
                else
                    ErrorText.Text = result["errors"][0]["message"].ToString();
                ErrorText.Visibility = Visibility.Visible;
            }
        }

        private void Close(object sender, RoutedEventArgs e) {
            Hide();
        }

        private void OnTextBoxKeyDown(object sender, KeyRoutedEventArgs e) {
            if (e.Key == Windows.System.VirtualKey.Enter) {
                if (resetMode)
                    Reset(null, null);
                else
                    Request(null, null);
            }
        }

    }
}
