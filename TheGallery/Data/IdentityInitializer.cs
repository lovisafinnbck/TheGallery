﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheGallery.Data
{
    public static class IdentityInitializer
    {
        // add admin account if it doesn't exist
        public static void SeedAdminUser(UserManager<IdentityUser> userManager)
        {
            if (userManager.FindByEmailAsync("admin@admin.se").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "admin@admin.se",
                    Email = "admin@admin.se",
                    EmailConfirmed = true
                };

                IdentityResult result = userManager.CreateAsync(user, "!Admin123").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
        // Add needed roles here
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admins").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }
    }
}
