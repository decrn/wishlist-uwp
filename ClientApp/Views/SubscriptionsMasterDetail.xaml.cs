using ClientApp.Models;
using ClientApp.Views;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClientApp {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SubscriptionMasterDetail : Page
    {
        public ICollection<List> Lists { get; set; }
        private List CurrentList;

        public SubscriptionMasterDetail() {
            // TODO: Use Viewmodels?
            Lists = App.dataService.GetSubscribedLists();
            InitializeComponent();

            // return the full detail list when opening detail panel
            MasterDetail.MapDetails = (selected) => {
                CurrentList = App.dataService.GetList(((List)selected).ListId);
                return CurrentList;
            };
        }

        private async void RequestAccess(object sender, RoutedEventArgs e) {
            RequestListAccess dialog = new RequestListAccess();
            dialog.ShowAsync();
        }

        private void ViewSubscribers(object sender, TappedRoutedEventArgs e) {
            SubscribersList dialog = new SubscribersList(CurrentList.SubscribedUsers);
            dialog.ShowAsync();
        }

        public void SelectList(List list) {
            if (list == null) MasterDetail.SelectedItem = null;
            else MasterDetail.SelectedItem = Lists.First((l) => l.ListId == list.ListId);
        }
    }
}
