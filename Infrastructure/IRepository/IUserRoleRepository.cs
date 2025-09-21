using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.IRepository;

namespace bca.api.Infrastructure.IRepository
{
    public interface IUserRoleRepository : IGenericRepository<UserRole>
    {

        Task<IEnumerable<UserRole>> GetUsersByRoleNameAsync(string roleName);
        Task AssignRoleAsync(UserRole userRole);
        Task RemoveRoleAsync(UserRole userRole);
        Task<List<ApplicationRole>> GetRolesByUserIdAsync(Guid userId);
    }
}
