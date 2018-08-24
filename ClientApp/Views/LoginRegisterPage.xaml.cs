using ClientApp.ViewModels;
using ClientApp.Views;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace ClientApp {

    public sealed partial class LoginRegisterPage : Page {

        private bool registerMode = false;

        public LoginRegisterPage() {
            this.InitializeComponent();
            App.dataService.LoadingIndicator = LoadingControl;
        }

        private async void Login(object sender, RoutedEventArgs e) {

            if (registerMode) {
                ToggleRegisterMode();
            } else {

                ErrorText.Visibility = Visibility.Collapsed;

                LoginViewModel vm = new LoginViewModel() {
                    Email = EmailBox.Text,
                    Password = PasswordBox.Password
                };

                ValidationContext validationContext = new ValidationContext(vm);
                List<ValidationResult> validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(vm, validationContext, validationResults, true);

                if (isValid) {
                    JObject result = await App.dataService.Login(vm);

                    if (result["success"].ToString() == "True") {
                        this.Frame.Navigate(typeof(MainPage));
                    } else {
                        var test = result["errors"];
                        if (result["errors"] == null)
                            ErrorText.Text = "Wrong credentials";
                        else
                            ErrorText.Text = result["errors"][0]["message"].ToString();
                        ErrorText.Visibility = Visibility.Visible;
                    }

                } else {
                    ErrorText.Text = validationResults[0].ErrorMessage;
                    ErrorText.Visibility = Visibility.Visible;
                }

            }
        }

        private async void Register(object sender, RoutedEventArgs e) {

            if (!registerMode) {
                ToggleRegisterMode();
            } else {

                ErrorText.Visibility = Visibility.Collapsed;

                RegisterViewModel vm = new RegisterViewModel() {
                    FirstName = FirstNameBox.Text,
                    LastName = LastNameBox.Text,
                    Email = EmailBox.Text,
                    Password = PasswordBox.Password,
                    ConfirmPassword = ConfirmPasswordBox.Password
                };

                ValidationContext validationContext = new ValidationContext(vm);
                List<ValidationResult> validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(vm, validationContext, validationResults, true);

                if (isValid) {

                    JObject result = await App.dataService.Register(vm);

                    if (result["success"].ToString() == "True") {
                        this.Frame.Navigate(typeof(MainPage));
                    } else {
                        var test = result["errors"];
                        if (result["errors"] == null)
                            ErrorText.Text = "Wrong credentials";
                        else
                            ErrorText.Text = result["errors"][0]["message"].ToString();
                        ErrorText.Visibility = Visibility.Visible;
                    }

                } else {
                    ErrorText.Text = validationResults[0].ErrorMessage;
                    ErrorText.Visibility = Visibility.Visible;
                }

            }

        }

        private void LoginGoogle(object sender, RoutedEventArgs e) {
            // TODO: Implement Google Login
            var messageDialog = new MessageDialog("This functionality hasn't been added yet.");
            messageDialog.ShowAsync();
        }

        private void LoginFacebook(object sender, RoutedEventArgs e) {
            // TODO: Implement Facebook Login
            var messageDialog = new MessageDialog("This functionality hasn't been added yet.");
            messageDialog.ShowAsync();
        }

        private void OpenForgotPassword(object sender, RoutedEventArgs e) {
            ContentDialog dialog = new ForgotPasswordDialog();
            dialog.ShowAsync();
        }

        private void OnTextBoxKeyDown(object sender, KeyRoutedEventArgs e) {
            if (e.Key == Windows.System.VirtualKey.Enter) {
                if (registerMode)
                    Register(null,null);
                else
                    Login(null, null);
            }
        }

        private void ToggleRegisterMode() {
            registerMode = !registerMode;
            FirstNameBox.Visibility = registerMode ? Visibility.Visible : Visibility.Collapsed;
            LastNameBox.Visibility = registerMode ? Visibility.Visible : Visibility.Collapsed;
            ConfirmPasswordBox.Visibility = registerMode ? Visibility.Visible : Visibility.Collapsed;
            LoginBackIcon.Visibility = registerMode ? Visibility.Visible : Visibility.Collapsed;
            ErrorText.Visibility = Visibility.Collapsed;
        }

    }
}