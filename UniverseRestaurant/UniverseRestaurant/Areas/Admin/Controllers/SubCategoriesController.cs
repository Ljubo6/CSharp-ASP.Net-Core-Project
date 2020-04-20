using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniverseRestaurant.Data;
using UniverseRestaurant.Models;
using UniverseRestaurant.Models.ViewModels;

namespace UniverseRestaurant.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoriesController : Controller
    {
        private readonly ApplicationDbContext db;

        [TempData]
        public string StatusMessage { get; set; }

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

        //GET - CREATE
        public async Task<IActionResult> Create()
        {
            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await this.db.Categories.ToListAsync(),
                SubCategory = new Models.SubCategory(),
                SubCategoryList = await this.db.SubCategories
                                            .OrderBy(p => p.Name)
                                            .Select(p => p.Name)
                                            .Distinct()
                                            .ToListAsync()
            };

            return View(model);
        }

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategoryAndCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesSubCategoryExist = this.db.SubCategories
                    .Include(s => s.Category)
                    .Where(s => s.Name == model.SubCategory.Name && s.Category.Id == model.SubCategory.CategoryId);

                if (doesSubCategoryExist.Count() > 0)
                {
                    //Error
                    StatusMessage = "Error : Sub Category exist under "
                        + doesSubCategoryExist.First().Category.Name
                        + " category.Please use another name.";
                }
                else
                {
                    this.db.SubCategories.Add(model.SubCategory);

                    await this.db.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }

            SubCategoryAndCategoryViewModel modelVM = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await this.db.Categories.ToListAsync(),
                SubCategory = model.SubCategory,
                SubCategoryList = await this.db.SubCategories
                .OrderBy(p => p.Name)
                .Select(p => p.Name)
                .ToListAsync(),
                StatusMessage = StatusMessage,
            };

            return View(modelVM);
        }

        [ActionName("GetSubCategory")]
        public async Task<IActionResult> GetSubCategory(int id)
        {
            List<SubCategory> subCategories = new List<SubCategory>();

            subCategories = await(from subCategory in this.db.SubCategories
                             where subCategory.CategoryId == id
                             select subCategory).ToListAsync();
            return Json(new SelectList(subCategories, "Id", "Name"));
        }

        //GET - EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subcategory = await this.db.SubCategories.SingleOrDefaultAsync(m => m.Id == id);

            if (subcategory == null)
            {
                return NotFound();
            }

            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await this.db.Categories.ToListAsync(),
                SubCategory = subcategory,
                SubCategoryList = await this.db.SubCategories
                                            .OrderBy(p => p.Name)
                                            .Select(p => p.Name)
                                            .Distinct()
                                            .ToListAsync()
            };

            return View(model);
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,SubCategoryAndCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesSubCategoryExist = this.db.SubCategories
                    .Include(s => s.Category)
                    .Where(s => s.Name == model.SubCategory.Name && s.Category.Id == model.SubCategory.CategoryId);

                if (doesSubCategoryExist.Count() > 0)
                {
                    //Error
                    StatusMessage = "Error : Sub Category exist under "
                        + doesSubCategoryExist.First().Category.Name
                        + " category.Please use another name.";
                }
                else
                {
                    var subCatFromDb = await this.db.SubCategories.FindAsync(id);

                    subCatFromDb.Name = model.SubCategory.Name;

                    this.db.SubCategories.Add(model.SubCategory);

                    await this.db.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }

            SubCategoryAndCategoryViewModel modelVM = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await this.db.Categories.ToListAsync(),
                SubCategory = model.SubCategory,
                SubCategoryList = await this.db.SubCategories
                .OrderBy(p => p.Name)
                .Select(p => p.Name)
                .ToListAsync(),
                StatusMessage = StatusMessage,
            };

            modelVM.SubCategory.Id = id;

            return View(modelVM);
        }
    }
}