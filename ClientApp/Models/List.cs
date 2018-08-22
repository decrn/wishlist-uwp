using System;
using System.Collections.Generic;
using System.Diagnostics;
using ClientApp.ViewModels;
using GalaSoft.MvvmLight;

namespace ClientApp.Models {
    public class List : ObservableObject {

        public int ListId { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                Debug.WriteLine("Setting Name in wrong place");
            }
        }

        public string Description { get; set; }

        public DateTime Deadline { get; set; }

        public User OwnerUser { get; set; }

        public bool IsHidden { get; set; }

        // TODO: convert string to Color?
        public string Color { get; set; }

        public string Icon { get; set; }

        public virtual List<Item> Items { get; set; }

        public virtual List<User> SubscribedUsers { get; set; }

        public virtual List<User> InvitedUsers { get; set; }

        public List() {
            Items = new List<Item>();
        }

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
