using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MySchedulerWork.Models;
using MySchedulerWork.Services;
using MySchedulerWork;

namespace MySchedulerWork.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IAuthorizationService _authorizationService;

        public UserController(ILogger<UserController> logger, IAuthorizationService authorizationService)
        {
            _logger = logger;
            _authorizationService = authorizationService;
        }
        public IActionResult Login(UserViewModel userModel = null)
        {
            ViewBag.UserModel = userModel;

            return View();
        }
        public IActionResult Register(UserViewModel userModel = null)
        {
            ViewBag.UserModel = userModel;

            return View();
        }

        public IActionResult LogOff()
        {
            HttpContext.Session.SetString("JwToken", "");
            return RedirectToAction("HelloPage", "Home");
        }

        public IActionResult Cabinet()
        {
            return View();
        }

        public IActionResult LoginAction(string username, string password)
        {
            string token = _authorizationService.GetToken(username, password);

            if (token == null)
            {
                return RedirectToAction("Login", new { userModel = new UserViewModel { UserName = username, Password = password, Error = "Wrong login or password"} });
            }

            HttpContext.Session.SetString("JwToken", token);

            return RedirectToAction("HelloPage", "Home");
        }

        public IActionResult RegisterAction(string username, string email, string password)
        {
            var user = new User() { UserName = username, Password = password, Email = email, Role = "User" };

            _authorizationService.AddUser(user);

            string token = _authorizationService.GetToken(username, password);

            if (token == null)
            {
                return BadRequest();
            }

            HttpContext.Session.SetString("JwToken", token);

            return RedirectToAction("HelloPage", "Home");
        }
    }
}
