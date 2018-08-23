using ClientApp.Models;
using ClientApp.ViewModels.ViewModels;
using System;

namespace ClientApp.ViewModels {
    public class ItemViewModel : NotificationBase<Item> {
        public ItemViewModel(Item item = null) : base(item) { }

        public int ItemId {
            get { return This.ItemId; }
        }

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

        public string Price {
            get => ItemPriceUsd != null ? "$" + ItemPriceUsd : "";
        }

        public bool IsCompleted {
            get { return CheckedByUser != null; }
            set { CheckedByUser = null; }
        }

        public override string ToString() {
            return ProductName;
        }

        public bool CanCheck {
            get { return !IsCompleted || CheckedByUser.Id == App.dataService.LoggedInUser.Id; }
        }

        public bool HasProductInfoUrl {
            get => ProductInfoUrl != null && ProductInfoUrl != "";
        }

        public Uri GetImageUrl() {
            return new Uri(ProductImageUrl ?? "https://via.placeholder.com/50x50");
        }

    }
}
