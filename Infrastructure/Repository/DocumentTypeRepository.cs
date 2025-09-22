using PropertyManage.Data;
using PropertyManage.Data.MasterEntities;
using PropertyManage.Infrastructure.IRepository;

namespace PropertyManage.Infrastructure.Repository
{
    public class DocumentTypeRepository : GenericRepository<DocumentType>, IDocumentTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public DocumentTypeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // Implement custom methods here if required
    }
}
