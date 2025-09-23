using AutoMapper;
using PropertyManage.Data.Entities;
using PropertyManage.Domain.DTOs;
using PropertyManage.Infrastructure.IRepository;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.ServiceInfra.Services
{
    public class UnitService: IUnitService
    {
        private readonly IUnitRepository _unitRepository;
        private readonly IMapper _mapper;

        public UnitService(IUnitRepository unitRepository, IMapper mapper)
        {
            _unitRepository = unitRepository;
            _mapper = mapper;
        }

        public async Task<UnitDTO> CreateAsync(UnitCreateDTO dto, Guid currentUserId)
        {
            if (await _unitRepository.ExistsByUnitNumberAsync(dto.PropertyId, dto.UnitNumber))
                throw new Exception("Unit number already exists for this property.");

            var unit = _mapper.Map<Unit>(dto);
            unit.CreatedBy = currentUserId.ToString();
            unit.CreatedAt = DateTime.UtcNow;
            unit.UpdatedBy = currentUserId.ToString();
            unit.UpdatedAt = DateTime.UtcNow;
            unit.Id = Guid.NewGuid();
            unit.IsActive = true;

            await _unitRepository.AddAsync(unit);
            await _unitRepository.SaveChangesAsync();

            return _mapper.Map<UnitDTO>(unit);
        }

        public async Task<UnitDTO> UpdateAsync(Guid id, UnitUpdateDTO dto, Guid currentUserId)
        {
            var unit = await _unitRepository.GetByIdAsync(id) ?? throw new Exception("Unit not found.");

            if (dto.UnitNumber != null && await _unitRepository.ExistsByUnitNumberAsync(unit.PropertyId, dto.UnitNumber, id))
                throw new Exception("Unit number already exists for this property.");

            _mapper.Map(dto, unit); // partial update
            unit.UpdatedBy = currentUserId.ToString();
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
