using AutoMapper;
using bca.api.Services;
using PropertyManage.Data.Entities;
using PropertyManage.Domain.DTOs;
using PropertyManage.Infrastructure.IRepository;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.ServiceInfra.Services
{
    public class SubscriptionPlanService : ISubscriptionPlanService
    {
        private readonly ISubscriptionPlanRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserContextService _currentUserService;

        public SubscriptionPlanService(ISubscriptionPlanRepository repository, IMapper mapper, IUserContextService currentUserService)
        {
            _repository = repository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task<SubscriptionPlanDTO> CreateAsync(SubscriptionPlanCreateDTO dto)
        {
            if (await _repository.ExistsByNameAsync(dto.PlanName))
                throw new Exception("Subscription plan with the same name already exists.");

            var entity = _mapper.Map<SubscriptionPlan>(dto);
            entity.Id = Guid.NewGuid();
            entity.CreatedBy = _currentUserService.UserId;
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedBy = _currentUserService.UserId;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = true;

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            return _mapper.Map<SubscriptionPlanDTO>(entity);
        }

        public async Task<SubscriptionPlanDTO> UpdateAsync(Guid id, SubscriptionPlanUpdateDTO dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw new Exception("Subscription plan not found.");

            // Partial update logic
            if (!string.IsNullOrEmpty(dto.PlanName)) entity.PlanName = dto.PlanName;
            if (dto.Price.HasValue) entity.Price = dto.Price.Value;
            if (!string.IsNullOrEmpty(dto.BillingCycle)) entity.BillingCycle = dto.BillingCycle;
            if (dto.MaxProperties.HasValue) entity.MaxProperties = dto.MaxProperties.Value;
            if (dto.MaxBuildings.HasValue) entity.MaxBuildings = dto.MaxBuildings.Value;
            if (dto.MaxUnits.HasValue) entity.MaxUnits = dto.MaxUnits.Value;
            if (dto.MaxUsers.HasValue) entity.MaxUsers = dto.MaxUsers.Value;
            if (dto.SupportIncluded.HasValue) entity.SupportIncluded = dto.SupportIncluded.Value;

            entity.UpdatedBy = _currentUserService.UserId;
            entity.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();

            return _mapper.Map<SubscriptionPlanDTO>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;

            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<SubscriptionPlanDTO> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;
            return _mapper.Map<SubscriptionPlanDTO>(entity);
        }

        public async Task<IEnumerable<SubscriptionPlanDTO>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<SubscriptionPlanDTO>>(entities);
        }
    }
}
