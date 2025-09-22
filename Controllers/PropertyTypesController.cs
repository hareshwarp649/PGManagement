using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyTypesController : ControllerBase
    {
        private readonly IPropertyTypeService _propertyTypeService;

        public PropertyTypesController(IPropertyTypeService propertyTypeService)
        {
            _propertyTypeService = propertyTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var propertyTypes = await _propertyTypeService.GetAllAsync();
            return Ok(propertyTypes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var propertyType = await _propertyTypeService.GetByIdAsync(id);
            if (propertyType == null)
            {
                return NotFound();
            }
            return Ok(propertyType);
        }

        [HttpGet("exists/{name}")]
        public async Task<IActionResult> ExistsByName(string name)
        {
            var exists = await _propertyTypeService.ExistsByNameAsync(name);
            return Ok(new { exists });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Domain.DTOs.CreatePropertyTypeDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _propertyTypeService.ExistsByNameAsync(dto.TypeName))
            {
                return Conflict(new { message = $"Property Type '{dto.TypeName}' already exists." });
            }
            var createdPropertyType = await _propertyTypeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdPropertyType.Id }, createdPropertyType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Domain.DTOs.PropertyTypeDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("ID mismatch");
            }
            if (await _propertyTypeService.ExistsByNameAsync(dto.TypeName))
            {
                return Conflict(new { message = $"Property Type '{dto.TypeName}' already exists." });
            }
            var updatedPropertyType = await _propertyTypeService.UpdateAsync(dto);
            if (updatedPropertyType == null)
            {
                return NotFound();
            }
            return Ok(updatedPropertyType);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _propertyTypeService.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
