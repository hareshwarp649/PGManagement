using bca.api.Enums;
using PropertyManage.Domain.Enums;

namespace bca.api.DTOs
{
    public class UserDocumentUploadDTO
    {
        public int UserId { get; set; }
        public DocumentType DocumentType { get; set; }
        public IFormFile File { get; set; } = default!;
    }
}
