using ClientApp.ViewModels.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ClientApp.ViewModels {
    public class ListMasterDetailViewModel : NotificationBase {

        public ListMasterDetailViewModel(string type) {
            if (type == "Owned") {
                Debug.WriteLine("GETTING all OWNED lists");
                foreach (var list in App.dataService.GetOwnedLists()) {
                    var nl = new ListViewModel(list);
                    _lists.Add(nl);
                }
            } else if (type == "Subscribed") {
                Debug.WriteLine("GETTING all SUBSCRIBED lists");
                foreach (var list in App.dataService.GetSubscribedLists()) {
                    var nl = new ListViewModel(list);
                    _lists.Add(nl);
                }
            }
            else {
                throw new ArgumentException("This type of list is not supported. Use 'Owned' or 'Subscribed'.");
            }

            
            foreach (var lvm in _lists) {
                Debug.WriteLine(lvm.Name);
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


        public ListViewModel GetDetailed(ListViewModel selected) {
            var vm = new ListViewModel(App.dataService.GetList(selected.ListId));
            SelectedDetail = vm;
            return vm;
        }
    }
}