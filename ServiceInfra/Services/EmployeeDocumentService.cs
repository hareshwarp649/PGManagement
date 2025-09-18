using AutoMapper;
using bca.api.DTOs;
using bca.api.Infrastructure.IRepository;
using Microsoft.AspNetCore.Mvc;
using PropertyManage.Data.Entities;

namespace bca.api.Services
{
    public class EmployeeDocumentService : IEmployeeDocumentService
    {
        private readonly IEmployeeDocumentRepository _documentRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public EmployeeDocumentService(IEmployeeDocumentRepository documentRepository, IWebHostEnvironment environment, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _documentRepository = documentRepository;
            _environment = environment;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<EmployeeDocumentDTO> UploadDocumentAsync(EmployeeDocumentUploadDTO uploadDTO)
        {
            // Ensure uploads directory exists
            string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Generate unique file name
            string uniqueFileName = $"{Guid.NewGuid()}_{uploadDTO.File.FileName}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save file to server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await uploadDTO.File.CopyToAsync(stream);
            }

            // Save document metadata to database
            var document = new EmployeeDocument
            {
                EmployeeId = uploadDTO.EmployeeId,
                DocumentType = uploadDTO.DocumentType,
                FileName = uploadDTO.File.FileName,
                FilePath = filePath,
                ContentType = uploadDTO.File.ContentType,
                FileSize = uploadDTO.File.Length
            };

            var savedDocument = await _documentRepository.AddAsync(document);
            return _mapper.Map<EmployeeDocumentDTO>(savedDocument);
        }

        public async Task<IEnumerable<EmployeeDocumentDTO>> GetEmployeeDocumentsAsync(int employeeId)
        {
            var documents = await _documentRepository.GetEmployeeDocumentsAsync(employeeId);
            var mappedDocuments = _mapper.Map<IEnumerable<EmployeeDocumentDTO>>(documents);

            foreach (var doc in mappedDocuments)
            {
                // Convert absolute file path to public URL
                var fileName = Path.GetFileName(doc.FilePath);
                doc.FileUrl = $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}/uploads/{fileName}";
            }

            return mappedDocuments;
        }

        public async Task<bool> DeleteDocumentAsync(Guid id)
        {
            var document = await _documentRepository.GetByIdAsync(id);
            if (document == null)
                return false;

            // Delete file from storage
            if (File.Exists(document.FilePath))
                File.Delete(document.FilePath);

            return await _documentRepository.DeleteAsync(id);
        }

        public async Task<FileStreamResult?> GetDocumentAsync(Guid documentId)
        {
            var document = await _documentRepository.GetByIdAsync(documentId);
            if (document == null || !File.Exists(document.FilePath))
                return null;

            var stream = new FileStream(document.FilePath, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(stream, document.ContentType) { FileDownloadName = document.FileName };
        }

        public async Task<EmployeeDocument?> GetByIdAsync(Guid documentId)
        {
            return await _documentRepository.GetByIdAsync(documentId);
        }
    }
}
