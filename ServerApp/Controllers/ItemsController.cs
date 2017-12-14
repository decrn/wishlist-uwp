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

        // PUT: api/Items/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem([FromRoute] int id) {

            User user = await _userManager.GetUserAsync(HttpContext.User);
            Item item = await _context.Item.SingleOrDefaultAsync(m => m.ItemId == id);

            // TODO: check if user is subscribed to list
            if (false)
                return Forbid();

            if (item.CheckedByUserId == user.Id) {
                item.CheckedByUserId = null;
            } else {
                item.CheckedByUserId = user.Id;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        // TODO: PATCH: api/Items/5
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

        // TODO: POST: api/Items
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

            Item item = await _context.Item.SingleOrDefaultAsync(m => m.ItemId == id);

            if (item == null)
                return NotFound();

            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (item.List.OwnerUserId != user.Id)
                return Forbid();

            _context.Item.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }
    }
}