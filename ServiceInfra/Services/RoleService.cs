using AutoMapper;
using bca.api.DTOs;
using bca.api.Infrastructure.IRepository;
using PropertyManage.Data.Entities;
using System.Collections.Generic;

namespace bca.api.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleDTO>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllWithPermissionsAsync();
            return _mapper.Map<IEnumerable<RoleDTO>>(roles);
        }

        public async Task<Role?> GetRoleByIdAsync(Guid id)
        {
            return await _roleRepository.GetByIdAsync(id, r => r.RolePermissions);
        }

        public async Task<Role> AddRoleAsync(Role role)
        {
            return await _roleRepository.AddAsync(role);
        }

        public async Task<Role?> UpdateRoleAsync(Guid id, Role role)
        {
            var existingRole = await _roleRepository.GetByIdAsync(id);
            if (existingRole == null) return null;

            existingRole.Name = role.Name;
            existingRole.Description = role.Description;
            return await _roleRepository.UpdateAsync(existingRole);
        }

        public async Task<bool> DeleteRoleAsync(Guid id)
        {
            return await _roleRepository.DeleteAsync(id);
        }

        public async Task<Role?> GetRoleByNameAsync(string name)
        {
            return await _roleRepository.GetByNameAsync(name);
        }
    }
}
