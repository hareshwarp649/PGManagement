using AutoMapper;
using bca.api.DTOs;
using bca.api.Infrastructure.IRepository;
using PropertyManage.Data.Entities;
using PropertyManage.Domain.DTOs;
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

        public async Task<RoleDTO?> GetRoleByIdAsync(Guid id)
        {
            var role = await _roleRepository.GetByIdWithPermissionsAsync(id);
            return role == null ? null : _mapper.Map<RoleDTO>(role);
        }
        public async Task<RoleDTO> CreateRoleAsync(RoleCreateDTO dto)
        {
            var role = _mapper.Map<ApplicationRole>(dto);
            role.Id = Guid.NewGuid();

            await _roleRepository.AddAsync(role);

            return _mapper.Map<RoleDTO>(role);
        }


        public async Task<RoleDTO?> UpdateRoleAsync(Guid id, RoleCreateDTO dto)
        {
            var existingRole = await _roleRepository.GetByIdAsync(id);
            if (existingRole == null) return null;

            existingRole.Name = dto.Name;
            existingRole.Description = dto.Description;
             await _roleRepository.UpdateAsync(existingRole);
            return _mapper.Map<RoleDTO?>(existingRole); 
        }


        public async Task<bool> DeleteRoleAsync(Guid id)
        {
            return await _roleRepository.DeleteAsync(id);
        }
    }
}
