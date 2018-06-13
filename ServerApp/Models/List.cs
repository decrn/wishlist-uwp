using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ServerApp.Models {
    public class List {

        // autogenerate
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ListId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        // optional
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        [ForeignKey("User")]
        public string OwnerUserId { get; set; }

        // defaults to false
        public bool IsHidden { get; set; }

        // defaults to false?
        public bool IsReadOnly { get; set; }

        // optional
        [JsonIgnore]
        public int Color { get; set; }

        // optional: consider default icon?
        public string Icon { get; set; }

        [InverseProperty("List")]
        public virtual ICollection<Item> Items { get; set; }

        public virtual ICollection<UserListSubscription> SubscribedUsers { get; set; }

        public bool IsSoon() {
            TimeSpan diff = Deadline - DateTime.Now;

            if (diff.TotalSeconds > 0 && diff.TotalHours < 24*3)
                return true;

            return false;
        }
    }
}
