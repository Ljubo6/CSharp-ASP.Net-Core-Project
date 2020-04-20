using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniverseRestaurant.Data;
using UniverseRestaurant.Models;
using UniverseRestaurant.Models.ViewModels;
using UniverseRestaurant.Utility;

namespace UniverseRestaurant.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuItemsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment hostEnvironment;

        [BindProperty]
        public MenuItemViewModel MenuItemVM { get; set; }

        public MenuItemsController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            this.db = db;
            this.hostEnvironment = hostEnvironment;
            this.MenuItemVM = new MenuItemViewModel()
            {
                Category = this.db.Categories,
                MenuItem = new Models.MenuItem()
            };
        }
        public async Task<IActionResult> Index()
        {
            var menuItems = await this.db.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).ToListAsync();

            return View(menuItems);
        }

        //GET - CREATE
        public IActionResult Create()
        {
            return this.View(MenuItemVM);
        }


        //POST - CreatePOST
        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            MenuItemVM.MenuItem.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if (!ModelState.IsValid)
            {
                return View(MenuItemVM);
            }

            this.db.Add(MenuItemVM.MenuItem);
            await this.db.SaveChangesAsync();


            string webRootPath = this.hostEnvironment.WebRootPath;

            var files = HttpContext.Request.Form.Files;

            var menuItemFromDB = await this.db.MenuItems.FindAsync(MenuItemVM.MenuItem.Id);

            if (files.Count > 0)
            {
                var uploads = Path.Combine(webRootPath,"images");

                var extension = Path.GetExtension(files[0].FileName);

                using(var filesStream = new FileStream(Path.Combine(uploads,MenuItemVM.MenuItem.Id + extension),FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }

                menuItemFromDB.Image = @"\images\" + MenuItemVM.MenuItem.Id + extension;
            }
            else
            {
                var uploads = Path.Combine(webRootPath,@"images\" + StaticDetail.DefaultFoodImage);
                System.IO.File.Copy(uploads, webRootPath + @"\images\" + MenuItemVM.MenuItem.Id + ".png");
                menuItemFromDB.Image = @"\images\" + MenuItemVM.MenuItem.Id + ".png";
            }

            await this.db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //GET - EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MenuItemVM.MenuItem = await this.db.MenuItems.Include(m => m.Category).Include(m => m.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);

            MenuItemVM.SubCategory = await this.db.SubCategories.Where(s => s.CategoryId == MenuItemVM.MenuItem.CategoryId).ToListAsync();

            if (MenuItemVM.MenuItem == null)
            {
                return NotFound();
            }
            return this.View(MenuItemVM);
        }


        //POST - EditPOST
        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPOST(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MenuItemVM.MenuItem.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if (!ModelState.IsValid)
            {
                MenuItemVM.SubCategory = await this.db.SubCategories
                    .Where(s => s.CategoryId == MenuItemVM.MenuItem.CategoryId)
                    .ToListAsync();
                return View(MenuItemVM);
            }

            string webRootPath = this.hostEnvironment.WebRootPath;

            var files = HttpContext.Request.Form.Files;

            var menuItemFromDB = await this.db.MenuItems.FindAsync(MenuItemVM.MenuItem.Id);

            if (files.Count > 0)
            {
                //New Image has been uploaded

                var uploads = Path.Combine(webRootPath, "images");

                var extension_new = Path.GetExtension(files[0].FileName);

                //Delete the original file
                var imagePath = Path.Combine(webRootPath,menuItemFromDB.Image.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                //we will upload the new file
                using (var filesStream = new FileStream(Path.Combine(uploads, MenuItemVM.MenuItem.Id + extension_new), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }

                menuItemFromDB.Image = @"\images\" + MenuItemVM.MenuItem.Id + extension_new;
            }

            menuItemFromDB.Name = MenuItemVM.MenuItem.Name;
            menuItemFromDB.Description = MenuItemVM.MenuItem.Description;
            menuItemFromDB.Price = MenuItemVM.MenuItem.Price;
            menuItemFromDB.Spicyness = MenuItemVM.MenuItem.Spicyness;
            menuItemFromDB.CategoryId = MenuItemVM.MenuItem.CategoryId;
            menuItemFromDB.SubCategoryId = MenuItemVM.MenuItem.SubCategoryId;

            await this.db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        

        //GET : Details MenuItems
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MenuItemVM.MenuItem = await this.db.MenuItems
                .Include(m => m.Category)
                .Include(m => m.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (MenuItemVM.MenuItem == null)
            {
                return NotFound();
            }

            return this.View(MenuItemVM);
        }

        //GET : Delete MenuItem
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MenuItemVM.MenuItem = await this.db.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);

            if (MenuItemVM.MenuItem == null)
            {
                return NotFound();
            }

            return View(MenuItemVM);
        }
        //POST Delete MenuItem
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string webRootPath = this.hostEnvironment.WebRootPath;
            MenuItem menuItem = await this.db.MenuItems.FindAsync(id);

            if (menuItem != null)
            {
                var imagePath = Path.Combine(webRootPath,menuItem.Image.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                this.db.MenuItems.Remove(menuItem);
                await this.db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}