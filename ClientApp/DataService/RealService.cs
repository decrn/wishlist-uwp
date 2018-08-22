using ClientApp.Models;
using ClientApp.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;


namespace ClientApp.DataService {

    public class RealService : IDataService {

        private HttpService HttpService = new HttpService();

        public Loading LoadingIndicator { get { return HttpService.LoadingIndicator; } set { HttpService.LoadingIndicator = value; }  }


        #region ACCOUNT

        public bool IsLoggedIn() {
            return HttpService.IsTokenSet();
        }

        public dynamic Login(LoginViewModel vm) {
            
            var body = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("Email", vm.Email),
                new KeyValuePair<string, string>("Password", vm.Password),
            });

            JObject obj = JObject.Parse(HttpService.Post("Account/Login", body, true));
            HttpService.TrySettingToken(obj);

            return obj;
        }

        public dynamic Register(RegisterViewModel vm) {

            var body = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("FirstName", vm.FirstName),
                new KeyValuePair<string, string>("LastName", vm.LastName),
                new KeyValuePair<string, string>("Email", vm.Email),
                new KeyValuePair<string, string>("Password", vm.Password),
                new KeyValuePair<string, string>("ConfirmPassword", vm.ConfirmPassword),
            });

            JObject obj = JObject.Parse(HttpService.Post("Account/Register", body, true));
            HttpService.TrySettingToken(obj);

            return obj;
        }

        public dynamic ChangePassword(ChangePasswordViewModel vm) {
            var body = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("OldPassword", vm.OldPassword),
                new KeyValuePair<string, string>("NewPassword", vm.NewPassword),
                new KeyValuePair<string, string>("ConfirmPassword", vm.ConfirmPassword),
            });

            JObject obj = JObject.Parse(HttpService.Post("Account/Password", body, true));

            return obj;
        }

        public dynamic EditAccount(EditAccountViewModel vm) {
            var body = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("FirstName", vm.FirstName),
                new KeyValuePair<string, string>("LastName", vm.LastName),
                new KeyValuePair<string, string>("Email", vm.Email)
            });

            JObject obj = JObject.Parse(HttpService.Post("Account/Edit", body, true));

            return obj;
        }

        public dynamic ForgotPassword(ForgotPasswordViewModel vm) {
            
            var body = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("Email", vm.Email)
            });

            JObject obj = JObject.Parse(HttpService.Post("Account/ForgotPassword", body, true));
            return obj;
        }

        public dynamic ResetPassword(ResetPasswordViewModel vm) {

            var body = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("Email", vm.Email),
                new KeyValuePair<string, string>("Password", vm.Password),
                new KeyValuePair<string, string>("ConfirmPassword", vm.ConfirmPassword),
                new KeyValuePair<string, string>("Code", vm.Code),
            });

            JObject obj = JObject.Parse(HttpService.Post("Account/ResetPassword", body, true));
            return obj;
        }

        public void Logout() {
            HttpService.Get("Account/Logout", false);
            HttpService.ClearToken();
        }

        #endregion


        #region USERS

        public User GetCurrentUser() {
            return GetUser("");
        }

        public User GetUser(string id) {
            JObject obj = JObject.Parse(HttpService.Get("Users/" + id));
            User user = obj.ToObject<User>();
            return user;
        }

        public List<List> GetOwnedLists() {
            JArray obj = JArray.Parse(HttpService.Get("Users/Lists"));
            List<List> lists = obj.ToObject<List<List>>();
            return lists;
        }

        public List<List> GetSubscribedLists() {
            JArray obj = JArray.Parse(HttpService.Get("Users/Subscriptions"));
            List<List> lists = obj.ToObject<List<List>>();
            return lists;
        }

        public void RequestAccess(string emailaddress) {
            HttpService.Post("Users/" + emailaddress, null);
        }

        #endregion


        #region LISTS

        public List GetList(int id) {
            JObject obj = JObject.Parse(HttpService.Get("Lists/" + id));
            List list = obj.ToObject<List>();
            return list;
        }

        // TODO: implement adding and editing lists

        public dynamic NewList(List list) {
            throw new NotImplementedException();
        }

        public dynamic EditList(List list) {
            throw new NotImplementedException();
        }

        public void SendInvitations(List list) {
            HttpService.Post("Lists/" + list.ListId, null);
        }

        public void UnsubscribeFromList(List list) {
            // this is fine, a subscriber cant remove the list and the owner cant subscribe to the list
            DeleteList(list); 
        }

        public void DeleteList(List list) {
            HttpService.Delete("Lists/" + list.ListId);
        }

        #endregion


        #region ITEMS

        public void MarkItem(Item item) {
            HttpService.Put("Items/" + item.ItemId, null);
        }

        public void UnMarkItem(Item item) {
            MarkItem(item);
        }

        // TODO: implement adding and editing items

        public dynamic NewItem(Item item) {
            throw new NotImplementedException();
        }

        public dynamic EditItem(Item item) {
            throw new NotImplementedException();
        }

        public void DeleteItem(Item item) {
            HttpService.Delete("Items/" + item.ItemId);
        }

        #endregion


        #region NOTIFICATIONS

        private List<Notification> Notifications;

        public List<Notification> GetNotifications() {
            JArray obj = JArray.Parse(HttpService.Get("Notifications"));
            Notifications = obj.ToObject<List<Notification>>();
            return Notifications;
        }

        public int GetUnreadNotificationCount() {
            return Notifications.FindAll(n => n.IsUnread).Count;
        }

        public void MarkAllNotificationsAsRead() {
            HttpService.Put("Notifications", null);
        }

        public void MarkNotification(Notification notif) {
            HttpService.Put("Notifications/" + notif.NotificationId, null);
        }

        public void ActOnNotification(Notification notif) {
            HttpService.Post("Notifications/" + notif.NotificationId, null);
        }

        #endregion

    }
}
