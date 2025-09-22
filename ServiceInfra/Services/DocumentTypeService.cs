using PropertyManage.Data.MasterEntities;
using PropertyManage.Infrastructure.IRepository;
using PropertyManage.Infrastructure.Repository;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.ServiceInfra.Services
{
    public class DocumentTypeService : IDocumentTypeService
    {
        private readonly IDocumentTypeRepository _repository;

        public DocumentTypeService(IDocumentTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DocumentType>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<DocumentType> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<DocumentType> CreateAsync(DocumentType documentType)
        {
            await _repository.AddAsync(documentType);
            await _repository.SaveChangesAsync();
            return documentType;
        }

        public async Task<DocumentType> UpdateAsync(DocumentType documentType)
        {
            await _repository.UpdateAsync(documentType);
            await _repository.SaveChangesAsync();
            return documentType;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

    }
}
