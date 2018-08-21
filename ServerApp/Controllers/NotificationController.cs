using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Data;
using ServerApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Controllers {

    [Produces("application/json")]
    [Route("api/Notifications")]
    [Authorize]
    public class NotificationsController : Controller {
        private readonly WishContext _context;
        private readonly UserManager<User> _userManager;

        public NotificationsController(WishContext context, UserManager<User> userManager) {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Notifications
        [HttpGet]
        public async Task<IEnumerable<Notification>> GetNotifications()
        {

            // first search for new deadlines and add notifications
            string userid = _userManager.GetUserId(HttpContext.User);
            User user = await _context.User
                                        .Include(u=>u.Notifications)
                                            .ThenInclude(n=>n.SubjectList)
                                        .Include(u => u.Notifications)
                                            .ThenInclude(n => n.SubjectUser)
                                        .Include(u=>u.SubscribedLists)
                                            .ThenInclude(s=>s.List)
                                        .SingleOrDefaultAsync(u => u.Id == userid);

            user.SubscribedLists.ToList().ForEach(l => {
                // Check if list deadline is soon & if there are already any Deadlinereminders for this list
                if (l.List.IsSoon() && !user.Notifications.Any(n => n.Type == NotificationType.DeadlineReminder && n.SubjectList.ListId == l.ListId)) {
                    Notification notif = new Notification(user, NotificationType.DeadlineReminder, l.List);
                    _context.Notification.Add(notif);
                    //user.Notifications.Add(notif);
                }
            });

            await _context.SaveChangesAsync();
            return user.Notifications;
        }

        // PUT: api/Notifications
        [HttpPut]
        public async Task<IActionResult> MarkAllNotificationsAsRead()
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            IEnumerable<Notification> notifications = _context.Notification.Where(n => n.OwnerUser.Id == user.Id && n.IsUnread);
            notifications.ToList().ForEach(n => n.MarkAsRead());

            await _context.SaveChangesAsync();
            return Ok();
        }

        // POST: api/Notifications/5
        [HttpPost("{id}")]
        public async Task<IActionResult> ExecuteNotificationAction([FromRoute] int id) {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            Notification notification = _context.Notification
                                                .Include(n => n.SubjectList)
                                                .Include(n => n.SubjectUser)
                                                .SingleOrDefault(n => n.OwnerUser.Id == user.Id && n.NotificationId == id);

            if (notification == null)
                return NotFound();

            if (notification.Type == NotificationType.ListInvitation) {
                // TODO: better way to add UserListSubscription and remove UserListInvite?
                _context.Database.ExecuteSqlCommand("DELETE FROM [UserListInvite] WHERE ListId=" + notification.SubjectList.ListId+ " AND UserId='"+user.Id+"';");
                _context.Database.ExecuteSqlCommand("INSERT INTO [UserListSubscription] VALUES (" + notification.SubjectList.ListId+",'"+user.Id+"');");
                Notification notif = new Notification(user, NotificationType.ListJoinSuccess, notification.SubjectList);
                _context.Notification.Add(notif);

            }

            notification.MarkAsRead();

            await _context.SaveChangesAsync();
            return Ok(notification);
        }

    }
}