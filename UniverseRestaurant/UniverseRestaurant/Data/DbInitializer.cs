using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniverseRestaurant.Models;
using UniverseRestaurant.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniverseRestaurant.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }


        public async void Initialize()
        {
            if (this.db.Roles.Any(r => r.Name == StaticDetail.ManagerUser)) return;

            this.roleManager.CreateAsync(new IdentityRole(StaticDetail.ManagerUser)).GetAwaiter().GetResult();
            this.roleManager.CreateAsync(new IdentityRole(StaticDetail.FrontDeskUser)).GetAwaiter().GetResult();
            this.roleManager.CreateAsync(new IdentityRole(StaticDetail.KitchenUser)).GetAwaiter().GetResult();
            this.roleManager.CreateAsync(new IdentityRole(StaticDetail.CustomerEndUser)).GetAwaiter().GetResult();

            this.userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                Name = "Lyubomir Lyubomirov",
                EmailConfirmed = true,
                PhoneNumber = "0888261853"
            }, "Admin123*").GetAwaiter().GetResult();

            IdentityUser user = await this.db.Users.FirstOrDefaultAsync(u => u.Email == "admin@gmail.com");

            await this.userManager.AddToRoleAsync(user, StaticDetail.ManagerUser);

        }

    }
}
