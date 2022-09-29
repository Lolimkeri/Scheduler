using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySchedulerWork.Data;
using MySchedulerWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MySchedulerWork.Controllers
{
    public class SchedulerController : Controller
    {
        private readonly MyAppContext _context;
        public SchedulerController(MyAppContext context)
        {
            _context = context;
        }
        public IActionResult Index(string error = null)
        {
            ViewBag.ProgramList = new SelectList(_context.CourseProgram.ToArray(), "Id", "Name");
            ViewBag.GroupList = new MultiSelectList(_context.Groups.ToArray(), "Id", "Name");
            ViewBag.Error = error;
            return View();
        }

        public void AutoGenerate(string sendedgroups = "")
        {
            string[] strgroupsId;
            if (sendedgroups == "")
            {
                strgroupsId = sendedgroups.Split(',');
                int[] groupsId = new int[strgroupsId.Length];
                for (int i = 0; i < strgroupsId.Length; i++)
                {
                    groupsId[i] = Convert.ToInt32(strgroupsId[i]);
                }
                List<Group> groups = new List<Group>();
                for (int i = 0; i < groupsId.Length; i++)
                    groups.Add(_context.Groups.First(p => p.Id == groupsId[i]));

                List<Pair> bestPairs = new List<Pair>();     
            }
        }

        public IActionResult MoveToCreator(string sendedgroups = "")
        {
            string CourseId;
            try
            {
                CourseId = Request.Form["programid"];
            }
            catch (Exception e)
            {
                CourseId = "-01";
            }
            string str;
            if (sendedgroups == "")
                str = Request.Form["groups"];
            else
                str = sendedgroups;

            if (str == null || str == "" || CourseId == null || CourseId == "")
            {
                return RedirectToAction("Index", "Scheduler", new { error = "Select at least one group in a course"});
            }


            ViewBag.GroupToSend = str;

            string[] strgroupsId = str.Split(',');
            int[] groupsId = new int[strgroupsId.Length];
            for (int i = 0; i < strgroupsId.Length; i++)
            {
                groupsId[i] = Convert.ToInt32(strgroupsId[i]);
            }
            List<Group> groups = new List<Group>();
            for (int i = 0; i < groupsId.Length; i++)
                groups.Add(_context.Groups.First(p => p.Id == groupsId[i]));
            ViewBag.CourseId = str;
            ViewBag.Groups = groups;
            ViewBag.TimeOfPairs = new string[] { "8:30-9:50", "10:10-11:30", "11:50-13:10", "13:30-14:50", "15:05-16:25", "16:40-18:00", "18:10-19:30", "19:40-21:00" };
            ViewBag.NameOfPairs = new string[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII" };
            ViewBag.Days = new string[] { "Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця" };

            //  var pp = _context.PairToFGroup.Where(p => p.Group.Id==0);
            //  var ppp = groups[0].PairToFGroups[0].Pair.Program_Subject
            Dictionary<string, List<Pair>> dict = new Dictionary<string, List<Pair>>();

            List<ConflictHelper> TeacherConflicts = new List<ConflictHelper>();
            List<ConflictHelper> AudienceConflicts = new List<ConflictHelper>();
            List<HasConflict> hasConflict = new List<HasConflict>();

            List<PairToFGroup> pairsToGroups = new List<PairToFGroup>();

            for (int i = 0; i < groups.Count; i++)
            {
                List<Pair> newlist = new List<Pair>();
                for (int j = 0; j < groups[i].PairToFGroups.Count; j++)
                {
                    // string newKey = groups[i].Name + groups[i].PairToFGroups[j].Pair.Day + groups[i].PairToFGroups[j].Pair.Number;
                    //  var IfExist = dict.Keys.FirstOrDefault(p=>p== newKey);
                    // if(IfExist==null)
                    // dict.Add(newKey, (groups[i].PairToFGroups[j].Pair,false));
                    // else
                    // dict.Add(newKey+"conf", (groups[i].PairToFGroups[j].Pair, true));
                    Pair curentPair = groups[i].PairToFGroups[j].Pair;
                    newlist.Add(curentPair);
                    pairsToGroups.Add(groups[i].PairToFGroups[j]);

                    #region Conflict
                    string TeacherKey = curentPair.Program_Subject.MainTeacher.ToString() + curentPair.Day + curentPair.Number;
                    string TeacherKey1 = null;
                    if(curentPair.Program_Subject.LabTeacher!=null)
                    {
                        TeacherKey1 = curentPair.Program_Subject.LabTeacher.ToString() + curentPair.Day + curentPair.Number;
                    }
                    string AudienceKey = curentPair.Audience.Name + curentPair.Day + curentPair.Number;

                    ConflictHelper findTeacher = TeacherConflicts.FirstOrDefault(p => p.Key == TeacherKey);
                    /*if(findTeacher==null)
                    {
                        _context.
                    }*/
                    if (findTeacher == null)
                    {
                        TeacherConflicts.Add(new ConflictHelper(TeacherKey, curentPair));
                        if(TeacherKey1!=null)
                        TeacherConflicts.Add(new ConflictHelper(TeacherKey1, curentPair));

                    }
                    if (findTeacher == null)
                    {
                        Pair findPair = _context.Pairs.FirstOrDefault(p => p.Number == curentPair.Number && p.Day == curentPair.Day && p.Program_Subject.MainTeacher.Id == curentPair.Program_Subject.MainTeacher.Id && p.Id != curentPair.Id);
                        if (findPair != null)
                        {
                            findTeacher = new ConflictHelper(TeacherKey, findPair);
                        }
                    }

                    if (findTeacher != null)
                    {

                        if (findTeacher.Pair.Id != curentPair.Id && hasConfByWeek(findTeacher.Pair.IsEverWeek, curentPair.IsEverWeek))
                        {
                            if (!((curentPair.Program_Subject.Type == "Лекція" && findTeacher.Pair.Program_Subject.Type == "Лекція" && curentPair.Program_Subject.Name == findTeacher.Pair.Program_Subject.Name && curentPair.Audience == findTeacher.Pair.Audience && curentPair.Program_Subject.Subject.Id == findTeacher.Pair.Program_Subject.Subject.Id) ))
                            {
                                string messageT = "Конфлікт викладача: " + findTeacher.Pair.Program_Subject.MainTeacher.ToString() + " в групах: " + groups[i].Name + " та " + findTeacher.Pair.PairToFGroups[0].Group.Name;
                                // findTeacher.SetConflict(findTeacher.Pair.Program_Subject.MainTeacher.ToString()); // Має пару в іншої групи
                                hasConflict.Add(new HasConflict(curentPair, messageT));
                                hasConflict.Add(new HasConflict(findTeacher.Pair, messageT));
                            }
                        }

                    }

                    ConflictHelper findAudience = AudienceConflicts.FirstOrDefault(p => p.Key == AudienceKey);
                    if (findAudience == null)
                    {
                        AudienceConflicts.Add(new ConflictHelper(TeacherKey, curentPair));                        
                    }
                    if (findAudience == null)
                    {
                        Pair findPair = _context.Pairs.FirstOrDefault(p => p.Number == curentPair.Number && p.Day == curentPair.Day && p.Audience.Name == curentPair.Audience.Name && p.Id != curentPair.Id);
                        if (findPair != null)
                        {
                            findAudience = new ConflictHelper(TeacherKey, findPair);
                        }
                    }
                    if(findAudience != null)                   
                    {
                        if (findAudience.Pair.Id != curentPair.Id && hasConfByWeek(findAudience.Pair.IsEverWeek, curentPair.IsEverWeek))
                        {
                            if (!(curentPair.Program_Subject.Type == "Лекція" && findAudience.Pair.Program_Subject.Type == "Лекція" && curentPair.Program_Subject.Name == findAudience.Pair.Program_Subject.Name && curentPair.Program_Subject.MainTeacher.ToShortString() == findAudience.Pair.Program_Subject.MainTeacher.ToShortString() && curentPair.Program_Subject.Subject.Id == findAudience.Pair.Program_Subject.Subject.Id))
                            {
                                string MessageA = "Конфлікт аудиторії: " + findAudience.Pair.Audience.Name + " в групах: " + groups[i].Name + " та " + findAudience.Pair.PairToFGroups[0].Group.Name;
                                //findAudience.SetConflict(findAudience.Pair.Audience.Name); ; // Є пара в цій аудиторії
                                hasConflict.Add(new HasConflict(curentPair, MessageA));
                                hasConflict.Add(new HasConflict(findAudience.Pair, MessageA));
                            }
                        }
                    }

                    int number = 0;
                    var tempList = new List<Pair>();
                    for (int l = 0; l < pairsToGroups.Count; l++)
                    {
                        var temp = pairsToGroups[l].Pair.Audience.Name + pairsToGroups[l].Pair.Day + pairsToGroups[l].Pair.Number;
                        if (AudienceKey == temp)
                        {
                            number += pairsToGroups[l].Group.AmountOfStudents;
                            tempList.Add(pairsToGroups[l].Pair);
                        }
                    }

                    if (number > curentPair.Audience.Capacity)
                    {
                        foreach (var item in tempList)
                        {
                            string MessageC = "Конфлікт аудиторії: " + item.Audience.Name + " не підходить по кількості студентів";
                            hasConflict.Add(new HasConflict(item, MessageC));
                        }
                    }
                    #endregion

                }
                //    newlist.OrderBy(p => p.Day).ThenBy(p => p.Number);
                dict.Add(groups[i].Name, newlist);
            }
            ViewBag.dict = dict;
            ViewBag.Conflict = hasConflict;
            return View("Test");
        }
        public bool IfThisId(int id, int[] arr)
        {
            bool rez = false;
            for (int i = 0; i < arr.Length; i++)
            {
                if (id == arr[i])
                    rez = true;
            }
            return rez;
        }
        public bool hasConfByWeek(int a1, int a2)
        {
            if (a1 == 1 && a2 == 2)
                return false;
            if (a1 == 2 && a2 == 1)
                return false;
            return true;
        }
        public IActionResult AddPair()
        {

            return View();
        }
    }
    public class HasConflict
    {
        public HasConflict(Pair p, string mes)
        {
            Pair = p;
            Message = mes;
        }
        public Pair Pair { get; set; }
        public string Message { get; set; }
    }
    public class ConflictHelper
    {
        public ConflictHelper(string k, Pair p, bool IsCon = false, string mess = "")
        {
            Key = k;
            Pair = p;
            IsConflict = IsCon;
            Message = mess;
        }
        public void SetConflict(string _Message)
        {
            IsConflict = true;
            Message = _Message;
        }
        public string Key { get; set; }
        public Pair Pair { get; set; }
        public string Message { get; set; }
        public bool IsConflict { get; set; }
    }
}
