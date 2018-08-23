using GalaSoft.MvvmLight;
using System;

namespace ClientApp.Models {
    public class Notification : ObservableObject {

        public int NotificationId { get; set; }

        public DateTime Timestamp { get; set; }

        public User OwnerUser { get; set; }

        public NotificationType Type { get; set; }

        public bool IsUnread { get; set; } = true;

        public User SubjectUser { get; set; }

        // null in case of JoinRequest
        public List SubjectList { get; set; }

        public string Message {
            get {
                switch (Type) {
                    case NotificationType.JoinRequest:
                        return SubjectUser.FirstName + " " + SubjectUser.LastName + " (" + SubjectUser.Email + ") wants you to add them to one of your lists";
                    case NotificationType.ListInvitation:
                        return SubjectList.OwnerUser.FirstName + " " + SubjectList.OwnerUser.LastName + " invited you to their list '" + SubjectList.Name + "'";
                    case NotificationType.DeadlineReminder:
                        return "The deadline for the list '" + SubjectList.Name + "' is coming up soon!";
                    case NotificationType.ListJoinSuccess:
                        return "" + SubjectUser.FirstName + " " + SubjectUser.LastName + " has joined your list '"+ SubjectList.Name +"'.";
                }
                return "";
            }
        }

        public string FormattedTimestamp {
            get { return Timestamp.ToString("dd MMMM yyyy | HH:mm"); }
        }

        public Notification() {}

    }
}

public enum NotificationType { JoinRequest, ListInvitation, DeadlineReminder, ListJoinSuccess }
