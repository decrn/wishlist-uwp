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

    public interface IDataService {

        // ACCOUNT

        bool IsLoggedIn();

        dynamic Login(string email, string password);

        dynamic Register(string email, string password);

        void Logout();

        // LISTS

        List<List> GetSubscribedLists();

        List<List> GetOwnedLists();

        List<Item> GetListItems(List list);

        void Write(List list);

        void Delete(List list);

    }
}
