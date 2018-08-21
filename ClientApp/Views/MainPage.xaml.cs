using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;


namespace ClientApp {

    public sealed partial class MainPage : Page {

        public static MainPage Current;

        public MainPage() {
            this.InitializeComponent();
            App.dataService.LoadingIndicator = LoadingControl;
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

        public void SetHeader(object header) {
            NavView.Header = header;
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args) {
            if (args.IsSettingsInvoked) {
                ContentFrame.Navigate(typeof(SettingsPage));
            } else {
                // find NavigationViewItem with Content that equals InvokedItem or in case content isnt a string, uses tag of parent
                NavigationViewItem item;
                if (args.InvokedItem.GetType() != typeof(string)) {
                    string tag = (string) ((FrameworkElement)((Panel) args.InvokedItem).Parent).Tag;
                    item = sender.MenuItems.OfType<NavigationViewItem>().First(x => x.Tag != null && (string)x.Tag == tag);
                } else {
                    item = sender.MenuItems.OfType<NavigationViewItem>().First(x => x.Content.GetType() == typeof(string) && (string)x.Content == (string)args.InvokedItem);
                }

                NavView_Navigate(item);
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

        private void OpenUserMenu(object sender, RoutedEventArgs e) {
            NavigationViewItem item = sender as NavigationViewItem;
            item.ContextFlyout.Placement = FlyoutPlacementMode.Right;
            item.ContextFlyout.ShowAt(item);
        }

        private void OpenNotificationsFlyout(object sender, RoutedEventArgs e) {
            NavigationViewItem item = sender as NavigationViewItem;
            item.ContextFlyout.Placement = FlyoutPlacementMode.Top;
            item.ContextFlyout.ShowAt(item);
        }

        private async void EditAccount(object sender, RoutedEventArgs e) {
            ContentFrame.Navigate(typeof(UserPage));
        }

        private async void Logout(object sender, RoutedEventArgs e) {
            App.dataService.Logout();
            App.GoToLogin();
        }

    }
}