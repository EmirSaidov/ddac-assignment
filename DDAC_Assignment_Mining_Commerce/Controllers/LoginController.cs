using DDAC_Assignment_Mining_Commerce.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDAC_Assignment_Mining_Commerce.Helper;
using Microsoft.AspNetCore.Http;
using DDAC_Assignment_Mining_Commerce.Services;

namespace DDAC_Assignment_Mining_Commerce.Controllers
{
    public class LoginController : Controller
    {
        private readonly MiningCommerceContext _context;
        private readonly AnalyticService _analytics;
        public LoginController(
            MiningCommerceContext _context,
            AnalyticService _analytics
            ){
            this._context = _context;
            this._analytics = _analytics;
        }

        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            return View("../User/Login");
        }

        public IActionResult Logout() {
            HttpContext.Session.Clear();
            return View("../User/Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserModel login)
        {
            if (login.email!=null && login.password != null)
            {
                UserModel user = this._context.User.FirstOrDefault<UserModel>(user => user.email.ToLower() == login.email.ToLower());
                if (user != null)
                {
                    //Wrong Password
                    if (user.password != login.password) {
                        ModelState.AddModelError("ValidationError", "Wrong Password");
                        return View("../User/Login", login);
                    }
                    BuyerModel isBuyer = this._context.Buyer.FirstOrDefault<BuyerModel>(buyer => buyer.user.ID == user.ID);
                    SellerModel isSeller = this._context.Seller.FirstOrDefault<SellerModel>(seller => seller.user.ID == user.ID);
                    AdminModel isAdmin = this._context.Admin.FirstOrDefault<AdminModel>(admin => admin.user.ID == user.ID);

                    if (isBuyer != null)
                    {
                        HttpContext.Session.Set<UserModel>("AuthUser", isBuyer.user);
                        HttpContext.Session.Set<BuyerModel>("AuthRole", isBuyer);
                        HttpContext.Session.Set<UserType>("UserType", UserType.BUYER);
                    }
                    else if (isSeller != null)
                    {
                        if (isSeller.is_approved) {
                            HttpContext.Session.Set<UserModel>("AuthUser", isSeller.user);
                            HttpContext.Session.Set<SellerModel>("AuthRole", isSeller);
                            HttpContext.Session.Set<UserType>("UserType", UserType.SELLER);
                        } else{
                            ModelState.AddModelError("ValidationError", "Your Seller Account has not been approved");
                            return View("../User/Login", login);
                        }
                        
                    }
                    else if (isAdmin != null) {
                        HttpContext.Session.Set<UserModel>("AuthUser", isAdmin.user);
                        HttpContext.Session.Set<AdminModel>("AuthRole", isAdmin);
                        HttpContext.Session.Set<UserType>("UserType", UserType.ADMIN);
                    }
                    this._analytics.pushAnalytics<LoginAnalytics>(new LoginAnalytics(user.ID, DateTime.Now));
                    return RedirectToAction(actionName: "Index", controllerName: "Home");
                }
                else { ModelState.AddModelError("ValidationError", "Login Invalid! Account does not exists"); }
            }
            return View("../User/Login", login);
        }
    }

}
