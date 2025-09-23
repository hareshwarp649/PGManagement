using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PropertyManage.Domain.DTOs;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyService _propertyService;

        public PropertiesController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        private Guid GetCurrentClientId()
        {
            var claim = User.FindFirst("clientId")?.Value;
            if (claim == null)
                throw new UnauthorizedAccessException("ClientId claim missing in token.");
            return Guid.Parse(claim);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clientId = GetCurrentClientId();
            var properties = await _propertyService.GetAllPropertiesAsync(clientId);
            return Ok(properties);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var clientId = GetCurrentClientId();
            var property = await _propertyService.GetPropertyByIdAsync(id, clientId);
            return Ok(property);
        }

        [HttpPost]
        public async Task<IActionResult> Add(PropertyCreateDTO dto)
        {
            var clientId = GetCurrentClientId();
            var result = await _propertyService.AddPropertyAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, PropertyUpdateDTO dto)
        {
            var clientId = GetCurrentClientId();
            var result = await _propertyService.UpdatePropertyAsync(id, dto, clientId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var clientId = GetCurrentClientId();
            var result = await _propertyService.DeletePropertyAsync(id, clientId);
            return Ok(result);
        }
    }
}
