using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;

namespace ClientApp.Models {
    public class List : ObservableObject {

        public int ListId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Deadline { get; set; }

        public User OwnerUser { get; set; }

        public bool IsHidden { get; set; }

        public string Color { get; set; }

        public string Icon { get; set; }

        public virtual List<Item> Items { get; set; }

        public virtual List<User> SubscribedUsers { get; set; }

        public virtual List<User> InvitedUsers { get; set; }

        public List() {
            Items = new List<Item>();
        }

        // TODO: Is this legal?

        public void AddItem(Item item) {
            if (!Items.Contains(item)) {
                Items.Add(item);
                App.dataService.EditList(this);
            }
        }

        public void RemoveItem(Item item) {
            if (Items.Contains(item)) {
                Items.Remove(item);
                App.dataService.EditList(this);
            }
        }

        public void Update(Item item) {
            App.dataService.EditList(this);
        }
    }
}
