using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySchedulerWork.Data;
using MySchedulerWork.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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

            var stream = HttpContext.Session.GetString("JwToken");
            var role = "";
            var username = "";
            if (!string.IsNullOrEmpty(stream))
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadToken(stream) as JwtSecurityToken;
                role = token.Claims.First(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
                username = token.Claims.First(claim => claim.Type == ClaimsIdentity.DefaultNameClaimType).Value;
            }

            ViewBag.Username = username;
            ViewBag.Role = role;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult HelloPage()
        {
            var stream = HttpContext.Session.GetString("JwToken");
            var role = "";
            var username = "";
            if (!string.IsNullOrEmpty(stream))
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadToken(stream) as JwtSecurityToken;
                role = token.Claims.First(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
                username = token.Claims.First(claim => claim.Type == ClaimsIdentity.DefaultNameClaimType).Value;
            }

            ViewBag.Username = username;
            ViewBag.Role = role;


            return View();
        }

        public IActionResult UsersInfoPAGE()
        {
            var stream = HttpContext.Session.GetString("JwToken");
            var role = "";
            var username = "";
            if (!string.IsNullOrEmpty(stream))
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadToken(stream) as JwtSecurityToken;
                role = token.Claims.First(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
                username = token.Claims.First(claim => claim.Type == ClaimsIdentity.DefaultNameClaimType).Value;
            }

            ViewBag.Username = username;
            ViewBag.Role = role;

            return View();
        }

        public IActionResult AdminsInfoPAGE()
        {
            var stream = HttpContext.Session.GetString("JwToken");
            var role = "";
            var username = "";
            if (!string.IsNullOrEmpty(stream))
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadToken(stream) as JwtSecurityToken;
                role = token.Claims.First(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
                username = token.Claims.First(claim => claim.Type == ClaimsIdentity.DefaultNameClaimType).Value;
            }

            ViewBag.Username = username;
            ViewBag.Role = role;


            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
