using AutoMapper;
using bca.api.DTOs;
using Microsoft.AspNetCore.Identity;
using bca.api.Models;
using bca.api.Infrastructure.IRepository;
using PropertyManage.Data.Entities;
using PropertyManage.Data;
using PropertyManage.Domain.Enums;

namespace bca.api.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IRoleService _roleService;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IMapper _mapper;

        public UserService(IMapper mapper, IUserRoleRepository userRoleRepository, UserManager<ApplicationUser> userManager, ApplicationDbContext context, IRoleService roleService)
        {
            _mapper = mapper;
            _userRoleRepository = userRoleRepository;
            _userManager = userManager;
            _context = context;
            _roleService = roleService;
        }

        public async Task<IEnumerable<UserDTO>> GetUsersByRoleAsync(string roleName)
        {
            var userRoles = await _userRoleRepository.GetUsersByRoleNameAsync(roleName);
            var users = userRoles.Select(ur => ur.User);
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<IdentityResult> RegisterSPOC(RegisterSPOCModel model)
        {
            var role = await _roleService.GetRoleByNameAsync("SPOC_IN_HOUSE");
            if (role == null)
                throw new Exception("Role Not found : SPOC_IN_HOUSE");

            // Start a transaction (if using EF Core)
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if(_context.ApplicationUsers.Any(x => x.UserType == UserType.Admin))
                        throw new Exception("SPOC already exists for this Bank.");

                    var user = new ApplicationUser
                    {
                        UserName = model.Username,
                       // BankId = model.BankId,
                        UserType = UserType.Admin
                    };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (!result.Succeeded)
                        return result;

                    // Assign Role
                    await _userRoleRepository.AddAsync(new UserRole { UserId = user.Id, RoleId = role.Id });

                    // Commit transaction
                    await transaction.CommitAsync();
                    return result;
                }
                catch (Exception ex)
                {
                    // Rollback on failure
                    await transaction.RollbackAsync();
                    throw new Exception($"Error: {ex.Message}");
                }
            }
        }

        public async Task<IdentityResult> RegisterAdmin(RegisterAdminModel model)
        {
            var role = await _roleService.GetRoleByNameAsync("ADMIN");
            if (role == null)
                throw new Exception("Role Not found : ADMIN");

            // Start a transaction (if using EF Core)
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.Username,
                        UserType = UserType.Admin
                    };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (!result.Succeeded)
                        return result;

                    // Assign role using Identity
                    await _userManager.AddToRoleAsync(user, "ADMIN");

                    // Assign Role
                    await _userRoleRepository.AddAsync(new UserRole { UserId = user.Id, RoleId = role.Id });

                    // Commit transaction
                    await transaction.CommitAsync();
                    return result;
                }
                catch (Exception ex)
                {
                    // Rollback on failure
                    await transaction.RollbackAsync();
                    throw new Exception($"Error: {ex.Message}");
                }
            }
        }

        public async Task<IdentityResult> RegisterSuperAdmin(RegisterAdminModel model)
        {
            var role = await _roleService.GetRoleByNameAsync("SUPERADMIN");
            if (role == null)
                throw new Exception("Role Not found : SUPERADMIN");

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.Username,
                        UserType = UserType.SuperAdmin // नया Enum Type
                    };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (!result.Succeeded)
                        return result;

                    // Assign role using Identity
                    await _userManager.AddToRoleAsync(user, "SUPERADMIN");

                    await _userRoleRepository.AddAsync(new UserRole { UserId = user.Id, RoleId = role.Id });
                    await transaction.CommitAsync();
                    return result;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error: {ex.Message}");
                }
            }
        }
    }
}
