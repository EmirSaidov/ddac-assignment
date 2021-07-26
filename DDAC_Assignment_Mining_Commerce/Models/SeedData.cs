using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider) {
            using (var context = new MiningCommerceContext(
             serviceProvider.GetRequiredService<DbContextOptions<MiningCommerceContext>>()))
            {
                seedUser(context);
                var users = context.User.ToDictionary(user => user.email, user => user);
                //Console.WriteLine("====================");
                //users.Select(i => $"{i.Key}: {i.Value}").ToList().ForEach(Console.WriteLine);
                //Console.WriteLine("====================");
                seedBuyer(context,users);
                seedSeller(context,users);
                var sellers = context.Seller.ToDictionary(seller => seller.user.email, seller => seller);  
                seedAdmin(context,users);
                seedProduct(context,sellers);

            }
        }

        public static void seedUser(MiningCommerceContext context) {
            if (context.User.Any()) return;
            context.User.AddRange(
                new  UserModel { 
                    fullname = "Buyer 1",
                    email = "buyer_1@mail.com",
                    DOB = DateTime.Parse("1989-2-12"),
                    gender = "M",
                    phone = "011-1234568",
                    password = "buyer_one"
                },
                new UserModel
                {
                    fullname = "Buyer 2",
                    email = "buyer_2@mail.com",
                    DOB = DateTime.Parse("1989-2-12"),
                    gender = "F",
                    phone = "011-1234568",
                    password = "buyer_two"
                },
                new UserModel
                {
                    fullname = "Seller 1",
                    email = "seller_1@mail.com",
                    DOB = DateTime.Parse("1989-2-12"),
                    gender = "F",
                    phone = "011-1234568",
                    password = "seller_one"
                },
                new UserModel
                {
                    fullname = "Seller 2",
                    email = "seller_2@mail.com",
                    DOB = DateTime.Parse("1989-2-12"),
                    gender = "M",
                    phone = "011-1234568",
                    password = "seller_two"
                },
                new UserModel
                {
                    fullname = "Admin",
                    email = "admin@mail.com",
                    DOB = DateTime.Parse("1989-2-12"),
                    gender = "M",
                    phone = "011-1234568",
                    password = "admin"
                }
            );
            context.SaveChanges();
        }

        public static void seedBuyer(MiningCommerceContext context,Dictionary<string,UserModel> users) {
            if (context.Buyer.Any()) return;
            context.Buyer.AddRange(
                new BuyerModel { 
                    user = users["buyer_1@mail.com"],
                    address = "No.20 bukit jalil 32",
                    shipping_address = "No.20 bukit jalil 32"
                },
                new BuyerModel
                {
                    user = users["buyer_2@mail.com"],
                    address = "No.20 bukit jalil 32",
                    shipping_address = "No.20 bukit jalil 32"
                }
                );
            context.SaveChanges();
        }

        public static void seedSeller(MiningCommerceContext context, Dictionary<string, UserModel> users)
        {
            if (context.Seller.Any()) return;
            context.Seller.AddRange(
                new SellerModel
                {
                    user = users["seller_1@mail.com"],
                    store_address = "No.20 bukit jalil 32",
                    store_contact = "565656565",
                    storeName = " Cave One ",
                    is_approved = true,
                },
                new SellerModel
               {
                user = users["seller_2@mail.com"],
                    store_address = "No.20 bukit jalil 32",
                    store_contact = "565656565",
                    storeName = " Cave Two",
                    is_approved = true,
                }
                );
            context.SaveChanges();
        }

        public static void seedAdmin(MiningCommerceContext context, Dictionary<string, UserModel> users)
        {
            if (context.Admin.Any()) return;
            context.Admin.AddRange(
                new AdminModel
                {
                    user = users["admin@mail.com"]
                    
                });

            context.SaveChanges();
        }

        public static void seedProduct(MiningCommerceContext context, Dictionary<string,SellerModel> sellers)
        {
            if (context.Product.Any()) return;
            context.Product.AddRange(
                new ProductModel
                {
                    seller = sellers["seller_1@mail.com"],
                    sellerID = sellers["seller_1@mail.com"].ID,
                    imageUri = "https://miningassignment.blob.core.windows.net/product/202107260125117905.png",
                    productName = "Gold Ore",
                    productPrice = 100,
                    productMass = 20,
                    productDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ac placerat est, id mollis lorem. Morbi felis arcu, dignissim et ornare ut, ornare non tortor. Suspendisse vel leo sem. Sed eu euismod neque. In vestibulum orci vel lectus finibus, tincidunt sagittis dui sodales. Morbi et sem eget sapien aliquam pellentesque. Vivamus convallis arcu elit, maximus aliquet leo luctus sed. Sed ac tortor pretium, tincidunt velit at, porttitor neque. Cras vestibulum congue ex, quis aliquam magna ullamcorper ut. Aliquam fermentum, libero et posuere pretium, augue mi consequat enim, finibus ultricies nulla ex vitae risus. Fusce ut odio dolor. Morbi ultrices sem.",
                },
                new ProductModel
                {
                    seller = sellers["seller_1@mail.com"],
                    sellerID = sellers["seller_1@mail.com"].ID,
                    imageUri = "https://miningassignment.blob.core.windows.net/product/202107260125117906.png",
                    productName = "Copper Ore",
                    productPrice = 10,
                    productMass = 800,
                    productDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ac placerat est, id mollis lorem. Morbi felis arcu, dignissim et ornare ut, ornare non tortor. Suspendisse vel leo sem. Sed eu euismod neque. In vestibulum orci vel lectus finibus, tincidunt sagittis dui sodales. Morbi et sem eget sapien aliquam pellentesque. Vivamus convallis arcu elit, maximus aliquet leo luctus sed. Sed ac tortor pretium, tincidunt velit at, porttitor neque. Cras vestibulum congue ex, quis aliquam magna ullamcorper ut. Aliquam fermentum, libero et posuere pretium, augue mi consequat enim, finibus ultricies nulla ex vitae risus. Fusce ut odio dolor. Morbi ultrices sem.",
                },
                new ProductModel
                {
                    seller = sellers["seller_1@mail.com"],
                    sellerID = sellers["seller_1@mail.com"].ID,
                    imageUri = "https://miningassignment.blob.core.windows.net/product/202107260125117907.png",
                    productName = "Tungsten Ore",
                    productPrice = 15,
                    productMass = 500,
                    productDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ac placerat est, id mollis lorem. Morbi felis arcu, dignissim et ornare ut, ornare non tortor. Suspendisse vel leo sem. Sed eu euismod neque. In vestibulum orci vel lectus finibus, tincidunt sagittis dui sodales. Morbi et sem eget sapien aliquam pellentesque. Vivamus convallis arcu elit, maximus aliquet leo luctus sed. Sed ac tortor pretium, tincidunt velit at, porttitor neque. Cras vestibulum congue ex, quis aliquam magna ullamcorper ut. Aliquam fermentum, libero et posuere pretium, augue mi consequat enim, finibus ultricies nulla ex vitae risus. Fusce ut odio dolor. Morbi ultrices sem.",
                },
                new ProductModel
                {
                    seller = sellers["seller_2@mail.com"],
                    sellerID = sellers["seller_2@mail.com"].ID,
                    imageUri = "https://miningassignment.blob.core.windows.net/product/202107260125117908.png",
                    productName = "Iron Ore",
                    productPrice = 20,
                    productMass = 500,
                    productDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ac placerat est, id mollis lorem. Morbi felis arcu, dignissim et ornare ut, ornare non tortor. Suspendisse vel leo sem. Sed eu euismod neque. In vestibulum orci vel lectus finibus, tincidunt sagittis dui sodales. Morbi et sem eget sapien aliquam pellentesque. Vivamus convallis arcu elit, maximus aliquet leo luctus sed. Sed ac tortor pretium, tincidunt velit at, porttitor neque. Cras vestibulum congue ex, quis aliquam magna ullamcorper ut. Aliquam fermentum, libero et posuere pretium, augue mi consequat enim, finibus ultricies nulla ex vitae risus. Fusce ut odio dolor. Morbi ultrices sem.",
                },
                new ProductModel
                {
                    seller = sellers["seller_2@mail.com"],
                    sellerID = sellers["seller_2@mail.com"].ID,
                    imageUri = "https://miningassignment.blob.core.windows.net/product/202107260125117909.png",
                    productName = "Silver Ore",
                    productPrice = 23,
                    productMass = 700,
                    productDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ac placerat est, id mollis lorem. Morbi felis arcu, dignissim et ornare ut, ornare non tortor. Suspendisse vel leo sem. Sed eu euismod neque. In vestibulum orci vel lectus finibus, tincidunt sagittis dui sodales. Morbi et sem eget sapien aliquam pellentesque. Vivamus convallis arcu elit, maximus aliquet leo luctus sed. Sed ac tortor pretium, tincidunt velit at, porttitor neque. Cras vestibulum congue ex, quis aliquam magna ullamcorper ut. Aliquam fermentum, libero et posuere pretium, augue mi consequat enim, finibus ultricies nulla ex vitae risus. Fusce ut odio dolor. Morbi ultrices sem.",
                });

            context.SaveChanges();
        }
    }

   
}
