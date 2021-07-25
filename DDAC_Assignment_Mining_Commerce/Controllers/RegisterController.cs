using DDAC_Assignment_Mining_Commerce.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Controllers
{
    public class RegisterController : Controller
    {
        private readonly MiningCommerceContext _context;
        public RegisterController(MiningCommerceContext _context) {
            this._context = _context;
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
        public async Task<IActionResult> RegisterBuyer( BuyerModel buyer)
        {
            if (ModelState.IsValid)
            {
                if (this._context.User.FirstOrDefault<UserModel>(user => user.email.ToLower() == buyer.user.email.ToLower()) == null) {
                    await this._context.User.AddAsync(buyer.user);
                    await this._context.Buyer.AddAsync(buyer);
                    await this._context.SaveChangesAsync();
                    //Redirect to Login
                    return RedirectToAction(actionName: "Login", controllerName: "Login");
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
