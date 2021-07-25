using DDAC_Assignment_Mining_Commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Controllers
{
    public class AdminController : Controller
    {
        MiningCommerceContext _context;
        public AdminController(MiningCommerceContext _context) {
            this._context = _context;
        }
        public async Task<IActionResult> Approve()
        {
            return View(await _context.Seller.Include(seller => seller.user).Where(seller=>!seller.is_approved).ToListAsync());
        }

        public async Task<IActionResult> ApproveSeller(int id) {
            Console.WriteLine("==========");
            Console.WriteLine(id);
            var seller_entity = _context.Seller.FirstOrDefault<SellerModel>(s => s.ID == id);
            seller_entity.is_approved = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: "Approve");
        }
    }
}
