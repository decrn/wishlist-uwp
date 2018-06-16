using ClientApp.Models;
using ClientApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace ClientApp.DataService {

    public class FakeService : IDataService {

        public static String Name = "Fake Data Service";
        public static User LoggedInUser;
        public static String JWTToken = "";

        // USER

        public bool IsLoggedIn() {
            return JWTToken != "";
        }        

        public dynamic Login(string email, string password) {
            Debug.WriteLine("GET /login/ for JWT Token with email " + email);
            JWTToken = "temp";
            return JWTToken;
        }

        public dynamic Register(RegisterViewModel vm) {
            Debug.WriteLine("GET /register/ for JWT Token with email " + vm.Email);
            JWTToken = "temp";
            return JWTToken;
        }

        public void Logout() {
            Debug.WriteLine("Logout");
            JWTToken = "";
            LoggedInUser = null;
        }

        public dynamic ChangePassword(string oldpassword, string newpassword, string confirmpassword) {
            throw new NotImplementedException();
        }

        public dynamic EditAccount(EditAccountViewModel vm) {
            throw new NotImplementedException();
        }

        public User GetCurrentUser() {
            throw new NotImplementedException();
        }

        public User GetUser(string id) {
            throw new NotImplementedException();
        }

        public List<List> GetOwnedLists() {
            throw new NotImplementedException();
        }

        public List<List> GetSubscribedLists() {
            throw new NotImplementedException();
        }

        public List<List> GetUsersPublicLists(string id) {
            throw new NotImplementedException();
        }

        public List GetList(string id) {
            throw new NotImplementedException();
        }

        public dynamic NewList(List list) {
            throw new NotImplementedException();
        }

        public List<Item> GetListItems(List list) {
            throw new NotImplementedException();
        }

        public dynamic EditList(List list) {
            throw new NotImplementedException();
        }

        public void SendInvitations(List list) {
            throw new NotImplementedException();
        }

        public void DeleteList(List list) {
            throw new NotImplementedException();
        }

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

        public List<Notification> GetNotifications() {
            throw new NotImplementedException();
        }

        public void MarkAllNotificationsAsRead() {
            throw new NotImplementedException();
        }

        public void MarkNotificationAsRead(Notification notification) {
            throw new NotImplementedException();
        }
    }
}
