﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ClientApp.Models {
    public class List {
        public int ListId { get; set; }

        public string Name { get; set; }

        public string EditableHash { get; set; }

        public string ViewableHash { get; set; }

        public string CuratorName { get; set; }

        public string Description { get; set; }

        public bool isReadOnly { get; set; }

        public System.Drawing.Color Color { get; set; }

        public char Icon { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
