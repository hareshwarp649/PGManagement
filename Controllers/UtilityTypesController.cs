using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilityTypesController : ControllerBase
    {
        private readonly IUtilityTypeService _utilityTypeService;

        public UtilityTypesController(IUtilityTypeService utilityTypeService)
        {
            _utilityTypeService = utilityTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var utilityTypes = await _utilityTypeService.GetAllAsync();
            return Ok(utilityTypes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var utilityType = await _utilityTypeService.GetByIdAsync(id);
            if (utilityType == null)
            {
                return NotFound();
            }
            return Ok(utilityType);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Data.MasterEntities.UtilityType utilityType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var createdUtilityType = await _utilityTypeService.CreateAsync(utilityType);
                return CreatedAtAction(nameof(GetById), new { id = createdUtilityType.Id }, createdUtilityType);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Data.MasterEntities.UtilityType utilityType)
        {
            if (id != utilityType.Id)
            {
                return BadRequest("ID mismatch");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedUtilityType = await _utilityTypeService.UpdateAsync(utilityType);
                if (updatedUtilityType == null)
                {
                    return NotFound();
                }
                return Ok(updatedUtilityType);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _utilityTypeService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
