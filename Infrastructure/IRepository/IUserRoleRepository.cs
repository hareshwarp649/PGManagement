using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.IRepository;

namespace bca.api.Infrastructure.IRepository
{
    public interface IUserRoleRepository : IGenericRepository<UserRole>
    {
        Task<IEnumerable<UserRole>> GetRolesByUserIdAsync(string userId);
        Task<IEnumerable<UserRole>> GetUsersByRoleNameAsync(string roleName);
        Task RemoveRolesAsync(string userId, List<int> roleIds);
        Task AddRangeAsync(IEnumerable<UserRole> userRoles);
        Task<bool> DeleteRolesAsync(string userId);
    }
}
