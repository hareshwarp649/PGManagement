using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PropertyManage.Data.MasterEntities;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentModesController : ControllerBase
    {
        private readonly IPaymentModeService _service;
        public PaymentModesController(IPaymentModeService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var modes = await _service.GetAllAsync();
            return Ok(modes);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var mode = await _service.GetByIdAsync(id);
            if (mode == null) return NotFound();
            return Ok(mode);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentMode mode)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var createdMode = await _service.CreateAsync(mode);
                return CreatedAtAction(nameof(GetById), new { id = createdMode.Id }, createdMode);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PaymentMode mode)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != mode.Id) return BadRequest("ID mismatch");
            try
            {
                var updatedMode = await _service.UpdateAsync(mode);
                return Ok(updatedMode);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
