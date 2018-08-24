using ClientApp.Models;
using ClientApp.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ClientApp.ViewModels {
    public class ListViewModel : NotificationBase<List> {
        public List List;

        public ListViewModel(List list = null) : base(list) {
            this.List = list;
            if (list.Items != null) {
                foreach (var item in list.Items) {
                    AddItem(new ItemViewModel(item));
                }
            }

            if (list.InvitedUsers != null) {
                foreach (var invited in list.InvitedUsers) {
                    _invitedUsers.Add(new UserViewModel(invited));
                }
            }

            if (list.SubscribedUsers != null) {
                foreach (var subscribed in list.SubscribedUsers) {
                    _subscribedUsers.Add(new UserViewModel(subscribed));
                }
            }
        }

        public int ListId {
            get => This.ListId;
        }

        public string Name {
            get => This.Name;
            set { SetProperty(This.Name, value, () => This.Name = value); }
        }

        public string Description {
            get => This.Description;
            set { SetProperty(This.Description, value, () => This.Description = value); }
        }

        public DateTime Deadline {
            get => This.Deadline;
            set { SetProperty(This.Deadline, value, () => This.Deadline = value); }
        }

        public string FormattedDeadline {
            get => Deadline.ToString("yyyy-MM-dd");
        }

        public string FormattedFullDeadline {
            get => Deadline.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public string SubscribersTeaster {
            get {
                int amount = 3;
                string list = "";
                try {
                    list = SubscribedUsers.Take(amount).Select(u => u.FirstName).Aggregate((current, next) => current + ", " + next);
                } catch { }
                if (SubscribedUsers.Count > amount) list = list + "...";
                return list;
            }
        }

        public string ClaimedWishesCount => Items.Count(i => i.IsCompleted) + "/" + Items.Count;

        public bool IsNew => This.ListId == -1;

        public bool HasNoItems => Items.Count < 1;

        public bool HasNoSubscribers => SubscribedUsers.Count < 1;

        public bool HasNoInvited => InvitedUsers.Count < 1;

        // TODO: Fix UserViewModel creation
        private UserViewModel _ownerUser;
        public UserViewModel OwnerUser {
            get => new UserViewModel(This.OwnerUser);
            set => SetProperty(ref _ownerUser, value);
            // set { SetProperty(This.OwnerUser, value, () => This.OwnerUser = value); }
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

        // methods

        public void Save() {
            if (IsNew)
                App.dataService.NewList(List);
            else
                App.dataService.EditList(List);
        }

        public async void SendInvitations() {
            await App.dataService.SendInvitations(List);
            App.ShowMessage("Invitations sent!");
        }

        public async void UnsubscribeAsync() {
            await App.dataService.UnsubscribeFromList(List);
            App.Reload();
        }

        static DateTime ConvertFromDateTimeOffset(DateTimeOffset dateTime) {
            if (dateTime.Offset.Equals(TimeSpan.Zero))
                return dateTime.UtcDateTime;
            if (dateTime.Offset.Equals(TimeZoneInfo.Local.GetUtcOffset(dateTime.DateTime)))
                return DateTime.SpecifyKind(dateTime.DateTime, DateTimeKind.Local);
            return dateTime.DateTime;
        }

        // items

        private ObservableCollection<ItemViewModel> _items = new ObservableCollection<ItemViewModel>();

        public ObservableCollection<ItemViewModel> Items {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public string ItemCount => This.Items == null ? "" : This.Items.Count + " items";

        void Item_OnNotifyPropertyChanged(Object sender, PropertyChangedEventArgs e) {
            List.Update((ItemViewModel) sender);
        }

        public void AddItem(Item item) {
            var ivm = new ItemViewModel(item);
            ivm.PropertyChanged += Item_OnNotifyPropertyChanged;
            Items.Add(ivm);
            List.AddItem(ivm);
            //SelectedItemIndex = Items.IndexOf(item);
        }

        public IEnumerable<IGrouping<string, ItemViewModel>> GetGroupedItems() {
            //Group the data
            var groups = from c in Items
                         group c by c.Category;
            //Set the grouped data to CollectionViewSource
            return groups;
        }

        // subscribed users

        private ObservableCollection<UserViewModel> _subscribedUsers = new ObservableCollection<UserViewModel>();

        public ObservableCollection<UserViewModel> SubscribedUsers {
            get => _subscribedUsers;
            set => SetProperty(ref _subscribedUsers, value);
        }


        // invited users

        private ObservableCollection<UserViewModel> _invitedUsers = new ObservableCollection<UserViewModel>();

        public ObservableCollection<UserViewModel> InvitedUsers {
            get => _invitedUsers;
            set => SetProperty(ref _invitedUsers, value);
        }
    }
}