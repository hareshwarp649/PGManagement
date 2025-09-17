using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.IRepository;

namespace bca.api.Infrastructure.IRepository
{
    public interface IUserDocumentRepository : IGenericRepository<UserDocument>
    {
        Task<IEnumerable<UserDocument>> GetUserDocumentsAsync(int userId);
    }
}
