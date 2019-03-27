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
    public class TodoesController : Controller{
        private readonly TodoContext _context;
        private readonly UserManager<User> _userManager;

        public TodoesController(TodoContext context, UserManager<User> userManager){
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<User> GetCurrentUser() {
            User usr = await GetCurrentUserAsync();
            return usr;
        }
        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Todoes
        public async Task<IActionResult> Index(){
            return View(await _context.Todo.Where(t => t.User == GetCurrentUser().Result).AsQueryable().ToListAsync());
        }

        // GET: Todoes/Details/5
        public async Task<IActionResult> Details(Guid? id){
            if (id == null){
                return NotFound();
            }

            var todo = await _context.Todo
                .FirstOrDefaultAsync(t => t.Id == id && t.User == GetCurrentUser().Result);
            // We check if the todo belong to the current user
            if (todo == null){
                return Unauthorized();
            }


            return View(todo);
        }

        // GET: Todoes/Create
        public IActionResult Create(){
            return View();
        }

        // POST: Todoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description")] Todo todo){
            if (ModelState.IsValid){
                todo.Id = Guid.NewGuid();
                todo.User = GetCurrentUser().Result;
                todo.CreationDate = DateTime.Now;
                todo.LastModificationDate = DateTime.Now;
                todo.Done = false;
                _context.Add(todo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }

        // GET: Todoes/Edit/5
        public async Task<IActionResult> Edit(Guid? id){

            // If the id is invalid
            if (id == null){
                return NotFound();
            }

            var todo = _context.Todo.First(t => t.Id == id && t.User == GetCurrentUser().Result);
            // If the todo doesn't belong to the current user
            if (todo == null){
                 return Unauthorized();
            }
            return View(todo);
        }

        // POST: Todoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Title,Description")] Todo todo){
            if (ModelState.IsValid) {
                // If the Todo doesn't belong to our user
                Todo curTodo = _context.Todo.First(t => t.Id == id && t.User == GetCurrentUser().Result);
                if (curTodo == null) {
                    return Unauthorized();
                }
                curTodo.Description = todo.Description;
                curTodo.Title = todo.Title;
                curTodo.LastModificationDate = DateTime.Now;
                try{
                    _context.Update(curTodo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException){
                    if (!TodoExists(todo.Id)){
                        return NotFound();
                    }else{
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }

        // GET: Todoes/Delete/5
        public async Task<IActionResult> Delete(Guid? id){
            // If we have an invalid id
            if (id == null){
                return NotFound();
            }

            var todo = await _context.Todo
                .FirstOrDefaultAsync(m => m.Id == id && m.User == GetCurrentUser().Result);
            // If we haven't found any todoes
            if (todo == null){
                return Unauthorized();
            }

            return View(todo);
        }

        // POST: Todoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id){
            var todo = _context.Todo.First(t => t.Id == id && t.User == GetCurrentUser().Result);
            // If the Todo doesn't belong to our user, we exit with an error code
            if (todo == null) {
                return Unauthorized();
            }
            // Remove the user
            _context.Todo.Remove(todo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TodoExists(Guid id){
            return _context.Todo.Any(e => e.Id == id);
        }
    }
}
