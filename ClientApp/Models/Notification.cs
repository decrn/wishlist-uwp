using System;

namespace ClientApp.Models {
    public class Notification {

        // autogenerate
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
                        return SubjectUser.GetFullName() + " (" + SubjectUser.Email + ") wants you to add them to one of your lists";
                        break;
                    case NotificationType.ListInvitation:
                        return SubjectList.OwnerUser.GetFullName() + " invited you to their list '" + SubjectList.Name + "'";
                        break;
                    case NotificationType.DeadlineReminder:
                        return "The deadline for the list '" + SubjectList.Name + "' is coming up soon!";
                        break;
                    case NotificationType.ListJoinSuccess:
                        return "" + SubjectUser.GetFullName() + " has joined your list '"+ SubjectList.Name +"'.";
                        break;
                }
                return "";
            }
        }

        public string FormattedTimestamp {
            get {
                return Timestamp.ToString("dd MMMM yyyy | HH:mm");
            }
        }

        public Notification() {}

    }
}

public enum NotificationType { JoinRequest, ListInvitation, DeadlineReminder, ListJoinSuccess }
