using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Data
{
    public class WishContext : DbContext
    {
        public WishContext (DbContextOptions<WishContext> options)
            : base(options)
        {
        }

        public virtual DbSet<List> List { get; set; }
        public virtual DbSet<Item> Item { get; set; }
    }
}
