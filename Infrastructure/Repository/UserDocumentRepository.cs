using bca.api.Infrastructure.IRepository;
using Microsoft.EntityFrameworkCore;
using PropertyManage.Data;
using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.Repository;
using System;

namespace bca.api.Infrastructure.Repository
{
    public class UserDocumentRepository : GenericRepository<UserDocument>, IUserDocumentRepository
    {
        private readonly ApplicationDbContext _context;

        public UserDocumentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDocument>> GetUserDocumentsAsync(int userId)
        {
            return await _context.UserDocuments
                .Where(d => d.UserId == userId)
                .ToListAsync();
        }
    }
}
