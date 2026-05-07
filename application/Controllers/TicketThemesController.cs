using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STO_Desk_backend.Models;
using STO_Desk_backend.Models.DTOs.TicketThemes;
using STO_Desk_backend.Models.Entities;
using STO_Desk_backend.Services.TicketThemes;

namespace STO_Desk_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketThemesController : ControllerBase
    {
        private readonly ITicketThemeCommandService _ticketThemeCommandService;
        private readonly ITicketThemeQueryService _ticketThemeQueryService;

        public TicketThemesController(
            ITicketThemeCommandService ticketThemeCommandService,
            ITicketThemeQueryService ticketThemeQueryService)
        {
            _ticketThemeCommandService = ticketThemeCommandService;
            _ticketThemeQueryService = ticketThemeQueryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketThemeDto>>> GetAll()
        {
            var result = await _ticketThemeQueryService.GetAllAsync();

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return Ok(result.TicketThemes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketThemeDto>> GetById(int id)
        {
            var result = await _ticketThemeQueryService.GetByIdAsync(id);

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return Ok(result.TicketTheme);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<TicketThemeDto>>> GetByCategoryId(int categoryId)
        {
            var result = await _ticketThemeQueryService.GetByCategoryIdAsync(categoryId);

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return Ok(result.TicketThemes);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<TicketThemeDto>> Create([FromBody] TicketThemeCreateDto dto)
        {
            var result = await _ticketThemeCommandService.CreateAsync(dto, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                if (result.ErrorMessage == "Forbid") return Forbid();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return Ok(result.TicketTheme);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var result = await _ticketThemeCommandService.RemoveAsync(id, User);

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
        public async Task<ActionResult<TicketThemeDto>> Update(int id, TicketThemeUpdateDto dto)
        {
            var result = await _ticketThemeCommandService.UpdateAsync(id, dto, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                if (result.ErrorMessage == "Forbid") return Forbid();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return Ok(result.TicketTheme);
        }
    }
}
