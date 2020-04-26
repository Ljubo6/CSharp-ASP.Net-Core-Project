using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniverseRestaurant.Data;
using UniverseRestaurant.Utility;

namespace UniverseRestaurant.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetail.ManagerUser)]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext db;

        public UsersController(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task< IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            return View(await this.db.ApplicationUser.Where(u => u.Id != claim.Value).ToListAsync());
        }

        public async Task<IActionResult> Lock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await this.db.ApplicationUser.FirstOrDefaultAsync(m => m.Id == id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            applicationUser.LockoutEnd = DateTime.Now.AddYears(1000);

            await this.db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UnLock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await this.db.ApplicationUser.FirstOrDefaultAsync(m => m.Id == id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            applicationUser.LockoutEnd = DateTime.Now;

            await this.db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //GET - DELETE
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await this.db.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return this.View(user);
        }

        //POST - DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await this.db.Users.FindAsync(id);

            if (user == null)
            {
                return this.View();
            }

            this.db.Users.Remove(user);

            await this.db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}