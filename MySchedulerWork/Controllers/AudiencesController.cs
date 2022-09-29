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
    public class AudiencesController : Controller
    {
        private readonly MyAppContext _context;

        public AudiencesController(MyAppContext context)
        {
            _context = context;
        }

        // GET: Audiences
        public async Task<IActionResult> Index()
        {
            return View(await _context.Audiences.ToListAsync());
        }

        // GET: Audiences/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var audience = await _context.Audiences
                .FirstOrDefaultAsync(m => m.Id == id);
            if (audience == null)
            {
                return NotFound();
            }

            return View(audience);
        }

        // GET: Audiences/Create
        public IActionResult Create(ErrorViewModel error = null)
        {
            ViewBag.Error = error;
            return View();
        }

        // POST: Audiences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Capacity")] Audience audience)
        {
            if (ModelState.IsValid)
            {
                var element = _context.Audiences.FirstOrDefault(g => g.Name == audience.Name);
                if (element != null)
                {
                    return Create(new ErrorViewModel()
                    {
                        ErrorMessage = "This Audience already exists"
                    });
                }
                    _context.Add(audience);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Audiences/Edit/5
        public async Task<IActionResult> Edit(int? id, ErrorViewModel error = null)
        {
            ViewBag.Error = error;
            if (id == null)
            {
                return NotFound();
            }

            var audience = await _context.Audiences.FindAsync(id);
            if (audience == null)
            {
                return NotFound();
            }
            return View(audience);
        }

        // POST: Audiences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Capacity")] Audience audience)
        {
            if (id != audience.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var element = _context.Audiences.FirstOrDefault(g => g.Name == audience.Name);
                if (element != null)
                {
                    return Create(new ErrorViewModel()
                    {
                        ErrorMessage = "This Audience already exists"
                    });
                }
                try
                {
                    _context.Update(audience);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AudienceExists(audience.Id))
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

        // GET: Audiences/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var audience = await _context.Audiences
                .FirstOrDefaultAsync(m => m.Id == id);
            if (audience == null)
            {
                return NotFound();
            }

            return View(audience);
        }

        // POST: Audiences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var audience = await _context.Audiences.FindAsync(id);
            _context.Audiences.Remove(audience);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool AudienceExists(int id)
        {
            return _context.Audiences.Any(e => e.Id == id);
        }
    }
}
