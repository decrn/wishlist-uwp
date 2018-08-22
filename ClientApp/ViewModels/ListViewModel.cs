using ClientApp.Models;
using ClientApp.ViewModels.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace ClientApp.ViewModels {
    public class ListViewModel : NotificationBase<List> {
        public List List;

        public ListViewModel(List list = null) : base(list) {
            this.List = list;
        }

        public int ListId {
            get => This.ListId;
            set { SetProperty(This.ListId, value, () => This.ListId = value); }
        }

        public string Name {
            get
            {
                Debug.WriteLine("Getting List.Name");
                return This.Name;
            }
            set
            {
                Debug.WriteLine("Setting List.Name");
                SetProperty(This.Name, value, () => This.Name = value);
            }
        }

        public string Description {
            get => This.Description;
            set { SetProperty(This.Description, value, () => This.Description = value); }
        }

        public DateTime Deadline {
            get => This.Deadline;
            set { SetProperty(This.Deadline, value, () => This.Deadline = value); }
        }

        public User OwnerUser {
            get => This.OwnerUser;
            set { SetProperty(This.OwnerUser, value, () => This.OwnerUser = value); }
        }

        public bool IsHidden {
            get => This.IsHidden;
            set { SetProperty(This.IsHidden, value, () => This.IsHidden = value); }
        }

        public string Color {
            get => This.Color;
            set { SetProperty(This.Color, value, () => This.Color = value); }
        }

        public string Icon {
            get => This.Icon;
            set { SetProperty(This.Icon, value, () => This.Icon = value); }
        }

        // items

        private ObservableCollection<ItemViewModel> _items = new ObservableCollection<ItemViewModel>();

        public ObservableCollection<ItemViewModel> Items {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public string ItemCount => This.Items == null ? "" : This.Items.Count + " items";

        //private int _selectedItemIndex;

        //public int SelectedItemIndex {
        //    get => _selectedItemIndex;
        //    set {
        //        if (SetProperty(ref _selectedItemIndex, value)) RaisePropertyChanged(nameof(SelectedItem));
        //    }
        //}

        //public ItemViewModel SelectedItem => _selectedItemIndex >= 0 ? _items[_selectedItemIndex] : null;

        //public void AddItem() {
        //    var item = new ItemViewModel();
        //    item.PropertyChanged += Item_OnNotifyPropertyChanged;
        //    Items.Add(item);
        //    list.AddItem(item);
        //    SelectedItemIndex = Items.IndexOf(item);
        //}

        //public void DeleteItem() {
        //    if (SelectedItemIndex != -1) {
        //        var item = Items[SelectedItemIndex];
        //        Items.RemoveAt(SelectedItemIndex);
        //        list.RemoveItem(item);
        //    }
        //}

        void Item_OnNotifyPropertyChanged(Object sender, PropertyChangedEventArgs e) {
            List.Update((ItemViewModel) sender);
        }

        // subscribed users

        private ObservableCollection<UserViewModel> _subscribedUsers = new ObservableCollection<UserViewModel>();

        public ObservableCollection<UserViewModel> SubscribedUsers {
            get => _subscribedUsers;
            set => SetProperty(ref _subscribedUsers, value);
        }
    }
}