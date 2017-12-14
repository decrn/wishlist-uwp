using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Data;
using ServerApp.Models;

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

        // GET: api/Lists
        [HttpGet]
        public async Task<IEnumerable<List>> GetListsAsync() {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            return _context.List.Include(l => l.Items).Where(l => l.OwnerUserId == user.Id);
        }

        // GET: api/Lists/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetList([FromRoute] int id) {
            List list = await _context.List.Include(l => l.Items).SingleOrDefaultAsync(m => m.ListId == id);

            if (list == null)
                return NotFound();

            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (list.OwnerUserId != user.Id)
                return Forbid();

            return Ok(list);
        }

        // TODO: PATCH: api/Lists/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchList([FromRoute] int id, [FromBody] List list) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (id != list.ListId) {
                return BadRequest();
            }

            _context.Entry(list).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // TODO: POST: api/Lists
        [HttpPost]
        public async Task<IActionResult> PostList([FromBody] List list) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            _context.List.Add(list);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetList", new { id = list.ListId }, list);
        }

        // DELETE: api/Lists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteList([FromRoute] int id) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            List list = await _context.List.SingleOrDefaultAsync(m => m.ListId == id);

            if (list == null)
                return NotFound();

            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (list.OwnerUserId != user.Id)
                return Forbid();

            _context.List.Remove(list);
            await _context.SaveChangesAsync();

            return Ok(list);
        }
    }
}