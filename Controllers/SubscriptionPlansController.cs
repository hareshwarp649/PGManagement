using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PropertyManage.Domain.DTOs;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class SubscriptionPlansController : ControllerBase
    {
        private readonly ISubscriptionPlanService _subscriptionPlanService;

        public SubscriptionPlansController(ISubscriptionPlanService subscriptionPlanService)
        {
            _subscriptionPlanService = subscriptionPlanService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var plans = await _subscriptionPlanService.GetAllAsync();
            return Ok(plans);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var plan = await _subscriptionPlanService.GetByIdAsync(id);
            if (plan == null) return NotFound();
            return Ok(plan);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SubscriptionPlanCreateDTO dto)
        {
            var created = await _subscriptionPlanService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] SubscriptionPlanUpdateDTO dto)
        {
            var updated = await _subscriptionPlanService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _subscriptionPlanService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
