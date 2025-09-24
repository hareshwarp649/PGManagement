using bca.api.Services;
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
        private readonly IUserContextService _userContextService;

        public UnitsController(IUnitService unitService, IUserContextService userContextService )
        {
            _unitService = unitService;
            _userContextService = userContextService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clientId = _userContextService.ClientId; 
            if (clientId == null)
                return BadRequest("ClientId missing in token.");
            var units = await _unitService.GetAllAsync();
            return Ok(units);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var clientId = _userContextService.ClientId;
            if (clientId == null)
                return BadRequest("ClientId missing in token.");

            var unit = await _unitService.GetByIdAsync(id);
            return Ok(unit);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UnitCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = _userContextService.UserId;   
            var clientId = _userContextService.ClientId; 

            if (clientId == null)
                return BadRequest("ClientId missing in token.");

            var result = await _unitService.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UnitUpdateDTO dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = _userContextService.UserId;
            var clientId = _userContextService.ClientId;

            if (clientId == null)
                return BadRequest("ClientId missing in token.");

            var result = await _unitService.UpdateAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _unitService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpGet("property/{propertyId}")]
        public async Task<IActionResult> GetAllByProperty(Guid propertyId)
        {
            var units = await _unitService.GetAllByPropertyAsync(propertyId);
            return Ok(units);
        }
    }
}
