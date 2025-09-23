using AutoMapper;
using bca.api.Services;
using PropertyManage.Data.MasterEntities;
using PropertyManage.Domain.DTOs;
using PropertyManage.Infrastructure.IRepository;
using PropertyManage.Infrastructure.Repository;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.ServiceInfra.Services
{
    public class PropertyTypeService: IPropertyTypeService
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;

        public PropertyTypeService(IPropertyTypeRepository propertyTypeRepository,IUserContextService userContextService, IMapper mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _userContextService = userContextService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PropertyTypeDTO>> GetAllAsync()
        {
            var types = await _propertyTypeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PropertyTypeDTO>>(types);
        }

        public async Task<PropertyTypeDTO?> GetByIdAsync(Guid id)
        {
            var type = await _propertyTypeRepository.GetByIdAsync(id);
            return type == null ? null : _mapper.Map<PropertyTypeDTO>(type);
        }

        public async Task<bool> ExistsByNameAsync(string typeName)
        {
            return await _propertyTypeRepository.ExistsByNameAsync(typeName);
        }

        public async Task<PropertyTypeDTO> CreateAsync(CreatePropertyTypeDTO dto)
        {
            if (await _propertyTypeRepository.ExistsByNameAsync(dto.TypeName))
                throw new Exception("Property type with the same name already exists.");

           
            var entity = _mapper.Map<PropertyType>(dto);

            entity.Id = Guid.NewGuid();
            entity.CreatedBy = _userContextService.UserId;
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedBy = _userContextService.UserId;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = true;

            await _propertyTypeRepository.AddAsync(entity);
            return _mapper.Map<PropertyTypeDTO>(entity);
        }

        public async Task<PropertyTypeDTO?> UpdateAsync(PropertyTypeDTO dto)
        {
            if (await _propertyTypeRepository.ExistsByNameAsync(dto.TypeName))
                throw new Exception("Property type with the same name already exists.");

            var entity = await _propertyTypeRepository.GetByIdAsync(dto.Id);
            if (entity == null) return null;

            _mapper.Map(dto, entity); // Auto update fields
            await _propertyTypeRepository.UpdateAsync(entity);

            return _mapper.Map<PropertyTypeDTO>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _propertyTypeRepository.DeleteAsync(id);
        }
    }
}
