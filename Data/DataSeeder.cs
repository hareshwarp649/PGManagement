using bca.api.Enums.Permissions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PropertyManage.Data.Entities;
using PropertyManage.Domain.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace PropertyManage.Data
{
    public static class DataSeeder
    {
        public static async Task SeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Ensure the database is migrated before seeding
                await context.Database.MigrateAsync();

               // await SeedBanks(context);
                await SeedRoles(context);
                await SeedPermissions(context);
                await SeedRolePermissions(context);

                // Super Admin Seeder (inject dependencies from scope)
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleService = scope.ServiceProvider.GetRequiredService<IRoleService>();
                var userRoleRepository = scope.ServiceProvider.GetRequiredService<IUserRoleRepository>();

                var superAdminSeeder = new SuperAdminSeeder(userManager, roleService, userRoleRepository, context);
                await superAdminSeeder.SeedSuperAdminAsync();
            }
        }

        private static async Task SeedPermissions(ApplicationDbContext context)
        {
            if (context.Permissions != null)
            {
                var existingNames = await context.Set<Permission>()
                    .Select(p => p.Name)
                    .ToListAsync();

                var enums = new[]
                {
                        typeof(OnboardingPermissions),
                        typeof(UserPermissions),
                        typeof(MasterDataPermissions),
                    };

                foreach (var enumType in enums)
                {
                    var category = enumType.Name.Replace("Permissions", "");

                    var values = Enum.GetValues(enumType).Cast<Enum>().Select(e =>
                    {
                        var member = enumType.GetMember(e.ToString()).First();
                        var display = member.GetCustomAttribute<DisplayAttribute>();

                        return new Permission
                        {
                            Name = e.ToString(),
                            Description = display?.Name ?? e.ToString(),
                            Category = category
                        };
                    });

                    foreach (var permission in values)
                    {
                        if (!existingNames.Contains(permission.Name))
                        {
                            context.Set<Permission>().Add(permission);
                        }
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedRoles(ApplicationDbContext context)
        {
            var enumRoles = Enum.GetValues(typeof(UserRoleEnum))
                        .Cast<UserRoleEnum>()
                        .Select(role => new Role
                        {
                            Name = role.ToString(), // e.g., "JR_AREA_MANAGER"
                            Description = GetEnumDescription(role) // e.g., "Junior Area Manager"
                        })
                        .ToList();

            var rolesToAdd = enumRoles
                .Where(er => !context.Roles.Any(r => r.Name == er.Name))
                .ToList();

            if (rolesToAdd.Count > 0)
            {
                await context.Roles.AddRangeAsync(rolesToAdd);
                await context.SaveChangesAsync();
            }
        }

        private static string GetEnumDescription(Enum value)
        {
            return value.GetType()
                        .GetField(value.ToString())?
                        .GetCustomAttribute<DescriptionAttribute>()?
                        .Description ?? value.ToString();
        }

        private static async Task SeedRolePermissions(ApplicationDbContext context)
        {
            if (!context.RolePermissions.Any())
            {
                var existingRolePermissions = await context.Set<RolePermission>()
                .Select(rp => new { rp.RoleId, rp.PermissionId })
                .ToListAsync();

                var allRoles = await context.Roles.ToListAsync();
                var allPermissions = await context.Permissions.ToListAsync();
                var rolePermissionMap = new Dictionary<string, string[]>
                {
                    [UserRoleEnum.SUPER_ADMIN.ToString()] = new[]
                    {
                        UserPermissions.CanManageAdmin.ToString(),
                        UserPermissions.CanManageSPOC.ToString(),
                        UserPermissions.CanManageEmployee.ToString(),
                        UserPermissions.CanViewExceptionLogs.ToString(),
                        MasterDataPermissions.CanManageMasterData.ToString(),
                        OnboardingPermissions.CanManageLocation.ToString(),
                        OnboardingPermissions.CanViewOnboardingRequest.ToString(),
                    },
                    [UserRoleEnum.ADMIN.ToString()] = new[]
                    {
                        UserPermissions.CanManageEmployee.ToString(),
                        UserPermissions.CanViewExceptionLogs.ToString(),
                        MasterDataPermissions.CanManageMasterData.ToString(),
                        OnboardingPermissions.CanManageLocation.ToString(),
                        OnboardingPermissions.CanViewOnboardingRequest.ToString(),
                    },
                    [UserRoleEnum.STATE_HEAD.ToString()] = new[]
                    {
                        OnboardingPermissions.CanManageLocation.ToString(),
                        OnboardingPermissions.CanViewOnboardingRequest.ToString()
                    },
                    [UserRoleEnum.SPOC_IN_HOUSE.ToString()] = new[]
                    {
                        OnboardingPermissions.CanManageLocation.ToString(),
                        OnboardingPermissions.CanViewOnboardingRequest.ToString(),
                        OnboardingPermissions.CanDeleteOnboardingRequest.ToString(),
                        OnboardingPermissions.CanGenrateOdLetter.ToString(),
                        OnboardingPermissions.CanAssignOdAccount.ToString(),
                        OnboardingPermissions.CanAssignKoCode.ToString()
                    },
                    [UserRoleEnum.JR_TERRITORY_MANAGER.ToString()] = new[]
                    {
                        OnboardingPermissions.CanManageLocation.ToString(),
                        OnboardingPermissions.CanViewOnboardingRequest.ToString()
                    },
                    [UserRoleEnum.TERRITORY_MANAGER.ToString()] = new[]
                    {
                        OnboardingPermissions.CanManageLocation.ToString(),
                        OnboardingPermissions.CanViewOnboardingRequest.ToString()
                    },
                    [UserRoleEnum.SR_TERRITORY_MANAGER.ToString()] = new[]
                    {
                        OnboardingPermissions.CanManageLocation.ToString(),
                        OnboardingPermissions.CanViewOnboardingRequest.ToString()
                    },
                    [UserRoleEnum.JR_REGIONAL_MANAGER.ToString()] = new[]
                    {
                        OnboardingPermissions.CanManageLocation.ToString(),
                        OnboardingPermissions.CanViewOnboardingRequest.ToString()
                    },
                    [UserRoleEnum.REGIONAL_MANAGER.ToString()] = new[]
                    {
                        OnboardingPermissions.CanManageLocation.ToString(),
                        OnboardingPermissions.CanViewOnboardingRequest.ToString()
                    },
                    [UserRoleEnum.SR_REGIONAL_MANAGER.ToString()] = new[]
                    {
                        OnboardingPermissions.CanManageLocation.ToString(),
                        OnboardingPermissions.CanViewOnboardingRequest.ToString()
                    },
                    [UserRoleEnum.JR_AREA_MANAGER.ToString()] = new[]
                    {
                        OnboardingPermissions.CanManageLocation.ToString(),
                        OnboardingPermissions.CanViewOnboardingRequest.ToString()
                    },
                    [UserRoleEnum.AREA_MANAGER.ToString()] = new[]
                    {
                        OnboardingPermissions.CanManageLocation.ToString(),
                        OnboardingPermissions.CanViewOnboardingRequest.ToString()
                    },
                    [UserRoleEnum.SR_AREA_MANAGER.ToString()] = new[]
                    {
                        OnboardingPermissions.CanManageLocation.ToString(),
                        OnboardingPermissions.CanViewOnboardingRequest.ToString()
                    },
                    // add more roles as needed
                };

                var rolePermissionsToAdd = new List<RolePermission>();

                foreach (var (roleName, permissionNames) in rolePermissionMap)
                {
                    var role = allRoles.FirstOrDefault(r => r.Name == roleName);
                    if (role == null) continue;

                    foreach (var permName in permissionNames)
                    {
                        var permission = allPermissions.FirstOrDefault(p => p.Name == permName);
                        if (permission == null) continue;

                        if (!existingRolePermissions.Any(rp => rp.RoleId == role.Id && rp.PermissionId == permission.Id))
                        {
                            rolePermissionsToAdd.Add(new RolePermission
                            {
                                RoleId = role.Id,
                                PermissionId = permission.Id
                            });
                        }
                    }
                }

                if (rolePermissionsToAdd.Any())
                {
                    context.Set<RolePermission>().AddRange(rolePermissionsToAdd);
                    await context.SaveChangesAsync();
                }
            }
        }

        //public static async Task SeedBanks(ApplicationDbContext context)
        //{
        //    if (!context.Banks.Any())
        //    {
        //        var existingBankNames = context.Banks.Select(b => b.Name).ToHashSet();

        //        var newBanks = Enum.GetValues(typeof(BankName))
        //            .Cast<BankName>()
        //            .Where(e => !existingBankNames.Contains(GetEnumDescription(e)))
        //            .Select(e => new Bank
        //            {
        //                Name = GetEnumDescription(e),
        //                ODCode = e.ToString(),
        //                ShortName = e.ToString()
        //            })
        //            .ToList();

        //        if (newBanks.Any())
        //        {
        //            await context.Banks.AddRangeAsync(newBanks);
        //            await context.SaveChangesAsync();
        //        }
        //    }
        //}

        //private static string GetEnumDescription(Enum value)
        //{
        //    return value.GetType()
        //                .GetField(value.ToString())?
        //                .GetCustomAttribute<DescriptionAttribute>()?
        //                .Description ?? value.ToString();
        //}




    }
}
