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
                    new List { ListId=0, Name="a", OwnerUserId=users[0].Id, Description="Description" },
                    new List { ListId=1, Name="b", OwnerUserId=users[0].Id, Description="Description" },
                    new List { ListId=2, Name="c", OwnerUserId=users[1].Id, Description="Description" }
                };
                foreach (List l in lists)
                    _context.List.Add(l);

                _context.SaveChanges();

                // seed items
                var items = new Item[]
                {
                    new Item { ItemId=0, ProductName="a", ListId=0, CheckedByUserId=users[1].Id },
                    new Item { ItemId=1, ProductName="b", ListId=0, ItemPriceUsd=19.99, CheckedByUserId=users[2].Id },
                    new Item { ItemId=2, ProductName="c", ListId=1, ItemPriceUsd=9.99 },
                    new Item { ItemId=3, ProductName="d", ListId=2 }
                };
                foreach (Item i in items)
                    _context.Item.Add(i);

                _context.SaveChanges();

                // TODO: fix seed notifications
                //var notifications = new Notification[]
                //{
                //    new Notification { Type=NotificationType.DeadlineReminder, ListId = lists[2].ListId },
                //    new Notification { Type=NotificationType.JoinRequest, UserId = users[1].Id },
                //    new Notification { Type=NotificationType.ListInvitation, ListId = lists[1].ListId },
                //    new Notification { Type=NotificationType.ListInvitation, ListId = lists[0].ListId }
                //};
                //foreach (Notification n in notifications)
                //    _context.Notification.Add(n);

                //_context.SaveChanges();

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
                Debug.WriteLine(sqlStatement);
                _context.Database.ExecuteSqlCommand(sqlStatement);
            }
        }
    }
}
