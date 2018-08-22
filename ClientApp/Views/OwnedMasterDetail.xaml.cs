using ClientApp.Models;
using ClientApp.Views;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClientApp {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OwnedMasterDetail : Page {

        public ICollection<List> Lists{ get; set; }
        private List CurrentList;

        string[] CategorySuggestions = new string[] { "Toys", "Homewares", "Gadgets", "Vouchers", "Varia" };

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

        public void SelectList(List list) {
            if (list == null) MasterDetail.SelectedItem = null;
            else MasterDetail.SelectedItem = Lists.First((l) => l.ListId == list.ListId);
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

        private void AddInvite(object sender, RoutedEventArgs e) {

        }

        private void Category_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args) {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput) {
                sender.ItemsSource = CategorySuggestions;
            }
        }

        private void Category_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args) {
            sender.Text = args.SelectedItem.ToString();
        }

        private void Category_GotFocus(object sender, RoutedEventArgs e) {
            AutoSuggestBox box = (AutoSuggestBox)sender;
            box.ItemsSource = CategorySuggestions;
            box.IsSuggestionListOpen = true;
        }
    }
}
