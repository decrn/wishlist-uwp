using ClientApp.Models;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClientApp {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OwnedMasterDetail : Page {

        public IList<List> Lists{ get; set; }
        private List CurrentList;

        public OwnedMasterDetail() {
            // TODO: Use Viewmodels?
            Lists = App.dataService.GetOwnedLists();
            InitializeComponent();

            // return the full detail list when opening detail panel
            MasterDetail.MapDetails = (selected) => {
                CurrentList = App.dataService.GetList(((List) selected).ListId);
                return CurrentList;
            };
        }

        private async void NewList(object sender, RoutedEventArgs e) {

            ContentDialog NewListDialog = new ContentDialog() {
                Title = "Create a new list",
                Content = new TextBox() { PlaceholderText = "Enter a list name" },
                PrimaryButtonText = "Create",
                CloseButtonText = "Cancel"
            };

            NewListDialog.PrimaryButtonClick += (x,y) => {
                string newname = ((TextBox)x.Content).Text;
                MasterDetail.SelectedItem = new List() { Name = newname };
            };

            NewListDialog.ShowAsync();
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
