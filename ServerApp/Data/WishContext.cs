using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Data {
    public class WishContext : IdentityDbContext<User> {
        public WishContext(DbContextOptions<WishContext> options) : base(options) {
        }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<List> List { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<List>().ToTable("List");
            modelBuilder.Entity<Item>().ToTable("Item");

            modelBuilder.Entity<Notification>().ToTable("Notification");
            modelBuilder.Entity<Notification>().HasOne(n => n.OwnerUser).WithMany(u => u.Notifications);
            modelBuilder.Entity<Notification>().HasOne(n => n.SubjectUser);
            modelBuilder.Entity<Notification>().HasOne(n => n.SubjectList);

            //modelBuilder.Entity<User>().HasMany(n => n.Notifications);
        }
    }
}
