using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PropertyManage.Domain.DTOs;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitsController : ControllerBase
    {
        private readonly IUnitService _unitService;

        public UnitsController(IUnitService unitService)
        {
            _unitService = unitService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UnitCreateDTO dto)
        {
            var userId = Guid.Parse(User.FindFirst("id").Value);
            var result = await _unitService.CreateAsync(dto, userId);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UnitUpdateDTO dto)
        {
            var userId = Guid.Parse(User.FindFirst("id").Value);
            var result = await _unitService.UpdateAsync(id, dto, userId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _unitService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var unit = await _unitService.GetByIdAsync(id);
            return Ok(unit);
        }

        [HttpGet("property/{propertyId}")]
        public async Task<IActionResult> GetAllByProperty(Guid propertyId)
        {
            var units = await _unitService.GetAllByPropertyAsync(propertyId);
            return Ok(units);
        }
    }
}
