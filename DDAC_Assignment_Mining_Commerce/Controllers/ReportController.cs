using DDAC_Assignment_Mining_Commerce.Models.Analytics;
using DDAC_Assignment_Mining_Commerce.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Models
{
    public class ReportController : Controller
    {
        private readonly AnalyticService _analytic;
        private readonly MiningCommerceContext _context;
        public ReportController(AnalyticService _analytic, MiningCommerceContext _context){
            this._analytic = _analytic;
            this._context = _context;
        }

        public readonly string view_src = "../Admin/Report";
        public async Task<IActionResult> Index()
        {
            ReportViewModel viewModel = new ReportViewModel();
            List<LoginAnalytic> loginAnalytics = await _analytic.getAnalytics<LoginAnalytic>(new LoginAnalytic());
            List<RegisterAnalytic> registrationAnalytics = await _analytic.getAnalytics<RegisterAnalytic>(new RegisterAnalytic());
            List<UserModel> users = await _context.User.ToListAsync<UserModel>();
            List<BuyerModel> buyers = await _context.Buyer.Include(buyer => buyer.user).ToListAsync<BuyerModel>();
            List<SellerModel> sellers = await _context.Seller.Include(seller => seller.user).ToListAsync<SellerModel>();
            List<ProductModel> products = await _context.Product.Include(product => product.seller).ToListAsync<ProductModel>();

            viewModel.login_ = loginAnalysis(loginAnalytics, users);
            viewModel.registration_ = registrationAnalysis(registrationAnalytics);
            viewModel.buyers = buyers;
            viewModel.sellers = sellers;
            viewModel.products = products;
            return View(view_src+"/Index",viewModel);
        }

        public LoginAnalyticRes loginAnalysis(List<LoginAnalytic> data,List<UserModel> user) {
            Dictionary<int, int> activeUser = new Dictionary<int, int>();
            Dictionary<string,string> top5ActiveUser = new Dictionary<string, string>();
            Dictionary<string, int> dailyLogin = new Dictionary<string, int>();
            data.ForEach(analytic => {
                //Active Users
                if (activeUser.ContainsKey(analytic.user_id))
                {
                    activeUser[analytic.user_id]++;
                }
                else activeUser.Add(analytic.user_id, 1);
                //Count daily login
                if (dailyLogin.ContainsKey(analytic.getLoginDate()))
                {
                    dailyLogin[analytic.getLoginDate()]++;
                }
                else dailyLogin.Add(analytic.getLoginDate(), 1);
            });
            //Top 5 Active Users
            Dictionary<int, int> temp = new Dictionary<int, int>();
            activeUser = activeUser.OrderByDescending(o => o.Value).Take(5).ToDictionary(pair=>pair.Key,pair=>pair.Value);
            foreach(KeyValuePair<int,int> active in activeUser) {
                UserModel userItem = user.Find(u => u.ID == active.Key);
                top5ActiveUser.Add(userItem.email,userItem.fullname);
            }
            return new LoginAnalyticRes { top5ActiveUser = top5ActiveUser, dailyLogin = dailyLogin };
        }

        public RegisterAnalyticRes registrationAnalysis(List<RegisterAnalytic> data)
        {
            Dictionary<string, int> dailyRegistration = new Dictionary<string, int>();
            data.ForEach(analytic => {
                //Count daily login
                if (dailyRegistration.ContainsKey(analytic.getRegisterDate()))
                {
                    dailyRegistration[analytic.getRegisterDate()]++;
                }
                else dailyRegistration.Add(analytic.getRegisterDate(), 1);
            });
            return new RegisterAnalyticRes { dailyRegistration = dailyRegistration };
        }
    }

    public class ReportViewModel
    {
        public LoginAnalyticRes login_ { get; set; }
        public RegisterAnalyticRes registration_ { get; set; }
        public List<BuyerModel> buyers { get; set; }
        public List<SellerModel> sellers { get; set; }
        public List<ProductModel> products { get; set; }
    }

    public class RegisterAnalyticRes {
        public Dictionary<string, int> dailyRegistration { get; set; }
    }

    public class LoginAnalyticRes {
        public Dictionary<string, string> top5ActiveUser { get; set; }
        public Dictionary<string, int> dailyLogin { get; set; }
    }

}
