using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
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
        public async Task<IActionResult> CheckItem([FromRoute] int id) {

            User user = await _userManager.GetUserAsync(HttpContext.User);
            Item item = await _context.Item.SingleOrDefaultAsync(m => m.ItemId == id);

            // TODO: check if user is subscribed to list
            if (false)
                return Forbid();

            if (item.CheckedByUserId == user.Id)
                item.CheckedByUserId = null;
            else
                item.CheckedByUserId = user.Id;

            await _context.SaveChangesAsync();

            return Ok();
        }

        // PATCH: api/Items/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchItem([FromRoute] int id, [FromBody] JsonPatchDocument<Item> patch)
        {
            Item item = await _context.Item.SingleOrDefaultAsync(m => m.ItemId == id);
            patch.ApplyTo(item, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (item.List.OwnerUserId != user.Id)
                return Forbid();

            await _context.SaveChangesAsync();

            return Ok(item);
        }

        // POST: api/Items
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] Item item) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (item.List == null || item.List.OwnerUserId != user.Id)
                return Forbid();

            _context.Item.Add(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem([FromRoute] int id) {
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