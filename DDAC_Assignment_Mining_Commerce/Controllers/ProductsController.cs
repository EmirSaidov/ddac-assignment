using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DDAC_Assignment_Mining_Commerce.Analytics;
using DDAC_Assignment_Mining_Commerce.Helper;
using DDAC_Assignment_Mining_Commerce.Models;
using DDAC_Assignment_Mining_Commerce.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MVCProductShop2011Lab4.Controllers
{
    public class ProductsController : Controller
    {
        private readonly MiningCommerceContext _context;
        private readonly BlobService _blobService;
        private readonly BusService _busService;
        private readonly AnalyticService _analytics;

        public ProductsController(MiningCommerceContext context, BlobService blobService, BusService busService, AnalyticService _analytics)
        {
            _context = context;
            _blobService = blobService;
            _busService = busService;
            this._analytics = _analytics;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Product.Where(product => product.sellerID == HttpContext.Session.Get<SellerModel>("AuthRole").ID).ToListAsync());
        }

        //GET: AllProducts
        public async Task<IActionResult> Display()
        {
            return View("../DisplayProducts/Display", await _context.Product.ToListAsync());
        }

        //GET: SearchedProduct
        public async Task<IActionResult> Search(string Name)
        {
            if (Name == null)
            {
                return View("../DisplayProducts/Display", await _context.Product.ToListAsync());
            }
            else
            {
                return View("../DisplayProducts/Display", await _context.Product.Where(product => product.productName.Contains(Name)).ToListAsync());

            }
        }



        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile image, double productPrice, [Bind("productName, productPrice, productMass, productDescription")] ProductModel product)
        {
            product.sellerID = HttpContext.Session.Get<SellerModel>("AuthRole").ID;

            if (ModelState.IsValid)
            {
                SellerModel seller = _context.Seller.FirstOrDefault<SellerModel>(s=> s.ID == product.sellerID);
                product.seller = seller;
                _ = _context.Add(product);
                _ = await _context.SaveChangesAsync();
                if (image != null) { product.UploadProfilePicture(image, _blobService); }
                _ = _busService.QueueNewProductNotification(NotificationType.NewProduct, product, null, null);
                _ = _analytics.logAnalytic<ProductAnalytic>(new ProductAnalytic(product.ID,product.seller.ID));
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductModel product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormFile image, [Bind("ID, imageUri, productName, productPrice, productMass, productDescription")] ProductModel product)
        {
            ProductModel oldProduct = await _context.Product
                .FirstOrDefaultAsync(m => m.ID == product.ID);
            _context.Entry(oldProduct).State = EntityState.Detached;
            product.sellerID = HttpContext.Session.Get<SellerModel>("AuthRole").ID;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    if (image != null)
                    {
                        product.UploadProfilePicture(image, _blobService);
                    }
                    _ = _busService.QueueNewProductNotification(NotificationType.EditProductPrice, product, oldProduct.productPrice, product.productPrice);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _blobService.deleteFromProductContainer(product.getProductPicName());
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            _ = _busService.QueueNewProductNotification(NotificationType.RemoveProduct, product, null, null);
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ID == id);
        }
    }
}
