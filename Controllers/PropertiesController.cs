using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PropertyManage.Domain.DTOs;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            if (claim == null) throw new UnauthorizedAccessException();
            return Guid.Parse(claim);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> GetAll()
        {
            if (User.IsInRole("SuperAdmin"))
                return Ok(await _propertyService.GetAllPropertiesAsync());

            return Ok(await _propertyService.GetAllPropertiesAsync(GetCurrentClientId()));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _propertyService.GetPropertyByIdAsync(id,
                User.IsInRole("SuperAdmin") ? null : GetCurrentClientId()));
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Add(PropertyCreateDTO dto)
        {
            var result = await _propertyService.AddPropertyAsync(dto, GetCurrentClientId());
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, PropertyUpdateDTO dto)
        {
            var result = await _propertyService.UpdatePropertyAsync(id, dto, GetCurrentClientId(), User.IsInRole("SuperAdmin"));
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _propertyService.DeletePropertyAsync(id, GetCurrentClientId(), User.IsInRole("SuperAdmin"));
            return Ok(result);
        }
    }
}
