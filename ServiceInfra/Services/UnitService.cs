using AutoMapper;
using bca.api.Services;
using Microsoft.EntityFrameworkCore;
using PropertyManage.Data.Entities;
using PropertyManage.Domain.DTOs;
using PropertyManage.Infrastructure.IRepository;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.ServiceInfra.Services
{
    public class UnitService: IUnitService
    {
        private readonly IUnitRepository _unitRepository;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;

        public UnitService(IUnitRepository unitRepository, IUserContextService userContextService, IMapper mapper)
        {
            _unitRepository = unitRepository;
            _userContextService = userContextService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UnitDTO>> GetAllAsync()
        {
            var units = await _unitRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UnitDTO>>(units);
        }

        public async Task<UnitDTO> CreateAsync(UnitCreateDTO dto)
        {
            if (await _unitRepository.ExistsByUnitNumberAsync(dto.PropertyId, dto.UnitNumber))
                throw new Exception("Unit number already exists for this property.");

            var unit = _mapper.Map<Unit>(dto);
            unit.Id = Guid.NewGuid();
            unit.CreatedBy = _userContextService.UserName;
            unit.CreatedAt = DateTime.UtcNow;
            unit.UpdatedBy = _userContextService.UserId;
            unit.UpdatedAt = DateTime.UtcNow;
            unit.Id = Guid.NewGuid();
            unit.IsActive = true;

            await _unitRepository.AddAsync(unit);
            await _unitRepository.SaveChangesAsync();

            return _mapper.Map<UnitDTO>(unit);
        }

        public async Task<UnitDTO> UpdateAsync(Guid id, UnitUpdateDTO dto)
        {
            var unit = await _unitRepository.GetByIdAsync(id) ?? throw new Exception("Unit not found.");

            if (!string.IsNullOrWhiteSpace(dto.UnitNumber))
            {
                if (await _unitRepository.ExistsByUnitNumberAsync(unit.PropertyId, dto.UnitNumber, id))
                    throw new Exception("Unit number already exists for this property.");
                unit.UnitNumber = dto.UnitNumber.Trim();
            }

            if (!string.IsNullOrWhiteSpace(dto.UnitType) && dto.UnitType != "string")
                unit.UnitType = dto.UnitType.Trim();

            if (dto.Capacity > 0)unit.Capacity = dto.Capacity.Value;     
            if (dto.FloorNumber >= 0) unit.FloorNumber = dto.FloorNumber.Value;
            if (dto.AreaInSqFt > 0) unit.AreaInSqFt = dto.AreaInSqFt.Value;


            unit.UpdatedBy = _userContextService.UserId;
            unit.UpdatedAt = DateTime.UtcNow;

            await _unitRepository.UpdateAsync(unit);
            await _unitRepository.SaveChangesAsync();

            return _mapper.Map<UnitDTO>(unit);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var unit = await _unitRepository.GetByIdAsync(id);
            if (unit == null) return false;

            await _unitRepository.DeleteAsync(id);
            await _unitRepository.SaveChangesAsync();
            return true;
        }

        public async Task<UnitDTO> GetByIdAsync(Guid id)
        {
            var unit = await _unitRepository.GetByIdAsync(id) ?? throw new Exception("Unit not found.");
            return _mapper.Map<UnitDTO>(unit);
        }

        public async Task<IEnumerable<UnitDTO>> GetAllByPropertyAsync(Guid propertyId)
        {
            var units = await _unitRepository.GetAllAsync(u => u.PropertyId == propertyId);
            return _mapper.Map<IEnumerable<UnitDTO>>(units);
        }
    }
}
