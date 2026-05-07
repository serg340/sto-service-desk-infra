using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STO_Desk_backend.Models.DTOs.RoleTicketCategories;
using STO_Desk_backend.Services.RoleTicketCategories;

namespace STO_Desk_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleTicketCategoriesController : ControllerBase
    {
        private readonly IRoleTicketCategoryCommandService _commandService;
        private readonly IRoleTicketCategoryQueryService _queryService;

        public RoleTicketCategoriesController(
            IRoleTicketCategoryCommandService commandService,
            IRoleTicketCategoryQueryService queryService)
        {
            _commandService = commandService;
            _queryService = queryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleTicketCategoryDto>>> GetAll()
        {
            var result = await _queryService.GetAllAsync();

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return Ok(result.RoleTicketCategories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleTicketCategoryDto>> GetById(int id)
        {
            var result = await _queryService.GetByIdAsync(id);

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return Ok(result.RoleTicketCategory);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<RoleTicketCategoryDto>> Create([FromBody] RoleTicketCategoryCreateDto dto)
        {
            var result = await _commandService.CreateAsync(dto, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                if (result.ErrorMessage == "Forbid") return Forbid();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return CreatedAtAction(nameof(GetById), new { id = result.RoleTicketCategory!.Id }, result.RoleTicketCategory);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var result = await _commandService.RemoveAsync(id, User);

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
        public async Task<ActionResult<RoleTicketCategoryDto>> Update(int id, RoleTicketCategoryUpdateDto dto)
        {
            var result = await _commandService.UpdateAsync(id, dto, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "ID mismatch") return BadRequest(new { message = "ID mismatch" });
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return Ok(result.RoleTicketCategory);
        }
    }
}
