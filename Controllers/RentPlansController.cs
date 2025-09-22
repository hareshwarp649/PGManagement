using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class RentPlansController : ControllerBase
    {
        private readonly IRentPlanService _rentPlanService;
        public RentPlansController(IRentPlanService rentPlanService)
        {
            _rentPlanService = rentPlanService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rentPlans = await _rentPlanService.GetAllAsync();
            return Ok(rentPlans);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var rentPlan = await _rentPlanService.GetByIdAsync(id);
            if (rentPlan == null)
                return NotFound();
            return Ok(rentPlan);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Data.MasterEntities.RentPlan rentPlan)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var createdRentPlan = await _rentPlanService.CreateAsync(rentPlan);
                return CreatedAtAction(nameof(GetById), new { id = createdRentPlan.Id }, createdRentPlan);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Data.MasterEntities.RentPlan rentPlan)
        {
            if (id != rentPlan.Id)
                return BadRequest("ID mismatch");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var updatedRentPlan = await _rentPlanService.UpdateAsync(rentPlan);
                if (updatedRentPlan == null)
                    return NotFound();
                return Ok(updatedRentPlan);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _rentPlanService.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
