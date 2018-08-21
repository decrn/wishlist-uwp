using ClientApp.Models;
using ClientApp.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System.Collections.Generic;

namespace ClientApp.DataService {

    public interface IDataService {

        Loading LoadingIndicator { get; set; }

        // ACCOUNT

        bool IsLoggedIn();

        dynamic Login(LoginViewModel vm);

        dynamic Register(RegisterViewModel vm);

        dynamic ChangePassword(ChangePasswordViewModel vm);
        
        dynamic ForgotPassword(ForgotPasswordViewModel vm);

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
