using ClientApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace ClientApp.DataService {
    public class FakeService {
        public static String Name = "Fake Data Service";

        public static List<List> GetSubscribedLists() {
            Debug.WriteLine("GET for Subscribed Lists.");

            return new List<List>() {
                    new List() { ListId=2, Name="John Locke's Birthday Wishes", OwnerUserId="John Locke's Mama"},
                    new List() { ListId=3, Name="Jessica's Maternity List", OwnerUserId="Jessica" },
                    new List() { ListId=4, Name="Babyborrel van den Sep De Laet", OwnerUserId="Senne De Laet"}
                };
        }

        public static List<List> GetOwnedLists() {
            Debug.WriteLine("GET for Owned Lists.");

            return new List<List>() {
                    new List() { ListId=1, Name="Tutti and Frutti Baby Shower", OwnerUserId="Desmond Tutu", Color=Color.FromArgb(255, 247, 34, 176) },
                };
        }

        // probably not gonna work? We called observable in een observable maar let's see I guess
        public static ObservableCollection<Item> GetListItems(List list) {
            Debug.WriteLine("GET items for list with name " + list.Name);

            return new ObservableCollection<Item>() {
                new Item() { ProductName="Baby Wipes", List=list },
                new Item() { ProductName="Baby shampoo", List=list, CheckedByUserId="temp" },
                new Item() { ProductName="Packet of Cigarettes", List=list, ItemPriceUsd=5.55 }
            };
        }

        public static void Write(List list) {
            Debug.WriteLine("POST List with name " + list.Name);
        }
        
        public static void Delete(List list) {
            Debug.WriteLine("POST List with name DELETE" + list.Name);
        }

    }
}
