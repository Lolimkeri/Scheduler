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
    public class CourseProgramsController : Controller
    {
        private readonly MyAppContext _context;

        public CourseProgramsController(MyAppContext context)
        {
            _context = context;
        }

        // GET: CoursePrograms
        public async Task<IActionResult> Index()
        {
            ViewBag.CoursePrograms = await _context.CourseProgram.ToListAsync();
            return View();
        }

        // GET: CoursePrograms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseProgram = await _context.CourseProgram
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseProgram == null)
            {
                return NotFound();
            }
            List<Program_Subject> ProgSubj = await _context.Program_Subjects
                .Where(p => p.ProgramId.Id == courseProgram.Id)
                .OrderBy(p => p.Subject.Name).ThenBy(p=>p.Type)
                .ToListAsync();

           /* List<ProgramToGroup> ProgGroup = await _context.ProgramToGroups
                .Where(p => p.CourseProgram.Id == courseProgram.Id)
                .ToListAsync();
           */
            ViewBag.ProgSubj = ProgSubj;
            //  ViewBag.ProgGroup = ProgGroup;

            // var GroupsToThisCourse = await _context.ProgramToGroups.Where(p => p.CourseProgram.Id == id).ToListAsync();

            ViewBag.ProgToGroup = courseProgram.Groups;

            var Groups = new SelectList( await _context.Groups.Where(g => g.CourseProgram.Id != courseProgram.Id && g.Course == courseProgram.Course).ToListAsync(),"Id","Name");
            ViewBag.Groups = Groups;

            return View(courseProgram);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProgToGroup()
        {
            int CourseId = Convert.ToInt32(Request.Form["courseid"]);
            if (Request.Form["groupid"] != "")
            {
                int groupid = Convert.ToInt32(Request.Form["groupid"]);
                CourseProgram courseprog = _context.CourseProgram.First(p => p.Id == CourseId);
                Group group = _context.Groups.First(p => p.Id == groupid);
                group.CourseProgram = courseprog;
                _context.Update(group);
                await _context.SaveChangesAsync();
                /*if (await _context.ProgramToGroups.FirstOrDefaultAsync(p => p.CourseProgram.Id == CourseId && p.Group.Id == groupid) == null)
                {
                    ProgramToGroup programToGroup = new ProgramToGroup();
                    programToGroup.CourseProgram = _context.CourseProgram.First(p => p.Id == CourseId);
                    programToGroup.Group = _context.Groups.First(p => p.Id == groupid);
                    _context.Add(programToGroup);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new { id = CourseId });
                }*/
            }
            return RedirectToAction("Details",new { id = CourseId });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteProgrameToGroup()
        {
            int id =Convert.ToInt32(Request.Form["idGr"]);
            int returnid = Convert.ToInt32(Request.Form["returnid"]); ;
            /*var programToGroup = await _context.ProgramToGroups
                .FirstOrDefaultAsync(m => m.Id == id);*/
            Group group = _context.Groups.FirstOrDefault(p => p.Id == id);
            CourseProgram course = _context.CourseProgram.FirstOrDefault(p => p.Id == returnid);
            if (group == null)
            {
                return NotFound();
            }
           

            Group Groupfordelete=  course.Groups.Find(p => p.Id == id);
            course.Groups.Remove(Groupfordelete);
            _context.Update(course);

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = returnid });
        }

        // GET: CoursePrograms/Create
        public IActionResult Create(ErrorViewModel error = null)
        {
            ViewBag.Error = error;
            return View();
        }

        // POST: CoursePrograms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Course")] CourseProgram courseProgram)
        {
            if (ModelState.IsValid)
            {
                var element = _context.CourseProgram.FirstOrDefault(g => g.Name == courseProgram.Name);
                if (element != null)
                {
                    return Create(new ErrorViewModel()
                    {
                        ErrorMessage = "This Program already exists"
                    });
                }
                _context.Add(courseProgram);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(courseProgram);
        }

        // GET: CoursePrograms/Edit/5
        public async Task<IActionResult> Edit(int? id, ErrorViewModel error = null)
        {
            ViewBag.Error = error;
            if (id == null)
            {
                return NotFound();
            }

            var courseProgram = await _context.CourseProgram.FindAsync(id);
            if (courseProgram == null)
            {
                return NotFound();
            }
            return View(courseProgram);
        }

        // POST: CoursePrograms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Course")] CourseProgram courseProgram)
        {
            if (id != courseProgram.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var element = _context.CourseProgram.FirstOrDefault(g => g.Name == courseProgram.Name);
                if (element != null)
                {
                    return Create(new ErrorViewModel()
                    {
                        ErrorMessage = "This Program already exists"
                    });
                }
                try
                {
                    _context.Update(courseProgram);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseProgramExists(courseProgram.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(courseProgram);
        }

        // GET: CoursePrograms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseProgram = await _context.CourseProgram
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseProgram == null)
            {
                return NotFound();
            }

            return View(courseProgram);
        }

        // POST: CoursePrograms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courseProgram = await _context.CourseProgram.FindAsync(id);
            _context.CourseProgram.Remove(courseProgram);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<string> GetOptions(int? id)
        {
            var result = "";

            List<Group> groups;

            if (id == -1)
            {
                groups = await _context.Groups.ToListAsync();
            }
            else
            {
                groups = await _context.Groups.Where(g => g.CourseProgram.Id == id).ToListAsync();
            }

            foreach(var group in groups)
            {
                result += "<option value=\"" + group.Id + "\">" + group.Name + "</option>";
            }
            return result;
        }

        private bool CourseProgramExists(int id)
        {
            return _context.CourseProgram.Any(e => e.Id == id);
        }
    }
}
