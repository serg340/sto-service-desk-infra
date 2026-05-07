using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STO_Desk_backend.Models;
using STO_Desk_backend.Models.DTOs.Stos;
using STO_Desk_backend.Models.DTOs.Users;
using STO_Desk_backend.Models.Entities;
using STO_Desk_backend.Models.Enums;
using STO_Desk_backend.Services.Stos;

namespace STO_Desk_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StosController : ControllerBase
    {
        private readonly IStoCommandService _stoCommandService;
        private readonly IStoQueryService _stoQueryService;

        public StosController(
            IStoCommandService stoCommandService,
            IStoQueryService stoQueryService)
        {
            _stoCommandService = stoCommandService;
            _stoQueryService = stoQueryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoDto>>> GetAll()
        {
            var result = await _stoQueryService.GetAllStosAsync();

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.Stos);
        }

        [HttpGet("region/{id}")]
        public async Task<ActionResult<IEnumerable<StoDto>>> GetByRegionId(int id)
        {
            var result = await _stoQueryService.GetStosByRegionIdAsync(id);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.Stos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StoDto>> GetById(int id)
        {
            var result = await _stoQueryService.GetStoByIdAsync(id);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.Sto);
        }

        [HttpGet("{id}/members")]
        [Authorize(Policy = "StoMemberPolicy")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetMembers(int id)
        {
            var result = await _stoQueryService.GetStoMembersAsync(id);

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.Members);
        }

        [HttpPost]
        [Authorize("StoOwner")]
        public async Task<ActionResult<StoDto>> Create([FromBody] StoCreateDto dto)
        {
            var result = await _stoCommandService.CreateStoAsync(dto, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.Sto);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<StoDto>> Update(int id,[FromBody] StoUpdateDto dto)
        {
            var result = await _stoCommandService.UpdateStoAsync(id, dto, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.Sto);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _stoCommandService.DeleteStoAsync(id, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { message = result.ErrorMessage });
            }

            return NoContent();
        }

        [HttpDelete("{stoId}/mechanics/{mechanicId}")]
        [Authorize]
        public async Task<IActionResult> RemoveMechanic(int stoId, int mechanicId)
        {
            var result = await _stoCommandService.RemoveMechanicFromStoAsync(stoId, mechanicId, User);

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
