using AutoMapper;
using bca.api.DTOs;
using bca.api.Infrastructure.IRepository;
using PropertyManage.Data.Entities;

namespace bca.api.Services
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;

        public RolePermissionService(IRolePermissionRepository rolePermissionRepository, IPermissionRepository permissionRepository, IMapper mapper)
        {
            _rolePermissionRepository = rolePermissionRepository;
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PermissionDTO>> GetPermissionsByRoleIdAsync(int roleId)
        {
            var rolePermissions = await _rolePermissionRepository.GetPermissionsByRoleIdAsync(roleId);
            return _mapper.Map<IEnumerable<PermissionDTO>>(rolePermissions.Select(rp => rp.Permission));
        }

        public async Task<bool> AddPermissionsAsync(int roleId, List<int> permissionIds)
        {
            var existingPermissions = await _rolePermissionRepository.GetPermissionsByRoleIdAsync(roleId);
            var existingPermissionIds = existingPermissions.Select(p => p.Permission.Id).ToList();

            var newPermissions = permissionIds
                .Where(pid => !existingPermissionIds.Contains(pid))
                .Select(pid => new RolePermission { RoleId = roleId, PermissionId = pid })
                .ToList();

            if (newPermissions.Any())
            {
                await _rolePermissionRepository.AddRangeAsync(newPermissions);
                return true;
            }
            return false;
        }

        public async Task<bool> RemovePermissionsAsync(int roleId, List<int> permissionIds)
        {
            await _rolePermissionRepository.RemovePermissionsAsync(roleId, permissionIds);
            return true;
        }
    }

}
