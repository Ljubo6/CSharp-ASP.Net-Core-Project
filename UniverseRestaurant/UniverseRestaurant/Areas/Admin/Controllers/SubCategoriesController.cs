using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniverseRestaurant.Data;

namespace UniverseRestaurant.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoriesController : Controller
    {
        private readonly ApplicationDbContext db;

        public SubCategoriesController(ApplicationDbContext db)
        {
            this.db = db;
        }

        //GET INDEX
        public async Task<IActionResult> Index()
        {
            var subCategories = await this.db.SubCategories.Include(s => s.Category).ToListAsync();

            return this.View(subCategories);
        }
    }
}