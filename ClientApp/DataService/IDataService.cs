using ClientApp.Models;
using ClientApp.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientApp.DataService {

    public interface IDataService {

        Loading LoadingIndicator { get; set; }

        // ACCOUNT

        bool IsLoggedIn();

        Task<JObject> Login(LoginViewModel vm);

        Task<JObject> Register(RegisterViewModel vm);

        Task<JObject> ChangePassword(ChangePasswordViewModel vm);

        Task<JObject> ForgotPassword(ForgotPasswordViewModel vm);

        Task<JObject> ResetPassword(ResetPasswordViewModel vm);

        Task<JObject> EditAccount(EditAccountViewModel vm);

        void Logout();


        // USERS

        Task<User> GetCurrentUser();

        Task<User> GetUser(string id);

        Task<List<List>> GetOwnedLists();

        Task<List<List>> GetSubscribedLists();

        void RequestAccess(string emailaddress);


        // LISTS

        Task<List> GetList(int id);

        Task<JObject> NewList(List list);

        Task<JObject> EditList(List list);

        void SendInvitations(List list);

        void UnsubscribeFromList(List list);
        
        void DeleteList(List list);


        // ITEMS

        void MarkItem(Item item);

        void UnMarkItem(Item item);

        Task<JObject> NewItem(Item item);

        Task<JObject> EditItem(Item item);

        void DeleteItem(Item item);


        // NOTIFICATIONS

        Task<List<Notification>> GetNotifications();

        int GetUnreadNotificationCount();

        void MarkAllNotificationsAsRead();

        void MarkNotification(Notification notification);

        void ActOnNotification(Notification notification);

    }
}
