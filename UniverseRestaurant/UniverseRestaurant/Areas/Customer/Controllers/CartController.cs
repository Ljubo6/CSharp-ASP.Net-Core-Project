using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniverseRestaurant.Data;
using UniverseRestaurant.Models.ViewModels;
using UniverseRestaurant.Utility;

namespace UniverseRestaurant.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext db;

        [BindProperty]
        public OrderDetailsCart detailCart { get; set; }

        public CartController(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            detailCart = new OrderDetailsCart()
            {
                OrderHeader = new Models.OrderHeader(),
            };

            detailCart.OrderHeader.OrderTotal = 0m;

            var claimsIdentity = (ClaimsIdentity)User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var cart = this.db.ShoppingCart.Where(c => c.ApplicationUserId == claim.Value);

            if (cart != null)
            {
                detailCart.listCart = cart.ToList();
            }

            foreach (var list in detailCart.listCart)
            {
                list.MenuItem = await this.db.MenuItems.FirstOrDefaultAsync(m => m.Id == list.MenuItemId);

                detailCart.OrderHeader.OrderTotal = detailCart.OrderHeader.OrderTotal + ((decimal)list.MenuItem.Price * list.Count);
                list.MenuItem.Description = StaticDetail.ConvertToRawHtml(list.MenuItem.Description);

                if (list.MenuItem.Description.Length > 100)
                {
                    list.MenuItem.Description = list.MenuItem.Description.Substring(0,99) + "...";
                }               
            }
            detailCart.OrderHeader.OrderTotalOriginal = detailCart.OrderHeader.OrderTotal;

            if (HttpContext.Session.GetString(StaticDetail.ssCouponCode) != null)
            {
                detailCart.OrderHeader.CouponCode = HttpContext.Session.GetString(StaticDetail.ssCouponCode);
                var couponFromDb = await this.db.Coupons.Where(c => c.Name.ToLower() == detailCart.OrderHeader.CouponCode.ToLower()).FirstOrDefaultAsync();

                detailCart.OrderHeader.OrderTotal = (decimal)StaticDetail.DiscountedPrice(couponFromDb,detailCart.OrderHeader.OrderTotalOriginal);
            }

            return View(detailCart);
        }
        public IActionResult AddCoupon()
        {
            if (detailCart.OrderHeader.CouponCode == null)
            {
                detailCart.OrderHeader.CouponCode = "";
            }

            HttpContext.Session.SetString(StaticDetail.ssCouponCode,detailCart.OrderHeader.CouponCode);

            return RedirectToAction(nameof(Index));
        }
    }
}