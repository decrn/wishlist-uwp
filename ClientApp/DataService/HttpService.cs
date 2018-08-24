using Microsoft.Toolkit.Uwp.UI.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.Security.Credentials;

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

            try {
                JWTToken = Vault.Retrieve("Wishlist", "token").Password;
            } catch { }
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
                    HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);
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

        public async Task<string> Handle(Func<Task<HttpResponseMessage>> apicall, bool showLoading = true) {
            if (showLoading && LoadingIndicator != null) LoadingIndicator.IsLoading = true;
            var response = "";

            try {
                HttpResponseMessage res = await apicall();
                if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                    //TODO: remember credentials to refresh auth
                    throw new Exception("Token invalid!");
                }
                response = await res.Content.ReadAsStringAsync();

            } catch {
                throw new Exception("Server Offline!");
            }

            if (showLoading && LoadingIndicator != null) LoadingIndicator.IsLoading = false;
            return response;
        }


        public async Task<string> Get(string path, bool showLoading = true) {
            return await Handle(() => HttpClient.GetAsync(new Uri(BaseUri + path)), showLoading);
        }


        public async Task<string> Post(string path, JObject json, bool showLoading = false) {
            var body = new StringContent("");
            if (json != null)
                body = new StringContent(json.ToString());
            body.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return await Handle(() => HttpClient.PostAsync(new Uri(BaseUri + path), body), showLoading);
        }


        public async Task<string> PostForm(string path, HttpContent body, bool showLoading = false) {
            return await Handle(() => HttpClient.PostAsync(new Uri(BaseUri + path), body), showLoading);
        }


        public async Task<string> Put(string path, JObject json, bool showLoading = false) {
            var body = new StringContent("");
            if (json != null)
                body = new StringContent(json.ToString());
            body.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return await Handle(() => HttpClient.PutAsync(new Uri(BaseUri + path), body), showLoading);
        }


        public async Task<string> Patch(string path, JObject json, bool showLoading = false) {
            var body = new StringContent("");
            if (json != null)
                body = new StringContent(json.ToString());
            body.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, new Uri(BaseUri + path)) { Content = body };
            return await Handle(() => HttpClient.SendAsync(request), showLoading);
        }


        public async Task<string> Delete(string path, bool showLoading = false) {
            return await Handle(() => HttpClient.DeleteAsync(new Uri(BaseUri + path)), showLoading);
        }

        #endregion

    }
}
