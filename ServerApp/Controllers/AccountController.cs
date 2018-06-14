using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc;
using ServerApp.Data;
using ServerApp.Models;
using ServerApp.ViewModels;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace ServerApp.Controllers {

    [Produces("application/json")]
    [Route("api/Account/[action]")]
    public class AccountController : Controller {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration
        ) {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        // POST: api/Account/Login
        [HttpPost]
        public async Task<object> Login(LoginViewModel model) {
            if (ModelState.IsValid) {
                SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);

                if (result.Succeeded) {
                    User user = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                    return Json(new { success = true, data = await GenerateJwtToken(user) });
                }

                return Json(new { success = false, errors = new[] { new { message = "Not logged in", data = result } } });
            }

            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => new { message = e.ErrorMessage }) });
        }

        // POST: api/Account/Register
        [HttpPost]
        public async Task<object> Register(RegisterViewModel model) {
            if (ModelState.IsValid) {

                try {
                    var addr = new System.Net.Mail.MailAddress(model.Email);
                    if (addr.Address != model.Email) throw new Exception();
                } catch {
                    return Json(new { success = false, errors = new[] { new { message = "Not a valid email address" } } });
                }

                User user = new User {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.Email
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded) {
                    return Json(new { success = true, data = await GenerateJwtToken(user) });
                }

                return Json(new { success = false, errors = result.Errors.Select(e => new { message = e.Description, data = e.Code }) });
            }

            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => new { message = e.ErrorMessage }) });
        }

        // GET: api/Account/Logout
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        // POST: api/Account/Password
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditAccountViewModel model) {

            if (ModelState.IsValid) {

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Json(new { success = false, errors = new[] { new { message = "Account doesn't exist" } } });

                if (model.Email != user.UserName) {
                    var setUserNameResult = await _userManager.SetUserNameAsync(user, model.Email);
                    if (!setUserNameResult.Succeeded) {
                        return Json(new { success = false, errors = setUserNameResult.Errors.Select(e => new { message = e.Description }) });
                    }
                }

                if (model.Email != user.Email) {
                    var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                    if (!setEmailResult.Succeeded) {
                        return Json(new { success = false, errors = setEmailResult.Errors.Select(e => new { message = e.Description }) });
                    }
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                await _userManager.UpdateAsync(user);

                return Json(new { success = true, data = user });

            }

            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => new { message = e.ErrorMessage }) });
        }
 
        // POST: api/Account/Password
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Password(ChangePasswordViewModel model) {
            if (ModelState.IsValid) {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Json(new { success = false, errors = new[] { new { message = "Account doesn't exist" } } });

                var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (!changePasswordResult.Succeeded) {
                    return Json(new { success = false, errors = changePasswordResult.Errors.Select(e => new { message = e.Description }) });
                }

                return Json(new { success = true });
            }

            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => new { message = e.ErrorMessage }) });
        }


        // Helpers

        private async Task<object> GenerateJwtToken(IdentityUser user) {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
