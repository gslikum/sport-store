using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace SportsStore.Data
{
    public static class IdentitySeedData
    {
        private const string AdminUserEmail = "admin@sportsstore.com";
        private const string AdminRoleName = "Admins";

        public static async Task SeedAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            // 1. Create Admins role if it doesn't exist (CIS 502 RBAC)
            if (!await roleManager.RoleExistsAsync(AdminRoleName))
            {
                await roleManager.CreateAsync(new IdentityRole(AdminRoleName));
            }

            // 2. Create default Admin user if it doesn't exist
            var adminUser = await userManager.FindByEmailAsync(AdminUserEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = AdminUserEmail,
                    Email = AdminUserEmail,
                    EmailConfirmed = true
                };

                // Create user with default secure password
                var result = await userManager.CreateAsync(adminUser, "Password123!");
                if (result.Succeeded)
                {
                    // Associate user with Admins role (Authorization boundary)
                    await userManager.AddToRoleAsync(adminUser, AdminRoleName);
                }
            }
        }
    }
}
