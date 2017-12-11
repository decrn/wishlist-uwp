using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ServerApp.Models {
    public class Item {
        
        // autogenerate
        public int ItemId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }

        // default to false
        public bool IsCompleted { get; set; }

        // default to false
        public bool IsCheckLocked { get; set; }

        // optional
        public string ProductInfoUrl { get; set; }

        // optional
        public string ProductImageUrl { get; set; }

        // optional
        public double ItemPriceUsd { get; set; }

       
        public virtual ICollection<Item> Items { get; set; }

        [ForeignKey("List")]
        public int ListId { get; set; }

        [JsonIgnore]
        public virtual List List { get; set; }
    }
}
