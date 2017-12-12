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

namespace ServerApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Account/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger _logger;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<AccountController> logger
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // POST: api/Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return Ok(model);
                }

                _logger.LogInformation("User failed to log in.");
                // TODO: don't return OK but ERROR/FAIL/..?
                return Ok(result);
            }

            _logger.LogInformation("User failed to log in.");
            // TODO: don't return OK but ERROR/FAIL/..?
            return Ok(ModelState.Values);
        }

        // POST: api/Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User {
                    Email = model.Email,
                    UserName = model.Email
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User registered.");
                    return Ok(model);
                }

                _logger.LogInformation("User failed to register.");
                // TODO: don't return OK but ERROR/FAIL/..?
                return Ok(result.Errors);
            }

            _logger.LogInformation("User failed to register.");
            // TODO: don't return OK but ERROR/FAIL/..?
            return Ok(ModelState.Values);
        }

        // GET: api/Account/Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return NoContent();
        }
    }
}
