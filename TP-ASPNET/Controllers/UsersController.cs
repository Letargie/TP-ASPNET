using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP_ASPNET.Models;

namespace TP_ASPNET.Controllers{
    [Authorize]
    public class UsersController : Controller{

        private readonly TodoContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UsersController(TodoContext context, UserManager<User> userManager, SignInManager<User> signInManager) {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<User> GetCurrentUser() {
            User usr = await GetCurrentUserAsync();
            return usr;
        }
        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);


        // GET: Users
        public async Task<IActionResult> Index() {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return View(user);
        }

        // GET: Users/Edit
        public async Task<IActionResult> Edit() {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return View(user);
        }



        // GET: Users/ChangePassword
        public async Task<IActionResult> ChangePassword() {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return View(user);
        }

        // POST: Users/ChangePassword
        [HttpPost]
        public async Task<IActionResult> ChangePassword(String OldPassword, String NewPassword) {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if (!ModelState.IsValid){
                return View(user);
            }

            

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, OldPassword, NewPassword);
            if (!changePasswordResult.Succeeded) {
                foreach (var error in changePasswordResult.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(user);
            }

            await _signInManager.RefreshSignInAsync(user);
            return View(user);
        }


    }
}
