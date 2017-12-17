using ClientApp.Models;
using ClientApp.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

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

        public string OwnerUserId {
            get { return This.OwnerUserId; }
            set { SetProperty(This.OwnerUserId, value, () => This.OwnerUserId = value); }
        }

        public Color Color {
            get { return This.Color; }
            set { SetProperty(This.Color, value, () => This.Color = value); }
        }

        public string ItemCount {
            get { return This.Items.Count + " items"; }
        }

        public List<Item> Items {
            get { return This.Items; }
            set { SetProperty(This.Items, value, () => This.Items = value); }
        }

        public List<string> ItemNames {
            get {
                List<string> itemnames = new List<string>();
                foreach (var item in Items) {
                    itemnames.Add(item.ProductName);
                }

                return itemnames;
            }
        }        

        public static ListViewModel FromList(List list) {
            var viewModel = new ListViewModel();

            viewModel.ListId = list.ListId;
            viewModel.Name = list.Name;
            viewModel.OwnerUserId = list.OwnerUserId;
            viewModel.Color = list.Color;
            viewModel.Items = list.Items;

            return viewModel;
        }

        // add observablelist of list items ...

    }
}
