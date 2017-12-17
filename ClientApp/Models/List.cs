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
using ClientApp.ViewModels;

namespace ClientApp.Models {
    public class List {
        public int ListId { get; set; }

        public string Name { get; set; }

        public string OwnerUserId { get; set; }

        public string Description { get; set; }

        public bool IsReadOnly { get; set; }

        public Color Color { get; set; }

        public string Icon { get; set; }

        public virtual List<Item> Items { get; set; }

        public List() {
            Items = new List<Item>();
            foreach (var item in FakeService.GetListItems(this)) {
                // awaiting better way to define an order in list items, this will have to do for now...
                Items.Add(item);
            }
        }

        public void AddItem(Item item) {
            if (!Items.Contains(item)) {
                Items.Add(item);
                FakeService.Write(this);
            }
        }

        public void RemoveItem(Item item) {
            if (Items.Contains(item)) {
                Items.Remove(item);
                FakeService.Delete(this);
            }
        }

        // add methods to check item off, make item read only, ...
    }
}
