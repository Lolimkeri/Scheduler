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
    class ForEveryWeek
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ForEveryWeek(int _Id, string _Name)
        {
            Id = _Id;
            Name = _Name;
        }
    }
    public class PairsController : Controller
    {
        private readonly MyAppContext _context;

        public PairsController(MyAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Pairs.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pair = await _context.Pairs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pair == null)
            {
                return NotFound();
            }

            return View(pair);
        }

        public IActionResult Create(int groupId, int day, int number, string returnGroup, int IsEveryWeek = 0)
        {
            Group group = _context.Groups.FirstOrDefault(p => p.Id == groupId);
            if (group.CourseProgram != null)
            {
                ////////// Помилка
            }
            List<Audience> audiences = _context.Audiences.ToList();
            ViewBag.Audiences = new SelectList(audiences, "Id", "Name");

            //ViewBag.Days = new SelectList( new string[] { "Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця" });

            List<Program_Subject> program_Subj = _context.Program_Subjects.Where(p => p.ProgramId.Id == group.CourseProgram.Id).ToList();
            List<PairToFGroup> PairToThatGroup = _context.PairToFGroup.Where(p => p.Group.Id == groupId).ToList();

            //program_Subj.RemoveAll(p => PairToThatGroup.FirstOrDefault(p1 => p1.Pair.Program_Subject.Id == p.Id && p1.Pair.IsEverWeek == 0) != null);

            List<ForEveryWeek> ForEveryWeekArr = new List<ForEveryWeek>();
            if (IsEveryWeek == 0)
            {
                ForEveryWeekArr.Add(new ForEveryWeek(0, "Кожного тижня"));
                ForEveryWeekArr.Add(new ForEveryWeek(1, "По чисельнику"));
                ForEveryWeekArr.Add(new ForEveryWeek(2, "По знаменнику"));
            }
            if (IsEveryWeek == 2)
            {
                ForEveryWeekArr.Add(new ForEveryWeek(2, "По знаменнику"));
            }
            if (IsEveryWeek == 1)
            {
                ForEveryWeekArr.Add(new ForEveryWeek(1, "По чисельнику"));
            }
            ViewBag.ForEveryWeek = new SelectList(ForEveryWeekArr, "Id", "Name");

            ViewBag.Subjects = new SelectList(program_Subj, "Id", "Name");
            ViewBag.Day = day;
            ViewBag.Number = number;
            ViewBag.GroupId = groupId;
            ViewBag.GroupToSend = returnGroup;
            var GroupsToCourse = _context.Groups.Where(p => p.CourseProgram.Id == group.CourseProgram.Id).ToArray();
            ViewBag.GroupList = new MultiSelectList(GroupsToCourse, "Id", "Name", new int[]{groupId});
            ViewBag.Group = groupId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create()
        {
            Pair pair = new Pair();
            int subjId = Convert.ToInt32(Request.Form["subject"]);
            int AudId = Convert.ToInt32(Request.Form["audience"]);
            int GroupId = Convert.ToInt32(Request.Form["group"]);
            int NumberId = Convert.ToInt32(Request.Form["number"]);
            int DayId = Convert.ToInt32(Request.Form["day"]);
            int IsEveryWeek = Convert.ToInt32(Request.Form["iseveryweek"]);

            string str = Request.Form["chosengroup"];
            string[] strgroupsId = str.Split(',');
            int[] groupsId = new int[strgroupsId.Length];
            for (int i = 0; i < strgroupsId.Length; i++)
            {
                groupsId[i] = Convert.ToInt32(strgroupsId[i]);
            }

            string sendedgroups = Request.Form["sendedgroups"];

            pair.Day = (DayOfWeek)DayId;
            pair.Program_Subject = await _context.Program_Subjects.FirstAsync(p => p.Id == subjId);
            pair.Audience = await _context.Audiences.FirstAsync(p => p.Id == AudId);
            pair.Number = NumberId;
            pair.IsEverWeek = IsEveryWeek;
            _context.Add(pair);
            await _context.SaveChangesAsync();
            for (int i = 0; i < groupsId.Length; i++)
            {
                PairToFGroup toGroup = new PairToFGroup();
                toGroup.Pair = pair;
                toGroup.Group = await _context.Groups.FirstAsync(p => p.Id == groupsId[i]);
                _context.Add(toGroup);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("MoveToCreator", "Scheduler", new { sendedgroups = sendedgroups });


        }

        public async Task<IActionResult> Edit(int id, int groupId, int day, int number, string returnGroup, int IsEveryWeek = 0)
        {


            var pair = await _context.Pairs.FindAsync(id);
            ViewBag.PairId = id;
            Group group = _context.Groups.FirstOrDefault(p => p.Id == groupId);
            if (group.CourseProgram != null)
            {
                ////////// Помилка
            }
            List<Audience> audiences = _context.Audiences.ToList();
            ViewBag.Audiences = new SelectList(audiences, "Id", "Name", pair.Audience.Id);

            //ViewBag.Days = new SelectList( new string[] { "Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця" });

            List<Program_Subject> program_Subj = _context.Program_Subjects.Where(p => p.ProgramId.Id == group.CourseProgram.Id).ToList();
            List<PairToFGroup> PairToThatGroup = _context.PairToFGroup.Where(p => p.Group.Id == groupId).ToList();


            //program_Subj.RemoveAll(p => p.Id != pair.Program_Subject.Id && (PairToThatGroup.FirstOrDefault(p1 => p1.Pair.Program_Subject.Id == p.Id) != null)); 



            ViewBag.IsEveryWeek = IsEveryWeek;




            ViewBag.Subjects = new SelectList(program_Subj, "Id", "Name", pair.Program_Subject.Id);
            ViewBag.Day = day;
            ViewBag.Number = number;
            ViewBag.GroupId = groupId;
            ViewBag.GroupToSend = returnGroup;

            if (pair == null)
            {
                return NotFound();
            }
            return View(pair);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit()
        {
            int pairId = Convert.ToInt32(Request.Form["pairid"]);
            Pair pair = _context.Pairs.FirstOrDefault(p => p.Id == pairId);
            int subjId = Convert.ToInt32(Request.Form["subject"]);
            int AudId = Convert.ToInt32(Request.Form["audience"]);
            int GroupId = Convert.ToInt32(Request.Form["group"]);
            int NumberId = Convert.ToInt32(Request.Form["number"]);
            int DayId = Convert.ToInt32(Request.Form["day"]);
            int IsEveryWeek = Convert.ToInt32(Request.Form["iseveryweek"]);
            string sendedgroups = Request.Form["sendedgroups"];

            //pair.Day = (DayOfWeek)DayId;
            pair.Program_Subject = await _context.Program_Subjects.FirstAsync(p => p.Id == subjId);
            pair.Audience = await _context.Audiences.FirstAsync(p => p.Id == AudId);
          //  pair.Number = NumberId;
           // pair.IsEverWeek = IsEveryWeek;



            try
            {
                _context.Update(pair);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PairExists(pair.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }
            return RedirectToAction("MoveToCreator", "Scheduler", new { sendedgroups = sendedgroups });
        }
        public async Task<IActionResult> Delete(int id, string returnGroup)
        {
            var pair = await _context.Pairs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pair != null)
            {
               List< PairToFGroup> pairTo = pair.PairToFGroups;
                for(int i=0;i< pairTo.Count;i++)
                {
                    _context.Remove(pairTo[i]);
                }
                _context.Remove(pair);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("MoveToCreator", "Scheduler", new { sendedgroups = returnGroup });
        }

        // POST: Pairs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pair = await _context.Pairs.FindAsync(id);
            _context.Pairs.Remove(pair);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PairExists(int id)
        {
            return _context.Pairs.Any(e => e.Id == id);
        }
    }
}
