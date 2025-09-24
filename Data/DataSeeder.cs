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
            var roles = new[] { "SuperAdmin", "Admin", "Client_Owner" };
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

            // ✅ Null checks
            if (string.IsNullOrWhiteSpace(superAdminSection["Email"]) || string.IsNullOrWhiteSpace(superAdminSection["Password"]))
                throw new Exception("SuperAdmin credentials missing in appsettings.json");

            if (string.IsNullOrWhiteSpace(adminSection["Email"]) || string.IsNullOrWhiteSpace(adminSection["Password"]))
                throw new Exception("Admin credentials missing in appsettings.json");


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
                    FullName = superAdminFullName,
                    EmailConfirmed=true
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
                    FullName = adminFullName,
                    EmailConfirmed=true
                };

                var result = await userMgr.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                    await userMgr.AddToRoleAsync(adminUser, "Admin");
            }

            //// 5️⃣ Create or Update SuperAdmin
            //await CreateOrUpdateUserAsync(userMgr, superAdminSection["Email"], superAdminSection["Password"], superAdminSection["FullName"], superAdminRole);

            //// 6️⃣ Create or Update Admin
            //await CreateOrUpdateUserAsync(userMgr, adminSection["Email"], adminSection["Password"], adminSection["FullName"], adminRole);
        }

        //private static async Task CreateOrUpdateUserAsync(UserManager<ApplicationUser> userMgr, string email, string password, string fullName, ApplicationRole role)
        //{
        //    var user = await userMgr.FindByEmailAsync(email);
        //    if (user == null)
        //    {
        //        user = new ApplicationUser
        //        {
        //            UserName = email,
        //            Email = email,
        //            FullName = fullName,
        //            EmailConfirmed = true
        //        };

        //        var result = await userMgr.CreateAsync(user, password);
        //        if (!result.Succeeded)
        //            throw new Exception($"Failed to create user {email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        //    }
        //    else
        //    {
        //        // Reset password if user already exists
        //        var token = await userMgr.GeneratePasswordResetTokenAsync(user);
        //        var result = await userMgr.ResetPasswordAsync(user, token, password);
        //        if (!result.Succeeded)
        //            throw new Exception($"Failed to reset password for {email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        //    }

        //    // Ensure user has role
        //    if (!await userMgr.IsInRoleAsync(user, role.Name))
        //    {
        //        var result = await userMgr.AddToRoleAsync(user, role.Name);
        //        if (!result.Succeeded)
        //            throw new Exception($"Failed to add role {role.Name} to user {email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        //    }
        //}
        
    }
    
}
