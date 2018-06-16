using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Data;
using ServerApp.Models;

namespace ServerApp.Controllers {

    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller {
        private readonly WishContext _context;
        private readonly UserManager<User> _userManager;

        public UsersController(WishContext context, UserManager<User> userManager) {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Users/Lists
        [HttpGet("Lists")]
        public async Task<IEnumerable<List>> GetOwnedLists() {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            return _context.List.Where(l => l.OwnerUser.Id == user.Id);
        }

        // GET: api/Users/Subscriptions
        [HttpGet("Subscriptions")]
        public async Task<IEnumerable<List>> GetSubscribedLists() {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            return _context.List.Where(l => 
                l.SubscribedUsers.Any(s=>
                    s.UserId == user.Id));
        }

        // GET: api/Users/
        [HttpGet]
        public async Task<IActionResult> GetLoggedInUser() {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            return await GetUser(user.Id);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] string id) {
            User loggedinuser = await _userManager.GetUserAsync(HttpContext.User);
            User user = await _context.User.Include(u => u.OwningLists).FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
                return NotFound();

            var publicuser = new {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                userName = user.UserName,
                Email = user.Email,
                Lists = _context.List.Where(l => l.OwnerUser.Id == user.Id).Where(l => !l.IsHidden || loggedinuser.Id == id)
            };

            return Ok(publicuser);
        }

        // POST: api/Users/5
        [HttpPost("{id}")]
        public async Task<IActionResult> SendRequestToUser([FromRoute] string id) {
            User loggedinuser = await _userManager.GetUserAsync(HttpContext.User);
            User selecteduser = await _context.User.Include(u => u.Notifications).FirstOrDefaultAsync(m => m.Id == id);

            if (selecteduser == null)
                return NotFound();

            new Notification(selecteduser, NotificationType.JoinRequest, null, loggedinuser);

            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}