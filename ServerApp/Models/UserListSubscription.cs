using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Models {
    public class UserListSubscription {
        // autogenerate
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int ListId { get; set; }
        public List List { get; set; }
    }
}
