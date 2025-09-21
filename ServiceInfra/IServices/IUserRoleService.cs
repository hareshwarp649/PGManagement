using bca.api.DTOs;
using PropertyManage.Data.Entities;

namespace bca.api.Services
{
    public interface IUserRoleService
    {

        Task AssignRoleAsync(UserRole userRole);
        Task RemoveRoleAsync(UserRole userRole);
        Task<List<ApplicationRole>> GetRolesByUserIdAsync(Guid userId);
    }
}
