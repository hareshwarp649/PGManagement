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
    public class ClientSubscriptionsController : ControllerBase
    {
        private readonly IClientSubscriptionService _clientSubscriptionService;
        private readonly IUserContextService _userContextService;
        public ClientSubscriptionsController(IClientSubscriptionService clientSubscriptionService, IUserContextService userContextService)
        {
            _clientSubscriptionService = clientSubscriptionService;
            _userContextService = userContextService;
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateClientSubscriptionDTO dto)
        {
            var created = await _clientSubscriptionService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin,Client")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClientSubscriptionDTO dto)
        {
            var result = await _clientSubscriptionService.UpdateAsync(id, dto);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var subscription = await _clientSubscriptionService.GetClientSubscriptionByIdAsync(id);
            return Ok(subscription);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var subscriptions = await _clientSubscriptionService.GetAllClientSubscriptionsAsync();
            return Ok(subscriptions);
        }
    }
}
