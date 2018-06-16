using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using System;

namespace ServerApp.Models {
    public class User : IdentityUser {

        // Id
        // Email

        public string FirstName;
        public string LastName;

        [JsonIgnore]
        public virtual ICollection<List> OwningLists { get; set; }

        [JsonIgnore]
        public virtual ICollection<UserListSubscription> SubscribedLists { get; set; }

        [JsonIgnore]
        public virtual ICollection<Notification> Notifications { get; set; }

        public void InviteToList(List list) {
            new Notification(this, NotificationType.ListInvitation, list);
        }

        public string GetFullName() {
            return FirstName + " " + LastName;
        }


        // quick and dirty way to JsonIgnore all EF properties
        [JsonIgnore] public override string UserName { get { return base.UserName; } set { base.UserName = value; } }
        [JsonIgnore] public override string PasswordHash { get { return base.PasswordHash; } set { base.PasswordHash = value; }}
        [JsonIgnore] public override string NormalizedEmail { get { return base.NormalizedEmail; } set { base.NormalizedEmail = value; }}
        [JsonIgnore] public override string NormalizedUserName { get { return base.NormalizedUserName; } set { base.NormalizedUserName = value; }}
        [JsonIgnore] public override string SecurityStamp { get { return base.SecurityStamp; } set { base.SecurityStamp = value; }}
        [JsonIgnore] public override string ConcurrencyStamp { get { return base.ConcurrencyStamp; } set { base.ConcurrencyStamp = value; }}
        [JsonIgnore] public override int AccessFailedCount { get { return base.AccessFailedCount; } set { base.AccessFailedCount = value; } }

    }
}
