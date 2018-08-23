using ClientApp.Models;
using ClientApp.ViewModels.ViewModels;
using System;
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
            set { SetProperty(This.ListId, value, () => This.ListId = value); }
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
            get { return Deadline.ToString("yyyy-MM-dd"); }
        }

        public string FormattedFullDeadline {
            get { return Deadline.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

        public string SubscribersTeaster {
            get {
                int amount = 3;
                string list = SubscribedUsers.Take(amount).Select(u => u.FirstName).Aggregate((current, next) => current + ", " + next);
                if (SubscribedUsers.Count > amount) list = list + "...";
                return list;
            }
        }

        public string ClaimedWishesCount {
            get { return Items.Where(i => i.IsCompleted).Count() + "/" + Items.Count; }
        }

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

        public void Save() {

        }

        static DateTime ConvertFromDateTimeOffset(DateTimeOffset dateTime) {
            if (dateTime.Offset.Equals(TimeSpan.Zero))
                return dateTime.UtcDateTime;
            else if (dateTime.Offset.Equals(TimeZoneInfo.Local.GetUtcOffset(dateTime.DateTime)))
                return DateTime.SpecifyKind(dateTime.DateTime, DateTimeKind.Local);
            else
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