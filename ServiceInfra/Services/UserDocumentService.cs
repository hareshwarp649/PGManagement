using AutoMapper;
using bca.api.DTOs;
using bca.api.Infrastructure.IRepository;
using Microsoft.AspNetCore.Mvc;
using PropertyManage.Data.Entities;

namespace bca.api.Services
{
    public class UserDocumentService : IUserDocumentService
    {
        private readonly IUserDocumentRepository _documentRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserDocumentService(IUserDocumentRepository documentRepository, IWebHostEnvironment environment, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _documentRepository = documentRepository;
            _environment = environment;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserDocumentDTO> UploadDocumentAsync(UserDocumentUploadDTO uploadDTO)
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
            var document = new UserDocument
            {
                UserId = uploadDTO.UserId,
                DocumentType = uploadDTO.DocumentType,
                FileName = uploadDTO.File.FileName,
                FilePath = filePath,
                ContentType = uploadDTO.File.ContentType,
                FileSize = uploadDTO.File.Length
            };

            var savedDocument = await _documentRepository.AddAsync(document);
            return _mapper.Map<UserDocumentDTO>(savedDocument);
        }

        public async Task<IEnumerable<UserDocumentDTO>> UploadMultipleDocumentsAsync(UserMultipleDocumentUploadDTO uploadDTO)
        {
            var uploadedDocuments = new List<UserDocument>();

            string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            foreach (var doc in uploadDTO.Documents)
            {
                string uniqueFileName = $"{Guid.NewGuid()}_{doc.File.FileName}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await doc.File.CopyToAsync(stream);
                }

                var document = new UserDocument
                {
                    UserId = uploadDTO.UserId,
                    DocumentType = doc.DocumentType,
                    FileName = doc.File.FileName,
                    FilePath = filePath,
                    ContentType = doc.File.ContentType,
                    FileSize = doc.File.Length
                };

                uploadedDocuments.Add(await _documentRepository.AddAsync(document));
            }

            return _mapper.Map<IEnumerable<UserDocumentDTO>>(uploadedDocuments);
        }

        public async Task<IEnumerable<UserDocumentDTO>> GetUserDocumentsAsync(int userId)
        {
            var documents = await _documentRepository.GetUserDocumentsAsync(userId);
            var mappedDocuments = _mapper.Map<IEnumerable<UserDocumentDTO>>(documents);

            foreach (var doc in mappedDocuments)
            {
                // Convert absolute file path to public URL
                var fileName = Path.GetFileName(doc.FilePath);
                doc.FileUrl = $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}/uploads/{fileName}";
            }

            return mappedDocuments;
        }

        public async Task<bool> DeleteDocumentAsync(int id)
        {
            var document = await _documentRepository.GetByIdAsync(id);
            if (document == null)
                return false;

            // Delete file from storage
            if (File.Exists(document.FilePath))
                File.Delete(document.FilePath);

            return await _documentRepository.DeleteAsync(id);
        }

        public async Task<FileStreamResult?> GetDocumentAsync(int documentId)
        {
            var document = await _documentRepository.GetByIdAsync(documentId);
            if (document == null || !File.Exists(document.FilePath))
                return null;

            var stream = new FileStream(document.FilePath, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(stream, document.ContentType) { FileDownloadName = document.FileName };
        }

        public async Task<UserDocument?> GetByIdAsync(int documentId)
        {
            return await _documentRepository.GetByIdAsync(documentId);
        }
    }
}
