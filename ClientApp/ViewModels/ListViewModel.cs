using ClientApp.Models;
using ClientApp.ViewModels.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace ClientApp.ViewModels {
    public class ListViewModel : NotificationBase<List> {
        public ListViewModel(List list = null) : base(list) { }

        public int ListId {
            get { return This.ListId; }
            set { SetProperty(This.ListId, value, () => This.ListId = value); }
        }

        public string Name {
            get { return This.Name; }
            set { SetProperty(This.Name, value, () => This.Name = value); }
        }

        public User OwnerUser {
            get { return This.OwnerUser; }
            set { SetProperty(This.OwnerUser, value, () => This.OwnerUser = value); }
        }

        public string Color {
            get { return This.Color; }
            set { SetProperty(This.Color, value, () => This.Color = value); }
        }

        public string ItemCount {
            get { return This.Items==null ? "" : This.Items.Count + " items"; }
        }

        List<ItemViewModel> _Items = new List<ItemViewModel>();
        public List<ItemViewModel> Items {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }

        public List<string> ItemNames {
            get
            {
                return Items.Select(i => i.ProductName).ToList();
                //List<string> itemnames = new List<string>();
                //foreach (var item in Items) {
                //    itemnames.Add(item.ProductName);
                //}

                //return itemnames;
            }
        }        

        public static ListViewModel FromList(List list) {
            var viewModel = new ListViewModel();

            viewModel.ListId = list.ListId;
            viewModel.Name = list.Name;
            viewModel.OwnerUser = list.OwnerUser;
            viewModel.Color = list.Color;
            if (list.Items != null)
                viewModel.Items = list.Items.Select(i => ItemViewModel.FromItem(i)).ToList();

            return viewModel;
        }

        // add observablelist of list items ...

    }
}
