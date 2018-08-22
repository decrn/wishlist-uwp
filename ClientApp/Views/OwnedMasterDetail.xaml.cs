using ClientApp.Models;
using ClientApp.Views;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ClientApp.ViewModels;
using GalaSoft.MvvmLight.Messaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClientApp {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OwnedMasterDetail : Page {

        public ListMasterDetailViewModel Lists { get; set; }

        public OwnedMasterDetail() {
            InitializeComponent();
            Lists = new ListMasterDetailViewModel("Owned");

            /*MasterDetail.MapDetails = selected => { Lists.SelectedList = selected; }

            MasterDetail.MapDetails = selected => { return Lists.SelectedList; };
            // return the full detail list when opening detail panel

            MasterDetail.MapDetails = (selected) => {
                CurrentList = App.dataService.GetList(((List) selected).ListId);
                return CurrentList;
            };*/
        }

        private async void NewList(object sender, RoutedEventArgs e) {
            NewListDialog dialog = new NewListDialog();
            dialog.ShowAsync();
        }

        private void Save(object sender, RoutedEventArgs e) {
            // check if list is new (no id) and use save or create new route
        }

        private void Send(object sender, RoutedEventArgs e) {

        }

        private void Export(object sender, RoutedEventArgs e) {

        }

        private void Delete(object sender, RoutedEventArgs e) {

        }

        private void AddNewWish(object sender, RoutedEventArgs e) {

        }
    }
}
