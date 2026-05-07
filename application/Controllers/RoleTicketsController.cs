using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STO_Desk_backend.Models.DTOs.RoleTickets;
using STO_Desk_backend.Models.DTOs.Shared;
using STO_Desk_backend.Services.RoleTickets;

namespace STO_Desk_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleTicketsController : ControllerBase
    {
        private readonly IRoleTicketCommandService _roleTicketCommandService;
        private readonly IRoleTicketQueryService _roleTicketQueryService;

        public RoleTicketsController(
            IRoleTicketCommandService roleTicketCommandService,
            IRoleTicketQueryService roleTicketQueryService)
        {
            _roleTicketCommandService = roleTicketCommandService;
            _roleTicketQueryService = roleTicketQueryService;
        }

        [HttpGet]
        [Authorize(Roles = "Operator,Admin")]
        public async Task<ActionResult<IEnumerable<RoleTicketDto>>> GetAll()
        {
            var result = await _roleTicketQueryService.GetAllRoleTicketsAsync(User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.RoleTickets);
        }

        [HttpGet("sto/{id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<RoleTicketDto>>> GetByStoId(int id)
        {
            var result = await _roleTicketQueryService.GetRoleTicketsByStoIdAsync(id, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.RoleTickets);
        }

        [HttpGet("user/{id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<RoleTicketDto>>> GetByUserId(int id)
        {
            var result = await _roleTicketQueryService.GetRoleTicketsByUserIdAsync(id, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.RoleTickets);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<RoleTicketDto>> GetById(int id)
        {
            var result = await _roleTicketQueryService.GetRoleTicketByIdAsync(id, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.RoleTicket);
        }

        [HttpGet("statuses")]
        public ActionResult<IEnumerable<EnumItemDto>> GetStatuses()
        {
            var statuses = _roleTicketQueryService.GetStatuses();
            return Ok(statuses);
        }

        [HttpGet("{id}/statuses/available")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<EnumItemDto>>> GetAvailableStatuses(int id)
        {
            var result = await _roleTicketQueryService.GetAvailableStatusesAsync(id, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.Statuses);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<RoleTicketDto>> Create([FromBody] RoleTicketCreateDto dto)
        {
            var result = await _roleTicketCommandService.CreateRoleTicketAsync(dto, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.RoleTicket);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<RoleTicketDto>> Update(int id, [FromBody] RoleTicketUpdateDto dto)
        {
            var result = await _roleTicketCommandService.UpdateRoleTicketAsync(id, dto, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.RoleTicket);
        }

        [HttpPatch("{id}/cancel")]
        [Authorize]
        public async Task<ActionResult<RoleTicketDto>> Cancel(int id)
        {
            var result = await _roleTicketCommandService.CancelRoleTicketAsync(id, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.RoleTicket);
        }

        [HttpPatch("{id}/assign")]
        [Authorize(Roles = "Operator,Admin")]
        public async Task<ActionResult<RoleTicketDto>> Assign(int id, [FromBody] RoleTicketAssignDto dto)
        {
            var result = await _roleTicketCommandService.AssignRoleTicketAsync(id, dto, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.RoleTicket);
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Operator,Admin")]
        public async Task<ActionResult<RoleTicketDto>> UpdateStatus(int id, [FromBody] RoleTicketStatusUpdateDto dto)
        {
            var result = await _roleTicketCommandService.UpdateRoleTicketStatusAsync(id, dto, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.RoleTicket);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _roleTicketCommandService.DeleteRoleTicketAsync(id, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { message = result.ErrorMessage });
            }

            return NoContent();
        }
    }
}
