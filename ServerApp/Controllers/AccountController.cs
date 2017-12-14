using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc;
using ServerApp.Data;
using ServerApp.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace ServerApp.Controllers {
    [Produces("application/json")]
    [Route("api/Account/[action]")]
    public class AccountController : Controller {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager
        ) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // POST: api/Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model) {
            if (ModelState.IsValid) {
                SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);

                if (result.Succeeded) {
                    return Json(model);
                }

                return Json(result);
            }

            return Json(ModelState.Values);
        }

        // POST: api/Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model) {
            if (ModelState.IsValid) {
                User user = new User {
                    Email = model.Email,
                    UserName = model.Email
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded) {
                    return Json(model);
                }

                return Json(result);
            }

            return Json(ModelState.Values);
        }

        // GET: api/Account/Logout
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return NoContent();
        }
    }
}
