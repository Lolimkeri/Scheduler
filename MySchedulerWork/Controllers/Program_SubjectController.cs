using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using MySchedulerWork.Data;
using MySchedulerWork.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MySchedulerWork.Controllers
{
    public class Program_SubjectController : Controller
    {
        private readonly MyAppContext _context;

        public Program_SubjectController(MyAppContext context)
        {
            _context = context;
        }

        // GET: Program_Subject
        public async Task<IActionResult> Index()
        {
            return View(await _context.Program_Subjects.ToListAsync());
        }


      

        // GET: Program_Subject/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var program_Subject = await _context.Program_Subjects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (program_Subject == null)
            {
                return NotFound();
            }

            return View(program_Subject);
        }

        // GET: Program_Subject/Create
        public IActionResult Create(int id, ErrorViewModel error = null)
        {
            var course = _context.CourseProgram.FirstOrDefault(c => c.Id == id);
            ViewBag.Error = error;
            SelectList Subjects = new SelectList(_context.Subjects.Where(s => s.Course == course.Course).ToArray(), "Id", "Name");
            ViewBag.Subjects = Subjects;

            SelectList Teachers = new SelectList(_context.Teachers.ToArray(), "Id", "FullName");
            ViewBag.Teachers = Teachers;

            ViewBag.CourseId = id;
            

            return View();
        }

        // POST: Program_Subject/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create()
        {
            string s1 = Request.Form["courseId"];
            string s2 = Request.Form["subjectId"];
            string s3 = Request.Form["mainteacherId"];
            string s4 = Request.Form["labteacherId"];
            string s5 = Request.Form["type"];

            if (s1 == "" || s2 == "" || s3 == "" || s5 == "")
            {
                return  Create(Convert.ToInt32(Request.Form["courseId"]), new ErrorViewModel()
                {
                    ErrorMessage = "All fields must be inputed"
                });
                //return RedirectToAction("Create", "Program_Subject", new { id = Request.Form["courseId"], error="All fields must be inputed" });
            }

            var course = _context.CourseProgram.FirstOrDefault(c => c.Id == int.Parse(s1));

            var program_Subject = new Program_Subject();
            if (ModelState.IsValid)
            {

                program_Subject.ProgramId = await _context.CourseProgram
                    .FirstAsync(p => p.Id == Convert.ToInt32(Request.Form["courseId"]));

                program_Subject.Subject = await _context.Subjects
                    .FirstAsync(p => p.Id == Convert.ToInt32(Request.Form["subjectId"]));

                program_Subject.MainTeacher = await _context.Teachers
                    .FirstAsync(p => p.Id == Convert.ToInt32(Request.Form["mainteacherId"]));

                program_Subject.Type = s5;

                if (course.Program_Subject.FirstOrDefault(p => p.MainTeacher.FullName == program_Subject.MainTeacher.FullName && p.Subject.Name == program_Subject.Subject.Name && p.Type == program_Subject.Type) != null)
                {
                    return Create(Convert.ToInt32(Request.Form["courseId"]), new ErrorViewModel()
                    {
                        ErrorMessage = "This subjext already exists"
                    });
                    //return RedirectToAction("Create", "Program_Subject", new { id = Request.Form["courseId"], error = "This subjext already exists" });
                }

                if (s4 != "")
                {
                    program_Subject.LabTeacher = await _context.Teachers
                    .FirstAsync(p => p.Id == Convert.ToInt32(s4));
                }
                _context.Add(program_Subject);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "CoursePrograms", new { id = Request.Form["courseId"] });
            }
            return RedirectToAction("Details", "CoursePrograms", new { id = Request.Form["courseId"] });
        }

        // GET: Program_Subject/Edit/5
        public async Task<IActionResult> Edit(int? id, ErrorViewModel error = null)
        {
            ViewBag.Error = error;
            if (id == null)
            {
                return NotFound();
            }
            var program_Subject = await _context.Program_Subjects.FindAsync(id);

            //SelectList Subjects = new SelectList(await _context.Subjects.ToArrayAsync(), "Id", "Name", program_Subject.Subject.Id);
            //SelectList Subjects = new SelectList(_context.Subjects.Where(s => s.Course == course.Course).ToArray(), "Id", "Name");
            SelectList Subjects = new SelectList(await _context.Subjects.Where(s => s.Course == program_Subject.Subject.Course).ToArrayAsync(), "Id", "Name", program_Subject.Subject.Id);
            ViewBag.Subjects = Subjects;

            SelectList Teachers = new SelectList(await _context.Teachers.ToArrayAsync(), "Id", "FullName", program_Subject.MainTeacher.Id);
            ViewBag.Teachers = Teachers;
            SelectList LabTeachers;
            if (program_Subject.LabTeacher != null)
            {
                LabTeachers = new SelectList(await _context.Teachers.ToArrayAsync(), "Id", "FullName", program_Subject.LabTeacher.Id);
            }
            else
            {
                LabTeachers = new SelectList(await _context.Teachers.ToArrayAsync(), "Id", "FullName");
            }
            ViewBag.LabTeachers = LabTeachers;

            ViewBag.CourseId = program_Subject.ProgramId.Id;

            if (program_Subject == null)
            {
                return NotFound();
            }
            return View(program_Subject);
        }

        // POST: Program_Subject/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type")] Program_Subject program_Subject)
        {
            if (id != program_Subject.Id)
            {

            }

            program_Subject.Subject = await _context.Subjects
                .FirstAsync(p => p.Id == Convert.ToInt32(Request.Form["subjectId"]));

            program_Subject.MainTeacher = await _context.Teachers
                .FirstAsync(p => p.Id == Convert.ToInt32(Request.Form["mainteacherId"]));

            program_Subject.Type = Request.Form["type"];
            string labteacher = Request.Form["labteacherId"];
            if (labteacher != "")
            {
                program_Subject.LabTeacher = await _context.Teachers
                .FirstAsync(p => p.Id == Convert.ToInt32(labteacher));
            }
            else
            {
                program_Subject.LabTeacher = null;
                _context.Entry(program_Subject).State = EntityState.Modified;


            }
            if (ModelState.IsValid)
            {
                var element = _context.Program_Subjects.FirstOrDefault(g => g.Subject.Name == program_Subject.Subject.Name && g.Type == program_Subject.Type && g.MainTeacher == program_Subject.MainTeacher);
                if (element != null)
                {
                    return await Edit(id, new ErrorViewModel()
                    {
                        ErrorMessage = "This Progtam_Subject already exists"
                    });
                }
                try
                {
                    
                        _context.Update(program_Subject);
                        await _context.SaveChangesAsync();
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Program_SubjectExists(program_Subject.Id))
                    {

                    }
                    else
                    {
                        throw;
                    }
                }
                /* return RedirectToAction("Details", new RouteValueDictionary(
     new { controller = "СoursePrograms", action = "Details", id = program_Subject.ProgramId.Id }));*/
                // return Redirect("/СoursePrograms/Details/"+ program_Subject.ProgramId.Id);
                return RedirectToAction("Details", "CoursePrograms", new { id = Request.Form["courseId"] });

            }
            //return Redirect("/СoursePrograms/Details/" + program_Subject.ProgramId.Id);
            return RedirectToAction("Details", "CoursePrograms", new { id = Request.Form["courseId"] });
        }

        // GET: Program_Subject/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var program_Subject = await _context.Program_Subjects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (program_Subject == null)
            {
                return NotFound();
            }
            
            ViewBag.CourseId = program_Subject.ProgramId.Id;
            return View(program_Subject);
        }

        // POST: Program_Subject/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var program_Subject = await _context.Program_Subjects.FindAsync(id);
            _context.Program_Subjects.Remove(program_Subject);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Details", "CoursePrograms", new { id = Request.Form["courseId"] });
            //return RedirectToAction(nameof(Index));
        }

        private bool Program_SubjectExists(int id)
        {
            return _context.Program_Subjects.Any(e => e.Id == id);
        }
    }
}
