using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PropertyManage.Data.Entities;
using PropertyManage.Domain.DTOs;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnersController : ControllerBase
    {
        private readonly IOwnerService _ownerService;
        private readonly IMapper _mapper;

        public OwnersController(IOwnerService ownerService, IMapper mapper)
        {
            _ownerService = ownerService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OwnerDetailsDTO>>> GetAll()
        {
            var owners = await _ownerService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<OwnerDetailsDTO>>(owners));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OwnerDetailsDTO>> GetById(Guid id)
        {
            var owner = await _ownerService.GetByIdAsync(id);
            if (owner == null) return NotFound();
            return Ok(_mapper.Map<OwnerDetailsDTO>(owner));
        }

        [HttpPost]
        public async Task<ActionResult<OwnerDetailsDTO>> Create(OwnerInputDTO input)
        {
            var owner = _mapper.Map<Owner>(input);
            var created = await _ownerService.CreateAsync(owner);
            return CreatedAtAction(nameof(GetById), new { id = created!.Id }, _mapper.Map<OwnerDetailsDTO>(created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, OwnerInputDTO input)
        {
            var existing = await _ownerService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            _mapper.Map(input, existing);
            await _ownerService.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _ownerService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpGet("{id}/properties")]
        public async Task<ActionResult<OwnerDetailsDTO>> GetOwnerWithProperties(Guid id)
        {
            var owner = await _ownerService.GetOwnerWithPropertiesAsync(id);
            if (owner == null) return NotFound();
            return Ok(_mapper.Map<OwnerDetailsDTO>(owner));
        }
    }
}
