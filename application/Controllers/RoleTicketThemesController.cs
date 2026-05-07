using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STO_Desk_backend.Models.DTOs.RoleTicketThemes;
using STO_Desk_backend.Models.DTOs.Shared;
using STO_Desk_backend.Services.RoleTicketThemes;

namespace STO_Desk_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleTicketThemesController : ControllerBase
    {
        private readonly IRoleTicketThemeCommandService _roleTicketThemeCommandService;
        private readonly IRoleTicketThemeQueryService _roleTicketThemeQueryService;

        public RoleTicketThemesController(
            IRoleTicketThemeCommandService roleTicketThemeCommandService,
            IRoleTicketThemeQueryService roleTicketThemeQueryService)
        {
            _roleTicketThemeCommandService = roleTicketThemeCommandService;
            _roleTicketThemeQueryService = roleTicketThemeQueryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleTicketThemeDto>>> GetAll()
        {
            var result = await _roleTicketThemeQueryService.GetAllAsync();

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return Ok(result.RoleTicketThemes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleTicketThemeDto>> GetById(int id)
        {
            var result = await _roleTicketThemeQueryService.GetByIdAsync(id);

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return Ok(result.RoleTicketTheme);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<RoleTicketThemeDto>>> GetByCategoryId(int categoryId)
        {
            var result = await _roleTicketThemeQueryService.GetByCategoryIdAsync(categoryId);

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return Ok(result.RoleTicketThemes);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("targetroles")]
        public ActionResult<IEnumerable<EnumItemDto>> GetTargetRoles()
        {
            var result = _roleTicketThemeQueryService.GetTargetRoles(User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                if (result.ErrorMessage == "Forbid") return Forbid();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return Ok(result.Roles);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<RoleTicketThemeDto>> Create([FromBody] RoleTicketThemeCreateDto dto)
        {
            var result = await _roleTicketThemeCommandService.CreateAsync(dto, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                if (result.ErrorMessage == "Forbid") return Forbid();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return CreatedAtAction(nameof(GetById), new { id = result.RoleTicketTheme!.Id }, result.RoleTicketTheme);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var result = await _roleTicketThemeCommandService.RemoveAsync(id, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                if (result.ErrorMessage == "Forbid") return Forbid();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<RoleTicketThemeDto>> Update(int id, RoleTicketThemeUpdateDto dto)
        {
            var result = await _roleTicketThemeCommandService.UpdateAsync(id, dto, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "ID mismatch") return BadRequest(new { message = "ID mismatch" });
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return Ok(result.RoleTicketTheme);
        }
    }
}
