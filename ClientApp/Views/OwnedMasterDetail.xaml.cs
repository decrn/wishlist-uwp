using ClientApp.Models;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClientApp {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OwnedMasterDetail : Page
    {

        public ICollection<List> Lists{ get; set; }

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
    }
}
