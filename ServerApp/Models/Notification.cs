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

        [Required]
        public string Message { get; set; }

        [Required]
        public NotificationType Type { get; set; }

        public bool IsUnread { get; set; } = true;

        [ForeignKey("User")]
        public string UserId { get; set; }

        // null in case of JoinRequest
        [ForeignKey("List")]
        public int ListId { get; set; }

        public void MarkAsRead() {
            IsUnread = false;
        }
    }
}

public enum NotificationType { JoinRequest, ListInvitation, DeadlineReminder }
