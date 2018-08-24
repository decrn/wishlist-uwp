using ClientApp.Models;
using ClientApp.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace ClientApp.ViewModels {
    public class UserViewModel : NotificationBase<User> {
        User User;

        public UserViewModel(User user = null) : base(user) {

            if (user == null) {
                User = new User();
            } else {
                User = user;
            }
            _SelectedSubscriptionIndex = -1;
            _SelectedOwnedIndex = -1;

            foreach (var list in User.SubscribedLists) {
                var nl = new ListViewModel(list);
                nl.PropertyChanged += List_OnNotifyPropertyChanged;
                _Subscriptions.Add(nl);
            }

            foreach (var list in User.OwningLists) {
                var nl = new ListViewModel(list);
                nl.PropertyChanged += List_OnNotifyPropertyChanged;
                _Owned.Add(nl);
            }

        }

        public string Id {
            get => This.Id;
            set { SetProperty(This.Id, value, () => This.Id = value); }
        }

        public string FirstName {
            get => This.FirstName;
            set { SetProperty(This.FirstName, value, () => This.FirstName = value); }
        }

        public string LastName {
            get => This.LastName;
            set { SetProperty(This.LastName, value, () => This.LastName = value); }
        }

        public string Email {
            get => This.Email;
            set { SetProperty(This.Email, value, () => This.Email = value); }
        }

        public string FullName {
            get { return FirstName + " " + LastName; }
        }

        public string BracketedEmail {
            get { return "(" + Email + ")"; }
        }

        // Subscriptions

        List<ListViewModel> _Subscriptions = new List<ListViewModel>();
        public List<ListViewModel> Subscriptions {
            get { return _Subscriptions; }
            set { SetProperty(ref _Subscriptions, value); }
        }
        
        public ListViewModel SelectedSubscription {
            get { return (_SelectedSubscriptionIndex >= 0) ? _Subscriptions[_SelectedSubscriptionIndex] : null; }
        }

        // used explicitly by ListDetailPage to grab content for a specific list
        public List GetSubscriptionById(int id) {
            foreach (var sub in _Subscriptions) {
                if (sub.ListId == id) { return sub; }
            }
            throw new IndexOutOfRangeException();
        }

        // _SelectedIndex is used internally when navigating into lists in the details view, when navigating back
        // This index remembers where the User left off and helps them scroll through all subscribed lists to where they were
        int _SelectedSubscriptionIndex;

        public int SelectedSubscriptionIndex {
            get { return _SelectedSubscriptionIndex; }
            set {
                if (SetProperty(ref _SelectedSubscriptionIndex, value)) { RaisePropertyChanged(nameof(SelectedSubscription)); }
            }
        }

        public void AddSubscription() {
            var list = new ListViewModel();
            list.PropertyChanged += List_OnNotifyPropertyChanged;
            Subscriptions.Add(list);
            User.RegisterSubscription(list);
            SelectedSubscriptionIndex = Subscriptions.IndexOf(list);
        }

        public void RemoveSubscription() {
            if (SelectedSubscriptionIndex != -1) {
                var sub = Subscriptions[SelectedSubscriptionIndex];
                Subscriptions.RemoveAt(SelectedSubscriptionIndex);
                User.RemoveSubscription(sub);
            }
        }

        // Owned lists

        List<ListViewModel> _Owned = new List<ListViewModel>();
        public List<ListViewModel> Owned {
            get { return _Owned; }
            set { SetProperty(ref _Owned, value); }
        }

        public ListViewModel SelectedOwned {
            get { return (_SelectedOwnedIndex >= 0) ? _Owned[_SelectedOwnedIndex] : null; }
        }

        // _SelectedIndex is used internally when navigating into lists in the details view, when navigating back
        // This index remembers where the User left off and helps them scroll through all subscribed lists to where they were
        int _SelectedOwnedIndex;

        public int SelectedOwnedIndex {
            get { return _SelectedOwnedIndex; }
            set {
                if (SetProperty(ref _SelectedOwnedIndex, value)) { RaisePropertyChanged(nameof(SelectedOwned)); }
            }
        }

        public void AddOwned() {
            var list = new ListViewModel();
            list.PropertyChanged += List_OnNotifyPropertyChanged;
            Owned.Add(list);
            User.RegisterOwned(list);
            SelectedOwnedIndex = Owned.IndexOf(list);
        }

        public void RemoveOwned() {
            if (SelectedOwnedIndex != -1) {
                var list = Owned[SelectedOwnedIndex];
                Owned.RemoveAt(SelectedOwnedIndex);
                User.RemoveOwned(list);
            }
        }

        // Other

        void List_OnNotifyPropertyChanged(Object sender, PropertyChangedEventArgs e) {
            User.Update((ListViewModel)sender);
        }

        public async static Task<UserViewModel> FromEmail(string email) {
            return new UserViewModel(await App.dataService.GetUser(email));
        }

        public User ToUser() => new User {
            Id = Id,
            Email = Email,
            FirstName = FirstName,
            LastName = LastName
        };

    }
}
