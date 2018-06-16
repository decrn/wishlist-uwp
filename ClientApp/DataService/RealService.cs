using ClientApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HttpClient = System.Net.Http.HttpClient;
using ClientApp.ViewModels;

namespace ClientApp.DataService {

    public class RealService : IDataService {

        // TODO: add super class or helper to deal with to/from json conversion & http calls

        public static readonly string Name = "Real Data Service";
        public static readonly string BaseUri = "http://localhost:64042/api/";

        private static Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        private string JWTToken = localSettings.Values.ContainsKey("JWTToken") ? localSettings.Values["JWTToken"].ToString() : "";
        private User LoggedInUser;


        #region ACCOUNT

        private bool ValidateJWT(string obj) {
            try {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadToken(obj) as JwtSecurityToken;
                return true;
            } catch (Exception e) {
                return false;
            }
        }

        public bool IsLoggedIn() {
            return JWTToken != "";
        }

        public dynamic Login(string email, string password) {

            var httpClient = new HttpClient();
            var content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("Email", email),
                new KeyValuePair<string, string>("Password", password),
            });

            var response = "";
            Task task = Task.Run(async () => {
                var res = await httpClient.PostAsync(new Uri(BaseUri + "Account/Login"), content);
                response = await res.Content.ReadAsStringAsync();
            });
            task.Wait();

            JObject obj = JObject.Parse(response);
            
            if (obj.ContainsKey("success") && obj["success"].ToString() == "True") {
                var token = obj["data"]["token"].ToString();
                if (ValidateJWT(token)) {
                    localSettings.Values["JWTToken"] = token;
                    LoggedInUser = obj["data"]["user"].ToObject<User>();
                }
            }

            return obj;
            
        }

        public dynamic Register(RegisterViewModel vm) {

            var httpClient = new HttpClient();
            
            var content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("FirstName", vm.FirstName),
                new KeyValuePair<string, string>("LastName", vm.LastName),
                new KeyValuePair<string, string>("Email", vm.Email),
                new KeyValuePair<string, string>("Password", vm.Password),
                new KeyValuePair<string, string>("ConfirmPassword", vm.ConfirmPassword),
            });

            var response = "";
            Task task = Task.Run(async () => {
                var res = await httpClient.PostAsync(new Uri(BaseUri + "Account/Register"), content);
                response = await res.Content.ReadAsStringAsync();
            });
            task.Wait();

            JObject obj = JObject.Parse(response);

            if (obj.ContainsKey("success") && obj["success"].ToString() == "True") {
                var token = obj["data"]["token"].ToString();
                if (ValidateJWT(token)) {
                    localSettings.Values["JWTToken"] = token;
                    LoggedInUser = obj["data"]["user"].ToObject<User>();
                }
            }
            return obj;
        }

        public dynamic ChangePassword(string oldpassword, string newpassword, string confirmpassword) {
            throw new NotImplementedException();
        }

        public dynamic EditAccount(EditAccountViewModel vm) {
            throw new NotImplementedException();
        }

        public void Logout() {

            var httpClient = new HttpClient();
            Task task = Task.Run(async () => {
                var res = await httpClient.GetAsync(new Uri(BaseUri + "Account/Logout"));
            });
            task.Wait();

            JWTToken = "";
            LoggedInUser = null;
            localSettings.Values.Remove("JWTToken");
        }

        #endregion

        #region USERS

        public User GetCurrentUser() {
            return GetUser("");
        }

        public User GetUser(string id) {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);
            var response = "";
            Task task = Task.Run(async () => {
                var res = await httpClient.GetAsync(new Uri(BaseUri + "Users/"+ id));
                response = await res.Content.ReadAsStringAsync();
            });
            task.Wait();

            JObject obj = JObject.Parse(response);
            User user = obj.ToObject<User>();

            return user;
        }

        public List<List> GetOwnedLists() {

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            var response = "";
            Task task = Task.Run(async () => {
                response = await httpClient.GetStringAsync(new Uri(BaseUri + "Users/Lists"));
            });
            task.Wait();

            JArray obj = JArray.Parse(response);
            List<List> lists = obj.ToObject<List<List>>();

            return lists;

        }

        public List<List> GetSubscribedLists() {

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            var response = "";
            Task task = Task.Run(async () => {
                response = await httpClient.GetStringAsync(new Uri(BaseUri + "Users/Subscriptions")); // sends GET request
            });
            task.Wait();

            JArray obj = JArray.Parse(response);
            List<List> lists = obj.ToObject<List<List>>();

            return lists;
        }

        #endregion

        #region LISTS

        public List GetList(int id) {

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            var response = "";
            Task task = Task.Run(async () => {
                response = await httpClient.GetStringAsync(new Uri(BaseUri + "Lists/"+id)); // sends GET request
            });
            task.Wait();

            JArray obj = JArray.Parse(response);
            List list = obj.ToObject<List>();

            return list;
        }

        public List<Item> GetListItems(List list) {

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            string response = "";
            Task task = Task.Run(async () => {
                response = await httpClient.GetStringAsync(new Uri(BaseUri + "Lists/" + list.ListId + "/Items")); // sends GET request
            });
            task.Wait();

            JArray obj = JArray.Parse(response);
            List<Item> items = obj.ToObject<List<Item>>();

            return items;
        }

        public dynamic NewList(List list) {
            throw new NotImplementedException();
        }

        public dynamic EditList(List list) {

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            StringContent content = new StringContent(
                JsonConvert.SerializeObject(list),
                System.Text.Encoding.UTF8
            );
            httpClient.PostAsync(new Uri(BaseUri + "Lists/"+list.ListId), content);

            return null;
        }

        public void SendInvitations(List list) {
            throw new NotImplementedException();
        }
        
        public void DeleteList(List list) {

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);
            httpClient.DeleteAsync(new Uri(BaseUri + "Lists/" + list.ListId));
        }

        #endregion

        #region ITEMS

        public void MarkItem(Item item) {
            throw new NotImplementedException();
        }

        public void UnMarkItem(Item item) {
            throw new NotImplementedException();
        }

        public dynamic NewItem(Item item) {
            throw new NotImplementedException();
        }

        public dynamic EditItem(Item item) {
            throw new NotImplementedException();
        }

        public void DeleteItem(Item item) {
            throw new NotImplementedException();
        }

        #endregion

        #region NOTIFICATIONS

        public List<Notification> GetNotifications() {

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            string response = "";
            Task task = Task.Run(async () => {
                response = await httpClient.GetStringAsync(new Uri(BaseUri + "Notifications"));
            });
            task.Wait();

            JArray obj = JArray.Parse(response);
            List<Notification> notifs = obj.ToObject<List<Notification>>();

            return notifs;
        }

        public void MarkAllNotificationsAsRead() {
            throw new NotImplementedException();
        }

        public void MarkNotificationAsRead(Notification notification) {
            throw new NotImplementedException();
        }

        #endregion
    }
}
