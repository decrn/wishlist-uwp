using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.UI;
using ClientApp.DataService;
using System.Collections.ObjectModel;
using System.Diagnostics;
using ClientApp.ViewModels;

namespace ClientApp.Models {
    public class List {

        private int _listId;
        public int ListId {
            get { return _listId; }
            set { _listId = value; updateItems(); }
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Deadline { get; set; }

        public User OwnerUser { get; set; }

        public bool IsHidden { get; set; }

        // TODO: convert string to Color?
        public string Color { get; set; }

        public string Icon { get; set; }

        public virtual List<Item> Items { get; set; }

        public virtual List<User> SubscribedUsers { get; set; }

        public List() {
            Items = new List<Item>();
        }

        private void updateItems() {
            if (Items.Count < 1)
                Items = App.dataService.GetListItems(this);
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
                App.dataService.DeleteList(this);
            }
        }

        // add methods to check item off, ...
    }
}
