using ClientApp.Helpers;
using ClientApp.Models;
using ClientApp.ViewModels;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ClientApp {
    public sealed partial class OwnedMasterDetail : PrintablePage {

        public ListMasterDetailViewModel Lists { get; set; }

        string[] CategorySuggestions = new string[] { "Toys", "Homewares", "Gadgets", "Vouchers", "Varia" };

        public OwnedMasterDetail() {
            Lists = new ListMasterDetailViewModel("Owned");
            this.InitializeComponent();
        }

        bool SkipNextSelectionChanged = false;
        private async void MasterDetail_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var simplelist = (ListViewModel)MasterDetail.SelectedItem;
            if (simplelist.ListId < 1) return;
            if (!SkipNextSelectionChanged) {
                var detailedlist = await Lists.GetDetailed(simplelist);

                SkipNextSelectionChanged = true;
                MasterDetail.SelectedItem = detailedlist;
            }
            SkipNextSelectionChanged = false;
        }

        public void SelectList(List list) {
            if (list == null) MasterDetail.SelectedItem = null;
            else MasterDetail.SelectedItem = Lists.Lists.First((l) => l.ListId == list.ListId);
        }

        private void NewList(object sender, RoutedEventArgs e) {
            ListViewModel vm = new ListViewModel(new List());
            MasterDetail.SelectedItem = vm;
            Lists.SelectedList = vm;
        }

        private void Save(object sender, RoutedEventArgs e) {
            // check if list is new (no id) and use 'save' or 'create new' route
            Lists.SelectedList.Save();
        }

        private void Send(object sender, RoutedEventArgs e) {
            if (!Lists.SelectedList.IsNew) {
                Lists.SelectedList.SendInvitations();
            } else {
                App.ShowMessage("You can't do this on a new list");
            }
        }

        private void Delete(object sender, RoutedEventArgs e) {
            if (!Lists.SelectedList.IsNew) {
                Lists.DeleteSelectedList();
            } else {
                App.ShowMessage("You can't do this on a new list");
            }
        }

        private void AddNewWish(object sender, RoutedEventArgs e) {
            Lists.SelectedList.Items.Add(new ItemViewModel());
        }

        private void AddInvite(object sender, RoutedEventArgs e) {
            Lists.SelectedList.InvitedUsers.Add(new UserViewModel());
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

        private void DeleteInvite(object sender, RoutedEventArgs e) {
            UserViewModel invite = (UserViewModel)((FrameworkElement)sender).Tag;
            Lists.SelectedList.RemoveInvite(invite);
        }

        private void DeleteItem(object sender, RoutedEventArgs e) {
            ItemViewModel item = (ItemViewModel)((FrameworkElement)sender).Tag;
            Lists.SelectedList.RemoveItem(item);
        }

        private void MoveItemUp(object sender, RoutedEventArgs e) {
            ItemViewModel item = (ItemViewModel)((FrameworkElement)sender).Tag;
            Lists.SelectedList.MoveItemUp(item);
        }

        private void MoveItemDown(object sender, RoutedEventArgs e) {
            ItemViewModel item = (ItemViewModel)((FrameworkElement)sender).Tag;
            Lists.SelectedList.MoveItemDown(item);
        }
    }
}
