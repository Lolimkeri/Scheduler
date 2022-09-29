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
    public class TeachersController : Controller
    {
        private readonly MyAppContext _context;

        public TeachersController(MyAppContext context)
        {
            _context = context;
        }

        // GET: Teachers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Teachers.ToListAsync());
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create(ErrorViewModel error = null)
        {
            ViewBag.Error = error;
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LastName,Name,FutherName")] Teacher teacher)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(teacher);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction("Index", "Home");
            //}
            //return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                var element = _context.Teachers.FirstOrDefault(g => g.Name == teacher.Name &&
                                                               g.LastName == teacher.LastName &&
                                                               g.FutherName == teacher.FutherName);
                if (element != null)
                {
                    return Create(new ErrorViewModel()
                    {
                        ErrorMessage = "This Teacher already exists"
                    });
                }

                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id, ErrorViewModel error = null)
        {
            ViewBag.Error = error;
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LastName,Name,FutherName")] Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var element = _context.Teachers.FirstOrDefault(g => g.Name == teacher.Name &&
                                                               g.LastName == teacher.LastName &&
                                                               g.FutherName == teacher.FutherName);
                if (element != null)
                {
                    return Create(new ErrorViewModel()
                    {
                        ErrorMessage = "This Teacher already exists"
                    });
                }
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.Id))
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

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }
    }
}
