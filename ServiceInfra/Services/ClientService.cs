using AutoMapper;
using bca.api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using PropertyManage.Data.Entities;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        public ClientService(IClientRepository clientRepository, IMapper mapper, IUserContextService userContextService,  UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
            _userContextService = userContextService;
            _userManager = userManager;
            _emailSender = emailSender;
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

            if (await _clientRepository.ExistsByNameAsync(dto.ClientName))
                throw new InvalidOperationException("A client with this name already exists.");

            var client = _mapper.Map<Client>(dto);
            client.Id = Guid.NewGuid();
            client.OnboardedAt = DateTime.UtcNow;
            client.CreatedAt = DateTime.UtcNow;
            client.UpdatedAt = DateTime.UtcNow;
            client.CreatedBy = _userContextService.UserId;
            client.UpdatedBy = _userContextService.UserId;
            client.IsActive = true;

            await _clientRepository.AddAsync(client);
            await _clientRepository.SaveChangesAsync();

            // ==== Identity User Creation ====
            var tempPassword = "Temp@" + Guid.NewGuid().ToString("N").Substring(0, 8); 

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = dto.ContactEmail,
                Email = dto.ContactEmail,
                PhoneNumber = dto.ContactPhone,
                ClientId = client.Id,
                EmailConfirmed = true,
                MustChangePassword = true
            };

            var result = await _userManager.CreateAsync(user, tempPassword);
            if (!result.Succeeded)
                throw new Exception($"User creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            // Assign ClientOwner Role
            await _userManager.AddToRoleAsync(user, "Client_Owner");

            // Send Email with temp password
            var subject = "Welcome to Retro! Your temporary password";
            var body = $"Hello {dto.ContactPerson},<br>Your account has been created.<br>" +
                       $"Username: {dto.ContactEmail}<br>Temporary Password: {tempPassword}<br>" +
                       $"Please login and change your password immediately.";

            await _emailSender.SendEmailAsync(dto.ContactEmail, subject, body);

            return _mapper.Map<ClientDTO>(client);
        }
        //private string GenerateTemporaryPassword(int length = 12)
        //{
        //    const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@$?_-";
        //    var random = new Random();
        //    return new string(Enumerable.Repeat(validChars, length)
        //      .Select(s => s[random.Next(s.Length)]).ToArray());
        //}

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
