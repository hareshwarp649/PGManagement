using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PropertyManage.Data.Entities;
using PropertyManage.Domain.DTOs;
using PropertyManage.ServiceInfra.IServices;
using PropertyManage.ServiceInfra.Services;

namespace PropertyManage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly IMapper _mapper;

        public CountriesController(ICountryService countryService, IMapper mapper)
        {
            _countryService = countryService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CountryDetails>>> GetAll()
        {
            var countries = await _countryService.GetAllAsync();
            var result = _mapper.Map<IEnumerable<CountryDetails>>(countries);
            return Ok(result);
        }

        // ✅ Get country by Id
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<CountryDetails>> GetById(Guid id)
        {
            var country = await _countryService.GetByIdAsync(id);
            if (country == null) return NotFound();

            var result = _mapper.Map<CountryDetails>(country);
            return Ok(result);
        }

        // ✅ Get country by Code
        [HttpGet("by-code/{code}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByCode(string code)
        {
            var country = await _countryService.GetByCodeAsync(code);
            if (country == null) return NotFound();

            var result = _mapper.Map<CountryDetails>(country);
            return Ok(result);
        }

        // ✅ Create country
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<CountryDetails>> Create([FromBody] CountryInput dto)
        {
            var country = _mapper.Map<Country>(dto);
            
            var created = await _countryService.CreateAsync(country);

            var result = _mapper.Map<CountryDetails>(created);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // ✅ Update country
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CountryInput dto)
        {
            var existing = await _countryService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            _mapper.Map(dto, existing); // dto → existing object
            var updated = await _countryService.UpdateAsync(existing);

            var result = _mapper.Map<CountryDetails>(updated);
            return Ok(result);
        }

        // ✅ Delete country
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _countryService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }

        // ✅ Get country with states
        [HttpGet("{id:guid}/with-states")]
        [AllowAnonymous]
        public async Task<ActionResult<CountryDetails>> GetCountryWithStates(Guid id)
        {
            var country = await _countryService.GetCountryWithStatesAsync(id);
            if (country == null) return NotFound();

            return Ok(country); 
        }

    }
}
