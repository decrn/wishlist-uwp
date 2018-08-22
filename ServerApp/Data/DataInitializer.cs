using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;
using System;
using System.Threading.Tasks;

namespace ServerApp.Data {
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
                    new List { Name="Birthday Jan", OwnerUser=users[0], Description="Just some ideas for when you come visit", Deadline=DateTime.UtcNow.AddDays(25).AddMinutes(555) },
                    new List { Name="Baby shower Charlotte", IsHidden = true, OwnerUser=users[0], Description="Let's welcome Charlotte with a bunch of gifts or a green shower!", Deadline=DateTime.UtcNow.AddDays(200) },
                    new List { Name="Wedding A&M", OwnerUser=users[1], Deadline=DateTime.UtcNow.AddDays(-5).AddHours(3) },
                    new List { Name="Last minute B-Day", OwnerUser=users[2], Description="Sorry, I didn't have time to make my list.", Deadline=DateTime.UtcNow.AddDays(2) },
                    new List { Name="Retirement Grandpa Charles", IsHidden=true, OwnerUser=users[2], Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quantum Aristoxeni ingenium consumptum videmus in musicis? Videsne, ut haec concinant? Quae cum magnifice primo dici viderentur, considerata minus probabantur.", Deadline=DateTime.UtcNow.AddDays(50).AddHours(-60) },
                };
                foreach (List l in lists)
                    _context.List.Add(l);

                _context.SaveChanges();

                // seed items
                var items = new Item[]
                {
                    new Item { ProductName="Playstation 4",         List=lists[0], ItemPriceUsd=379.99, CheckedByUser=users[1], Category="Toys" },
                    new Item { ProductName="Headset",               List=lists[0], ItemPriceUsd=030.00, CheckedByUser=users[1], Category="Gadgets" },
                    new Item { ProductName="Lego train",            List=lists[0], ItemPriceUsd=120.00, Category="Toys" },
                    new Item { ProductName="Batman costume",        List=lists[0], Category="Varia" },
                    new Item { ProductName="Extra PS4 game",        List=lists[0], ItemPriceUsd=059.99 },
                    new Item { ProductName="Barbie dolls",          List=lists[1], ItemPriceUsd=025.00, CheckedByUser=users[2] },
                    new Item { ProductName="Duplo blocks",          List=lists[1], ItemPriceUsd=050.00 },
                    new Item { ProductName="Nice cutlery",          List=lists[2], ItemPriceUsd=249.98, CheckedByUser=users[0] },
                    new Item { ProductName="Traditional adult toy", List=lists[2], ItemPriceUsd=050.00, CheckedByUser=users[2] },
                    new Item { ProductName="Yoga set",              List=lists[2] },
                    new Item { ProductName="Tent",                  List=lists[3], ItemPriceUsd=149.99, CheckedByUser=users[0] },
                    new Item { ProductName="Cooking pot",           List=lists[3], ItemPriceUsd=075.00 },
                    new Item { ProductName="Pepper & saltmill",     List=lists[4], ItemPriceUsd=075.00, CheckedByUser=users[1] }
                };
                foreach (Item i in items)
                    _context.Item.Add(i);

                _context.SaveChanges();

                var notifications = new Notification[]
                {
                    //new Notification(users[0], NotificationType.DeadlineReminder, lists[3]),
                    new Notification(users[0], NotificationType.JoinRequest, null, users[2]),
                    new Notification(users[0], NotificationType.ListJoinSuccess, lists[1], users[2]) { IsUnread = false },

                    new Notification(users[2], NotificationType.ListInvitation, lists[0]),
                    new Notification(users[1], NotificationType.ListInvitation, lists[1]),
                    new Notification(users[0], NotificationType.ListInvitation, lists[4]) { Timestamp = DateTime.UtcNow.AddHours(-1) },
                };
                foreach (Notification n in notifications)
                    _context.Notification.Add(n);

                _context.SaveChanges();

                // seed list subscriptions
                var sqlStatement = "INSERT INTO [UserListSubscription] VALUES ";
                var subs = new UserListSubscription[]
                {
                    new UserListSubscription { ListId=lists[0].ListId, UserId=users[1].Id },
                    new UserListSubscription { ListId=lists[1].ListId, UserId=users[2].Id },
                    new UserListSubscription { ListId=lists[2].ListId, UserId=users[0].Id },
                    new UserListSubscription { ListId=lists[2].ListId, UserId=users[2].Id },
                    new UserListSubscription { ListId=lists[3].ListId, UserId=users[0].Id },
                    new UserListSubscription { ListId=lists[4].ListId, UserId=users[1].Id },
                };
                foreach (UserListSubscription s in subs)
                    sqlStatement += "( " + s.ListId + ", '" + s.UserId + "' ),";

                sqlStatement = sqlStatement.Remove(sqlStatement.Length - 1) + ";";
                _context.Database.ExecuteSqlCommand(sqlStatement);

                // seed list invites
                var sqlStatement2 = "INSERT INTO [UserListInvite] VALUES ";
                var invs = new UserListInvite[]
                {
                    new UserListInvite { ListId=lists[0].ListId, UserId=users[2].Id },
                    new UserListInvite { ListId=lists[1].ListId, UserId=users[1].Id },
                    new UserListInvite { ListId=lists[4].ListId, UserId=users[0].Id },
                };
                foreach (UserListInvite s in invs)
                    sqlStatement2 += "( " + s.ListId + ", '" + s.UserId + "' ),";

                sqlStatement2 = sqlStatement2.Remove(sqlStatement2.Length - 1) + ";";
                _context.Database.ExecuteSqlCommand(sqlStatement2);
            }
        }
    }
}
