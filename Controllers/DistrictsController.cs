using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PropertyManage.Data.Entities;
using PropertyManage.Domain.DTOs;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictsController : ControllerBase
    {
        private readonly IDistrictService _districtService;
        private readonly IMapper _mapper;

        public DistrictsController(IDistrictService districtService, IMapper mapper)
        {
            _districtService = districtService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DistrictDetails>>> GetAll()
        {
            var districts = await _districtService.GetAllDistrictsAsync();
            var districtDetailsList = _mapper.Map<IEnumerable<DistrictDetails>>(districts);
            return Ok(districtDetailsList);
        }

        [HttpGet("state/{stateId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DistrictDetails>>> GetByState(Guid stateId)
        {
            var districts = await _districtService.GetDistrictsByStateAsync(stateId);
            var districtDetailsList = _mapper.Map<IEnumerable<DistrictDetails>>(districts);
            return Ok(districtDetailsList);
        }


        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<DistrictDetails>> GetById(Guid id)
        {
            var district = await _districtService.GetDistrictByIdAsync(id);
            if (district == null)
                return NotFound();

            var districtDetails = _mapper.Map<DistrictDetails>(district);
            return Ok(districtDetails);
        }

        [HttpPost]
        public async Task<ActionResult<DistrictDetails>> Create([FromBody] DistrictInput districtInput)
        {
            var district = _mapper.Map<District>(districtInput);
            var createdDistrict = await _districtService.CreateDistrictAsync(district);
            var districtDetails = _mapper.Map<DistrictDetails>(createdDistrict);
            return CreatedAtAction(nameof(GetById), new { id = createdDistrict.Id }, districtDetails);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] DistrictInput districtInput)
        {
            var district = await _districtService.GetDistrictByIdAsync(id);
            if (district == null)
                return NotFound();

            _mapper.Map(districtInput, district);
            var updatedDistrict = await _districtService.UpdateDistrictAsync(district);

            if (updatedDistrict == null)
                return NotFound();

            var districtDetails = _mapper.Map<DistrictDetails>(updatedDistrict);
            return Ok(districtDetails);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await _districtService.DeleteDistrictAsync(id) ? NoContent() : NotFound();
        }

        [HttpGet("state/{stateId}/name/{districtName}")]
        [AllowAnonymous]
        public async Task<ActionResult<DistrictDetails>> GetByStateAndDistrictName(Guid stateId, string districtName)
        {
            var district = await _districtService.GetDistrictByStateAndDistrictNameAsync(stateId, districtName);
            if (district == null)
                return NotFound();
            var districtDetails = _mapper.Map<DistrictDetails>(district);
            return Ok(districtDetails);
        }

    }
}
