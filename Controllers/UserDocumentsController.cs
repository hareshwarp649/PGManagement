using bca.api.DTOs;
using bca.api.Enums;
using bca.api.Helpers;
using bca.api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyManage.Domain.Enums;

namespace bca.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDocumentsController : ControllerBase
    {
        private readonly IUserDocumentService _documentService;
        private readonly IWebHostEnvironment _env;

        public UserDocumentsController(IUserDocumentService documentService, IWebHostEnvironment env)
        {
            _documentService = documentService;
            _env = env;
        }

        //[AllowAnonymous]
        //[HttpGet("document-list")]
        //public ActionResult<IEnumerable<EnumValueDTO>> GetDocumentTypes()
        //{
        //    var excludedValues = new[] { DocumentType.Affidavit, DocumentType.PassbookOrChequePhoto };

        //    var documentTypes = EnumHelper.GetEnumValuesWithDescriptions<DocumentType>()
        //        .Where(dto => !excludedValues.Contains((DocumentType)dto.IntValue))
        //        .ToList();

        //    return Ok(documentTypes);
        //}

        [HttpPost("upload")]
        public async Task<ActionResult<UserDocumentDTO>> Upload([FromForm] UserDocumentUploadDTO uploadDTO)
        {
            var document = await _documentService.UploadDocumentAsync(uploadDTO);
            return CreatedAtAction(nameof(GetByUser), new { userId = document.Id }, document);
        }

        [HttpPost("upload-multiple")]
        public async Task<ActionResult<IEnumerable<UserDocumentDTO>>> UploadMultiple([FromForm] UserMultipleDocumentUploadDTO uploadDTO)
        {
            var documents = await _documentService.UploadMultipleDocumentsAsync(uploadDTO);
            return Ok(documents);
        }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(int id)
        {
            var document = await _documentService.GetDocumentAsync(id);
            if (document == null)
            {
                return NotFound("Document not found.");
            }
            return document;
        }

        [HttpGet("preview/{id}")]
        public async Task<IActionResult> PreviewDocument(int id)
        {
            var document = await _documentService.GetByIdAsync(id);
            var fileName = Path.GetFileName(document.FilePath);
            var filePath = Path.Combine(_env.WebRootPath, "uploads", fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found.");

            var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
            return Ok(new { fileUrl });
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<UserDocumentDTO>>> GetByUser(int userId)
        {
            return Ok(await _documentService.GetUserDocumentsAsync(userId));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _documentService.DeleteDocumentAsync(id) ? NoContent() : NotFound();
        }
    }
}
