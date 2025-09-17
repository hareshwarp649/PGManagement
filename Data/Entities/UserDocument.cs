using PropertyManage.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PropertyManage.Data.Entities
{
    public class UserDocument
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        //public BCAUser? User { get; set; }

        public DocumentType DocumentType { get; set; }
        public string FileName { get; set; } = string.Empty; // Original file name
        public string FilePath { get; set; } = string.Empty; // Stored file path
        public string ContentType { get; set; } = string.Empty; // MIME type (image/jpeg, application/pdf, etc.)
        public long FileSize { get; set; } // File size in bytes
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
