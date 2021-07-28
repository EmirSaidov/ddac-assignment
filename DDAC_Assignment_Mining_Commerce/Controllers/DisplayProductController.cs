using DDAC_Assignment_Mining_Commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Controllers
{
    public class DisplayProductController : Controller
    {
        private readonly MiningCommerceContext _context;
        public DisplayProductController(MiningCommerceContext _context)
        {
            this._context = _context;
        }

        
        public IActionResult Minerals()
        {
            return View("../Products/Minerals");
        }

        public IActionResult Pay()
        {
            return View("../Products/Pay");
        }
        public IActionResult Receipt()
        {
            return View("../Products/Receipt");
        }
        public IActionResult Cart()
        {
            return View("../Products/Cart");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
