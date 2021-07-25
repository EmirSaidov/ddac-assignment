using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DDAC_Assignment_Mining_Commerce.Helper;
using DDAC_Assignment_Mining_Commerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MVCSellerShop2011Lab4.Controllers
{
    public class StoreController : Controller
    {
        private readonly MiningCommerceContext _context;

        public StoreController(MiningCommerceContext context)
        {
            _context = context;
        }

        // GET: Sellers/Details/5
        public async Task<IActionResult> Details()
        {
            int id = HttpContext.Session.Get<SellerModel>("AuthRole").ID;
            var seller = await _context.Seller
                .FirstOrDefaultAsync(m => m.ID == id);
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }

        // GET: Sellers/Edit/5
        public async Task<IActionResult> Edit()
        {
            int id = HttpContext.Session.Get<SellerModel>("AuthRole").ID;
            var seller = await _context.Seller.FindAsync(id);
            if (seller == null)
            {
                return NotFound();
            }
            return View(seller);
        }

        // POST: Sellers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID, storeName, store_address, store_contact")] SellerModel seller)
        {
            if (id != seller.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(seller);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SellerExists(seller.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details));
            }
            return View(seller);
        }

        private bool SellerExists(int id)
        {
            return _context.Seller.Any(e => e.ID == id);
        }
    }
}
