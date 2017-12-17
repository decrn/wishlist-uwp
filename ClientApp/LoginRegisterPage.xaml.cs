using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ClientApp.DataService;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace ClientApp {
    
    public sealed partial class LoginRegisterPage : Page {
        public LoginRegisterPage() {
            this.InitializeComponent();
        }

        private void Login(object sender, RoutedEventArgs e) {

            string email = EmailBox.Text;
            string password = PasswordBox.Password;

            try {
                dynamic result = RealService.Login(email,password);

                if (result.GetType() == typeof(string))
                    this.Frame.Navigate(typeof(SubscriptionDetailPage));
                else if (result is JArray)
                    ErrorText.Text = result[0].errorMessage;
                else if (result is JObject)
                    ErrorText.Text = result.errors[0].description;

            } catch (Exception ex) {
                ErrorText.Text = "Wrong Credentials";
            }
        }

        private void Register(object sender, RoutedEventArgs e) {

            string email = EmailBox.Text;
            string password = PasswordBox.Password;

            try {
                dynamic result = RealService.Register(email, password);

                if (result.GetType() == typeof(string))
                    this.Frame.Navigate(typeof(SubscriptionDetailPage));
                else if (result is JArray)
                    ErrorText.Text = result[0].errorMessage;
                else if (result is JObject)
                    ErrorText.Text = result.errors[0].description;

            } catch (Exception ex) {
                ErrorText.Text = "Something went wrong while registering";
            }
        }
    }
}
