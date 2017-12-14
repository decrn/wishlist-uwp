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
    [Route("api/Items")]
    [Authorize]
    public class ItemsController : Controller {
        private readonly WishContext _context;
        private readonly UserManager<User> _userManager;

        public ItemsController(WishContext context, UserManager<User> userManager) {
            _context = context;
            _userManager = userManager;
        }

        // PATCH: api/Items/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchItem([FromRoute] int id, [FromBody] Item item) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (id != item.ItemId) {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Items
        [HttpPost]
        public async Task<IActionResult> PostItem([FromBody] Item item) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            _context.Item.Add(item);
            await _context.SaveChangesAsync();

            // return the newly created item
            return CreatedAtAction("GetItem", new { id = item.ItemId }, item);
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem([FromRoute] int id) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            User user = await _userManager.GetUserAsync(HttpContext.User);
            Item item = await _context.Item.SingleOrDefaultAsync(m => m.ItemId == id && m.List.OwnerUserId == user.Id);
            if (item == null) {
                return NotFound();
            }

            _context.Item.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }
    }
}