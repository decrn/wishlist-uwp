using ClientApp.Models;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClientApp {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OwnedMasterDetail : Page, INotifyPropertyChanged {

        public IList<List> Lists{ get; set; }

        public OwnedMasterDetail() {
            // TODO: Use Viewmodels?
            Lists = App.dataService.GetOwnedLists();
            InitializeComponent();
        }

        private void NewList(object sender, RoutedEventArgs e) {
            // TODO: Implement making new list
        }

        ContentDialog NewListDialog = new ContentDialog() {
            Title = "Create a new list",
            Content = new TextBox() { PlaceholderText = "Enter a list name" },
            PrimaryButtonText = "Create list",
            CloseButtonText = "Cancel"
        };

        private void Save(object sender, RoutedEventArgs e) {

        }

        private void Send(object sender, RoutedEventArgs e) {

        }

        private void Export(object sender, RoutedEventArgs e) {

        }

        private void Delete(object sender, RoutedEventArgs e) {

        }

        private void AddNewWish(object sender, RoutedEventArgs e) {

        }

        private void LoadDetails(object sender, SelectionChangedEventArgs e) {
            List selected = ((List) e.AddedItems[0]);
            List complete = App.dataService.GetList(selected.ListId);

            // TODO: Fix loading details when opening detail panel
            // find selected listitem and replace it with new one
            for (int i=0; i< Lists.Count; i++) {
                if (Lists[i].ListId == selected.ListId) {
                    Lists[i] = complete;
                    RaisePropertyChanged(nameof(Lists));
                }
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
