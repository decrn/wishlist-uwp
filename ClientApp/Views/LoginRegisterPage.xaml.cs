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
using ClientApp.ViewModels;

namespace ClientApp {

    public sealed partial class LoginRegisterPage : Page {

        private bool registerMode = false;

        public LoginRegisterPage() {
            this.InitializeComponent();
        }

        private void Login(object sender, RoutedEventArgs e) {

            if (registerMode) {
                FirstNameBox.Visibility = Visibility.Collapsed;
                LastNameBox.Visibility = Visibility.Collapsed;
                ConfirmPasswordBox.Visibility = Visibility.Collapsed;
                registerMode = false;
            } else {

                ErrorText.Visibility = Visibility.Collapsed;

                string email = EmailBox.Text;
                string password = PasswordBox.Password;
                JObject result = App.dataService.Login(email, password);

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
            }
        }

        private void Register(object sender, RoutedEventArgs e) {

            if (!registerMode) {
                FirstNameBox.Visibility = Visibility.Visible;
                LastNameBox.Visibility = Visibility.Visible;
                ConfirmPasswordBox.Visibility = Visibility.Visible;
                registerMode = true;
            } else {
                // TODO: can you make viewmodels in xaml?
                RegisterViewModel vm = new RegisterViewModel() {
                    FirstName = FirstNameBox.Text,
                    LastName = LastNameBox.Text,
                    Email = EmailBox.Text,
                    Password = PasswordBox.Password,
                    ConfirmPassword = ConfirmPasswordBox.Password
                };

                try {
                    dynamic result = App.dataService.Register(vm);

                    if (result.GetType() == typeof(string))
                        this.Frame.Navigate(typeof(MainPage));
                    else if (result is JArray) {
                        ErrorText.Text = result[0].errorMessage;
                        ErrorText.Visibility = Visibility.Visible;
                    } else if (result is JObject) {
                        ErrorText.Text = result.errors[0].description;
                        ErrorText.Visibility = Visibility.Visible;
                    }
                } catch (Exception ex) {
                    ErrorText.Text = "Something went wrong while registering";
                }

            }

        }

        private void LoginGoogle(object sender, RoutedEventArgs e) {
            // TODO: Implement Google Login
            Login(sender, e);
        }

        private void LoginFacebook(object sender, RoutedEventArgs e) {
            // TODO: Implement Facebook Login
            Login(sender, e);
        }

    }
}