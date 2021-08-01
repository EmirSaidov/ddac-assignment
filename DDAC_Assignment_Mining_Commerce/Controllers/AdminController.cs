using DDAC_Assignment_Mining_Commerce.Models;
using DDAC_Assignment_Mining_Commerce.Models.Analytics;
using DDAC_Assignment_Mining_Commerce.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Controllers
{
    public class AdminController : Controller
    {
        private readonly MiningCommerceContext _context;
        private readonly BlobService _blob;
        private readonly CosmosTableService _cosmosTable;
        private readonly AnalyticService _analytic;
        public AdminController(MiningCommerceContext _context, BlobService _blob, CosmosTableService _cosmosTable, AnalyticService _analytic) {
            this._context = _context;
            this._blob = _blob;
            this._cosmosTable = _cosmosTable;
            this._analytic = _analytic;
        }
        public async Task<IActionResult> Approve()
        {
            return View(await _context.Seller.Include(seller => seller.user).Where(seller => !seller.is_approved).ToListAsync());
        }

        public async Task<IActionResult> ApproveSeller(int id) {
            var seller_entity = _context.Seller.FirstOrDefault<SellerModel>(s => s.ID == id);
            seller_entity.is_approved = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: "Approve");
        }

        public IActionResult Register() {
            return View("../Admin/Register");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAdmin(AdminModel admin, IFormFile profile_picture)
        {
            if (ModelState.IsValid)
            {
                if (this._context.User.FirstOrDefault<UserModel>(user => user.email.ToLower() == admin.user.email.ToLower()) == null)
                {
                    await this._context.User.AddAsync(admin.user);
                    await this._context.Admin.AddAsync(admin);
                    await this._context.SaveChangesAsync();
                    admin.user.UploadProfilePicture(profile_picture, this._blob);
                    await admin.user.setUserRole(_cosmosTable, UserType.ADMIN);
                    TempData["registration_status"] = "Admin Account Created";
                }
                else { ModelState.AddModelError(string.Empty, "Account with Email Already Exists"); }
            }
            return View("../Admin/Register", admin);
        }

        //Report Stuff
    }

}
