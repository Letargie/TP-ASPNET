using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP_ASPNET.Models;
using Microsoft.AspNetCore.Identity;
using ASPNetCoreIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using ASPNetCoreIdentity.Models.AccountViewModels;
using Microsoft.Extensions.Logging;

namespace TP_ASPNET.Controllers
{
    public class ConnectionController : Controller {

        public SignInManager<User> SignIn { get; set; }
        public UserManager<User> UserManager { get; set; }
        public ILogger<ConnectionController> Logger { get; set; }
        public TodoContext Context { get; set; }

        public ConnectionController(UserManager<User> userManager, SignInManager<User> signIn, ILogger<ConnectionController> logger, TodoContext context){
            this.SignIn = signIn;
            this.UserManager = userManager;
            this.Logger = logger;
            this.Context = context;

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
 
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Email, Email = model.Email };
                user.InscriptionDate = DateTime.Now;
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    Logger.LogInformation("User created a new account with password.");
 
                    await SignIn.SignInAsync(user, isPersistent: false);
                    Logger.LogInformation("User created a new account with password.");
                    return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }
 
            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null) {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null) {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid) {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await SignIn.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded) {
                    Logger.LogInformation("User logged in.");
                    User usr = Context.User.First(
                        u => u.NormalizedEmail == model.Email);
                    usr.LastConnectionDate = DateTime.Now;
                    Context.Update(usr);
                    await Context.SaveChangesAsync();

                    return RedirectToLocal(returnUrl);
                }
                if (result.IsLockedOut) {
                    Logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                } else {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout() {
            await SignIn.SignOutAsync();
            Logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }



        #region Helpers

        private void AddErrors(IdentityResult result) {
            foreach (var error in result.Errors) {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl) {
            if (Url.IsLocalUrl(returnUrl)) {
                return Redirect(returnUrl);
            } else {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}