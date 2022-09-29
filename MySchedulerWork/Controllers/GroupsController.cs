using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySchedulerWork.Data;
using MySchedulerWork.Models;

namespace MySchedulerWork.Controllers
{
    public class GroupsController : Controller
    {
        private readonly MyAppContext _context;

        public GroupsController(MyAppContext context)
        {
            _context = context;
        }

        // GET: Groups
        public async Task<IActionResult> Index()
        {
            return View(await _context.Groups.ToListAsync());
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // GET: Groups/Create
        public IActionResult Create(ErrorViewModel error = null)
        {
            ViewBag.Error = error;
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Course,AmountOfStudents")] Group @group)
        {
            if (ModelState.IsValid)
            {
                var element = _context.Groups.FirstOrDefault(g => g.Name == @group.Name);
                if (element != null)
                {
                    return Create(new ErrorViewModel() 
                    { 
                      ErrorMessage = "This group already exists"
                    });
                }

                _context.Add(@group);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index","Home");
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id, ErrorViewModel error = null)
        {
            ViewBag.Error = error;
            if (id == null)
            {
                return NotFound();
            }
            
            var @group = await _context.Groups.FindAsync(id);
            if (@group == null)
            {
                return NotFound();
            }
            return View(@group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Course,AmountOfStudents")] Group @group)
        {
            if (id != @group.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var element = _context.Groups.FirstOrDefault(g => g.Name == @group.Name);
                if (element != null)
                {
                    return await Edit(id, new ErrorViewModel()
                    {
                        ErrorMessage = "This group already exists"
                    });
                }
                try
                {
                    _context.Update(@group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(@group.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _context.Groups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var group = await _context.Groups.FindAsync(id);

            if (group.CourseProgram != null)
            {
                var course = await _context.CourseProgram.FirstOrDefaultAsync(c => c.Id == group.CourseProgram.Id);
                course.Groups.Remove(group);
                group.CourseProgram = null;
            }

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var @group = await _context.Groups.FirstOrDefaultAsync(u => u.Id == id);
            if (@group == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _context.Groups.Remove(@group);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete successful" });
        }

        private bool GroupExists(int id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}
