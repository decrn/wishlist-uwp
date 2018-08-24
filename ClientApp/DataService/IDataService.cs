using ClientApp.Models;
using ClientApp.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientApp.DataService {

    public interface IDataService {

        Loading LoadingIndicator { get; set; }
        
        User LoggedInUser { get; set; }

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

        Task SendInvitations(List list);

        Task UnsubscribeFromList(List list);
        
        Task DeleteList(List list);


        // ITEMS

        void MarkItem(Item item);

        void UnMarkItem(Item item);

        Task<JObject> NewItem(Item item);

        Task<JObject> EditItem(Item item);

        Task DeleteItem(Item item);


        // NOTIFICATIONS

        Task<List<Notification>> GetNotifications();

        Task MarkAllNotificationsAsRead();

        Task MarkNotification(Notification notification);

        Task ActOnNotification(Notification notification);

    }
}
