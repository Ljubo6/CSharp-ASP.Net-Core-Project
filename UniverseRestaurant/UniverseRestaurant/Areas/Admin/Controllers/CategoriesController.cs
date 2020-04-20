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
            return View();
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
            return View(category);
        }
    }
}