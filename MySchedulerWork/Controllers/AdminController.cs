using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zephyrus.Controllers
{
    public class AdminController : Controller
    {

        public AdminController()
        {
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateAnime()
        {
            string temp = "It works";

            return Ok(temp);
        }
    }
}
