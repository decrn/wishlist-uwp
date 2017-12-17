using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var list = _context.List.Include(l => l.Items).Include(l => l.SubscribedUsers).SingleOrDefault(m => m.ListId == id);
            
            if (list == null)
                return NotFound();

            // server doesn't return anything when SubscribedUsers is set
            var subs = list.SubscribedUsers;
            list.SubscribedUsers = null;
            
            User user = await _userManager.GetUserAsync(HttpContext.User);

            if (!list.IsHidden)
                return Ok(list);

            if (list.OwnerUserId == user.Id || subs.Any(s => s.UserId == user.Id))
                return Ok(list);

            return Forbid();
        }

        // PUT: api/Lists/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutList([FromRoute] int id, [FromBody] List list) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != list.ListId)
                return BadRequest();

            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (list.OwnerUserId != user.Id)
                return Forbid();

            _context.Entry(list).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(list);
        }

        // PATCH: api/Lists/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchList([FromRoute] int id, [FromBody] JsonPatchDocument<List> patch) {
            List list = await _context.List.SingleOrDefaultAsync(m => m.ListId == id);
            patch.ApplyTo(list, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (list.OwnerUserId != user.Id)
                return Forbid();

            await _context.SaveChangesAsync();

            return Ok(list);
        }

        // POST: api/Lists
        [HttpPost]
        public async Task<IActionResult> AddList([FromBody] List list) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User user = await _userManager.GetUserAsync(HttpContext.User);
            list.OwnerUserId = user.Id;

            _context.List.Add(list);
            await _context.SaveChangesAsync();

            return Ok(list);
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