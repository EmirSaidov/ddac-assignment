using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DDAC_Assignment_Mining_Commerce.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDAC_Assignment_Mining_Commerce.Models;
using Microsoft.EntityFrameworkCore;
using DDAC_Assignment_Mining_Commerce.Services;
using Microsoft.Azure.Cosmos.Table;

namespace DDAC_Assignment_Mining_Commerce.Controllers
{
    public class UserController : Controller
    {
        private readonly MiningCommerceContext _context;
        private readonly BlobService _blob;
        private readonly CosmosTableService _cosmosTable;
        private readonly TableService _tableService;
        public UserController(MiningCommerceContext _context,BlobService _blob, CosmosTableService _cosmosTable, TableService _tableService) {
            this._context = _context;
            this._blob = _blob;
            this._cosmosTable = _cosmosTable;
            this._tableService = _tableService;
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

        public IActionResult ViewSeller(int id)
        {
            SellerModel seller = _context.Seller.Include(s => s.user).First(s => s.ID == id);
            return RedirectToAction(actionName:"ViewUser",new { id = seller.user.ID });
        }

        public async Task<IActionResult> ViewUser(int id, string returnURL = null) {
            UserModel user = this._context.User.FirstOrDefault<UserModel>(user => user.ID == id);
            ViewData["returnURL"] = Request.Headers["Referer"].ToString();
            if (user != null)
            {
                var roleCheck = user.getUserRole(_cosmosTable).Result;
                BuyerModel isBuyer = null;
                SellerModel isSeller = null;
                AdminModel isAdmin = null;
                //Extra steps taken for guarding customers without role in Cosmos Table
                if (roleCheck == "" || roleCheck == "B")
                {
                    isBuyer = this._context.Buyer.FirstOrDefault<BuyerModel>(buyer => buyer.user.ID == user.ID);
                }
                if (roleCheck == "" || roleCheck == "S")
                {
                    isSeller = this._context.Seller.FirstOrDefault<SellerModel>(seller => seller.user.ID == user.ID);
                }
                if (roleCheck == "" || roleCheck == "A")
                {
                    isAdmin = this._context.Admin.FirstOrDefault<AdminModel>(admin => admin.user.ID == user.ID);
                }
                if (isBuyer != null)
                {
                    if (roleCheck == "") {await Task.Run(() => isBuyer.user.setUserRole(_cosmosTable, UserType.BUYER)); }
                    return View("../User/View/Buyer", isBuyer);
                   
                }
                else if (isSeller != null)
                {
                    if (roleCheck == "") { await Task.Run(() => isSeller.user.setUserRole(_cosmosTable, UserType.SELLER)); }
                    return View("../User/View/Seller", isSeller);

                }
                else if (isAdmin != null)
                {
                    if (roleCheck == "") { await Task.Run(() => isAdmin.user.setUserRole(_cosmosTable, UserType.ADMIN)); }
                    return View("../User/View/Admin", isAdmin);
                }
            }
            return View(new ErrorViewModel());
        }

        public IActionResult ViewProfile()
        {
            UserModel AuthUser = HttpContext.Session.Get<UserModel>("AuthUser");
            switch (HttpContext.Session.Get<UserType>("UserType"))
            {
                case UserType.ADMIN:
                    return View("../User/View/Admin", HttpContext.Session.Get<AdminModel>("AuthRole"));
                case UserType.BUYER:
                    return View("../User/View/Buyer", HttpContext.Session.Get<BuyerModel>("AuthRole"));
                case UserType.SELLER:
                    return View("../User/View/Seller", HttpContext.Session.Get<SellerModel>("AuthRole"));
                default:
                    return RedirectToAction(actionName: "Index", controllerName: "Login");
            }
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
        public async Task<IActionResult> EditBuyer(BuyerModel buyer, IFormFile profile_picture)
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
                            buyer.user.UploadProfilePicture(profile_picture,this._blob);
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
        public async Task<IActionResult> EditSeller(SellerModel seller,IFormFile profile_picture)
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
                        seller.user.UploadProfilePicture(profile_picture, this._blob);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdmin(AdminModel admin, IFormFile profile_picture)
        {
            if (ModelState.IsValid)
            {
                UserModel user_context = this._context.User.FirstOrDefault<UserModel>(user => user.email.ToLower() == admin.user.email.ToLower());
                if (user_context == null || user_context.ID == admin.user.ID)
                {
                    try
                    {
                        //Untrack UserModel
                        _context.Entry(user_context).State = EntityState.Detached;
                        _context.Update(admin);
                        await _context.SaveChangesAsync();
                        admin.user.UploadProfilePicture(profile_picture, this._blob);
                        HttpContext.Session.Set<AdminModel>("AuthRole", admin);
                        HttpContext.Session.Set<UserModel>("AuthUser", admin.user);
                        TempData["edit_status"] = "Profile is updated";
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (_context.Buyer.Any(e => e.ID == admin.ID))
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
            return View("../User/Edit/Admin", admin);
        }

        public async Task<bool> GetSubscriptionStatusAsync(string? sellerID)
        {
            if (sellerID != null)
            {
                string buyerID = HttpContext.Session.Get<BuyerModel>("AuthRole").ID.ToString();
                Subscription subscription = await _tableService.GetSubscription(sellerID, buyerID);
                if (subscription != null)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task Subscribe(string? sellerID)
        {
            if (sellerID != null)
            {
                int buyerID = HttpContext.Session.Get<BuyerModel>("AuthRole").ID;
                Subscription subscription = new Subscription();
                subscription.sellerID = int.Parse(sellerID);
                subscription.buyerID = buyerID;
                subscription.AssignPartitionKey();
                subscription.AssignRowKey();
                _tableService.Subscribe(subscription);
            }
        }

        public async Task Unsubscribe(string? sellerID)
        {
            if (sellerID != null)
            {
                string buyerID = HttpContext.Session.Get<BuyerModel>("AuthRole").ID.ToString();
                Subscription subscription = await _tableService.GetSubscription(sellerID, buyerID);
                if (subscription != null)
                {
                    _tableService.Unsubscribe(subscription);
                }
            }
        }
    }

}
