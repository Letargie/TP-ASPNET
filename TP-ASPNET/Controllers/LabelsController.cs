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
    public class LabelsController : Controller{
        private readonly TodoContext _context;
        private readonly UserManager<User> _userManager;

        public LabelsController(TodoContext context, UserManager<User> userManager){
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<User> GetCurrentUser() {
            User usr = await GetCurrentUserAsync();
            return usr;
        }
        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Labels
        public async Task<IActionResult> Index(){
            return View(await _context.Label.ToListAsync());
        }

        // GET: Labels/Create
        public IActionResult Create(){
            return View();
        }

        // POST: Labels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Text")] Label label){
            if (ModelState.IsValid){
                label.Id = Guid.NewGuid();
                _context.Add(label);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(label);
        }

        // GET: Labels/Delete/5
        public async Task<IActionResult> Delete(Guid id){
            var label = _context.Label.First(l => l.Id == id);
            // If the Todo doesn't belong to our user, we exit with an error code
            if (label == null){
                return NotFound();
            }
            _context.Label.Remove(label);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        private bool LabelExists(Guid id){
            return _context.Todo.Any(e => e.Id == id);
        }
    }
}
