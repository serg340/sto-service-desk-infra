using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STO_Desk_backend.Models.DTOs.Regions;
using STO_Desk_backend.Services.Regions;

namespace STO_Desk_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionCommandService _regionCommandService;
        private readonly IRegionQueryService _regionQueryService;

        public RegionsController(
            IRegionCommandService regionCommandService,
            IRegionQueryService regionQueryService
            )
        {
            _regionQueryService = regionQueryService;
            _regionCommandService = regionCommandService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegionDto>>> GetAll()
        {
            var result = await _regionQueryService.GetAllAsync();

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return Ok(result.Regions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RegionDto>> GetById(int id)
        {
            var result = await _regionQueryService.GetByIdAsync(id);

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return Ok(result.Region);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<RegionDto>> Create([FromBody] RegionCreateDto dto)
        {
            var result = await _regionCommandService.CreateAsync(dto);

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return Ok(result.Region);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var result = await _regionCommandService.RemoveAsync(id);

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<RegionDto>> Update(int id, RegionUpdateDto dto)
        {
            var result = await _regionCommandService.UpdateAsync(id, dto);

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return Ok(result.Region);
        }
    }
}
