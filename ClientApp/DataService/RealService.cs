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

            JObject obj = HttpService.Post("Account/ResetPassword", body);
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

            JObject obj = HttpService.Post("Account/ResetPassword", body);
            HttpService.TrySettingToken(obj);

            return obj;
        }

        public dynamic ChangePassword(ChangePasswordViewModel vm) {
            throw new NotImplementedException();
        }

        public dynamic EditAccount(EditAccountViewModel vm) {
            throw new NotImplementedException();
        }

        public dynamic ForgotPassword(ForgotPasswordViewModel vm) {
            
            var body = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("Email", vm.Email)
            });

            JObject obj = HttpService.Post("Account/ForgotPassword", body);
            return obj;
        }

        public dynamic ResetPassword(ResetPasswordViewModel vm) {

            var body = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("Email", vm.Email),
                new KeyValuePair<string, string>("Password", vm.Password),
                new KeyValuePair<string, string>("ConfirmPassword", vm.ConfirmPassword),
                new KeyValuePair<string, string>("Code", vm.Code),
            });

            JObject obj = HttpService.Post("Account/ResetPassword", body);
            return obj;
        }

        public void Logout() {
            HttpService.Get("Account/Logout");
            HttpService.ClearToken();
        }

        #endregion


        #region USERS

        public User GetCurrentUser() {
            return GetUser("");
        }

        public User GetUser(string id) {
            JObject obj = HttpService.Get("Users/" + id);
            User user = obj.ToObject<User>();
            return user;
        }

        public List<List> GetOwnedLists() {
            JObject obj = HttpService.Get("Users/Lists");
            List<List> lists = obj.ToObject<List<List>>();
            return lists;
        }

        public List<List> GetSubscribedLists() {
            JObject obj = HttpService.Get("Users/Subscriptions");
            List<List> lists = obj.ToObject<List<List>>();
            return lists;
        }

        public void RequestAccess(string emailaddress) {
            HttpService.Post("Users/" + emailaddress, null);
        }

        #endregion


        #region LISTS

        public List GetList(int id) {
            JObject obj = HttpService.Get("Lists/" + id);
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

        public List<Notification> GetNotifications() {
            JObject obj = HttpService.Get("Notifications");
            List<Notification> notifs = obj.ToObject<List<Notification>>();
            return notifs;
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
