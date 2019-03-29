using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNetCoreIdentity.Models.ConnectionViewModels;
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
            return View(await _context.Todo.Include(t => t.TodoLabels).ThenInclude(tdl => tdl.Label)
                .Where(t => t.User == GetCurrentUser().Result).AsQueryable().ToListAsync());
        }

        // GET: Todoes/List
        public async Task<IActionResult> List() {
            return View(await _context.Todo.Include(t => t.TodoLabels).ThenInclude(tdl => tdl.Label)
                .Where(t => t.User == GetCurrentUser().Result).AsQueryable().ToListAsync());
        }

        // GET: Todoes/Details/5
        public async Task<IActionResult> Details(Guid? id){
            if (id == null){
                return NotFound();
            }

            var todo = await _context.Todo.Include(t => t.TodoLabels).ThenInclude(tdl => tdl.Label)
                .FirstOrDefaultAsync(t => t.Id == id && t.User == GetCurrentUser().Result);
            // We check if the todo belong to the current user
            if (todo == null){
                return Unauthorized();
            }


            return View(todo);
        }

        // GET: Todoes/Create
        public IActionResult Create(){
            TodoViewModel tvm = new TodoViewModel();
            tvm.Labels = _context.Label.ToList();
            return View(tvm);
        }

        // POST: Todoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description")] Todo todo, List<Guid> TodoLabels) {
            if (ModelState.IsValid){
                todo.Id = Guid.NewGuid();
                todo.User = GetCurrentUser().Result;
                todo.CreationDate = DateTime.Now;
                todo.LastModificationDate = DateTime.Now;
                todo.Done = false;
                todo.TodoLabels = new List<TodoLabel>();
                foreach (Guid labelId in TodoLabels) {
                    TodoLabel tdl = new TodoLabel();
                    tdl.Todo = todo;
                    tdl.LabelGuid = labelId;
                    todo.TodoLabels.Add(tdl);
                }
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

            var todo = _context.Todo.Include(t => t.TodoLabels).ThenInclude(tl => tl.Label).First(t => t.Id == id && t.User == GetCurrentUser().Result);
            // If the todo doesn't belong to the current user
            if (todo == null){
                 return Unauthorized();
            }
           
            TodoViewModel tvm = new TodoViewModel();
            tvm.Todo = todo;
            tvm.Labels = _context.Label.ToList();
            return View(tvm);
        }

        // POST: Todoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Title,Description")] Todo todo, List<Guid> TodoLabels){
            if (ModelState.IsValid) {
                // If the Todo doesn't belong to our user
                Todo curTodo = _context.Todo.First(t => t.Id == id && t.User == GetCurrentUser().Result);
                if (curTodo == null) {
                    return Unauthorized();
                }
                curTodo.Description = todo.Description;
                curTodo.Title = todo.Title;
                curTodo.LastModificationDate = DateTime.Now;
                curTodo.TodoLabels = new List<TodoLabel>();
                foreach (Guid labelId in TodoLabels){
                    TodoLabel tdl = new TodoLabel();
                    tdl.Todo = curTodo;
                    tdl.LabelGuid = labelId;
                    curTodo.TodoLabels.Add(tdl);
                }
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
            TodoViewModel tvm = new TodoViewModel();
            tvm.Todo = todo;
            tvm.Labels = _context.Label.ToList();
            return View(tvm);
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
            var todo = _context.Todo.Include(t => t.TodoLabels).First(t => t.Id == id && t.User == GetCurrentUser().Result);
            // If the Todo doesn't belong to our user, we exit with an error code
            if (todo == null) {
                return Unauthorized();
            }
            // Remove the todolabels that were linked to our todo
            foreach (var todoLabel in todo.TodoLabels){
                _context.TodoLabels.Remove(todoLabel);
            }
            _context.Todo.Remove(todo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Todoes/Done
        [HttpGet]
        public IActionResult Done(Guid id, string returnUrl = null) {
            return SetDone(id, true, returnUrl);
        }

        // GET: Todoes/Undo
        [HttpGet]
        public IActionResult Undo(Guid id, string returnUrl = null) {
            return SetDone(id, false, returnUrl);
        }


        public IActionResult SetDone(Guid id, Boolean isDone, string returnUrl = null) {
            var todo = _context.Todo.First(t => t.Id == id && t.User == GetCurrentUser().Result);
            // If the Todo doesn't belong to our user, we exit with an error code
            if (todo == null) {
                return Unauthorized();
            }

            todo.Done = isDone;
            _context.Todo.Update(todo);
            _context.SaveChanges();
            if (returnUrl != null){
                return RedirectToAction(returnUrl, "Todoes");
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TodoExists(Guid id){
            return _context.Todo.Any(e => e.Id == id);
        }
    }
}
