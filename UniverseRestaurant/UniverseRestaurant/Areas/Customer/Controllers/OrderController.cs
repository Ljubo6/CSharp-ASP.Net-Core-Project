using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniverseRestaurant.Data;
using UniverseRestaurant.Models;
using UniverseRestaurant.Models.ViewModels;

namespace UniverseRestaurant.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext db;

        public OrderController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [Authorize]
        public async Task<IActionResult> Confirm(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            OrderDetailsViewModel orderDetailsViewModel = new OrderDetailsViewModel()
            {
                OrderHeader = await this.db.OrderHeader.Include(o => o.ApplicationUser).FirstOrDefaultAsync(o => o.Id == id && o.UserId == claim.Value),
                OrderDetails = await this.db.OrderDetails.Where(o => o.OrderId == id).ToListAsync(),
            };

            return this.View(orderDetailsViewModel);
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> OrderHistory(int cartId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<OrderDetailsViewModel> orderList = new List<OrderDetailsViewModel>();

            List<OrderHeader> orderHeaderList = await this.db.OrderHeader.Include(o => o.ApplicationUser).Where(u => u.UserId == claim.Value).ToListAsync();

            foreach (OrderHeader item in orderHeaderList)
            {
                OrderDetailsViewModel individual = new OrderDetailsViewModel
                {
                    OrderHeader = item,
                    OrderDetails = await this.db.OrderDetails.Where(o => o.OrderId == item.Id).ToListAsync(),
                };

                orderList.Add(individual);
            }
            return this.View(orderList);
        }
    }
}