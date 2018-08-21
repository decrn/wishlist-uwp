using Microsoft.Toolkit.Uwp.UI.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.Securtiy.Credentials;

namespace ClientApp.DataService {
    class HttpService {


        public static readonly string BaseUri = "http://localhost:64042/api/";
        private string JWTToken = "";

        PasswordVault Vault;

        // Toggle indicator with LoadingIndicator.IsLoading
        public Loading LoadingIndicator { get; set; }

        HttpClient HttpClient;

        public HttpService() {
            HttpClient = new HttpClient();
            Vault = new PasswordVault();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);
        }


        #region Auth

        public bool IsTokenSet() {
            return JWTToken != "";
        }

        public bool TrySettingToken(JObject obj) {

            if (obj.ContainsKey("success") && obj["success"].ToString() == "True") {
                var token = obj["data"]["token"].ToString();
                if (ValidateJWT(token)) {
                    JWTToken = token;
                    Vault.Add(new PasswordCredential("Wishlist", "token", token));
                    return true;
                }
            }

           return false;
        }

        public void ClearToken() {
            Vault.Remove(new PasswordCredential("Wishlist", "token", JWTToken));
            JWTToken = "";
        }

        private bool ValidateJWT(string obj) {
            try {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadToken(obj) as JwtSecurityToken;
                return true;
            } catch (Exception e) {
                return false;
            }
        }

        #endregion

        #region Requests

        public dynamic Handle(Func<Task<HttpResponseMessage>> apicall, bool showLoading = true) {

            var response = "";
            Task task = Task.Run(async () => {
                var res = await apicall();
                response = await res.Content.ReadAsStringAsync();
            });
            task.Wait();

            return response;
        }


        public JObject Get(string path) {
            if (showLoading && LoadingIndicator != null) LoadingIndicator.IsLoading = true;

            var response = Handle(() => HttpClient.GetAsync(new Uri(BaseUri + path)) );

            if (showLoading && LoadingIndicator != null) LoadingIndicator.IsLoading = false;

            return JObject.Parse(response);
        }


        public dynamic Post(string path, HttpContent body) {
            var response = Handle(() => HttpClient.PostAsync( new Uri(BaseUri + path), body) );
            return response;
        }


        public dynamic Put(string path, HttpContent body) {
            var response = Handle(() => HttpClient.PutAsync(new Uri(BaseUri + path), body));
            return response;
        }


        public dynamic Patch(string path, HttpContent body) {

            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, new Uri(BaseUri + path)) { Content = body };

            var response = Handle(() => HttpClient.SendAsync(request));
            return response;
        }


        public dynamic Delete(string path) {
            var response = Handle(() => HttpClient.DeleteAsync(new Uri(BaseUri + path)));
            return response;
        }

        #endregion

    }
}
