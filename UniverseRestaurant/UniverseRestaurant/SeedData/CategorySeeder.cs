using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniverseRestaurant.Data;
using UniverseRestaurant.Models;

namespace UniverseRestaurant.SeedData
{
    public class CategorySeeder
    {
        private readonly ApplicationDbContext dbContext;

        public CategorySeeder(IServiceProvider serviceProvider, ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task SeedDataAsync()
        {
            await SeedSaladsAsync();
            await SeedAppetizerAsync();
            await SeedMainCourseAsync();
            await SeedBeveragesAsync();
            await SeedDessertAsync();
        }

        private async Task SeedDessertAsync()
        {
            var category = this.dbContext.Categories.FirstOrDefault(x => x.Name == "Dessert");
            if (category != null)
            {
                return;
            }
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Dessert",
            });
            await this.dbContext.SaveChangesAsync();
        }

        private async Task SeedBeveragesAsync()
        {
            var category = this.dbContext.Categories.FirstOrDefault(x => x.Name == "Beverages");
            if (category != null)
            {
                return;
            }
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Beverages",
            });
            await this.dbContext.SaveChangesAsync();
        }

        private async Task SeedMainCourseAsync()
        {
            var category = this.dbContext.Categories.FirstOrDefault(x => x.Name == "Main Course");
            if (category != null)
            {
                return;
            }
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Main Course",
            });
            await this.dbContext.SaveChangesAsync();
        }

        private async Task SeedAppetizerAsync()
        {
            var category = this.dbContext.Categories.FirstOrDefault(x => x.Name == "Appetizer");
            if (category != null)
            {
                return;
            }
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Appetizer",
            });
            await this.dbContext.SaveChangesAsync();
        }

        private async Task SeedSaladsAsync()
        {
            var category = this.dbContext.Categories.FirstOrDefault(x => x.Name == "Salads");
            if (category != null)
            {
                return;
            }
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Salads",

            });
            await this.dbContext.SaveChangesAsync();
        }
    }
}
