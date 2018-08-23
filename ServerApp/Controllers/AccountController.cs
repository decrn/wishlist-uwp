using dotnet_g24.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServerApp.Data;
using ServerApp.Models;
using ServerApp.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace ServerApp.Controllers {

    [Produces("application/json")]
    [Route("api/Account/[action]")]
    public class AccountController : Controller {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly WishContext _context;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration,
            IEmailSender emailSender,
            WishContext context
        ) {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _context = context;
        }

        #region Generic account routes

        // POST: api/Account/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<object> Login(LoginViewModel model) {
            if (ModelState.IsValid) {
                SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);

                if (result.Succeeded) {
                    User user = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                    return Json(new { success = true, data = new { user = user, token = GenerateJwtToken(user) } });
                }

                return Json(new { success = false, errors = new[] { new { message = "Wrong credentials.", data = result } } });
            }

            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => new { message = e.ErrorMessage }) });
        }

        // POST: api/Account/Register
        [HttpPost]
        [AllowAnonymous]
        public async Task<object> Register(RegisterViewModel model) {
            if (ModelState.IsValid) {

                User user = new User {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.Email
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded) {
                    return Json(new { success = true, data = new { user = user, token = GenerateJwtToken(user) } });
                }

                return Json(new { success = false, errors = result.Errors.Select(e => new { message = e.Description, data = e.Code }) });
            }

            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => new { message = e.ErrorMessage }) });
        }

        // GET: api/Account/Logout
        [Authorize]
        [HttpGet]
        public async Task<object> Logout() {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        #endregion

        #region Edit account

        // POST: api/Account/Edit
        [Authorize]
        [HttpPost]
        public async Task<object> Edit(EditAccountViewModel model) {

            if (ModelState.IsValid) {

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Json(new { success = false, errors = new[] { new { message = "Account doesn't exist." } } });

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
        public async Task<object> Password(ChangePasswordViewModel model) {
            if (ModelState.IsValid) {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Json(new { success = false, errors = new[] { new { message = "Account doesn't exist." } } });

                var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (!changePasswordResult.Succeeded) {
                    return Json(new { success = false, errors = changePasswordResult.Errors.Select(e => new { message = e.Description }) });
                }

                return Json(new { success = true });
            }

            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => new { message = e.ErrorMessage }) });
        }

        #endregion

        #region Forgot password

        [HttpPost]
        [AllowAnonymous]
        public async Task<object> ForgotPassword(ForgotPasswordViewModel model) {
            if (ModelState.IsValid) {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null) {
                    return Json(new { success = false, errors = new[] { new { message = "Account doesn't exist." } } });
                }

                // For more information on how to enable account confirmation and password reset please
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var hash = GenerateMD5(code);
                await _userManager.AddClaimAsync(user, new Claim(hash, code));
                await _emailSender.SendEmailAsync(model.Email, "Reset password of Wishlist app", 
                   "Please enter this code: <b> "+hash+" </b> into the application along with a new password.");

                return Json(new { success = true });
            }

            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => new { message = e.ErrorMessage }) });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<object> ResetPassword(ResetPasswordViewModel model) {
            if (ModelState.IsValid) {
                
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null) {
                    return Json(new { success = false, errors = new[] { new { message = "Account doesn't exist." } } });
                }

                Claim claim = (await _userManager.GetClaimsAsync(user)).SingleOrDefault(c => c.Type == model.Code.Trim());
                if (claim == null) {
                    return Json(new { success = false, errors = new[] { new { message = "Wrong code." } } });
                }

                var result = await _userManager.ResetPasswordAsync(user, claim.Value, model.Password);
                await _userManager.RemoveClaimAsync(user, claim);
                if (result.Succeeded) {
                    return Json(new { success = true });
                }

                return Json(new { success = false, errors = result.Errors.Select(e => new { message = e.Description }) });

            }

            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => new { message = e.ErrorMessage }) });
        }

        #endregion

        #region Helpers

        private string GenerateJwtToken(IdentityUser user) {
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

        private static string GenerateMD5(string input) {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++) {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        #endregion

    }
}
