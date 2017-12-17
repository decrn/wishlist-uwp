using ClientApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using Newtonsoft.Json;
using HttpClient = System.Net.Http.HttpClient;

namespace ClientApp.DataService {
    public class RealService {
        public static String Name = "Real Data Service";
        // temp token for testing
        public static String JWTToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhQGRvbWFpbi5jb20iLCJqdGkiOiI3NmFlMzMwMi1kMjNhLTQyY2EtODQ2OS1kMTk4ZjExYzMwZGUiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjExZjY3YmUzLWNhMDktNDRkYS1iNGRhLWZjYzYxOTVmZGQ3NSIsImV4cCI6MTUxNTg2MTE0MSwiaXNzIjoibG9jYWxob3N0IiwiYXVkIjoibG9jYWxob3N0In0.Y847BwdSZh9qK-kI8MUFxMAHXrKslZHL4KNaSUNjIIs";

        public bool IsLoggedIn {
            get { return JWTToken != ""; }
        }

        public static void Login(string email, string password) {
            Debug.WriteLine("GET /login/ for JWT Token");

            var httpClient = new HttpClient();
            var content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("Email", email),
                new KeyValuePair<string, string>("Password", password),
            });

            httpClient.PostAsync(new Uri(App.BaseUri + "Account/Login"), content);
        }

        public static void Logout() {
            Debug.WriteLine("Logout");
            JWTToken = "";
        }

        public static List<List> GetSubscribedLists() {
            Debug.WriteLine("GET for Subscribed Lists.");

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            var response = "";
            Task task = Task.Run(async () => {
                response = await httpClient.GetStringAsync(new Uri(App.BaseUri + "Users/Subscriptions")); // sends GET request
            });
            task.Wait();
            // TODO: convert int representation of color to Color
            return JsonConvert.DeserializeObject<List<List>>(response);
        }

        public static List<List> GetOwnedLists() {
            Debug.WriteLine("GET for Owned Lists.");

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            var response = "";
            Task task = Task.Run(async () => {
                response = await httpClient.GetStringAsync(new Uri(App.BaseUri + "Users/Lists")); // sends GET request
            });
            task.Wait();
            // TODO: convert int representation of color to Color
            return JsonConvert.DeserializeObject<List<List>>(response);
        }

        // probably not gonna work? We called observable in een observable maar let's see I guess
        public static List<Item> GetListItems(List list) {
            Debug.WriteLine("GET items for list with name " + list.Name);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            string response = "";
            Task task = Task.Run(async () => {
                response = await httpClient.GetStringAsync(new Uri(App.BaseUri + "Lists/"+list.ListId)); // sends GET request
            });
            task.Wait();
            Debug.WriteLine("resp: "+response);
            return JsonConvert.DeserializeObject<List>(response).Items;
        }

        public static void Write(List list) {
            Debug.WriteLine("POST List with name " + list.Name);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            StringContent content = new StringContent(
                JsonConvert.SerializeObject(list),
                System.Text.Encoding.UTF8
            );
            httpClient.PostAsync(new Uri(App.BaseUri + "Lists/"+list.ListId), content);
        }
        
        public static void Delete(List list) {
            Debug.WriteLine("DELETE List with name " + list.Name);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);
            httpClient.DeleteAsync(new Uri(App.BaseUri + "Lists/" + list.ListId));
        }

    }
}
