using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using ClientApp.DataService;
using ClientApp.ViewModels;
using System;

namespace ClientApp.Models {
    public class User {

        public string Id;
        public string Email;
        public string FirstName;
        public string LastName;

        public virtual ICollection<List> OwningLists { get; set; }

        public virtual ICollection<List> SubscribedLists { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }

        public User() {
            SubscribedLists = App.dataService.GetSubscribedLists();
            OwningLists = App.dataService.GetOwnedLists();
        }

        public void RegisterSubscription(List list) {
            if (!SubscribedLists.Contains(list)) {
                SubscribedLists.Add(list);
                App.dataService.EditList(list);
            }
        }

        internal void Update(ListViewModel sender) {
            throw new NotImplementedException();
        }

        public void RegisterOwned(List list) {
            if (!OwningLists.Contains(list)) {
                OwningLists.Add(list);
                App.dataService.EditList(list);
            }
        }

        public void RemoveSubscription(List list) {
            if (SubscribedLists.Contains(list)) {
                SubscribedLists.Remove(list);
                App.dataService.DeleteList(list);
            }
        }

        public void RemoveOwned(List list) {
            if (SubscribedLists.Contains(list)) {
                SubscribedLists.Remove(list);
                App.dataService.DeleteList(list);
            }
        }

        public void Add(List list) {
            if (!OwningLists.Contains(list)) {
                OwningLists.Add(list);
                App.dataService.EditList(list);
            }
        }

        public void Delete(List list) {
            if (OwningLists.Contains(list)) {
                OwningLists.Remove(list);
                App.dataService.DeleteList(list);
            }
        }

        public string GetFullName() {
            return FirstName + " " + LastName;
        }

        // add methods for users to be able to change their info, pw, emails, ...
    }
}
