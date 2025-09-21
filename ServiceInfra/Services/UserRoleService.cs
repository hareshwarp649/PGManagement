using AutoMapper;
using bca.api.DTOs;
using bca.api.Infrastructure.IRepository;
using Microsoft.EntityFrameworkCore;
using PropertyManage.Data.Entities;

namespace bca.api.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IMapper _mapper;

        public UserRoleService(IUserRoleRepository userRoleRepository, IMapper mapper)
        {
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
        }

        public async Task AssignRoleAsync(UserRole userRole)
        {
            await _userRoleRepository.AssignRoleAsync(userRole);
        }

        public async Task RemoveRoleAsync(UserRole userRole)
        {
            await _userRoleRepository.RemoveRoleAsync(userRole);
        }

        public async Task<List<ApplicationRole>> GetRolesByUserIdAsync(Guid userId)
        {
            return await _userRoleRepository.GetRolesByUserIdAsync(userId);
        }
    }

}
