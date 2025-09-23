using AutoMapper;
using bca.api.Services;
using PropertyManage.Data.Entities;
using PropertyManage.Domain.DTOs;
using PropertyManage.Infrastructure.IRepository;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.ServiceInfra.Services
{
    public class ClientSubscriptionService: IClientSubscriptionService
    {
        private readonly IClientSubscriptionRepository _clientSubscriptionRepository;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;
        public ClientSubscriptionService(IClientSubscriptionRepository clientSubscriptionRepository, IUserContextService userContextService ,IMapper mapper)
        {
            _clientSubscriptionRepository = clientSubscriptionRepository;
            _userContextService = userContextService;
            _mapper = mapper;
        }
        public async Task<ClientSubscriptionDTO> CreateAsync(CreateClientSubscriptionDTO dto)
        {
            if (await _clientSubscriptionRepository.ExistsByClientAndPlanAsync(dto.ClientId, dto.SubscriptionPlanId))
            {
                throw new InvalidOperationException("Client already has an active subscription for this plan.");
            }
            var entity = _mapper.Map<ClientSubscription>(dto);
            entity.Id = Guid.NewGuid();
            entity.CreatedBy = _userContextService.UserId;
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedBy = _userContextService.UserName;
            entity.UpdatedAt = DateTime.UtcNow;
            await _clientSubscriptionRepository.AddAsync(entity);
            await _clientSubscriptionRepository.SaveChangesAsync();

            return _mapper.Map<ClientSubscriptionDTO>(entity);
        }

        public async Task<ClientSubscriptionDTO> GetClientSubscriptionByIdAsync(Guid id)
        {
            var entity = await _clientSubscriptionRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException("Client subscription not found.");
            }

            var clientId = _userContextService.ClientId;
            if (clientId.HasValue && entity.ClientId != clientId.Value)
                throw new UnauthorizedAccessException("Access denied.");

            return _mapper.Map<ClientSubscriptionDTO>(entity);
        }

        public async Task<IEnumerable<ClientSubscriptionDTO>> GetAllClientSubscriptionsAsync()
        {
            var clientId = _userContextService.ClientId;
            var entities = await _clientSubscriptionRepository.GetAllAsync();

            if (clientId.HasValue)
            {
                entities = entities.Where(cs => cs.ClientId == clientId.Value).ToList();
            }
            return _mapper.Map<IEnumerable<ClientSubscriptionDTO>>(entities);
        }
        public async Task<ClientSubscriptionDTO> UpdateAsync(Guid id, UpdateClientSubscriptionDTO dto)
        {
            var entity = await _clientSubscriptionRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException("Client subscription not found.");
            }
            if (dto.SubscriptionPlanId.HasValue)
                entity.SubscriptionPlanId = dto.SubscriptionPlanId.Value;
            if (dto.StartDate.HasValue)
                entity.StartDate = dto.StartDate.Value;
            if (dto.EndDate.HasValue)
                entity.EndDate = dto.EndDate.Value;
            if (dto.IsActive.HasValue)
                entity.IsActive = dto.IsActive.Value;

            entity.UpdatedBy = _userContextService.UserName;
            entity.UpdatedAt = DateTime.UtcNow;
            await _clientSubscriptionRepository.UpdateAsync(entity);
            await _clientSubscriptionRepository.SaveChangesAsync();

           return _mapper.Map<ClientSubscriptionDTO>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _clientSubscriptionRepository.GetByIdAsync(id);
            if (entity == null) return false;

            await _clientSubscriptionRepository.DeleteAsync(id);
            await _clientSubscriptionRepository.SaveChangesAsync();
            return true;
        }

    }
}
