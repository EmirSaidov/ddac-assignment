using DDAC_Assignment_Mining_Commerce.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDAC_Assignment_Mining_Commerce.Helper;
using Microsoft.AspNetCore.Http;
using DDAC_Assignment_Mining_Commerce.Services;
using DDAC_Assignment_Mining_Commerce.Models.Analytics;

namespace DDAC_Assignment_Mining_Commerce.Controllers
{
    public class LoginController : Controller
    {
        private readonly MiningCommerceContext _context;
        private readonly AnalyticService _analytics;
        private readonly CosmosTableService _cosmosTable;
        private readonly TableService _tableService;
        public LoginController(
            MiningCommerceContext _context,
            AnalyticService _analytics,
            CosmosTableService _cosmosTable,
            TableService _tableService
            )
        {
            this._context = _context;
            this._analytics = _analytics;
            this._cosmosTable = _cosmosTable;
            this._tableService = _tableService;
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

                    var roleCheck = user.getUserRole(_cosmosTable).Result;
                    BuyerModel isBuyer =null;
                    SellerModel isSeller=null;
                    AdminModel isAdmin=null;
                    //Extra steps taken for guarding customers without role in Cosmos Table
                    if (roleCheck == "" || roleCheck=="B") {
                        isBuyer = this._context.Buyer.FirstOrDefault<BuyerModel>(buyer => buyer.user.ID == user.ID);
                    }
                    if (roleCheck == "" || roleCheck == "S") {
                        isSeller = this._context.Seller.FirstOrDefault<SellerModel>(seller => seller.user.ID == user.ID);
                    }
                    if (roleCheck == "" || roleCheck == "A") {
                        isAdmin = this._context.Admin.FirstOrDefault<AdminModel>(admin => admin.user.ID == user.ID);
                    }
                    if (isBuyer != null)
                    {
                        if (roleCheck == "") { await Task.Run(()=> isBuyer.user.setUserRole(_cosmosTable, UserType.BUYER)); }
                        HttpContext.Session.Set<UserModel>("AuthUser", isBuyer.user);
                        HttpContext.Session.Set<BuyerModel>("AuthRole", isBuyer);
                        HttpContext.Session.Set<UserType>("UserType", UserType.BUYER);
                        IEnumerable<Notification> notifications = await _tableService.GetNotificationsByPK(isBuyer.ID.ToString());
                        HttpContext.Session.Set<IEnumerable<Notification>>("Notifications", notifications);
                    }
                    else if (isSeller != null)
                    {
                        if (roleCheck == "") { await Task.Run(() => isSeller.user.setUserRole(_cosmosTable, UserType.SELLER)); }
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
                        if (roleCheck == "") { await Task.Run(() => isAdmin.user.setUserRole(_cosmosTable, UserType.ADMIN)); }
                        HttpContext.Session.Set<UserModel>("AuthUser", isAdmin.user);
                        HttpContext.Session.Set<AdminModel>("AuthRole", isAdmin);
                        HttpContext.Session.Set<UserType>("UserType", UserType.ADMIN);
                    }
                    await this._analytics.pushAnalytics<LoginAnalytic>(new LoginAnalytic(user.ID));
                    return RedirectToAction(actionName: "Index", controllerName: "Home");
                }
                else { ModelState.AddModelError("ValidationError", "Login Invalid! Account does not exists"); }
            }
            return View("../User/Login", login);
        }
    }

}
