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
        public async Task<PermissionDTO?> GetPermissionByIdAsync(Guid id)
        {
            var permission = await _permissionRepository.GetByIdAsync(id, p => p.RolePermissions);
            return _mapper.Map<PermissionDTO>(permission);
        }

        public async Task<PermissionDTO> AddPermissionAsync(PermissionDTO dto)
        {
            var permission = _mapper.Map<Permission>(dto);
            permission.Id = Guid.NewGuid();

            await _permissionRepository.AddAsync(permission);
            await _permissionRepository.SaveChangesAsync();

            return _mapper.Map<PermissionDTO>(permission);
        }

        public async Task<PermissionDTO?> UpdatePermissionAsync(Guid id, PermissionDTO permission)
        {
            var existingPermission = await _permissionRepository.GetByIdAsync(id);
            if (existingPermission == null) return null;

            //existingPermission.Name = permission.Name;
            _mapper.Map(permission,existingPermission);
            var updatedPermission = await _permissionRepository.UpdateAsync(existingPermission);
            return _mapper.Map<PermissionDTO>(updatedPermission);
        }

        public async Task<bool> DeletePermissionAsync(Guid id)
        {
            await _permissionRepository.DeleteAsync(id);
            return true;
        }

      
    }
}
