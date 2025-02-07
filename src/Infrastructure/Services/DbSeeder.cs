using System;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class DbSeeder
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            // Seed Roles
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole<int>("Admin"));

            if (!await roleManager.RoleExistsAsync("SupportAgent"))
                await roleManager.CreateAsync(new IdentityRole<int>("SupportAgent"));

            if (!await roleManager.RoleExistsAsync("Customer"))
                await roleManager.CreateAsync(new IdentityRole<int>("Customer"));

            // Seed Admin User
            if (await userManager.FindByEmailAsync("admin@support.com") == null)
            {
                var admin = new User
                {
                    UserName = "admin@support.com",
                    Email = "admin@support.com",
                    FullName = "System Administrator"
                };

                var result = await userManager.CreateAsync(admin, "Admin@1234");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}