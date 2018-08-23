using ClientApp.Models;
using ClientApp.ViewModels.ViewModels;
using System;

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

        public string Category {
            get { return This.Category; }
            set { SetProperty(This.Category, value, () => This.Category = value); }
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

        // TODO: implement Item.CanCheck
        // Should return true if: !IsCompleted || CheckedByUser.Id == CurrentUser.Id
        public bool CanCheck {
            get { return true; }
            set { }
        }

        public Uri GetImageUrl() {
            return new Uri(ProductImageUrl ?? "https://via.placeholder.com/50x50");
        }

        public static ItemViewModel FromItem(Item item) {
            var viewModel = new ItemViewModel {
                ProductName = item.ProductName,
                IsCompleted = item.IsCompleted
            };

            return viewModel;
        }

    }
}
