using DDAC_Assignment_Mining_Commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DDAC_Assignment_Mining_Commerce.Services;
using Microsoft.AspNetCore.Http;


namespace DDAC_Assignment_Mining_Commerce.Controllers
{
    public class DisplayProductController : Controller
    {
        private readonly MiningCommerceContext _context;
        private readonly BlobService _blobService;
        public DisplayProductController(MiningCommerceContext context, BlobService blobService)
        {
            _context = context;
            _blobService = blobService;

        }

        public IActionResult Pay()
        {
            return View("../DisplayProduct/Pay");
        }
        public IActionResult Receipt()
        {
            return View("../DisplayProduct/Receipt");
        }
        public IActionResult Cart()
        {
            return View("../DisplayProduct/Cart");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
