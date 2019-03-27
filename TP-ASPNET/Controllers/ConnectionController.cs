using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP_ASPNET.Models;

namespace TP_ASPNET.Controllers
{
    public class ConnectionController : Controller {

        public SignInManager<User> SignIn { get; set; }

        public ConnectionController(SignInManager<User> signIn)
        {
            this.SignIn = signIn;
        }

        public IActionResult Login() {
            if (!HttpContext.User.Identity.IsAuthenticated) {
                return Challenge();
            }

            return RedirectToAction("Index", "Todoes");
        }

        [HttpPost]
        public async Task<IActionResult> Logout(string returnUrl = null) {
            await SignIn.SignOutAsync();

            if (returnUrl != null) {
                return LocalRedirect(returnUrl);
            } else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}