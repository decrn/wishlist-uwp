using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ServerApp.Models {
    public class Item {

        public int ItemId { get; set; }

        public string Name { get; set; }

        public bool IsCompleted { get; set; }

        [ForeignKey("List")]
        public int ListId { get; set; }

        [JsonIgnore]
        public virtual List List { get; set; }
    }
}
