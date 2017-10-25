using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ServerApp.Models {
    public class List {
        public int ListId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
