using PropertyManage.Data.MasterEntities;

namespace PropertyManage.ServiceInfra.IServices
{
    public interface IDocumentTypeService
    {
        Task<IEnumerable<DocumentType>> GetAllAsync();
        Task<DocumentType> GetByIdAsync(Guid id);
        Task<DocumentType> CreateAsync(DocumentType documentType);
        Task<DocumentType> UpdateAsync(DocumentType documentType);
        Task<bool> DeleteAsync(Guid id);
    }
}
