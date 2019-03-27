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

        public UsersController(TodoContext context, UserManager<User> userManager) {
            _context = context;
            _userManager = userManager;
        }


    }
}
