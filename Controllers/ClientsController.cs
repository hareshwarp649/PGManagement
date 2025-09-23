using bca.api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PropertyManage.Domain.DTOs;
using PropertyManage.ServiceInfra.IServices;

namespace PropertyManage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IUserContextService _userContextService;

        public ClientsController(IClientService clientService, IUserContextService userContextService)
        {
            _clientService = clientService;
            _userContextService = userContextService;
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await _clientService.GetAllAsync();
            return Ok(clients);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "SuperAdmin,ClientAdmin")]
        public async Task<IActionResult> GetClientById(Guid id)
        {
            var isSuperAdmin = User.IsInRole("SuperAdmin");
            var requestingClientId = _userContextService.ClientId;

            var client = await _clientService.GetByIdAsync(id, requestingClientId, isSuperAdmin);
            if (client == null) return NotFound();
            return Ok(client);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create([FromBody] ClientCreateDTO dto)
        {
            var created = await _clientService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetClientById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "SuperAdmin,ClientAdmin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ClientUpdateDTO dto)
        {
            var isSuperAdmin = User.IsInRole("SuperAdmin");
            var requestingClientId = _userContextService.ClientId;

            var updated = await _clientService.UpdateAsync(id, dto, isSuperAdmin, requestingClientId);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _clientService.DeleteAsync(id, true);
            if (!success) return NotFound();
            return NoContent();
        }

    }
}
