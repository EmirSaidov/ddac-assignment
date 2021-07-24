using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View("../User/Login");
        }
    }
}
