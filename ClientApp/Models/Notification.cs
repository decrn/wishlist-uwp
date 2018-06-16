using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ClientApp.Models {
    public class Notification {

        // autogenerate
        public int NotificationId { get; set; }

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
