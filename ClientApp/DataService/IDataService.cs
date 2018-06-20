using ClientApp.Models;
using ClientApp.ViewModels;
using System.Collections.Generic;

namespace ClientApp.DataService {

    public interface IDataService {


        // ACCOUNT

        bool IsLoggedIn();

        dynamic Login(string email, string password);

        dynamic Register(RegisterViewModel vm);

        dynamic ChangePassword(string oldpassword, string newpassword, string confirmpassword);

        dynamic ForgotPassword(string email);

        dynamic ResetPassword(ResetPasswordViewModel vm);

        dynamic EditAccount(EditAccountViewModel vm);

        void Logout();


        // USERS

        User GetCurrentUser();

        User GetUser(string id);

        List<List> GetOwnedLists();

        List<List> GetSubscribedLists();


        // LISTS

        List GetList(int id);

        List<Item> GetListItems(List list);

        dynamic NewList(List list);

        dynamic EditList(List list);

        void SendInvitations(List list);

        void DeleteList(List list);


        // ITEMS

        void MarkItem(Item item);

        void UnMarkItem(Item item);

        dynamic NewItem(Item item);

        dynamic EditItem(Item item);

        void DeleteItem(Item item);


        // NOTIFICATIONS

        List<Notification> GetNotifications();

        void MarkAllNotificationsAsRead();

        void MarkNotificationAsRead(Notification notification);

    }
}
