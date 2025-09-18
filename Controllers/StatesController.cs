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
    public class StatesController : ControllerBase
    {
        private readonly IStateService _stateService;
        private readonly IMapper _mapper;

        public StatesController(IStateService stateService, IMapper mapper)
        {
            _stateService = stateService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<StateDetails>>> GetAll()
        {
            var states = await _stateService.GetAllStatesAsync();
            var stateDetailsList = _mapper.Map<IEnumerable<StateDetails>>(states);
            return Ok(stateDetailsList);
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<StateDetails>> GetById(Guid id)
        {
            var state = await _stateService.GetStateByIdAsync(id);
            if (state == null)
                return NotFound();

            var stateDetails = _mapper.Map<StateDetails>(state);
            return Ok(stateDetails);
        }

        [HttpGet("district/{districtId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<StateDetails>>> GetStatesByDistrictId(Guid districtId)
        {
            var states = await _stateService.GetStateByDistrictIdAsync(districtId);
            var stateDetailsList = _mapper.Map<IEnumerable<StateDetails>>(states);
            return Ok(stateDetailsList);
        }


        [HttpPost]
        public async Task<ActionResult<StateDetails>> Create([FromBody] StateInput stateInput)
        {
            var state = _mapper.Map<State>(stateInput);
            var createdState = await _stateService.AddStateAsync(state);

            var stateDetails = _mapper.Map<StateDetails>(createdState);
            return CreatedAtAction(nameof(GetById), new { id = createdState.Id }, stateDetails);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] StateInput stateInput)
        {
            var state = await _stateService.GetStateByIdAsync(id);
            if (state == null)
                return NotFound();

            _mapper.Map(stateInput, state);
            var updatedState = await _stateService.UpdateStateAsync(state);

            if (updatedState == null)
                return NotFound();

            var stateDetails = _mapper.Map<StateDetails>(updatedState);
            return Ok(stateDetails);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await _stateService.DeleteStateAsync(id) ? NoContent() : NotFound();
        }
    }
}
