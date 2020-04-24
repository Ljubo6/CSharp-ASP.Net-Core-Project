using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using UniverseRestaurant.Data;
using UniverseRestaurant.Models;
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


        public async Task<IActionResult> Summary()
        {
            detailCart = new OrderDetailsCart()
            {
                OrderHeader = new Models.OrderHeader(),
            };

            detailCart.OrderHeader.OrderTotal = 0m;

            var claimsIdentity = (ClaimsIdentity)User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ApplicationUser applicationUser = await this.db.ApplicationUser.Where(c => c.Id == claim.Value).FirstOrDefaultAsync();

            var cart = this.db.ShoppingCart.Where(c => c.ApplicationUserId == claim.Value);

            if (cart != null)
            {
                detailCart.listCart = cart.ToList();
            }

            foreach (var list in detailCart.listCart)
            {
                list.MenuItem = await this.db.MenuItems.FirstOrDefaultAsync(m => m.Id == list.MenuItemId);

                detailCart.OrderHeader.OrderTotal = detailCart.OrderHeader.OrderTotal + ((decimal)list.MenuItem.Price * list.Count);              
            }
            detailCart.OrderHeader.OrderTotalOriginal = detailCart.OrderHeader.OrderTotal;

            detailCart.OrderHeader.PickUpName = applicationUser.Name;
            detailCart.OrderHeader.PhoneNumber = applicationUser.PhoneNumber;
            detailCart.OrderHeader.PickUpTime = DateTime.Now;


            if (HttpContext.Session.GetString(StaticDetail.ssCouponCode) != null)
            {
                detailCart.OrderHeader.CouponCode = HttpContext.Session.GetString(StaticDetail.ssCouponCode);
                var couponFromDb = await this.db.Coupons.Where(c => c.Name.ToLower() == detailCart.OrderHeader.CouponCode.ToLower()).FirstOrDefaultAsync();

                detailCart.OrderHeader.OrderTotal = (decimal)StaticDetail.DiscountedPrice(couponFromDb, detailCart.OrderHeader.OrderTotalOriginal);
            }

            return View(detailCart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(string stripeToken)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            detailCart.listCart = await this.db.ShoppingCart.Where(c => c.ApplicationUserId == claim.Value).ToListAsync();

            detailCart.OrderHeader.PaymentStatus = StaticDetail.PaymentStatusPending;            
            detailCart.OrderHeader.OrderDate = DateTime.Now;            
            detailCart.OrderHeader.UserId = claim.Value;            
            detailCart.OrderHeader.Status = StaticDetail.PaymentStatusPending;            
            detailCart.OrderHeader.PickUpTime = Convert.ToDateTime(detailCart.OrderHeader.PickUpDate.ToShortDateString() + " " + detailCart.OrderHeader.PickUpTime.ToShortTimeString());

            List<OrderDetails> orderDetailsList = new List<OrderDetails>();

            this.db.OrderHeader.Add(detailCart.OrderHeader);

            await this.db.SaveChangesAsync();

            detailCart.OrderHeader.OrderTotalOriginal = 0;

            foreach (var item in detailCart.listCart)
            {
                item.MenuItem = await this.db.MenuItems.FirstOrDefaultAsync(m => m.Id == item.MenuItemId);
                OrderDetails orderDetails = new OrderDetails
                {
                    MenuItemId = item.MenuItemId,
                    OrderId = detailCart.OrderHeader.Id,
                    Description = item.MenuItem.Description,
                    Name = item.MenuItem.Name,
                    Price = (decimal)item.MenuItem.Price,
                    Count = item.Count
                };

                detailCart.OrderHeader.OrderTotalOriginal += orderDetails.Count * orderDetails.Price;

                this.db.OrderDetails.Add(orderDetails);
            }
            
            if (HttpContext.Session.GetString(StaticDetail.ssCouponCode) != null)
            {
                detailCart.OrderHeader.CouponCode = HttpContext.Session.GetString(StaticDetail.ssCouponCode);
                var couponFromDb = await this.db.Coupons.Where(c => c.Name.ToLower() == detailCart.OrderHeader.CouponCode.ToLower()).FirstOrDefaultAsync();

                detailCart.OrderHeader.OrderTotal = (decimal)StaticDetail.DiscountedPrice(couponFromDb, detailCart.OrderHeader.OrderTotalOriginal);
            }
            else
            {
                detailCart.OrderHeader.OrderTotal = detailCart.OrderHeader.OrderTotalOriginal;
            }

            detailCart.OrderHeader.CouponCodeDiscount = detailCart.OrderHeader.OrderTotalOriginal - detailCart.OrderHeader.OrderTotalOriginal;

            await this.db.SaveChangesAsync();

            this.db.ShoppingCart.RemoveRange(detailCart.listCart);

            HttpContext.Session.SetInt32(StaticDetail.ssShoppingCartCount,0);

            await this.db.SaveChangesAsync();

            var options = new ChargeCreateOptions
            {
                Amount = Convert.ToInt32(detailCart.OrderHeader.OrderTotal * 100),
                Currency = "usd",
                Description = "Orders ID : " + detailCart.OrderHeader.Id,
                Source = stripeToken,
            };

            var service = new ChargeService();
            Charge charge = service.Create(options);

            if (charge.BalanceTransactionId == null)
            {
                detailCart.OrderHeader.PaymentStatus = StaticDetail.PaymentStatusRejected;
            }
            else
            {
                detailCart.OrderHeader.TransactionId = charge.BalanceTransactionId;
            }

            if (charge.Status.ToLower() == "succeeded")
            {
                detailCart.OrderHeader.PaymentStatus = StaticDetail.PaymentStatusApproved;
                detailCart.OrderHeader.Status = StaticDetail.StatusSubmitted;
            }
            else
            {
                detailCart.OrderHeader.PaymentStatus = StaticDetail.PaymentStatusRejected;
            }

            await this.db.SaveChangesAsync();

            return RedirectToAction("Index","Home");
            //return RedirectToAction("Confirm","Order",new { id = detailCart.OrderHeader.Id});
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

        public IActionResult RemoveCoupon()
        {

            HttpContext.Session.SetString(StaticDetail.ssCouponCode, string.Empty);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Plus(int cartId)
        {
            var cart = await this.db.ShoppingCart.FirstOrDefaultAsync(c => c.Id == cartId);
            cart.Count++;

            await this.db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Minus(int cartId)
        {
            var cart = await this.db.ShoppingCart.FirstOrDefaultAsync(c => c.Id == cartId);
            if (cart.Count == 1)
            {
                this.db.ShoppingCart.Remove(cart);
                await this.db.SaveChangesAsync();

                var count = this.db.ShoppingCart.Where(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count();

                HttpContext.Session.SetInt32(StaticDetail.ssShoppingCartCount,count);
            }
            else
            {
                cart.Count--;
                await this.db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int cartId)
        {
            var cart = await this.db.ShoppingCart.FirstOrDefaultAsync(c => c.Id == cartId);

            this.db.ShoppingCart.Remove(cart);
            await this.db.SaveChangesAsync();

            var ctn = this.db.ShoppingCart.Where(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count();

            HttpContext.Session.SetInt32(StaticDetail.ssShoppingCartCount, ctn);

            return RedirectToAction(nameof(Index));
        }
    }
}