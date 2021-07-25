using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DDAC_Assignment_Mining_Commerce.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDAC_Assignment_Mining_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace DDAC_Assignment_Mining_Commerce.Controllers
{
    public class UserController : Controller
    {
        private readonly MiningCommerceContext _context;
        public UserController(MiningCommerceContext _context) {
            this._context = _context;
        }

        public IActionResult Edit()
        {
            switch (HttpContext.Session.Get<UserType>("UserType")) {
                case UserType.ADMIN:
                    return View("../User/Edit/Admin", HttpContext.Session.Get<AdminModel>("AuthRole"));
                case UserType.BUYER:
                    return View("../User/Edit/Buyer", HttpContext.Session.Get<BuyerModel>("AuthRole"));
                case UserType.SELLER:
                    return View("../User/Edit/Seller", HttpContext.Session.Get<SellerModel>("AuthRole"));
                default:
                    return RedirectToAction(actionName: "Index", controllerName: "Login");
            }
        }

        public IActionResult Password()
        {
            UserModel AuthUser = HttpContext.Session.Get<UserModel>("AuthUser");
            return View("../User/Edit/Password",AuthUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Password(UserModel user, string ConfirmPassword) {
            if (user.password != null && ConfirmPassword != null)
            {
                if (user.password == ConfirmPassword)
                {
                    UserModel user_entity = _context.User.First<UserModel>(u => u.ID == user.ID);
                    user_entity.password = user.password;
                    await _context.SaveChangesAsync();
                    TempData["password_status"] = "Password Updated";
                }
                else
                {
                    ModelState.AddModelError("ValidationError", "Password Field and Confirmed Password Field do not match");
                }
            }
            return View("../User/Edit/Password",user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBuyer(BuyerModel buyer)
        {
            if (ModelState.IsValid)
            {
                UserModel user_context = this._context.User.FirstOrDefault<UserModel>(user => user.email.ToLower() == buyer.user.email.ToLower());
                if (user_context == null || user_context.ID == buyer.user.ID)
                {
                        try
                        {
                            //Untrack UserModel
                            _context.Entry(user_context).State = EntityState.Detached;
                            _context.Update(buyer);
                            await _context.SaveChangesAsync();
                            HttpContext.Session.Set<BuyerModel>("AuthRole", buyer);
                            HttpContext.Session.Set<UserModel>("AuthUser", buyer.user);
                            TempData["edit_status"] = "Profile is updated";
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (_context.Buyer.Any(e => e.ID == buyer.ID))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }
                    return RedirectToAction(actionName: "Edit");
                }
                else { ModelState.AddModelError(string.Empty, "Account with Email Already Exists"); }
            }
            return View("../User/Edit/Buyer", buyer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSeller(SellerModel seller)
        {
            if (ModelState.IsValid)
            {
                UserModel user_context = this._context.User.FirstOrDefault<UserModel>(user => user.email.ToLower() == seller.user.email.ToLower());
                if (user_context == null || user_context.ID == seller.user.ID)
                {
                    try
                    {
                        //Untrack UserModel
                        _context.Entry(user_context).State = EntityState.Detached;
                        _context.Update(seller);
                        await _context.SaveChangesAsync();
                        HttpContext.Session.Set<SellerModel>("AuthRole", seller);
                        HttpContext.Session.Set<UserModel>("AuthUser", seller.user);
                        TempData["edit_status"] = "Profile is updated";
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (_context.Seller.Any(e => e.ID == seller.ID))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(actionName: "Edit");
                }
                else { ModelState.AddModelError(string.Empty, "Account with Email Already Exists"); }
            }
            return View("../User/Edit/Seller", seller);
        }
    }

}
