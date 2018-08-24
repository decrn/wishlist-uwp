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

        public User LoggedInUser { get; set; }


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
            LoggedInUser = null;
        }

        #endregion


        #region USERS

        public async Task<User> GetCurrentUser() {
            LoggedInUser = await GetUser("");
            return LoggedInUser;
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

            // convert UserListSubscribe model from server to Users
            JArray subs = obj["subscribedUsers"] as JArray;
            for (int i = 0; i < subs.Count; i++) {
                subs[i] = subs[i]["user"];
            }

            // convert UserListInvite model from server to Users
            JArray invited = obj["invitedUsers"] as JArray;
            for (int i = 0; i < invited.Count; i++) {
                invited[i] = invited[i]["user"];
            }

            List list = obj.ToObject<List>();
            return list;
        }

        public async Task<JObject> NewList(List list) {
            JObject body = JObject.FromObject(list);
            JObject obj = JObject.Parse(await HttpService.Post("Lists", body, true));
            return obj;
        }

        public async Task<JObject> EditList(List list) {
            JObject body = JObject.FromObject(list);

            // convert User models to UserListInvite model for server
            JArray invites = body["InvitedUsers"] as JArray;
            for (int i = 0; i < invites.Count; i++) {
                var user = invites[i];
                var invite = new JObject();
                invite["id"] = 0;
                invite["user"] = user;
                invites[i] = invite;
            }

            // remove unneeded properties
            body.Remove("SubscribedUsers");

            JObject obj = JObject.Parse(await HttpService.Put("Lists/" + list.ListId, body, true));
            return obj;
        }

        public async Task SendInvitations(List list) {
            await HttpService.Post("Lists/" + list.ListId, null);
        }

        public async Task UnsubscribeFromList(List list) {
            // this is fine, a subscriber cant remove the list and the owner cant subscribe to the list
            await DeleteList(list); 
        }

        public async Task DeleteList(List list) {
            await HttpService.Delete("Lists/" + list.ListId);
        }

        #endregion


        #region ITEMS

        public async void MarkItem(Item item) {
            HttpService.Put("Items/" + item.ItemId, null);
        }

        public async void UnMarkItem(Item item) {
            MarkItem(item);
        }

        // isn't used, never tested but should work
        public async Task<JObject> NewItem(Item item) {
            JObject body = JObject.FromObject(item);
            JObject obj = JObject.Parse(await HttpService.Post("Items", body, true));
            return obj;
        }

        // isn't used, never tested but should work
        public async Task<JObject> EditItem(Item item) {
            JObject body = JObject.FromObject(item);
            JObject obj = JObject.Parse(await HttpService.Post("Items/" + item.ItemId, body, true));
            return obj;
        }

        public async Task DeleteItem(Item item) {
            await HttpService.Delete("Items/" + item.ItemId);
        }

        #endregion


        #region NOTIFICATIONS

        public async Task<List<Notification>> GetNotifications() {
            JArray obj = JArray.Parse(await HttpService.Get("Notifications"));
            return obj.ToObject<List<Notification>>(); ;
        }

        public async Task MarkAllNotificationsAsRead() {
            await HttpService.Put("Notifications", null);
        }

        public async Task MarkNotification(Notification notif) {
            await HttpService.Put("Notifications/" + notif.NotificationId, null);
        }

        public async Task ActOnNotification(Notification notif) {
            await HttpService.Post("Notifications/" + notif.NotificationId, null);
        }

        #endregion

    }
}
