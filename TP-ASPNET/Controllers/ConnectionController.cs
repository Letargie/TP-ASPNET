using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace TP_ASPNET.Controllers
{
    public class ConnectionController : Controller {
        public IActionResult Login() {
            Console.WriteLine("On essaie de se connect");
            if (!HttpContext.User.Identity.IsAuthenticated) {
                return Challenge();
            }

            return RedirectToAction("Index", "Todoes");
        }

        [HttpPost]
        public IActionResult Logout() {
            return new SignOutResult();
        }
    }
}