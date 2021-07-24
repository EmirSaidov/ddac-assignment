using DDAC_Assignment_Mining_Commerce.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Controllers
{
    public class LoginController : Controller
    {
        private readonly MiningCommerceContext _context;
        public LoginController(MiningCommerceContext _context){
            this._context = _context;
        }

        public IActionResult Login()
        {
            return View("../User/Login");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserModel login)
        {
            if (ModelState.IsValid)
            {
                UserModel user = this._context.User.FirstOrDefault<UserModel>(user => user.email.ToLower() == login.email.ToLower());
                UserType userType;
                if (user != null)
                {
                    BuyerModel isBuyer = this._context.Buyer.FirstOrDefault<BuyerModel>(buyer => buyer.user.ID == user.ID);
                    SellerModel isSeller = this._context.Seller.FirstOrDefault<SellerModel>(seller => seller.user.ID == user.ID);
                    AdminModel isAdmin = this._context.Admin.FirstOrDefault<AdminModel>(admin => admin.user.ID == user.ID);

                    if (isBuyer != null)
                    {
                        userType = UserType.BUYER;
                    }
                    else if (isSeller != null)
                    {
                        userType = UserType.SELLER;
                    }
                    else if (isAdmin != null) {
                        userType = UserType.ADMIN;
                    }
                }
                else { ModelState.AddModelError(string.Empty, "Login Invalid! Account does not exists"); }
            }
            return View("../User/Login", login);
        }
    }

}
