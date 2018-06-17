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

        public Notification() {}

    }
}

public enum NotificationType { JoinRequest, ListInvitation, DeadlineReminder }
