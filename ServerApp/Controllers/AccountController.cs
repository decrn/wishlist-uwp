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
                    return await GenerateJwtToken(user);
                }

                return Json(result);
            }

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            return Json(allErrors);
        }

        // POST: api/Account/Register
        [HttpPost]
        public async Task<object> Register(RegisterViewModel model) {
            if (ModelState.IsValid) {

                try {
                    var addr = new System.Net.Mail.MailAddress(model.Email);
                    if (addr.Address != model.Email) throw new Exception();
                } catch {
                    return Json(new[] {
                            new {errorMessage = "Not a valid email"}
                        }
                    );
                }

                User user = new User {
                    Email = model.Email,
                    UserName = model.Email
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded) {
                    return await GenerateJwtToken(user);
                }

                return Json(result);
            }

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            return Json(allErrors);
        }

        // GET: api/Account/Logout
        [Authorize]
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return Ok();
        }

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
