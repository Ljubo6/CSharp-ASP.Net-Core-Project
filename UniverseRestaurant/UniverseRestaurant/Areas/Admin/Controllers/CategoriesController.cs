using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniverseRestaurant.Data;
using UniverseRestaurant.Models;

namespace UniverseRestaurant.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext db;

        public CategoriesController(ApplicationDbContext db)
        {
            this.db = db;
        }

        //GET
        public async Task<IActionResult> Index()
        {
            return View(await this.db.Categories.ToListAsync());
        }

        // GET - CREATE
        public IActionResult Create()
        {
            return this.View();
        }

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                //if valid
                this.db.Add(category);

                await this.db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return this.View(category);
        }

        //GET - EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await this.db.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return this.View(category);
        }

        // POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                this.db.Update(category);

                await this.db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return this.View(category);
        }


        //GET - DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await this.db.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return this.View(category);
        }

        //POST - DELETE
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var category = await this.db.Categories.FindAsync(id);

            if (category == null)
            {
                return this.View();
            }

            this.db.Categories.Remove(category);

            await this.db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //GET - DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await this.db.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return this.View(category);
        }
    }
}