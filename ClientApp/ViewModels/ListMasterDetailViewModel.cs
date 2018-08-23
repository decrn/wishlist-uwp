using ClientApp.Models;
using ClientApp.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ClientApp.ViewModels {
    public class ListMasterDetailViewModel : NotificationBase {

        public ListMasterDetailViewModel(string type) {
            Initialize(type);
        }

        private async void Initialize(string type) {

            if (type == "Owned") {
                List<List> lists = await App.dataService.GetOwnedLists();
                foreach (var list in lists) {
                    var nl = new ListViewModel(list);
                    _lists.Add(nl);
                }

            } else if (type == "Subscribed") {
                List<List> lists = await App.dataService.GetSubscribedLists();
                foreach (var list in lists) {
                    var nl = new ListViewModel(list);
                    _lists.Add(nl);
                }

            } else {
                throw new ArgumentException("This type of list is not supported. Use 'Owned' or 'Subscribed'.");
            }
        }

        private ObservableCollection<ListViewModel> _lists = new ObservableCollection<ListViewModel>();

        public ObservableCollection<ListViewModel> Lists {
            get => _lists;
            set => SetProperty(ref _lists, value);
        }

        public int SelectedListIndex {
            get => Lists.IndexOf(SelectedList);
        }

        private ListViewModel _selectedList;

        public ListViewModel SelectedList {
            get => _selectedList;
            set { SetProperty(ref _selectedList, value); }
        }

        public void AddList(List list) {
            Debug.WriteLine("Adding list");
            var lvm = new ListViewModel(list);
            Lists.Add(lvm);
            SelectedList = lvm;
        }

        public void DeleteSelectedList() {
            Debug.WriteLine("Removing List: ", SelectedList.Name);
            if (SelectedList != null) {
                var list = SelectedList;
                Lists.Remove(list);
            }
        }


        public async Task<ListViewModel> GetDetailed(ListViewModel selected) {
            List list = await App.dataService.GetList(selected.ListId);
            ListViewModel newvm = new ListViewModel(list);
            SelectedList = newvm;
            return newvm;
        }
    }
}