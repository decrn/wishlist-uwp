using ClientApp.ViewModels;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        private async void Request(object sender, RoutedEventArgs e) {

            ErrorText.Visibility = Visibility.Collapsed;
            ForgotPasswordViewModel vm = new ForgotPasswordViewModel() { Email = EmailBox.Text };

            ValidationContext validationContext = new ValidationContext(vm);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(vm, validationContext, validationResults, true);

            if (isValid) {
                JObject result = await App.dataService.ForgotPassword(vm);

                if (result["success"].ToString() == "True") {

                    // Really, I tried to do it with x:Bind but the ui wouldn't update
                    EmailBox.IsReadOnly = true;
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

            } else {
                ErrorText.Text = validationResults[0].ErrorMessage;
                ErrorText.Visibility = Visibility.Visible;
            }

        }

        private async void Reset(object sender, RoutedEventArgs e) {
            ErrorText.Visibility = Visibility.Collapsed;

            ResetPasswordViewModel vm = new ResetPasswordViewModel() {
                Email = EmailBox.Text,
                Password = PasswordBox.Password,
                ConfirmPassword = ConfirmPasswordBox.Password,
                Code = CodeBox.Text
            };

            ValidationContext validationContext = new ValidationContext(vm);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(vm, validationContext, validationResults, true);

            if (isValid) {
                JObject result = await App.dataService.ResetPassword(vm);

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

            } else {
                ErrorText.Text = validationResults[0].ErrorMessage;
                ErrorText.Visibility = Visibility.Visible;
            }

        }

        private void Close(object sender, RoutedEventArgs e) {
            Hide();
        }

        private void OnTextBoxKeyDown(object sender, KeyRoutedEventArgs e) {
            if (e.Key == Windows.System.VirtualKey.Enter) {
                if (resetMode)
                    Reset(this, null);
                else
                    Request(this, null);
            }
        }

    }
}
