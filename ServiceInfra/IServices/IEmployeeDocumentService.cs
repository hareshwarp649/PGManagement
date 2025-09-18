using bca.api.DTOs;
using Microsoft.AspNetCore.Mvc;
using PropertyManage.Data.Entities;

namespace bca.api.Services
{
    public interface IEmployeeDocumentService
    {
        Task<EmployeeDocumentDTO> UploadDocumentAsync(EmployeeDocumentUploadDTO uploadDTO);
        Task<IEnumerable<EmployeeDocumentDTO>> GetEmployeeDocumentsAsync(int employeeId);
        Task<bool> DeleteDocumentAsync(int id);
        Task<FileStreamResult?> GetDocumentAsync(int documentId);
        Task<EmployeeDocument?> GetByIdAsync(int documentId);
    }
}
