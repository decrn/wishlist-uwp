using ClientApp.Models;
using ClientApp.ViewModels.ViewModels;

namespace ClientApp.ViewModels {
    public class ItemViewModel : NotificationBase<Item> {
        public ItemViewModel(Item item = null) : base(item) { }

        public string ProductName {
            get { return This.ProductName; }
            set { SetProperty(This.ProductName, value, () => This.ProductName = value); }
        }

        public User CheckedByUser {
            get { return This.CheckedByUser; }
            set { SetProperty(This.CheckedByUser, value, () => This.CheckedByUser = value); }
        }

        public string Description {
            get { return This.Description; }
            set { SetProperty(This.Description, value, () => This.Description = value); }
        }

        public string ProductInfoUrl {
            get { return This.ProductInfoUrl; }
            set { SetProperty(This.ProductInfoUrl, value, () => This.ProductInfoUrl = value); }
        }

        public string ProductImageUrl {
            get { return This.ProductImageUrl; }
            set { SetProperty(This.ProductImageUrl, value, () => This.ProductImageUrl = value); }
        }

        public double? ItemPriceUsd {
            get { return This.ItemPriceUsd; }
            set { SetProperty(This.ItemPriceUsd, value, () => This.ItemPriceUsd = value); }
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
