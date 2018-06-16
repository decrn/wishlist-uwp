using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Data
{
    public class DataInitializer
    {
        private WishContext _context;
        private readonly UserManager<User> _userManager;

        public DataInitializer(WishContext context, UserManager<User> userManager) {
            _context = context;
            _userManager = userManager;
        }

        public async Task Seed()
        {
            _context.Database.EnsureDeleted();
            if (_context.Database.EnsureCreated()) {

                // seed users
                var users = new User[]
                {
                    new User { FirstName="Jan", LastName="Janssens", Id="11f67be3-ca09-44da-b4da-fcc6195fdd75", Email="a@domain.com" },
                    new User { FirstName="Peter", LastName="Peterson", Id="17f330d4-f49b-4a40-9c09-971487c263ca", Email="b@domain.com" },
                    new User { FirstName="Catherine", LastName="McCarthy", Id="381d2bee-f75a-499f-8d6e-ee5331b205a8", Email="c@domain.com" }
                };
                foreach (User u in users) {
                    u.UserName = u.Email;
                    await _userManager.CreateAsync(u, "qwerty12345");
                }

                _context.SaveChanges();

                // seed list
                var lists = new List[]
                {
                    new List { ListId=0, Name="Verjaardag Jan", OwnerUser=users[0], Description="Voor op het feestje af te geven", Deadline=new DateTime(2018,12,31) },
                    new List { ListId=1, Name="Babyborrel Charlotte", OwnerUser=users[0], Description="Een jonge spruit!", Deadline=new DateTime(2019,05,12) },
                    new List { ListId=2, Name="Trouw", OwnerUser=users[1], Description="Al gepasseerd eigenlijk", Deadline=new DateTime(2018,01,01) }
                };
                foreach (List l in lists)
                    _context.List.Add(l);

                _context.SaveChanges();

                // seed items
                var items = new Item[]
                {
                    new Item { ItemId=0, ProductName="Playstation", List=lists[0], CheckedByUser=users[1] },
                    new Item { ItemId=1, ProductName="Tent", List=lists[0], ItemPriceUsd=19.99, CheckedByUser=users[2] },
                    new Item { ItemId=2, ProductName="Ovenschotel", List=lists[1], ItemPriceUsd=9.99 },
                    new Item { ItemId=3, ProductName="Barbies", List=lists[2] }
                };
                foreach (Item i in items)
                    _context.Item.Add(i);

                _context.SaveChanges();

                var notifications = new Notification[]
                {
                    new Notification(users[0], NotificationType.DeadlineReminder, lists[0]) { NotificationId = 0 },
                    new Notification(users[0], NotificationType.JoinRequest, null, users[1]) { NotificationId = 1 },
                    new Notification(users[0], NotificationType.ListInvitation, lists[1]) { NotificationId = 2 },
                    new Notification(users[1], NotificationType.ListInvitation, lists[2]) { NotificationId = 3 }
                };
                foreach (Notification n in notifications)
                    _context.Notification.Add(n);

                _context.SaveChanges();

                // seed list subscriptions
                var sqlStatement = "INSERT INTO [UserListSubscription] VALUES ";
                var subs = new UserListSubscription[]
                {
                    new UserListSubscription { Id=0, ListId=0, UserId=users[1].Id },
                    new UserListSubscription { Id=1, ListId=0, UserId=users[2].Id },
                    new UserListSubscription { Id=2, ListId=1, UserId=users[0].Id }
                };
                foreach (UserListSubscription s in subs)
                    sqlStatement += "( " + s.Id + ", " + s.ListId + ", '" + s.UserId + "' ),";

                sqlStatement = sqlStatement.Remove(sqlStatement.Length - 1) + ";";
                _context.Database.ExecuteSqlCommand(sqlStatement);
            }
        }
    }
}
