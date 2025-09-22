using AutoMapper;
using PropertyManage.Data.Entities;
using PropertyManage.Domain.DTOs;
using PropertyManage.Infrastructure.IRepository;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.ServiceInfra.Services
{
    public class TenantService:ITenantService
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IMapper _mapper;

        public TenantService(ITenantRepository tenantRepository, IMapper mapper)
        {
            _tenantRepository = tenantRepository;
            _mapper = mapper;
        }

        public async Task<TenantDTO> CreateAsync(TenantCreateDTO dto)
        {
            if (await _tenantRepository.ExistsByEmailAsync(dto.Email))
                throw new Exception("Tenant with this email already exists.");

            var tenant = _mapper.Map<Tenant>(dto);
            await _tenantRepository.AddAsync(tenant);
            await _tenantRepository.SaveChangesAsync();
            return _mapper.Map<TenantDTO>(tenant);
        }

        public async Task<TenantDTO> UpdateAsync(Guid id, TenantUpdateDTO dto)
        {
            var tenant = await _tenantRepository.GetByIdAsync(id);
            if (tenant == null) throw new Exception("Tenant not found.");

            // Manual field-by-field update (Partial Update)
            if (!string.IsNullOrEmpty(dto.FullName))
                tenant.FullName = dto.FullName;

            if (!string.IsNullOrEmpty(dto.Email))
                tenant.Email = dto.Email;

            if (!string.IsNullOrEmpty(dto.PhoneNumber))
                tenant.PhoneNumber = dto.PhoneNumber;

            if (dto.MoveInDate.HasValue)
                tenant.MoveInDate = dto.MoveInDate.Value;

            if (dto.MoveOutDate.HasValue)
                tenant.MoveOutDate = dto.MoveOutDate.Value;

            if (dto.UnitId.HasValue)
                tenant.UnitId = dto.UnitId.Value;

            if (dto.TenantType.HasValue)
                tenant.TenantType = dto.TenantType.Value;

            // Save changes
            await _tenantRepository.UpdateAsync(tenant);
            await _tenantRepository.SaveChangesAsync();

            return _mapper.Map<TenantDTO>(tenant);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var tenant = await _tenantRepository.GetByIdAsync(id);
            if (tenant == null) return false;

            await _tenantRepository.DeleteAsync(id);
            await _tenantRepository.SaveChangesAsync();
            return true;
        }

        public async Task<TenantDTO> GetByIdAsync(Guid id)
        {
            var tenant = await _tenantRepository.GetByIdAsync(id);
            if (tenant == null) return null;
            return _mapper.Map<TenantDTO>(tenant);
        }

        public async Task<IEnumerable<TenantDTO>> GetAllAsync()
        {
            var tenants = await _tenantRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TenantDTO>>(tenants);
        }
    }
}
