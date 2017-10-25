using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerApp.Models;

namespace ClientApp {
    public class Item {
        public int ItemId { get; set; }

        public string Name { get; set; }

        public bool IsCompleted { get; set; }

        public virtual List List { get; set; }
    }
}
