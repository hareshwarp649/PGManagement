using bca.api.Enums.Permissions;
using bca.api.Infrastructure.IRepository;
using bca.api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PropertyManage.Data.Entities;
using PropertyManage.Domain.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace PropertyManage.Data
{
    public static class DataSeeder
    {
        public static async Task SeedData(IServiceProvider sp)
        {
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            // 1. Seed Permissions
            var permNames = new[]
            {
             "Apartment.Create",
             "Apartment.View",
             "Tenant.View",
             "Payment.Record",
             "Roles.Manage",
             "Permissions.Manage"
            };

            foreach (var p in permNames)
            {
                if (!await db.Permissions.AnyAsync(x => x.Name == p))
                    db.Permissions.Add(new Permission { Name = p, Description = p });
            }
            await db.SaveChangesAsync();

            // 🔹 Seed Roles
            var roles = new[] { "SuperAdmin", "Admin" };
            foreach (var roleName in roles)
            {
                if (await roleMgr.FindByNameAsync(roleName) == null)
                {
                    await roleMgr.CreateAsync(new ApplicationRole
                    {
                        Name = roleName,
                        NormalizedName = roleName.ToUpper()
                    });
                }
            }

            // 🔹 Assign All Permissions to SuperAdmin
            var allPerms = await db.Permissions.ToListAsync();
            var superAdminRole = await roleMgr.FindByNameAsync("SuperAdmin");
            foreach (var perm in allPerms)
            {
                if (!await db.RolePermissions.AnyAsync(rp => rp.RoleId == superAdminRole.Id && rp.PermissionId == perm.Id))
                    db.RolePermissions.Add(new RolePermission { RoleId = superAdminRole.Id, PermissionId = perm.Id });
            }

            // Assign selected permissions to Admin
            var adminRole = await roleMgr.FindByNameAsync("Admin");
            foreach (var perm in allPerms)
            {
                if (!await db.RolePermissions.AnyAsync(rp => rp.RoleId == adminRole.Id && rp.PermissionId == perm.Id))
                    db.RolePermissions.Add(new RolePermission { RoleId = adminRole.Id, PermissionId = perm.Id });
            }
            await db.SaveChangesAsync();


            // 🔹 Read credentials from appsettings
            var superAdminSection = config.GetSection("SeedData:SuperAdmin");
            var adminSection = config.GetSection("SeedData:Admin");

            // 🔹 Create SuperAdmin user
            var superAdminEmail = superAdminSection["Email"];
            var superAdminPassword = superAdminSection["Password"];
            var superAdminFullName = superAdminSection["FullName"];

            var superAdminUser = await userMgr.FindByEmailAsync(superAdminEmail);
            if (superAdminUser == null)
            {
                superAdminUser = new ApplicationUser
                {
                    Email = superAdminEmail,
                    UserName = superAdminEmail,
                    FullName = superAdminFullName
                };

                var result = await userMgr.CreateAsync(superAdminUser, superAdminPassword);
                if (result.Succeeded)
                    await userMgr.AddToRoleAsync(superAdminUser, "SuperAdmin");
            }

            // 🔹 Create Admin user
            var adminEmail = adminSection["Email"];
            var adminPassword = adminSection["Password"];
            var adminFullName = adminSection["FullName"];

            var adminUser = await userMgr.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    FullName = adminFullName
                };

                var result = await userMgr.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                    await userMgr.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
