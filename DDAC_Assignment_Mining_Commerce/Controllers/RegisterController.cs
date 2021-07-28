using DDAC_Assignment_Mining_Commerce.Models;
using DDAC_Assignment_Mining_Commerce.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Controllers
{
    public class RegisterController : Controller
    {
        private readonly MiningCommerceContext _context;
        private readonly BlobService _blob;
        public RegisterController(
            MiningCommerceContext _context,
            BlobService _blob
            ) {
            this._context = _context;
            this._blob = _blob;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Buyer()
        {
            return View("../User/Register/Buyer");
        }

        public IActionResult Seller()
        {
            return View("../User/Register/Seller");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterBuyer( BuyerModel buyer, IFormFile profile_picture)
        {
            if (ModelState.IsValid)
            {
                if (this._context.User.FirstOrDefault<UserModel>(user => user.email.ToLower() == buyer.user.email.ToLower()) == null) {
                    await this._context.User.AddAsync(buyer.user);
                    await this._context.Buyer.AddAsync(buyer);
                    await this._context.SaveChangesAsync();
                    if (profile_picture != null)
                    {
                        try
                        {
                            this._blob.uploadImgToBlobContainer("profilepicture", buyer.user.getProfilePicName(), profile_picture);
                        }
                        catch (Exception ex) {
                            Debug.WriteLine("Profile Picture Upload Failed+ex");
                            Debug.WriteLine(ex);
                        }
                    }
                   
                    //Redirect to Login
                    return RedirectToAction(actionName: "Index", controllerName: "Login");
                }
                else { ModelState.AddModelError(string.Empty, "Account with Email Already Exists"); }
            }
            return View("../User/Register/Buyer",buyer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterSeller(SellerModel seller)
        {
            if (ModelState.IsValid)
            {
                if (this._context.User.FirstOrDefault<UserModel>(user => user.email.ToLower() == seller.user.email.ToLower()) == null)
                {
                    await this._context.User.AddAsync(seller.user);
                    await this._context.Seller.AddAsync(seller);
                    await this._context.SaveChangesAsync();
                    //Redirect to Login
                    return RedirectToAction(actionName: "Index", controllerName: "Login");
                }
                else { ModelState.AddModelError(string.Empty, "Account with Email Already Exists"); }
            }
            
            return View("../User/Register/Seller", seller);
        }
    }
}
