using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerApp.Models {
    public class Notification {

        // autogenerate
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public User OwnerUser { get; set; }

        [Required]
        public NotificationType Type { get; set; }

        public bool IsUnread { get; set; } = true;

        public User SubjectUser { get; set; }

        // null in case of JoinRequest
        public List SubjectList { get; set; }


        public Notification() {}

        public Notification(User user, NotificationType type, List list=null, User otheruser=null) {
            OwnerUser = user;
            Type = type;

            if (list != null)
                SubjectList = list;

            if (otheruser != null)
                SubjectUser = otheruser;
        }


        public void MarkAsRead() {
            IsUnread = false;
        }

    }
}

public enum NotificationType { JoinRequest, ListInvitation, DeadlineReminder }
