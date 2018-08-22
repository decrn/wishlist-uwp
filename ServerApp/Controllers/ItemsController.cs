using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Data;
using ServerApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Controllers {

    [Produces("application/json")]
    [Route("api/Items")]
    [Authorize]
    public class ItemsController : Controller {
        private readonly WishContext _context;
        private readonly UserManager<User> _userManager;

        public ItemsController(WishContext context, UserManager<User> userManager) {
            _context = context;
            _userManager = userManager;
        }

        // PUT: api/Items/5
        [HttpPut("{id}")]
        public async Task<IActionResult> CheckItem([FromRoute] int id) {

            User user = await _userManager.GetUserAsync(HttpContext.User);
            Item item = await _context.Item.Include(i => i.List).Include(i => i.List.SubscribedUsers).SingleOrDefaultAsync(m => m.ItemId == id);

            if (!item.List.SubscribedUsers.Any(s => s.UserId == user.Id))
                return Forbid();

            if (item.CheckedByUser == null && item.CheckedByUser == user)
                item.CheckedByUser = null;
            else
                item.CheckedByUser = user;

            await _context.SaveChangesAsync();

            return Ok();
        }

        // POST: api/Items
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] Item item) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (item.List == null || item.List.OwnerUser.Id != user.Id)
                return Forbid();

            _context.Item.Add(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }

        // TODO: Add server route for edit item: wait untill we know list edit works and which way works best

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem([FromRoute] int id) {
            Item item = await _context.Item.SingleOrDefaultAsync(m => m.ItemId == id);

            if (item == null)
                return NotFound();

            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (item.List.OwnerUser.Id != user.Id)
                return Forbid();

            _context.Item.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }
    }
}