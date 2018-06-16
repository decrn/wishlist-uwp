using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ServerApp.Models {
    public class Notification {

        // autogenerate
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NotificationId { get; set; }

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
