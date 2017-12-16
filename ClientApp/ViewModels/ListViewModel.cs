using ClientApp.Models;
using ClientApp.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace ClientApp.ViewModels {
    public class ListViewModel : NotificationBase<List> {
        public ListViewModel(List list = null) : base(list) { }

        public string Name {
            get { return This.Name; }
            set { SetProperty(This.Name, value, () => This.Name = value); }
        }

        public string OwnerUserId {
            get { return This.OwnerUserId; }
            set { SetProperty(This.OwnerUserId, value, () => This.OwnerUserId = value); }
        }

        public Color color {
            get { return This.Color; }
            set { SetProperty(This.Color, value, () => This.Color = value); }
        }

        public string ItemCount {
            get { return This.Items.Count + " items"; }
        }

        public ICollection<Item> Items {
            get { return This.Items; }
        }

        // add observablelist of list items ...

    }
}
