using ClientApp.Models;
using ClientApp.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ClientApp.DataService {

    public class FakeService : IDataService {

        private string JWTToken = "skippinglogin";

        public Loading LoadingIndicator { get; set; }

        public User LoggedInUser { get; set; }

        // ACCOUNT

        public bool IsLoggedIn() {
            return JWTToken != "";
        }        

        public async Task<JObject> Login(LoginViewModel vm) {
            JWTToken = "temp";
            return await Task.FromResult(
                JObject.FromObject(new { success = true, data = new { user = GetCurrentUser(), token = JWTToken } })
            );
        }

        public async Task<JObject> Register(RegisterViewModel vm) {
            JWTToken = "temp";
            return await Task.FromResult(
                JObject.FromObject(new { success = true, data = new { user = GetCurrentUser(), token = JWTToken } })
            );
        }

        public async Task<JObject> ChangePassword(ChangePasswordViewModel vm) {
            return await Task.FromResult(
                JObject.FromObject(new { success = true })
            );
        }

        public async Task<JObject> EditAccount(EditAccountViewModel vm) {
            return await Task.FromResult(
                JObject.FromObject(new { success = true })
            );
        }

        public async Task<JObject> ForgotPassword(ForgotPasswordViewModel vm) {
            return await Task.FromResult(
                JObject.FromObject(new { success = true })
            );
        }

        public async Task<JObject> ResetPassword(ResetPasswordViewModel vm) {
            return await Task.FromResult(
                JObject.FromObject(new { success = true })
            );
        }

        public void Logout() {
            Debug.WriteLine("Logout");
            LoggedInUser = null;
            JWTToken = "";
        }

        // USER

        public async Task<User> GetCurrentUser() {
            Debug.WriteLine("GET currentuser");
            LoggedInUser = GetFakeUser();
            return await Task.FromResult(
                new User() {
                    FirstName = "Peter",
                    LastName = "Petersson",
                    Email = "peter@domain.com",
                    Id = "4875185"
                }
            );
        }

        private User GetFakeUser(int id = 0) {
            if (id == 1)
                return new User() {
                    FirstName = "Karel",
                    LastName = "Carlston",
                    Email = "carl@domain.com",
                    Id = "2884857",
                    //OwningLists = GetOwnedLists()
                };
            return new User() {
                FirstName = "Jan",
                LastName = "Janssens",
                Email = "jan@domain.com",
                Id = "54674654",
                //OwningLists = GetOwnedLists()
            };
        }

        public async Task<User> GetUser(string id) {
            Debug.WriteLine("GET user "+id);
            return await Task.FromResult(
                GetFakeUser()
            );
        }

        public async Task<List<List>> GetOwnedLists() {
            Debug.WriteLine("GET ownedlists");
            return await Task.FromResult(new List<List> {
                new List { ListId=0, Name="Verjaardag Jan", OwnerUser=GetFakeUser(), Description ="Voor op het feestje af te geven", Deadline=new DateTime(2018,12,31) },
                new List { ListId=1, Name="Babyborrel Charlotte", OwnerUser=GetFakeUser(), Description ="Een jonge spruit!", Deadline=new DateTime(2019,05,12) },
                new List { ListId=2, Name="Trouw", OwnerUser=GetFakeUser(1), Description="Al gepasseerd eigenlijk", Deadline=new DateTime(2018,01,01) }
            });
        }

        public Task<List<List>> GetSubscribedLists() {
            return GetOwnedLists();
        }

        public void RequestAccess(string emailaddress) {
            Debug.WriteLine("POST request access " + emailaddress);
        }

        // LISTS

        private List GetFakeList(int id = 0) {
            List list;

            if (id == 2)
                list = new List { ListId = 2, Name = "Trouw", OwnerUser = GetFakeUser(1), Description = "Al gepasseerd eigenlijk", Deadline = new DateTime(2018, 01, 01) };
            else if (id == 1)
                list = new List { ListId = 0, Name = "Verjaardag Jan", OwnerUser = GetFakeUser(), Description = "Voor op het feestje af te geven", Deadline = new DateTime(2018, 12, 31) };
            else
                list = new List { ListId = 0, Name = "Verjaardag Jan", OwnerUser = GetFakeUser(), Description = "Voor op het feestje af te geven", Deadline = new DateTime(2018, 12, 31), SubscribedUsers = new List<User> { GetFakeUser(), GetFakeUser(1) } };

            list.Items = new List<Item> {
                new Item { ItemId=0, ProductName="Playstation",  CheckedByUser=GetFakeUser() },
                new Item { ItemId=1, ProductName="Tent", ItemPriceUsd=19.99, CheckedByUser=GetFakeUser(1) },
                new Item { ItemId=2, ProductName="Ovenschotel", ItemPriceUsd=9.99 },
                new Item { ItemId=3, ProductName="Barbies" }
            };

            return list;
        }

        public async Task<List> GetList(int id) {
            Debug.WriteLine("GET list "+id);
            return await Task.FromResult(
                GetFakeList()
            );
        }

        public async Task<JObject> NewList(List list) {
            Debug.WriteLine("POST newlist");
            return await Task.FromResult(
                JObject.FromObject(new { success = true })
            );
        }

        public async Task<JObject> EditList(List list) {
            Debug.WriteLine("POST editlist");
            return await Task.FromResult(
                JObject.FromObject(new { success = true })
            );
        }

        public Task SendInvitations(List list) {
            Debug.WriteLine("POST sendinvitations");
            throw new Exception("Not supported in Fake Data Service");
        }

        public Task UnsubscribeFromList(List list) {
            this.DeleteList(list);
            throw new Exception("Not supported in Fake Data Service");
        }

        public Task DeleteList(List list) {
            Debug.WriteLine("DELETE list "+list.ListId);
            throw new Exception("Not supported in Fake Data Service");
        }

        public void MarkItem(Item item) {
            Debug.WriteLine("POST markitem "+item.ItemId);
        }

        public void UnMarkItem(Item item) {
            Debug.WriteLine("POST unmarkitem " + item.ItemId);
        }

        public async Task<JObject> NewItem(Item item) {
            Debug.WriteLine("POST newitem");
            return await Task.FromResult(
                JObject.FromObject(new { success = true })
            );
        }

        public async Task<JObject> EditItem(Item item) {
            Debug.WriteLine("POST editItem");
            return await Task.FromResult(
                JObject.FromObject(new { success = true })
            );
        }

        public Task DeleteItem(Item item) {
            Debug.WriteLine("DELETE item " + item.ItemId);
            throw new Exception("Not supported in Fake Data Service");
        }

        public async Task<List<Notification>> GetNotifications() {
            Debug.WriteLine("GET notifications");
            return await Task.FromResult(
                new List<Notification> {
                new Notification() { OwnerUser = GetFakeUser(), Type = NotificationType.DeadlineReminder, SubjectList = GetFakeList(1), NotificationId = 0, IsUnread=true },
                new Notification() { OwnerUser = GetFakeUser(), Type = NotificationType.JoinRequest, SubjectUser = GetFakeUser(1), NotificationId = 1, IsUnread=true },
                new Notification() { OwnerUser = GetFakeUser(), Type = NotificationType.ListInvitation, SubjectList = GetFakeList(2), NotificationId = 2, IsUnread=false },
                new Notification() { OwnerUser = GetFakeUser(), Type = NotificationType.ListInvitation, SubjectList = GetFakeList(1), NotificationId = 3, IsUnread=true },
                new Notification() { OwnerUser = GetFakeUser(), Type = NotificationType.ListInvitation, SubjectList = GetFakeList(0), NotificationId = 4, IsUnread=false },
                new Notification() { OwnerUser = GetFakeUser(), Type = NotificationType.ListJoinSuccess, SubjectList = GetFakeList(2), SubjectUser=GetFakeUser(1), NotificationId = 5, IsUnread=false }
            });
        }

        public Task MarkAllNotificationsAsRead() {
            Debug.WriteLine("POST markallnotifs");
            throw new Exception("Not supported in Fake Data Service");
        }

        public Task MarkNotification(Notification notification) {
            Debug.WriteLine("POST execmarknotif "+notification.NotificationId);
            throw new Exception("Not supported in Fake Data Service");
        }

        public Task ActOnNotification(Notification notification) {
            Debug.WriteLine("POST execmarknotif " + notification.NotificationId);
            throw new Exception("Not supported in Fake Data Service");
        }

    }
}
