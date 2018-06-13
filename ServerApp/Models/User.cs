using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ServerApp.Models {
    public class User : IdentityUser {

        // Id
        // Email
        // UserName
        // PasswordHash

        public string FirstName;
        public string LastName;

        [JsonIgnore]
        public virtual ICollection<List> OwningLists { get; set; }

        [JsonIgnore]
        public virtual ICollection<UserListSubscription> SubscribedLists { get; set; }

        [JsonIgnore]
        public virtual ICollection<Notification> Notifications { get; set; }

        public void InviteToList(List list) {
            // TODO: Improve notification constructor
            Notifications.Add(new Notification() { Type = NotificationType.ListInvitation, ListId = list.ListId });
        }
    }
}
