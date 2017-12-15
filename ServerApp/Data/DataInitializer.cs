﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
            _context.Database.EnsureCreated(); //if db does not exist, it will create database but do nothing

            // Look for any data
            if (_context.User.Any() || _context.List.Any() || _context.Item.Any())
                return;

            // seed users
            var users = new User[]
            {
                new User { Id="11f67be3-ca09-44da-b4da-fcc6195fdd75", Email="a@domain.com" },
                new User { Id="17f330d4-f49b-4a40-9c09-971487c263ca", Email="b@domain.com" },
                new User { Id="381d2bee-f75a-499f-8d6e-ee5331b205a8", Email="c@domain.com" }
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

            // seed list subscriptions
            //var subs = new UserListSubscription[]
            //{
            //    new UserListSubscription { Id=0, ListId=0, UserId=users[1].Id },
            //    new UserListSubscription { Id=1, ListId=0, UserId=users[2].Id },
            //    new UserListSubscription { Id=2, ListId=1, UserId=users[0].Id }
            //};
            //foreach (UserListSubscription s in subs)
            //    _context.UserListSubscription.Add(s);

            //_context.SaveChanges();
        }
    }
}