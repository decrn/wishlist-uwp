using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ClientApp.Models {
    public class Item {

        public int ItemId { get; set; }

        public string Name { get; set; }

        public bool IsCompleted { get; set; }

        public virtual List List { get; set; }
    }
}
