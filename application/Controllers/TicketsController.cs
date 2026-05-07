using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STO_Desk_backend.Models;
using STO_Desk_backend.Models.DTOs.Shared;
using STO_Desk_backend.Models.DTOs.Tickets;
using STO_Desk_backend.Models.Entities;
using STO_Desk_backend.Models.Enums;
using STO_Desk_backend.Services.Tickets;
using System.ComponentModel.DataAnnotations;

namespace STO_Desk_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketCommandService _ticketCommandService;
        private readonly ITicketQueryService _ticketQueryService;

        private readonly IMapper _mapper;

        public TicketsController(
            ITicketCommandService ticketCommandService,
            ITicketQueryService ticketQueryService,
            IMapper mapper)
        {
            _ticketCommandService = ticketCommandService;
            _ticketQueryService = ticketQueryService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Operator,Admin")]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetAll()
        {
            var result = await _ticketQueryService.GetAllTicketsAsync(User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            List<TicketDto>? ticketDtos = _mapper.Map<List<TicketDto>?>(result.Tickets);
            return Ok(ticketDtos);
        }

        [HttpGet("sto/{id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetByStoId(int id)
        {
            var result = await _ticketQueryService.GetTicketsByStoIdAsync(id, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            List<TicketDto>? ticketDtos = _mapper.Map<List<TicketDto>?>(result.Tickets);
            return Ok(ticketDtos);
        }

        [HttpGet("user/{id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetByUserId(int id)
        {
            var result = await _ticketQueryService.GetTicketsByUserIdAsync(id, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            List<TicketDto>? ticketDtos = _mapper.Map<List<TicketDto>?>(result.Tickets);
            return Ok(ticketDtos);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<TicketDto>> GetById(int id)
        {
            var result = await _ticketQueryService.GetTicketByIdAsync(id, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            TicketDto? ticketDto = _mapper.Map<TicketDto?>(result.Ticket);
            return Ok(ticketDto);
        }
        
        [HttpGet("statuses")]
        public ActionResult<IEnumerable<EnumItemDto>> GetStatuses()
        {
            List<EnumItemDto> statuses = _ticketQueryService.GetStatusesAsync();
            return Ok(statuses);
        }

        [HttpGet("{id}/statuses/available")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<EnumItemDto>>> GetAvailableStatuses(int id)
        {
            var result = await _ticketQueryService.GetAvailableStatuses(id, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return Ok(result.Statuses);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<TicketDto>> Create([FromBody] TicketCreateDto dto)
        {
            var result = await _ticketCommandService.CreateTicketAsync(dto, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return Ok(result.Ticket);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<TicketDto>> Update(int id, [FromBody] TicketUpdateDto dto)
        {
            var result = await _ticketCommandService.UpdateTicketAsync(id, dto, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return NoContent();
        }

        [HttpPatch("{id}/cancel")]
        [Authorize]
        public async Task<ActionResult<TicketDto>> Cancel(int id)
        {
            var result = await _ticketCommandService.CancelTicketAsync(id, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return NoContent();
        }

        [HttpPatch("{id}/assign")]
        [Authorize]
        public async Task<ActionResult<TicketDto>> Assign(int id, [FromBody] TicketAssignDto dto)
        {
            var result = await _ticketCommandService.AssignTicketAsync(id, dto, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return NoContent();
        }

        [HttpPatch("{id}/status")]
        [Authorize]
        public async Task<ActionResult<TicketDto>> UpdateStatus(int id, [FromBody] TicketStatusUpdateDto dto)
        {
            var result = await _ticketCommandService.UpdateTicketStatusAsync(id, dto, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _ticketCommandService.DeleteTicketAsync(id, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return NoContent();
        }
    }
}
