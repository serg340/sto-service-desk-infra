using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STO_Desk_backend.Models.DTOs.Auth;
using STO_Desk_backend.Models.DTOs.Users;
using STO_Desk_backend.Models.Entities;
using STO_Desk_backend.Services.Auth;
using STO_Desk_backend.Services.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace STO_Desk_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserCommandService _userCommandService;
        private readonly IUserQueryService _userQueryService;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public UsersController(
            IUserCommandService userCommandService,
            IUserQueryService userQueryService,
            IAuthService authService,
            IMapper mapper)
        {
            _userCommandService = userCommandService;
            _userQueryService = userQueryService;
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpDto dto)
        {
            var result = await _authService.SignUpAsync(dto);

            if (!result.Success)
            {
                if (result.Errors != null)
                    return BadRequest(result.Errors);
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(new { token = result.Token, email = result.Email });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn(LogInDto dto)
        {
            var result = await _authService.LogInAsync(dto);

            if (!result.Success)
                return Unauthorized(new { message = result.ErrorMessage });

            return Ok(new { token = result.Token, email = result.Email });
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, UserUpdateDto dto)
        {
            var result = await _userCommandService.UpdateUserAsync(id, dto, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.User);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var result = await _userQueryService.GetCurrentUserAsync(User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.User);
        }

        [HttpGet("users")]
        [Authorize(Roles = "Operator,Admin")]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            var result = await _userQueryService.GetAllUsersAsync(User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.Users);
        }

        [HttpGet("user/{id}")]
        [Authorize(Roles = "Operator,Admin")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var result = await _userQueryService.GetUserByIdAsync(id, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.User);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userCommandService.DeleteUserAsync(id, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Unauthorized") return Unauthorized();
                if (result.ErrorMessage == "Forbid") return Forbid();
                if (result.ErrorMessage == "NotFound") return NotFound();
                return BadRequest(new { message = result.ErrorMessage });
            }

            return NoContent();
        }

        [HttpPost("{id}/roles")]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> AssignRole(int id, [FromBody] AssignRoleDto dto)
        {
            var result = await _userCommandService.AssignRoleAsync(id, dto.RoleName, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                if (result.ErrorMessage == "Forbid") return Forbid();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return Ok();
        }

        [HttpDelete("{id}/roles")]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> RemoveRole(int id, [FromBody] RemoveRoleDto dto)
        {
            var result = await _userCommandService.RemoveRoleAsync(id, dto.RoleName, User);

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return NotFound();
                if (result.ErrorMessage == "Forbid") return Forbid();
                return BadRequest(new { msg = result.ErrorMessage });
            }

            return NoContent();
        }
    }
}
