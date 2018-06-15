using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
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
using ClientApp.Models;
using Newtonsoft.Json;


namespace ClientApp {
    
    public sealed partial class MainPage : Page {

        public static MainPage Current;

        public MainPage() {
            this.InitializeComponent();
            Current = this;
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e) {

            // set the initial SelectedItem 
            foreach (NavigationViewItemBase item in NavView.MenuItems) {
                if (item is NavigationViewItem && item.Tag.ToString() == "owned") {
                    NavView_Navigate((NavigationViewItem) item);
                    break;
                }
            }
            
            ContentFrame.Navigated += On_Navigated;

        }

        public void setHeader(object header) {
            NavView.Header = header;
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args) {
            if (args.IsSettingsInvoked) {
                ContentFrame.Navigate(typeof(SettingsPage));
            } else {
                // find NavigationViewItem with Content that equals InvokedItem
                var item = sender.MenuItems.OfType<NavigationViewItem>().First(x => (string)x.Content == (string)args.InvokedItem);
                NavView_Navigate(item as NavigationViewItem);
            }
        }

        private void NavView_Navigate(NavigationViewItem item) {
            switch (item.Tag) {
                case "owned":
                    ContentFrame.Navigate(typeof(OwnedMasterDetail));
                    break;

                case "subscription":
                    ContentFrame.Navigate(typeof(SubscriptionMasterDetail));
                    break;

                case "user":
                    item.ContextFlyout.ShowAt(item);
                    break;
            }
        }

        private void On_Navigated(object sender, NavigationEventArgs e) {
            NavView.IsBackEnabled = ContentFrame.CanGoBack;

            if (ContentFrame.SourcePageType == typeof(SettingsPage)) {
                NavView.SelectedItem = NavView.SettingsItem as NavigationViewItem;

            } else {

                // Define tag name for each possible Page filling the ContentFrame
                Dictionary<Type, string> lookup = new Dictionary<Type, string>() {
                    {typeof(OwnedMasterDetail), "owned"},
                    {typeof(SubscriptionMasterDetail), "subscription"},
                    {typeof(UserPage), "user"}
                };

                Type type = ContentFrame.SourcePageType;
                if (!lookup.Keys.Contains(type)) return;
                String stringTag = lookup[type];

                // set the new SelectedItem  
                foreach (NavigationViewItemBase item in NavView.MenuItems) {
                    if (item is NavigationViewItem && item.Tag.Equals(stringTag)) {
                        item.IsSelected = true;
                        break;
                    }
                }
            }
        }

        private async void EditAccount(object sender, RoutedEventArgs e) {
            ContentFrame.Navigate(typeof(UserPage));
        }

        private async void Logout(object sender, RoutedEventArgs e) {
            App.dataService.Logout();
            App.goToLogin();
        }
    }
}