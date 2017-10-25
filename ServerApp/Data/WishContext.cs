using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Models
{
    public class WishContext : DbContext
    {
        public WishContext (DbContextOptions<WishContext> options)
            : base(options)
        {
        }

        public DbSet<ServerApp.Models.List> List { get; set; }

        public DbSet<ServerApp.Models.Item> Item { get; set; }
    }
}
