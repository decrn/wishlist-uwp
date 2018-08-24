using ClientApp.DataService;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ClientApp {

    sealed partial class App : Application {

        public static IDataService dataService;
        public static SettingsService settingsService;

        public App() {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            settingsService = new SettingsService();

            if (settingsService.UseFakeDataService)
                dataService = new FakeService();
            else
                dataService = new RealService();

            if (settingsService.GetThemeSetting() == Theme.Light)
                this.RequestedTheme = ApplicationTheme.Light;
            else if (settingsService.GetThemeSetting() == Theme.Dark)
                this.RequestedTheme = ApplicationTheme.Dark;
        }


        // Lifecycle events

        protected override void OnLaunched(LaunchActivatedEventArgs e) {
            Frame rootFrame = Window.Current.Content as Frame;
            
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null) {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated) {}

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false) {
                if (rootFrame.Content == null) {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    if (dataService.IsLoggedIn())
                        rootFrame.Navigate(typeof(MainPage), e.Arguments);
                    else
                        rootFrame.Navigate(typeof(LoginRegisterPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }

            //draw into the title bar
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            //remove the solid-colored backgrounds behind the caption controls and system back button
            var viewTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.ButtonBackgroundColor = Colors.Transparent;
            viewTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            viewTitleBar.ButtonForegroundColor = (Color)Resources["SystemBaseHighColor"];

        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e) {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e) {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }


        // Background task

        protected async override void OnBackgroundActivated(BackgroundActivatedEventArgs args) {
            base.OnBackgroundActivated(args);
            IBackgroundTaskInstance taskInstance = args.TaskInstance;
            BackgroundTaskDeferral _deferral = taskInstance.GetDeferral();

            //IDataService dataService = new RealService();
            IDataService dataService = new FakeService();
            SettingsService settingsService = new SettingsService();

            if (dataService.IsLoggedIn() && settingsService.GetBackgroundTaskEnabledSetting()) {

                List<Models.Notification> notifications = await dataService.GetNotifications();

                notifications.FindAll(n => n.IsUnread).ForEach(n => {
                    Debug.WriteLine(DateTime.Now + " - " + n.Message);
                    string title = "Wishlist Notification";
                    switch (n.Type) {
                        case NotificationType.DeadlineReminder:
                            title = "Wishlist Deadline Reminder";
                            break;
                        case NotificationType.JoinRequest:
                            title = "Join Wishlist Request";
                            break;
                        case NotificationType.ListInvitation:
                            title = "Wishlist Invitation";
                            break;
                    }
                    ShowNotification(title, n.Message);
                });

            }

            _deferral.Complete();

        }


        // Methods

        public static void ShowMessage(string message) {
            var messageDialog = new MessageDialog(message);
            messageDialog.ShowAsync();
        }

        public static void Reload() {
            // cheeky way of reloading
            var _Frame = Window.Current.Content as Frame;
            _Frame.Navigate(typeof(MainPage));
            _Frame.GoBack();
        }

        // helper function because I'm sick of looking this up while testing
        public static void Wait(int milliseconds) {
            System.Threading.Tasks.Task.Delay(milliseconds).Wait();
        }

        public static void ShowNotification(string title, string message) {
            ToastContent toastContent = new ToastContent() {
                Visual = new ToastVisual() {
                    BindingGeneric = new ToastBindingGeneric() {
                        Children = {
                            new AdaptiveText() {
                                Text = title
                            },
                            new AdaptiveText() {
                                Text = message,
                                HintMinLines = 2
                            }
                        }
                    }
                }
            };

            var toast = new ToastNotification(toastContent.GetXml());
            toast.Group = "wishListNotifications";
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        public static void GoToLogin() {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(LoginRegisterPage));
        }

        public void SetApplicationTheme(ApplicationTheme theme) {
            this.RequestedTheme = theme;
        }
    }
}
