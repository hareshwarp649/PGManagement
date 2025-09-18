using AutoMapper;
using bca.api.DTOs;
using bca.api.Infrastructure.IRepository;
using Microsoft.EntityFrameworkCore;
using PropertyManage.Data.Entities;

namespace bca.api.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IMapper _mapper;

        public UserRoleService(IUserRoleRepository userRoleRepository, IMapper mapper)
        {
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleDTO>> GetUserRolesAsync(string userId)
        {
            var userRoles = await _userRoleRepository.GetRolesByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<RoleDTO>>(userRoles.Select(ur => ur.Role));
        }

        public async Task<bool> AddRolesAsync(string userId, List<int> roleIds)
        {
            var existingRoles = await _userRoleRepository.GetRolesByUserIdAsync(userId);
            var existingRoleIds = existingRoles.Select(r => r.RoleId).ToList();

            var newRoles = roleIds
                .Where(rid => !existingRoleIds.Contains(rid))
                .Select(rid => new UserRole { UserId = userId, RoleId = rid })
                .ToList();

            if (newRoles.Any())
            {
                await _userRoleRepository.AddRangeAsync(newRoles);
                return true;
            }

            return false;
        }

        public async Task<bool> RemoveRolesAsync(string userId, List<int> roleIds)
        {
            await _userRoleRepository.RemoveRolesAsync(userId, roleIds);
            return true;
        }

        public async Task<bool> DeleteRolesAsync(string userId)
        {
            return await _userRoleRepository.DeleteRolesAsync(userId);
        }
    }

}
