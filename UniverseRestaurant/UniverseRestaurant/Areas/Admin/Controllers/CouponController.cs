using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniverseRestaurant.Data;
using UniverseRestaurant.Models;
using UniverseRestaurant.Utility;

namespace UniverseRestaurant.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetail.ManagerUser)]
    public class CouponController : Controller
    {
        private readonly ApplicationDbContext db;

        public CouponController(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            return this.View(await this.db.Coupons.ToListAsync());
        }

        //GET - CREATE
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Coupon coupons)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;

                if (files.Count > 0)
                {
                    byte[] picture1 = null;
                    using (var fileStream1 = files[0].OpenReadStream())
                    {
                        using (var memortStream1 = new MemoryStream())
                        {
                            fileStream1.CopyTo(memortStream1);
                            picture1 = memortStream1.ToArray();
                        }
                    }
                    coupons.Picture = picture1;
                }

                this.db.Coupons.Add(coupons);

                await this.db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return this.View(coupons);
        }

        //GET - EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coupon = await this.db.Coupons.SingleOrDefaultAsync(m => m.Id == id);

            if (coupon == null)
            {
                return NotFound();
            }

            return this.View(coupon);
        }

        //POST = EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Coupon coupons)
        {
            if (coupons.Id == 0)
            {
                return NotFound();
            }

            var couponFromDb = await this.db.Coupons.Where(c => c.Id == coupons.Id).FirstOrDefaultAsync();

            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;

                if (files.Count > 0)
                {
                    byte[] picture1 = null;
                    using (var fileStream1 = files[0].OpenReadStream())
                    {
                        using (var memoryStream1 = new MemoryStream())
                        {
                            fileStream1.CopyTo(memoryStream1);
                            picture1 = memoryStream1.ToArray();
                        }
                    }
                    couponFromDb.Picture = picture1;
                }

                couponFromDb.MinimumAmount = coupons.MinimumAmount;
                couponFromDb.Name = coupons.Name;
                couponFromDb.Discount = coupons.Discount;
                couponFromDb.CouponType = coupons.CouponType;
                couponFromDb.IsActive = coupons.IsActive;

                await this.db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return this.View();
        }

        //GET - DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coupon = await this.db.Coupons.Where(m => m.Id == id).FirstOrDefaultAsync();

            if (coupon == null)
            {
                return NotFound();
            }

            return View(coupon);
        }

        //GET - DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coupon = await this.db.Coupons.Where(m => m.Id == id).FirstOrDefaultAsync();

            if (coupon == null)
            {
                return NotFound();
            }

            return View(coupon);
        }

        //POST - DeleteConfirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coupons = await this.db.Coupons.SingleOrDefaultAsync(m => m.Id == id);

            this.db.Coupons.Remove(coupons);

            await this.db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}