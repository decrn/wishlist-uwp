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
            List list = await _context.List.Include(l => l.Items).SingleOrDefaultAsync(m => m.ListId == id);

            if (list == null)
                return NotFound();

            User user = await _userManager.GetUserAsync(HttpContext.User);
            // TODO: also allow viewing list if subscribed or public
            if (list.OwnerUserId != user.Id)
                return Forbid();

            return Ok(list);
        }

        // PATCH: api/Lists/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchList([FromRoute] int id, [FromBody] JsonPatchDocument<List> patch) {
            List list = await _context.List.SingleOrDefaultAsync(m => m.ListId == id);
            patch.ApplyTo(list, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // TODO: check if user is owner

            await _context.SaveChangesAsync();

            return Ok(list);
        }

        // POST: api/Lists
        [HttpPost]
        public async Task<IActionResult> AddList([FromBody] List list) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            // TODO: add owneduser as loggedinuser
            _context.List.Add(list);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetList", new { id = list.ListId }, list);
        }

        // DELETE: api/Lists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteList([FromRoute] int id) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
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