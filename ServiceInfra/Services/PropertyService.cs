using AutoMapper;
using bca.api.Services;
using PropertyManage.Data.Entities;
using PropertyManage.Domain.DTOs;
using PropertyManage.Infrastructure.IRepository;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.ServiceInfra.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUserContextService _currentUserService;
        private readonly IMapper _mapper;

        public PropertyService(IPropertyRepository propertyRepository, IUserContextService userContextService, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _currentUserService = userContextService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PropertyDTO>> GetAllPropertiesAsync(Guid? clientId = null)
        {
            var query = clientId.HasValue
                ? await _propertyRepository.GetAllAsync(p => p.ClientId == clientId.Value)
                : await _propertyRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<PropertyDTO>>(query);
        }

        public async Task<PropertyDTO> GetPropertyByIdAsync(Guid id, Guid? clientId = null)
        {
            var property = await _propertyRepository.GetByIdAsync(id);
            if (property == null) throw new KeyNotFoundException("Property not found");
            if (clientId.HasValue && property.ClientId != clientId.Value) throw new UnauthorizedAccessException();

            return _mapper.Map<PropertyDTO>(property);
        }

        public async Task<PropertyDTO> AddPropertyAsync(PropertyCreateDTO dto)
        {
            //if (await _propertyRepository.ExistsByNameAsync(dto.PropertyName, clientId))
            //    throw new InvalidOperationException("Property name already exists for this client");

            var clientsId = _currentUserService.ClientId;
            if (clientsId == null)
                throw new Exception("Client not found.");

            

            var property = new Propertiy
            {
                PropertyName = dto.PropertyName,
                PropertyTypeId = dto.PropertyTypeId,
                ClientId = clientsId.Value,   // current client
                Address = dto.Address,
                StateId = dto.StateId,
                DistrictId = dto.DistrictId,
                CountryId = dto.CountryId,
                FloorCount = dto.FloorCount,
                TotalRooms = dto.TotalRooms,
                AreaInSqFt = dto.AreaInSqFt,
                CreatedBy = _currentUserService.UserName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _propertyRepository.AddAsync(property);
            return _mapper.Map<PropertyDTO>(property);
        }

        public async Task<PropertyDTO> UpdatePropertyAsync(Guid id, PropertyUpdateDTO dto, Guid clientId, bool isSuperAdmin = false)
        {
            var property = await _propertyRepository.GetByIdAsync(id);
            if (property == null) throw new KeyNotFoundException("Property not found");
            if (!isSuperAdmin && property.ClientId != clientId) throw new UnauthorizedAccessException();

            // Partial Update Logic
            if (!string.IsNullOrWhiteSpace(dto.PropertyName))
            {
                if (await _propertyRepository.ExistsByNameAsync(dto.PropertyName, property.ClientId, id))
                    throw new InvalidOperationException("Property name already exists for this client");
                property.PropertyName = dto.PropertyName;
            }
            if (dto.PropertyTypeId.HasValue) property.PropertyTypeId = dto.PropertyTypeId.Value;
            if (!string.IsNullOrWhiteSpace(dto.Address)) property.Address = dto.Address;
            if (dto.CountryId.HasValue) property.CountryId = dto.CountryId.Value;
            if (dto.StateId.HasValue) property.StateId = dto.StateId.Value;
            if (dto.DistrictId.HasValue) property.DistrictId = dto.DistrictId.Value;
            if (dto.FloorCount.HasValue) property.FloorCount = dto.FloorCount.Value;
            if (dto.TotalRooms.HasValue) property.TotalRooms = dto.TotalRooms.Value;
            if (dto.AreaInSqFt.HasValue) property.AreaInSqFt = dto.AreaInSqFt.Value;

            property.UpdatedBy=_currentUserService.UserId;
            property.UpdatedAt = DateTime.UtcNow;
            
            await _propertyRepository.UpdateAsync(property);

            return _mapper.Map<PropertyDTO>(property);
        }

        public async Task<bool> DeletePropertyAsync(Guid id, Guid clientId, bool isSuperAdmin = false)
        {
            var property = await _propertyRepository.GetByIdAsync(id);
            if (property == null) return false;
            if (!isSuperAdmin && property.ClientId != clientId) throw new UnauthorizedAccessException();

           return await _propertyRepository.DeleteAsync(id);
           
        }
    }
}
