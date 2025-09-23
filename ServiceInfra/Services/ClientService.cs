using AutoMapper;
using bca.api.Services;
using PropertyManage.Domain.DTOs;
using PropertyManage.Infrastructure.IRepository;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.ServiceInfra.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        public ClientService(IClientRepository clientRepository, IMapper mapper, IUserContextService userContextService)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<IEnumerable<ClientDTO>> GetAllAsync()
        {
            var clients = await _clientRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ClientDTO>>(clients);
        }

        public async Task<ClientDTO?> GetByIdAsync(Guid id, Guid? requestingClientId = null, bool isSuperAdmin = false)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null) return null;

            if (!isSuperAdmin && requestingClientId.HasValue && client.Id != requestingClientId.Value)
                throw new UnauthorizedAccessException("Not allowed to access this client.");

            return _mapper.Map<ClientDTO>(client);
        }

        public async Task<ClientDTO> CreateAsync(ClientCreateDTO dto)
        {
            if (await _clientRepository.ExistsByEmailAsync(dto.ContactEmail))
                throw new InvalidOperationException("A client with this email already exists.");

            var client = _mapper.Map<Data.Entities.Client>(dto);
            client.Id = Guid.NewGuid();
            client.OnboardedAt = DateTime.UtcNow;
            client.CreatedAt = DateTime.UtcNow;
            client.UpdatedAt = DateTime.UtcNow;
            client.CreatedBy = _userContextService.UserId;
            client.UpdatedBy = _userContextService.UserId;
            client.IsActive = true;
            await _clientRepository.AddAsync(client);
            await _clientRepository.SaveChangesAsync();
            return _mapper.Map<ClientDTO>(client);
        }

        public async Task<ClientDTO?> UpdateAsync(Guid id, ClientUpdateDTO dto, bool isSuperAdmin = false, Guid? requestingClientId = null)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null) return null;

            if (!isSuperAdmin && requestingClientId.HasValue && client.Id != requestingClientId.Value)
                throw new UnauthorizedAccessException("Not allowed to update this client.");

            // Manual partial update (only update if dto property provided)
            if (!string.IsNullOrWhiteSpace(dto.ClientName)) client.ClientName = dto.ClientName;
            if (!string.IsNullOrWhiteSpace(dto.Address)) client.Address = dto.Address;
            if (!string.IsNullOrWhiteSpace(dto.City)) client.City = dto.City;
            if (!string.IsNullOrWhiteSpace(dto.State)) client.State = dto.State;
            if (!string.IsNullOrWhiteSpace(dto.Pincode)) client.Pincode = dto.Pincode;
            if (!string.IsNullOrWhiteSpace(dto.ContactPerson)) client.ContactPerson = dto.ContactPerson;


            if (dto.ContactEmail != null && await _clientRepository.ExistsByEmailAsync(dto.ContactEmail, id))
                throw new InvalidOperationException("A client with this email already exists.");

            if (!string.IsNullOrWhiteSpace(dto.ContactPhone)) client.ContactPhone = dto.ContactPhone;

            _mapper.Map(dto, client); // partial update

            client.UpdatedAt = DateTime.UtcNow;
            client.UpdatedBy = _userContextService.UserId;
            await _clientRepository.UpdateAsync(client);
            await _clientRepository.SaveChangesAsync();
            return _mapper.Map<ClientDTO>(client);
        }

        public async Task<bool> DeleteAsync(Guid id, bool isSuperAdmin = false)
        {
            if (!isSuperAdmin)
                throw new UnauthorizedAccessException("Only super admins can delete clients.");

            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null) return false;

            await _clientRepository.DeleteAsync(id);
            await _clientRepository.SaveChangesAsync();
            return true;
        }
    }
}
