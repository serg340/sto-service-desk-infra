using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using STO_Desk_backend.Models;
using STO_Desk_backend.Models.DTOs.Users;
using STO_Desk_backend.Models.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace STO_Desk_backend.Services.Users
{
    public class UserQueryService : IUserQueryService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserQueryService(
            UserManager<User> userManager,
            ApplicationDbContext context,
            IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }

        public async Task<(bool Success, UserDto? User, string? ErrorMessage)> GetCurrentUserAsync(ClaimsPrincipal currentUserPrincipal)
        {
            User? user = await _userManager.GetUserAsync(currentUserPrincipal);
            if (user == null) return (false, null, "Unauthorized");

            UserDto userDto = _mapper.Map<UserDto>(user);
            return (true, userDto, null);
        }

        public async Task<(bool Success, UserDto? User, string? ErrorMessage)> GetUserByIdAsync(int id, ClaimsPrincipal currentUserPrincipal)
        {
            if (!currentUserPrincipal.IsInRole("Admin") && !currentUserPrincipal.IsInRole("Operator"))
            {
                User? currentUser = await _userManager.GetUserAsync(currentUserPrincipal);
                if (currentUser == null) return (false, null, "Unauthorized");

                if (currentUser.Id != id)
                {
                    return (false, null, "Forbid");
                }
            }

            User? user = await _context.Users.FindAsync(id);
            if (user == null) return (false, null, "NotFound");

            UserDto userDto = _mapper.Map<UserDto>(user);
            return (true, userDto, null);
        }

        public async Task<(bool Success, List<UserDto>? Users, string? ErrorMessage)> GetAllUsersAsync(ClaimsPrincipal currentUserPrincipal)
        {
            if (!currentUserPrincipal.IsInRole("Admin") && !currentUserPrincipal.IsInRole("Operator")) return (false, null, "Forbid");

            List<User> users = await _context.Users.ToListAsync();
            List<UserDto> userDtos = _mapper.Map<List<UserDto>>(users);
            return (true, userDtos, null);


        }
    }
}
