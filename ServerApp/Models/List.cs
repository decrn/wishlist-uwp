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

        // autogenerate
        public string EditableHash { get; set; }

        // autogenerate
        public string ViewableHash { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string CuratorName { get; set; }

        // optional
        public string Description { get; set; }

        // defaults to false
        public bool isHidden { get; set; }

        // defaults to false?
        public bool isReadOnly { get; set; }

        // optional
        public int Color { get; set; }

        // optional: consider default icon?
        public string Icon { get; set; }

        [InverseProperty("List")]
        public virtual ICollection<Item> Items { get; set; }
    }
}
