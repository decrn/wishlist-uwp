using ClientApp.Models;
using ClientApp.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ClientApp.ViewModels {
    public class ListViewModel : NotificationBase<List> {
        private readonly List _list;

        public ListViewModel(List list = null) : base(list) {
            _list = list;
        }

        public int ListId {
            get => This.ListId;
            set { SetProperty(This.ListId, value, () => This.ListId = value); }
        }

        public string Name {
            get => This.Name;
            set { SetProperty(This.Name, value, () => This.Name = value); }
        }

        public User OwnerUser {
            get => This.OwnerUser;
            set { SetProperty(This.OwnerUser, value, () => This.OwnerUser = value); }
        }

        public string Color {
            get => This.Color;
            set { SetProperty(This.Color, value, () => This.Color = value); }
        }

        public string ItemCount => This.Items == null ? "" : This.Items.Count + " items";

        private ObservableCollection<ItemViewModel> _items = new ObservableCollection<ItemViewModel>();

        public ObservableCollection<ItemViewModel> Items {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        private int _selectedItemIndex;

        public int SelectedItemIndex {
            get => _selectedItemIndex;
            set {
                if (SetProperty(ref _selectedItemIndex, value)) RaisePropertyChanged(nameof(SelectedItem));
            }
        }

        public ItemViewModel SelectedItem => _selectedItemIndex >= 0 ? _items[_selectedItemIndex] : null;

        public void AddItem() {
            var item = new ItemViewModel();
            item.PropertyChanged += Item_OnNotifyPropertyChanged;
            Items.Add(item);
            _list.AddItem(item);
            SelectedItemIndex = Items.IndexOf(item);
        }

        public void DeleteItem() {
            if (SelectedItemIndex != -1) {
                var item = Items[SelectedItemIndex];
                Items.RemoveAt(SelectedItemIndex);
                _list.RemoveItem(item);
            }
        }

        void Item_OnNotifyPropertyChanged(Object sender, PropertyChangedEventArgs e) {
            _list.Update((ItemViewModel) sender);
        }
    }
}