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

            modelBuilder.Entity<UserListInvite>()
                .HasOne(l => l.List)
                .WithMany(l => l.InvitedUsers)
                .HasForeignKey(i => i.ListId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserListSubscription>()
                .HasOne(l => l.List)
                .WithMany(l => l.SubscribedUsers)
                .HasForeignKey(i => i.ListId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Item>()
                .HasOne(l => l.List)
                .WithMany(l => l.Items)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Notification>()
                .HasOne(l => l.SubjectList)
                .WithMany(l => l.Notifications)
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<User>().HasMany(n => n.Notifications);
        }
    }
}
