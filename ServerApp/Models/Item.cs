using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerApp.Models {
    public class Item {

        // autogenerate
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }

        public User CheckedByUser { get; set; }

        // optional
        public string Description { get; set; }

        // optional
        public string ProductInfoUrl { get; set; }

        // optional
        public string ProductImageUrl { get; set; }

        // optional
        public double? ItemPriceUsd { get; set; }

        [JsonIgnore]
        public List List { get; set; }
    }
}
