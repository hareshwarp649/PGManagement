using AutoMapper;
using bca.api.DTOs;
using bca.api.Infrastructure.IRepository;
using PropertyManage.Data.Entities;

namespace bca.api.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;

        public PermissionService(IPermissionRepository permissionRepository, IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PermissionDTO>> GetAllPermissionsAsync()
        {
            var permissions = await _permissionRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PermissionDTO>>(permissions);
        }
        public async Task<PermissionDTO?> GetPermissionByIdAsync(int id)
        {
            var permission = await _permissionRepository.GetByIdAsync(id, p => p.RolePermissions);
            return _mapper.Map<PermissionDTO>(permission);
        }

        public async Task<PermissionDTO> AddPermissionAsync(Permission permission)
        {
            var addedPermission = await _permissionRepository.AddAsync(permission);
            return _mapper.Map<PermissionDTO>(addedPermission);
        }

        public async Task<PermissionDTO?> UpdatePermissionAsync(int id, Permission permission)
        {
            var existingPermission = await _permissionRepository.GetByIdAsync(id);
            if (existingPermission == null) return null;

            existingPermission.Name = permission.Name;
            var updatedPermission = await _permissionRepository.UpdateAsync(existingPermission);
            return _mapper.Map<PermissionDTO>(updatedPermission);
        }

        public async Task<bool> DeletePermissionAsync(int id)
        {
            await _permissionRepository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<PermissionDTO>> SearchAndSortPermissionsAsync(string? name, string? category, string? description, string? sortBy, string? sortOrder, int pageNumber, int pageSize)
        {
            var permissions = await _permissionRepository.SearchAndSortPermissionsAsync(name, category, description, sortBy, sortOrder, pageNumber, pageSize);
            return _mapper.Map<IEnumerable<PermissionDTO>>(permissions);
        }

        public async Task<bool> HasPermissionAsync(string userName, string permissionName)
        {
            return await _permissionRepository.HasPermissionAsync(userName, permissionName);
        }
    }
}
