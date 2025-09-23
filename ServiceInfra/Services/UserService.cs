using AutoMapper;
using bca.api.DTOs;
using Microsoft.AspNetCore.Identity;
using bca.api.Models;
using bca.api.Infrastructure.IRepository;
using PropertyManage.Data.Entities;
using PropertyManage.Data;
using PropertyManage.Domain.Enums;
using NPOI.SS.Formula.Functions;
using PropertyManage.Domain.DTOs;
using Microsoft.EntityFrameworkCore;

namespace bca.api.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IRoleService _roleService;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IMapper _mapper;

        public UserService(IMapper mapper, IUserRoleRepository userRoleRepository, UserManager<ApplicationUser> userManager, ApplicationDbContext context, RoleManager<ApplicationRole> roleManager,  IRoleService roleService)
        {
            _mapper = mapper;
            _userRoleRepository = userRoleRepository;
            _userManager = userManager;
            _context = context;
            _roleService = roleService;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<UserDTO>> GetUsersByRoleAsync(string roleName)
        {
            var userRoles = await _userRoleRepository.GetUsersByRoleNameAsync(roleName);
            var users = userRoles.Select(ur => ur.User);
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }
       
        public async Task<UserDTO> GetByIdAsync(Guid id)
        {
            var user = await _userManager.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) throw new KeyNotFoundException();
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            var users = await _userManager.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).ToListAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<UserDTO> CreateAsync(UserCreateDTO dto)
        {
            var user = new ApplicationUser 
            {
                Email = dto.Email, 
                UserName = dto.Email, 
                FullName = dto.FullName, 
                EntityId = dto.EntityId ,
                //ClientId = dto.ClientId,
            };
            var res = await _userManager.CreateAsync(user, dto.Password);

            if (!res.Succeeded) 
                throw new Exception(string.Join(",", res.Errors.Select(e => e.Description)));

            if (dto.Roles != null && dto.Roles.Any())
            {
                await _userManager.AddToRolesAsync(user, dto.Roles);
            }
            return _mapper.Map<UserDTO>(user);
        }

        public async Task SoftDeleteAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) throw new KeyNotFoundException();
            user.IsDeleted = true;
            await _userManager.UpdateAsync(user);
        }

        public async Task<UserDTO> RegisterUserAsync(RegisterUserModel model)
        {
            // 1️⃣ Role check
            var role = await _roleManager.FindByNameAsync(model.RoleName);
            if (role == null)
                throw new Exception($"Role not found: {model.RoleName}");

            if (model.ClientId.HasValue)
            {
                var clientExists = await _context.Clients.AnyAsync(c => c.Id == model.ClientId.Value);
                if (!clientExists)
                    throw new Exception($"Client not found with Id: {model.ClientId}");
            }

            // 2️⃣ Create User
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Username,
                UserType = model.UserType,
                ClientId= model.ClientId,
                FullName = model.FullName,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join(",", result.Errors.Select(e => e.Description)));


            // 3️⃣ Assign role via Identity
            await _userManager.AddToRoleAsync(user, model.RoleName);

            // 4️⃣ Assign role in custom UserRole table
            await _userRoleRepository.AddAsync(new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            });

            return new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                Roles = new List<string> { model.RoleName }
            };

        }
    }
}
