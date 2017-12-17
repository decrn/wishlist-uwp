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
        
        [JsonIgnore]
        public virtual ICollection<List> OwningLists { get; set; }

        [JsonIgnore]
        public virtual ICollection<List> SubscribedLists { get; set; }

        public User() {
            SubscribedLists = FakeService.GetSubscribedLists();
            OwningLists = FakeService.GetOwnedLists();
        }

        public void RegisterSubscription(List list) {
            if (!SubscribedLists.Contains(list)) {
                SubscribedLists.Add(list);
                FakeService.Write(list);
            }
        }

        internal void Update(ListViewModel sender) {
            throw new NotImplementedException();
        }

        public void RegisterOwned(List list) {
            if (!OwningLists.Contains(list)) {
                OwningLists.Add(list);
                FakeService.Write(list);
            }
        }

        public void RemoveSubscription(List list) {
            if (SubscribedLists.Contains(list)) {
                SubscribedLists.Remove(list);
                FakeService.Delete(list);
            }
        }

        public void RemoveOwned(List list) {
            if (SubscribedLists.Contains(list)) {
                SubscribedLists.Remove(list);
                FakeService.Delete(list);
            }
        }

        public void Add(List list) {
            if (!OwningLists.Contains(list)) {
                OwningLists.Add(list);
                FakeService.Write(list);
            }
        }

        public void Delete(List list) {
            if (OwningLists.Contains(list)) {
                OwningLists.Remove(list);
                FakeService.Delete(list);
            }
        }

        public void UpdateOwnedList(List list) {
            // update... requires iterating over lists with linq, fuck that
        }

        // add methods for users to be able to change their info, pw, emails, ...
    }
}
