using ClientApp.Models;
using ClientApp.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClientApp.DataService {

    public class RealService : IDataService {

        private HttpService HttpService = new HttpService();

        public Loading LoadingIndicator { get { return HttpService.LoadingIndicator; } set { HttpService.LoadingIndicator = value; }  }


        #region ACCOUNT

        public bool IsLoggedIn() {
            return HttpService.IsTokenSet();
        }

        public async Task<JObject> Login(LoginViewModel vm) {
            
            var body = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("Email", vm.Email),
                new KeyValuePair<string, string>("Password", vm.Password),
            });

            JObject obj = JObject.Parse(await HttpService.PostForm("Account/Login", body, true));
            HttpService.TrySettingToken(obj);

            return obj;
        }

        public async Task<JObject> Register(RegisterViewModel vm) {

            var body = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("FirstName", vm.FirstName),
                new KeyValuePair<string, string>("LastName", vm.LastName),
                new KeyValuePair<string, string>("Email", vm.Email),
                new KeyValuePair<string, string>("Password", vm.Password),
                new KeyValuePair<string, string>("ConfirmPassword", vm.ConfirmPassword),
            });

            JObject obj = JObject.Parse(await HttpService.PostForm("Account/Register", body, true));
            HttpService.TrySettingToken(obj);

            return obj;
        }

        public async Task<JObject> ChangePassword(ChangePasswordViewModel vm) {
            var body = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("OldPassword", vm.OldPassword),
                new KeyValuePair<string, string>("NewPassword", vm.NewPassword),
                new KeyValuePair<string, string>("ConfirmPassword", vm.ConfirmPassword),
            });

            JObject obj = JObject.Parse(await HttpService.PostForm("Account/Password", body, true));

            return obj;
        }

        public async Task<JObject> EditAccount(EditAccountViewModel vm) {
            var body = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("FirstName", vm.FirstName),
                new KeyValuePair<string, string>("LastName", vm.LastName),
                new KeyValuePair<string, string>("Email", vm.Email)
            });

            JObject obj = JObject.Parse(await HttpService.PostForm("Account/Edit", body, true));

            return obj;
        }

        public async Task<JObject> ForgotPassword(ForgotPasswordViewModel vm) {
            
            var body = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("Email", vm.Email)
            });

            JObject obj = JObject.Parse(await HttpService.PostForm("Account/ForgotPassword", body, true));
            return obj;
        }

        public async Task<JObject> ResetPassword(ResetPasswordViewModel vm) {

            var body = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("Email", vm.Email),
                new KeyValuePair<string, string>("Password", vm.Password),
                new KeyValuePair<string, string>("ConfirmPassword", vm.ConfirmPassword),
                new KeyValuePair<string, string>("Code", vm.Code),
            });

            JObject obj = JObject.Parse(await HttpService.PostForm("Account/ResetPassword", body, true));
            return obj;
        }

        public async void Logout() {
            await HttpService.Get("Account/Logout", false);
            HttpService.ClearToken();
        }

        #endregion


        #region USERS

        public async Task<User> GetCurrentUser() {
            return await GetUser("");
        }

        public async Task<User> GetUser(string id) {
            JObject obj = JObject.Parse(await HttpService.Get("Users/" + id));
            User user = obj.ToObject<User>();
            return user;
        }

        public async Task<List<List>> GetOwnedLists() {
            JArray obj = JArray.Parse(await HttpService.Get("Users/Lists"));
            List<List> lists = obj.ToObject<List<List>>();
            return lists;
        }

        public async Task<List<List>> GetOwnedListsAsync() {
            JArray obj = JArray.Parse(await HttpService.Get("Users/Lists"));
            List<List> lists = obj.ToObject<List<List>>();
            return lists;
        }

        public async Task<List<List>> GetSubscribedLists() {
            JArray obj = JArray.Parse(await HttpService.Get("Users/Subscriptions"));
            List<List> lists = obj.ToObject<List<List>>();
            return lists;
        }

        public async void RequestAccess(string emailaddress) {
            await HttpService.Post("Users/" + emailaddress, null);
        }

        #endregion


        #region LISTS

        public async Task<List> GetList(int id) {
            JObject obj = JObject.Parse(await HttpService.Get("Lists/" + id));
            List list = obj.ToObject<List>();
            return list;
        }

        // TODO: implement adding and editing lists

        public async Task<JObject> NewList(List list) {
            JObject body = JObject.FromObject(list);
            JObject obj = JObject.Parse(await HttpService.Post("Lists", body, true));
            return obj;
        }

        public async Task<JObject> EditList(List list) {
            JObject body = JObject.FromObject(list);
            JObject obj = JObject.Parse(await HttpService.Put("Lists/" + list.ListId, body, true));
            return obj;
        }

        public async void SendInvitations(List list) {
            await HttpService.Post("Lists/" + list.ListId, null);
        }

        public async void UnsubscribeFromList(List list) {
            // this is fine, a subscriber cant remove the list and the owner cant subscribe to the list
            DeleteList(list); 
        }

        public async void DeleteList(List list) {
            HttpService.Delete("Lists/" + list.ListId);
        }

        #endregion


        #region ITEMS

        public async void MarkItem(Item item) {
            HttpService.Put("Items/" + item.ItemId, null);
        }

        public async void UnMarkItem(Item item) {
            MarkItem(item);
        }

        // TODO: implement adding and editing items

        public async Task<JObject> NewItem(Item item) {
            JObject body = JObject.FromObject(item);
            JObject obj = JObject.Parse(await HttpService.Post("Items", body, true));
            return obj;
        }

        public async Task<JObject> EditItem(Item item) {
            JObject body = JObject.FromObject(item);
            JObject obj = JObject.Parse(await HttpService.Post("Items/" + item.ItemId, body, true));
            return obj;
        }

        public async void DeleteItem(Item item) {
            HttpService.Delete("Items/" + item.ItemId);
        }

        #endregion


        #region NOTIFICATIONS

        private List<Notification> Notifications;

        public async Task<List<Notification>> GetNotifications() {
            JArray obj = JArray.Parse(await HttpService.Get("Notifications"));
            Notifications = obj.ToObject<List<Notification>>();
            return Notifications;
        }

        public async Task<List<Notification>> GetNotificationsAsync() {
            JArray obj = JArray.Parse(await HttpService.Get("Notifications"));
            Notifications = obj.ToObject<List<Notification>>();
            return Notifications;
        }

        public int GetUnreadNotificationCount() {
            return Notifications.FindAll(n => n.IsUnread).Count;
        }

        public async void MarkAllNotificationsAsRead() {
            HttpService.Put("Notifications", null);
        }

        public async void MarkNotification(Notification notif) {
            HttpService.Put("Notifications/" + notif.NotificationId, null);
        }

        public async void ActOnNotification(Notification notif) {
            await HttpService.Post("Notifications/" + notif.NotificationId, null);
        }

        #endregion

    }
}
