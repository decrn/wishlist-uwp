using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Data;
using ServerApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Controllers {

    [Produces("application/json")]
    [Route("api/Lists")]
    [Authorize]
    public class ListsController : Controller {
        private readonly WishContext _context;
        private readonly UserManager<User> _userManager;

        public ListsController(WishContext context, UserManager<User> userManager) {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Lists/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetList([FromRoute] int id) {
            var list = _context.List
                .Include(l => l.OwnerUser)
                .Include(l => l.Items)
                .Include(l => l.SubscribedUsers)
                    .ThenInclude(s => s.User)
                .Include(l => l.InvitedUsers)
                    .ThenInclude(s => s.User)
                .SingleOrDefault(m => m.ListId == id);

            if (list == null)
                return NotFound();
            
            User user = await _userManager.GetUserAsync(HttpContext.User);

            if (!list.IsHidden)
                return Ok(list);

            if (list.OwnerUser.Id == user.Id || list.SubscribedUsers.Any(s => s.UserId == user.Id))
                return Ok(list);

            return Forbid();
        }

        // PUT: api/Lists/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateListPut([FromRoute] int id, [FromBody] List list) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != list.ListId)
                return BadRequest();

            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (list.OwnerUser.Id != user.Id)
                return Forbid();

            _context.Entry(list).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(list);
        }

        // PATCH: api/Lists/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateListPatch([FromRoute] int id, [FromBody] JsonPatchDocument<List> patch) {
            List list = await _context.List.SingleOrDefaultAsync(m => m.ListId == id);
            patch.ApplyTo(list, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (list.OwnerUser.Id != user.Id)
                return Forbid();

            await _context.SaveChangesAsync();

            return Ok(list);
        }

        // POST: api/Lists/5
        [HttpPost("{id}")]
        public async Task<IActionResult> SendListInvitations([FromRoute] int id) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List list = await _context.List.SingleOrDefaultAsync(l => l.ListId == id);

            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (list.OwnerUser.Id != user.Id)
                return Forbid();

            list.InvitedUsers.ToList().ForEach(u => u.User.InviteToList(list));
            await _context.SaveChangesAsync();

            return Ok();
        }

        // POST: api/Lists
        [HttpPost]
        public async Task<IActionResult> AddList([FromBody] List list) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User user = await _userManager.GetUserAsync(HttpContext.User);
            list.OwnerUser.Id = user.Id;

            _context.List.Add(list);
            await _context.SaveChangesAsync();

            return Ok(list);
        }

        // DELETE: api/Lists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteList([FromRoute] int id) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            List list = await _context.List.Include(l => l.OwnerUser).Include(l => l.SubscribedUsers).SingleOrDefaultAsync(m => m.ListId == id);

            if (list == null)
                return NotFound();

            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (list.OwnerUser.Id != user.Id) {
                if (list.SubscribedUsers.Any(u => u.UserId == user.Id)) {

                    // remove subscription
                    list.SubscribedUsers.Remove(list.SubscribedUsers.First(u => u.UserId == user.Id));

                } else {
                    return Forbid();
                }
            } else {
                 // remove list itself
                _context.List.Remove(list);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}