using ClientApp.Models;
using ClientApp.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.ViewModels {
    public class ItemViewModel : NotificationBase<Item> {
        public ItemViewModel(Item item = null) : base(item) { }

        public string Name {
            get { return This.ProductName; }
            set { SetProperty(This.ProductName, value, () => This.ProductName = value); }
        }

        public bool IsCompleted {
            get { return This.IsCompleted; }
            set { SetProperty(This.IsCompleted, value, () => This.IsCompleted = value); }
        }
    }
}
