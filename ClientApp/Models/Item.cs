﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ClientApp.Models {
    public class Item {

        public int ItemId { get; set; }

        public string ProductName { get; set; }

        public User CheckedByUser { get; set; }

        public string Description { get; set; }

        public string ProductInfoUrl { get; set; }

        public string ProductImageUrl { get; set; }

        public double? ItemPriceUsd { get; set; }

        public virtual List List { get; set; }

        public bool IsCompleted {
            get { return CheckedByUser != null; }
            set { CheckedByUser = null; }
        }
    }
}