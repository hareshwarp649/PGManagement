using bca.api.Enums;
using PropertyManage.Domain.Enums;

namespace bca.api.DTOs
{
    public class UserMultipleDocumentUploadDTO
    {
        public int UserId { get; set; }
        public List<DocumentUploadItemDTO> Documents { get; set; } = new();
    }

    public class DocumentUploadItemDTO
    {
        public DocumentType DocumentType { get; set; }
        public IFormFile File { get; set; } = default!;
    }
}
