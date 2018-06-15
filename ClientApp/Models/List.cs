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

        public string OwnerUserId { get; set; }

        public string Description { get; set; }

        public bool IsReadOnly { get; set; }

        public Color Color { get; set; }

        public string Icon { get; set; }

        public virtual List<Item> Items { get; set; }

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
                App.dataService.Write(this);
            }
        }

        public void RemoveItem(Item item) {
            if (Items.Contains(item)) {
                Items.Remove(item);
                App.dataService.Delete(this);
            }
        }

        // add methods to check item off, make item read only, ...
    }
}
