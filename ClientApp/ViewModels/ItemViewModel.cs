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

        public string ProductName {
            get { return This.ProductName; }
            set { SetProperty(This.ProductName, value, () => This.ProductName = value); }
        }

        public bool IsCompleted {
            get { return This.IsCompleted; }
            set { SetProperty(This.IsCompleted, value, () => This.IsCompleted = value); }
        }

        public override string ToString() {
            return ProductName;
        }

        public static ItemViewModel FromItem(Item item) {
            var viewModel = new ItemViewModel();

            viewModel.ProductName = item.ProductName;
            viewModel.IsCompleted= item.IsCompleted;

            return viewModel;
        }

    }
}
