
using PropertyManage.Data.Entities;

namespace bca.api.Services
{
    public interface IUserContextService
    {
        //Task<ApplicationUser?> GetCurrentUserAsync(bool ensureNotDeleted, bool ensureNotBCA);
        string UserId { get; }      // JWT claim se UserId
        string UserName { get; }
        Guid? ClientId { get; }
    }

}
