using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STO_Desk_backend.Models;
using STO_Desk_backend.Models.DTOs.TicketCategories;
using STO_Desk_backend.Models.Entities;
using STO_Desk_backend.Services.TicketCategories;

namespace STO_Desk_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketCategoriesController : ControllerBase
    {
        private readonly ITicketCategoryCommandService _ticketCategoryCommandService;
        private readonly ITicketCategoryQueryService _ticketCategoryQueryService;

        public TicketCategoriesController(
            ITicketCategoryCommandService ticketCategoryCommandService,
            ITicketCategoryQueryService ticketCategoryQueryService)
        {
            _ticketCategoryCommandService = ticketCategoryCommandService;
            _ticketCategoryQueryService = ticketCategoryQueryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketCategoryDto>>> GetAll()
        {
            var result = await _ticketCategoryQueryService.GetAllAsync();

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return Ok(result.TicketCategories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketCategoryDto>> GetById(int id)
        {
            var result = await _ticketCategoryQueryService.GetByIdAsync(id);

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return Ok(result.TicketCategory);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<TicketCategoryDto>> Create([FromBody] TicketCategoryCreateDto dto)
        {
            var result = await _ticketCategoryCommandService.CreateAsync(dto, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                if (result.ErrorMessage == "Forbid") return Forbid();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return Ok(result.TicketCategory);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var result = await _ticketCategoryCommandService.RemoveAsync(id, User);

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
        public async Task<ActionResult<TicketCategoryDto>> Update(int id, TicketCategoryUpdateDto dto)
        {
            var result = await _ticketCategoryCommandService.UpdateAsync(id, dto, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                if (result.ErrorMessage == "Forbid") return Forbid();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return Ok(result.TicketCategory);
        }
    }
}
