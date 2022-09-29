using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySchedulerWork.Data;
using MySchedulerWork.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MySchedulerWork.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyAppContext _context;

        public HomeController(MyAppContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.Groups = _context.Groups.ToList();
            ViewBag.Teachers = _context.Teachers.ToList();
            ViewBag.Subjects = _context.Subjects.ToList();
            ViewBag.Audiences = _context.Audiences.ToList();
            ViewBag.CoursePrograms = _context.CourseProgram.ToList();
       
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
