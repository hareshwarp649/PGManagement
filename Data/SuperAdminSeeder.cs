using bca.api.Infrastructure.IRepository;
using bca.api.Services;
using EnumsNET;
using Microsoft.AspNetCore.Identity;
using PropertyManage.Data.Entities;
using PropertyManage.Domain.Enums;

namespace PropertyManage.Data
{
    public class SuperAdminSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRoleService _roleService;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly ApplicationDbContext _context;

        public SuperAdminSeeder(
            UserManager<ApplicationUser> userManager,
            IRoleService roleService,
            IUserRoleRepository userRoleRepository,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleService = roleService;
            _userRoleRepository = userRoleRepository;
            _context = context;
        }

        public async Task SeedSuperAdminAsync()
        {
            const string roleName = "SUPER_ADMIN";
            const string username = "Asuper-admin";
            const string password = "RRpassword@1234"; // Replace with secure password

            // 1. Ensure role exists
            var role = await _roleService.GetRoleByNameAsync(roleName);
            if (role == null)
            {
                if (role == null)
                    throw new Exception($"Role Not found : {roleName}");
            }

            // 2. Check if super-admin user exists
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                // Ensure role assigned
                var userRoles = await _userRoleRepository.GetRolesByUserIdAsync(user.Id); // Optional method
                if (!userRoles.Any(x => x.RoleId == role.Id))
                {
                    await _userRoleRepository.AddAsync(new UserRole { UserId = user.Id, RoleId = role.Id });
                }
                return; // Already seeded
            }

            // 3. Create user and assign role inside transaction
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    user = new ApplicationUser
                    {
                        UserName = username,
                        Email = "superadmin@example.com", // Optional
                        UserType = UserType.SuperAdmin,
                        EmailConfirmed = true
                    };

                    var createResult = await _userManager.CreateAsync(user, password);
                    if (!createResult.Succeeded)
                        throw new Exception("User creation failed: " + string.Join(", ", createResult.Errors.Select(e => e.Description)));

                    //await _userManager.AddToRolesAsync(user, new[] { "SUPER_ADMIN", "Admin","User" });

                    await _userRoleRepository.AddAsync(new UserRole
                    {
                        UserId = user.Id,
                        RoleId = role.Id
                    });

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
