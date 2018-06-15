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

namespace ClientApp.DataService {

    public class RealService : IDataService {

        static Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public static String Name = "Real Data Service";
        public static readonly string BaseUri = "http://localhost:64042/api/";

        public static String JWTToken = localSettings.Values.ContainsKey("JWTToken") ? localSettings.Values["JWTToken"].ToString() : "";

        // ACCOUNT

        public bool validJWT(string obj) {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadToken(obj) as JwtSecurityToken;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool IsLoggedIn() {
            return JWTToken != "";
        }

        public dynamic Login(string email, string password) {
            Debug.WriteLine("GET /login/ for JWT Token with email " + email);

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

            var obj = JsonConvert.DeserializeObject<object>(response);
            if (validJWT(obj.ToString()))
                localSettings.Values["JWTToken"] = obj.ToString();
            
            return obj;
        }

        public dynamic Register(string email, string password) {
            Debug.WriteLine("GET /register/ for JWT Token with email "+ email);

            var httpClient = new HttpClient();
            var content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("Email", email),
                new KeyValuePair<string, string>("Password", password),
            });

            var response = "";
            Task task = Task.Run(async () => {
                var res = await httpClient.PostAsync(new Uri(BaseUri + "Account/Register"), content);
                response = await res.Content.ReadAsStringAsync();
            });
            task.Wait();

            var obj = JsonConvert.DeserializeObject<object>(response);
            Debug.WriteLine(obj);
            return obj;
        }

        public void Logout() {
            Debug.WriteLine("Logout");
            JWTToken = "";
        }

        // LISTS

        public List<List> GetSubscribedLists() {
            Debug.WriteLine("GET for Subscribed Lists.");

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            var response = "";
            Task task = Task.Run(async () => {
                response = await httpClient.GetStringAsync(new Uri(BaseUri + "Users/Subscriptions")); // sends GET request
            });
            task.Wait();
            // TODO: convert int representation of color to Color
            return JsonConvert.DeserializeObject<List<List>>(response);
        }

        public List<List> GetOwnedLists() {
            Debug.WriteLine("GET for Owned Lists.");

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            var response = "";
            Task task = Task.Run(async () => {
                response = await httpClient.GetStringAsync(new Uri(BaseUri + "Users/Lists")); // sends GET request
            });
            task.Wait();
            // TODO: convert int representation of color to Color
            return JsonConvert.DeserializeObject<List<List>>(response);
        }

        public List<Item> GetListItems(List list) {
            Debug.WriteLine("GET items for list with name " + list.Name);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            string response = "";
            Task task = Task.Run(async () => {
                response = await httpClient.GetStringAsync(new Uri(BaseUri + "Lists/"+list.ListId+"/Items")); // sends GET request
            });
            task.Wait();
            var obj = JsonConvert.DeserializeObject<List<Item>>(response);
            return obj;
        }

        public void Write(List list) {
            Debug.WriteLine("POST List with name " + list.Name);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            StringContent content = new StringContent(
                JsonConvert.SerializeObject(list),
                System.Text.Encoding.UTF8
            );
            httpClient.PostAsync(new Uri(BaseUri + "Lists/"+list.ListId), content);
        }
        
        public void Delete(List list) {
            Debug.WriteLine("DELETE List with name " + list.Name);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);
            httpClient.DeleteAsync(new Uri(BaseUri + "Lists/" + list.ListId));
        }

    }
}
