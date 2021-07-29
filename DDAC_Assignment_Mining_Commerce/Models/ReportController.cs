using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Models
{
    public class ReportController : Controller
    {
        public readonly string view_src = "../Admin/Report";
        public IActionResult Index()
        {
            return View(view_src+"/Index");
        }
    }
}
