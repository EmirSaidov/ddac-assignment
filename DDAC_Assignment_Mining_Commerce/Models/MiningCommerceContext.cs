using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DDAC_Assignment_Mining_Commerce.Models
{
    public class MiningCommerceContext : DbContext
    {
        public MiningCommerceContext(DbContextOptions<MiningCommerceContext> options) : base(options)
        {
        }

        public DbSet<UserModel> User { get; set; }
        public DbSet<SellerModel> Seller { get; set; }
        public DbSet<BuyerModel> Buyer { get; set; }
        public DbSet<AdminModel> Admin { get; set; }

    }
}
