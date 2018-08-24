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

        private async Task AddListDependencies(List list) {

            // TODO: removing items and invites server side

            list.Items.ToList().ForEach(item => {
                if (item.ItemId > 0) {
                    // yes the lazy way
                    var trueitem = _context.Item.First(i => i.ItemId == item.ItemId);
                    trueitem.ProductName = item.ProductName;
                    trueitem.Description = item.Description;
                    trueitem.ProductInfoUrl = item.ProductInfoUrl;
                    trueitem.ProductImageUrl = item.ProductImageUrl;
                    trueitem.Category = item.Category;
                    trueitem.ItemPriceUsd = item.ItemPriceUsd;

                } else {
                    Item newitem = new Item {
                        ProductName = item.ProductName,
                        Description = item.Description,
                        ProductInfoUrl = item.ProductInfoUrl,
                        ProductImageUrl = item.ProductImageUrl,
                        Category = item.Category,
                        ItemPriceUsd = item.ItemPriceUsd,
                        List = list
                    };
                    _context.Item.Add(newitem);
                }
            });

            await _context.SaveChangesAsync();

            // TODO: don't save double keys
            list.InvitedUsers.ToList().ForEach(user => {
                // only save existing mailaddresses
                var foundusers = _context.User.Where(u => u.Email == user.User.Email);
                if (foundusers.Count() == 1) {
                    _context.Database.ExecuteSqlCommand("INSERT INTO [UserListInvite] VALUES (" + list.ListId + ",'" + foundusers.First().Id + "');");
                    foundusers.First();
                }
            });

            await _context.SaveChangesAsync();
        }

        // PUT: api/Lists/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateListPut([FromRoute] int id, [FromBody] List list) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != list.ListId)
                return BadRequest();

            User currentuser = await _userManager.GetUserAsync(HttpContext.User);
            if (list.OwnerUser.Id != currentuser.Id)
                return Forbid();

            _context.Entry(list).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            await AddListDependencies(list);

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

            List list = await _context.List.Include(l => l.InvitedUsers).ThenInclude(u => u.User).SingleOrDefaultAsync(l => l.ListId == id);

            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (list.OwnerUser.Id != user.Id)
                return Forbid();


            // TODO: check if not sending multiple motifications to same person
            list.InvitedUsers.ToList().ForEach(u => {
                Notification notif = u.User.InviteToList(list);
                _context.Notification.Add(notif);
            });
            await _context.SaveChangesAsync();

            return Ok();
        }

        // POST: api/Lists
        [HttpPost]
        public async Task<IActionResult> AddList([FromBody] List list) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User user = await _userManager.GetUserAsync(HttpContext.User);
            list.OwnerUser = user;

            _context.List.Add(list);
            await _context.SaveChangesAsync();

            await AddListDependencies(list);

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