using ClientApp.Models;
using ClientApp.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.ViewModels {
    public class UserViewModel : NotificationBase {

        User user;

        public UserViewModel() {

            user = new User();
            _SelectedIndex = -1;

            foreach (var list in user.OwningLists) {

                var nl = new ListViewModel(list);

                nl.PropertyChanged += List_OnNotifyPropertyChanged;

                _Lists.Add(nl);
            }

        }

        ObservableCollection<ListViewModel> _Lists = new ObservableCollection<ListViewModel>();
        public ObservableCollection<ListViewModel> Lists {
            get { return _Lists; }
            set { SetProperty(ref _Lists, value); }
        }
        public ListViewModel SelectedList {
            get { return (_SelectedIndex >= 0) ? _Lists[_SelectedIndex] : null; }
        }

        int _SelectedIndex;

        public int SelectedIndex {

            get { return _SelectedIndex; }

            set {
                if (SetProperty(ref _SelectedIndex, value)) { RaisePropertyChanged(nameof(SelectedList)); }
            }

        }

        public void Add() {
            var list = new ListViewModel();
            list.PropertyChanged += List_OnNotifyPropertyChanged;
            Lists.Add(list);
            user.Add(list);
            SelectedIndex = Lists.IndexOf(list);
        }

        public void Delete() {
            if (SelectedIndex != -1) {
                var person = Lists[SelectedIndex];
                Lists.RemoveAt(SelectedIndex);
                user.Delete(person);
            }
        }

        void List_OnNotifyPropertyChanged(Object sender, PropertyChangedEventArgs e) {

            user.Update((ListViewModel)sender);

        }

    }
}
