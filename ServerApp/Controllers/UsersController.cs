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
        public async Task<IEnumerable<List>> GetOwnedListsAsync() {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            // TODO: only show public lists when not owner logged in
            return _context.List.Include(l => l.Items).Where(l => l.OwnerUserId == user.Id);
        }

        // GET: api/Users/Subscriptions
        [HttpGet("Subscriptions")]
        public async Task<IEnumerable<List>> GetSubscribedListsAsync() {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            // TODO: only show subscribed lists from user
            return _context.List.Include(l => l.Items).Where(l => l.SubscribedUsers == null);
        }

        // GET: api/Users/
        [HttpGet]
        public async Task<IActionResult> GetLoggedInUser() {
            User user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] string id) {
            User user = await _context.User.SingleOrDefaultAsync(m => m.Id == id);

            if (user == null)
                return NotFound();

            // TODO: filter private info, like hidden owned lists

            return Ok(user);
        }

    }
}