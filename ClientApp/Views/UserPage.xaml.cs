using ClientApp.ViewModels;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ClientApp {

    public sealed partial class UserPage : Page {

        EditAccountViewModel User;
        ChangePasswordViewModel Passwords;

        public UserPage() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            User = new EditAccountViewModel();
            Passwords = new ChangePasswordViewModel();
        }

        private void Save(object sender, RoutedEventArgs e) {

            UserError.Visibility = Visibility.Collapsed;
            ValidationContext validationContext = new ValidationContext(User);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(User, validationContext, validationResults, true);

            if (isValid) {
                JObject result = App.dataService.EditAccount(User);

                if (result["success"].ToString() == "True") {
                    this.Frame.Navigate(typeof(UserPage));
                } else {
                    UserError.Text = result["errors"][0]["message"].ToString();
                    UserError.Visibility = Visibility.Visible;
                }

            } else {
                UserError.Text = validationResults[0].ErrorMessage;
                UserError.Visibility = Visibility.Visible;
            }
        }

        private void ChangePassword(object sender, RoutedEventArgs e) {

            PasswordsError.Visibility = Visibility.Collapsed;
            ValidationContext validationContext = new ValidationContext(Passwords);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(Passwords, validationContext, validationResults, true);

            if (isValid) {
                JObject result = App.dataService.ChangePassword(Passwords);

                if (result["success"].ToString() == "True") {
                    this.Frame.Navigate(typeof(UserPage));
                } else {
                    PasswordsError.Text = result["errors"][0]["message"].ToString();
                    PasswordsError.Visibility = Visibility.Visible;
                }

            } else {
                PasswordsError.Text = validationResults[0].ErrorMessage;
                PasswordsError.Visibility = Visibility.Visible;
            }
        }
    }

}
