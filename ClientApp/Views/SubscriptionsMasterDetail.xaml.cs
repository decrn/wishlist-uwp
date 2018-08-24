using ClientApp.Helpers;
using ClientApp.Models;
using ClientApp.ViewModels;
using ClientApp.Views;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Appointments;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace ClientApp {
    public sealed partial class SubscriptionMasterDetail : PrintablePage {

        public ListMasterDetailViewModel Lists { get; set; }

        public SubscriptionMasterDetail() {
            Lists = new ListMasterDetailViewModel("Subscribed");
            InitializeComponent();
        }

        bool SkipNextSelectionChanged = false;
        private async void MasterDetail_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (!SkipNextSelectionChanged) {
                var simplelist = (ListViewModel)MasterDetail.SelectedItem;
                var detailedlist = await Lists.GetDetailed(simplelist);

                SkipNextSelectionChanged = true;
                MasterDetail.SelectedItem = detailedlist;
                ItemGrouping.Source = detailedlist.GetGroupedItems();
            }
            SkipNextSelectionChanged = false;
        }

        // TODO: move these methods to the viewmodel or are they fine here?

        private void RequestAccess(object sender, RoutedEventArgs e) {
            RequestListAccess dialog = new RequestListAccess();
            dialog.ShowAsync();
        }

        private void Unsubscribe(object sender, RoutedEventArgs e) {
            Lists.SelectedList.UnsubscribeAsync();
        }

        private async void AddToCalendar(object sender, RoutedEventArgs e) {

            var appt = new Appointment();
            appt.Subject = Lists.SelectedList.Name;
            appt.Details = "Reminder to buy something";
            appt.StartTime = Lists.SelectedList.Deadline;

            var element = sender as FrameworkElement;
            GeneralTransform generalTransform = element.TransformToVisual((FrameworkElement) element.Parent);
            Rect rect = generalTransform.TransformBounds(new Rect(new Point(element.Margin.Left, element.Margin.Top), element.RenderSize));

            AppointmentManager.ShowAddAppointmentAsync(appt, rect, Windows.UI.Popups.Placement.Default);
        }

        private void OpenLink(object sender, RoutedEventArgs e) {
            // The URI to launch
            string url = (string)((Button)sender).Tag;

            if (url != null) {
                var promptOptions = new Windows.System.LauncherOptions();
                promptOptions.TreatAsUntrusted = true;
                Windows.System.Launcher.LaunchUriAsync(new System.Uri(url), promptOptions);
            }
        }

        private void ViewSubscribers(object sender, TappedRoutedEventArgs e) {
            List<UserViewModel> subs = Lists.SelectedList.SubscribedUsers.ToList();
            SubscribersList dialog = new SubscribersList(subs);
            dialog.ShowAsync();
        }

        public void SelectList(List list) {
            if (list == null) MasterDetail.SelectedItem = null;
            else MasterDetail.SelectedItem = Lists.Lists.FirstOrDefault((l) => l.ListId == list.ListId);
        }

        private void CheckItem(object sender, TappedRoutedEventArgs e) {
            ItemViewModel item = (ItemViewModel)((FrameworkElement)sender).Tag;
            item.Check();
        }
    }
}
