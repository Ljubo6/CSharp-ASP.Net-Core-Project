using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniverseRestaurant.Data;
using UniverseRestaurant.Models;
using UniverseRestaurant.Models.ViewModels;
using UniverseRestaurant.Utility;

namespace UniverseRestaurant.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;

        public HomeController(ApplicationDbContext db)
        {
            this.db = db;
        }
        //GET - INDEX
        public async Task<IActionResult> Index()
        {
            IndexViewModel IndexVM = new IndexViewModel()
            {
                MenuItem = await this.db.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).ToListAsync(),
                Category = await this.db.Categories.ToListAsync(),
                Coupon = await this.db.Coupons.Where(c => c.IsActive == true).ToArrayAsync(),
            };

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                var cnt = this.db.ShoppingCart.Where(u => u.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(StaticDetail.ssShoppingCartCount, cnt);
            }

            return View(IndexVM);
        }

        //GET - DETAILS
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var menuItemFromDb = await this.db.MenuItems
                .Include(m => m.Category)
                .Include(m => m.SubCategory)
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();

            ShoppingCart cartObj = new ShoppingCart()
            {
                MenuItem = menuItemFromDb,
                MenuItemId = menuItemFromDb.Id,
            };

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                var count = this.db.ShoppingCart.Where(u => u.ApplicationUserId == claim.Value).ToList().Count();
                HttpContext.Session.SetInt32(StaticDetail.ssShoppingCartCount,count);
            }

            return this.View(cartObj);
        }


        //POST - DETAILS
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(ShoppingCart CartObject)
        {
            CartObject.Id = 0;
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                CartObject.ApplicationUserId = claim.Value;

                ShoppingCart cartFromDb = await this.db.ShoppingCart
                    .Where(c => c.ApplicationUserId == CartObject.ApplicationUserId && c.MenuItemId == CartObject.MenuItemId)
                    .FirstOrDefaultAsync();
                if (cartFromDb == null)
                {
                    await this.db.AddAsync(CartObject);
                }
                else
                {
                    cartFromDb.Count = cartFromDb.Count + CartObject.Count;
                }

                await this.db.SaveChangesAsync();

                var count = this.db.ShoppingCart.Where(c => c.ApplicationUserId == CartObject.ApplicationUserId).ToList().Count();

                HttpContext.Session.SetInt32(StaticDetail.ssShoppingCartCount,count);

                return RedirectToAction("Index");
            }
            else
            {
                var menuItemFromDb = await this.db.MenuItems
               .Include(m => m.Category)
               .Include(m => m.SubCategory)
               .Where(m => m.Id == CartObject.MenuItemId)
               .FirstOrDefaultAsync();

                ShoppingCart cartObj = new ShoppingCart()
                {
                    MenuItem = menuItemFromDb,
                    MenuItemId = menuItemFromDb.Id,
                };

                return View(cartObj);
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
