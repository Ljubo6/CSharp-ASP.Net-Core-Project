using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniverseRestaurant.Data;
using UniverseRestaurant.Models;

namespace UniverseRestaurant.SeedData
{
    public class SubCategorySeeder
    {
        private readonly ApplicationDbContext dbContext;

        public SubCategorySeeder(IServiceProvider serviceProvider, ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task SeedDataAsync()
        {
            await SeedFruitAsync();
            await SeedVegetableAsync();
            await SeedMeatAsync();

            await SeedSpicyAsync();
            await SeedNonSpicyAsync();

            await SeedFastMealAsync();
            await SeedOrdinaryMealAsync();

            await SeedFizzyDrinksAsync();
            await SeedJuiceDrinksAsync();
            await SeedWaterAsync();

            await SeedSoltyDessertAsync();
            await SeedSweetDessertAsync();


        }

        private async Task SeedSweetDessertAsync()
        {
            var subCategory = this.dbContext.SubCategories.FirstOrDefault(x => x.Name == "Sweet Dessert");
            if (subCategory != null)
            {
                return;
            }
            await dbContext.SubCategories.AddAsync(new SubCategory
            {
                Name = "Sweet Dessert",
                CategoryId = 5,

            });
            await this.dbContext.SaveChangesAsync();
        }

        private async Task SeedSoltyDessertAsync()
        {
            var subCategory = this.dbContext.SubCategories.FirstOrDefault(x => x.Name == "Solty Dessert");
            if (subCategory != null)
            {
                return;
            }
            await dbContext.SubCategories.AddAsync(new SubCategory
            {
                Name = "Solty Dessert",
                CategoryId = 5,

            });
            await this.dbContext.SaveChangesAsync();
        }

        private async Task SeedWaterAsync()
        {
            var subCategory = this.dbContext.SubCategories.FirstOrDefault(x => x.Name == "Water");
            if (subCategory != null)
            {
                return;
            }
            await dbContext.SubCategories.AddAsync(new SubCategory
            {
                Name = "Water",
                CategoryId = 4,

            });
            await this.dbContext.SaveChangesAsync();
        }

        private async  Task SeedJuiceDrinksAsync()
        {
            var subCategory = this.dbContext.SubCategories.FirstOrDefault(x => x.Name == "Juice Drink");
            if (subCategory != null)
            {
                return;
            }
            await dbContext.SubCategories.AddAsync(new SubCategory
            {
                Name = "Juice Drink",
                CategoryId = 4,

            });
            await this.dbContext.SaveChangesAsync();
        }

        private async Task SeedFizzyDrinksAsync()
        {
            var subCategory = this.dbContext.SubCategories.FirstOrDefault(x => x.Name == "Fizzy Drink");
            if (subCategory != null)
            {
                return;
            }
            await dbContext.SubCategories.AddAsync(new SubCategory
            {
                Name = "Fizzy Drink",
                CategoryId = 4,

            });
            await this.dbContext.SaveChangesAsync();
        }

        private async Task SeedOrdinaryMealAsync()
        {
            var subCategory = this.dbContext.SubCategories.FirstOrDefault(x => x.Name == "Ordinary Meal");
            if (subCategory != null)
            {
                return;
            }
            await dbContext.SubCategories.AddAsync(new SubCategory
            {
                Name = "Ordinary Meal",
                CategoryId = 3,

            });
            await this.dbContext.SaveChangesAsync();
        }

        private async Task SeedFastMealAsync()
        {
            var subCategory = this.dbContext.SubCategories.FirstOrDefault(x => x.Name == "Fast Meal");
            if (subCategory != null)
            {
                return;
            }
            await dbContext.SubCategories.AddAsync(new SubCategory
            {
                Name = "Fast Meal",
                CategoryId = 3,

            });
            await this.dbContext.SaveChangesAsync();
        }

        private async  Task SeedNonSpicyAsync()
        {
            var subCategory = this.dbContext.SubCategories.FirstOrDefault(x => x.Name == "Non Spicy");
            if (subCategory != null)
            {
                return;
            }
            await dbContext.SubCategories.AddAsync(new SubCategory
            {
                Name = "Non Spicy",
                CategoryId = 2,

            });
            await this.dbContext.SaveChangesAsync();
        }

        private async Task SeedSpicyAsync()
        {
            var subCategory = this.dbContext.SubCategories.FirstOrDefault(x => x.Name == "Spicy");
            if (subCategory != null)
            {
                return;
            }
            await dbContext.SubCategories.AddAsync(new SubCategory
            {
                Name = "Spicy",
                CategoryId = 2,

            });
            await this.dbContext.SaveChangesAsync();
        }

        private async Task SeedMeatAsync()
        {
            var subCategory = this.dbContext.SubCategories.FirstOrDefault(x => x.Name == "Meat");
            if (subCategory != null)
            {
                return;
            }
            await dbContext.SubCategories.AddAsync(new SubCategory
            {
                Name = "Meat",
                CategoryId = 1,

            });
            await this.dbContext.SaveChangesAsync();
        }

        private async Task SeedVegetableAsync()
        {
            var subCategory = this.dbContext.SubCategories.FirstOrDefault(x => x.Name == "Vegetable");
            if (subCategory != null)
            {
                return;
            }
            await dbContext.SubCategories.AddAsync(new SubCategory
            {
                Name = "Vegetable",
                CategoryId = 1,

            });
            await this.dbContext.SaveChangesAsync();
        }

        private async Task SeedFruitAsync()
        {
            var subCategory = this.dbContext.SubCategories.FirstOrDefault(x => x.Name == "Fruit");
            if (subCategory != null)
            {
                return;
            }
            await dbContext.SubCategories.AddAsync(new SubCategory
            {
                Name = "Fruit",
                CategoryId = 1,

            });
            await this.dbContext.SaveChangesAsync();
        }

    }
}
