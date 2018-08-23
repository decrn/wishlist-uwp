using ClientApp.Models;
using ClientApp.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

                List<List> lists = await App.dataService.GetOwnedLists();

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

        public ListViewModel SelectedDetail;

        private ListViewModel _selectedList;

        public ListViewModel SelectedList {
            get => _selectedList;
            set {
                SetProperty(ref _selectedList, value);
            }
        }

        public void AddList() {
            var list = new ListViewModel();
            Lists.Add(list);
            //SelectedListIndex = Lists.IndexOf(list);
        }

        /*public void DeleteItem() {
            if (SelectedItemIndex != -1) {
                var item = Lists[SelectedItemIndex];
                Lists.RemoveAt(SelectedItemIndex);
                list.RemoveItem(item);
            }
        }*/


        public async Task<ListViewModel> GetDetailed(ListViewModel selected) {
            List list = await App.dataService.GetList(selected.ListId);
            var vm = new ListViewModel(list);
            SelectedDetail = vm;
            return vm;
        }
    }
}