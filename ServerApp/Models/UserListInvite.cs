using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerApp.Models {
    public class UserListInvite {
        // autogenerate
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonIgnore]
        public string UserId { get; set; }

        public User User { get; set; }

        [JsonIgnore]
        public int ListId { get; set; }
        [JsonIgnore]
        public List List { get; set; }
    }
}
