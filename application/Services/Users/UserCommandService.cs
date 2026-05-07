using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using STO_Desk_backend.Models;
using STO_Desk_backend.Models.Entities;
using System.Security.Claims;
using STO_Desk_backend.Models.DTOs.Users;

namespace STO_Desk_backend.Services.Users
{
    public class UserCommandService : IUserCommandService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserCommandService(
            UserManager<User> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            ApplicationDbContext context,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _mapper = mapper;
        }

        public async Task<(bool Success, UserDto? User, string? ErrorMessage)> UpdateUserAsync(int id, UserUpdateDto dto, ClaimsPrincipal currentUserPrincipal)
        {
            User? currentUser = await _userManager.GetUserAsync(currentUserPrincipal);
            if (currentUser == null) return (false, null, "Unauthorized");

            bool isAdminOrOperator = currentUserPrincipal.IsInRole("Admin") || currentUserPrincipal.IsInRole("Operator");
            
            if (currentUser.Id != id && !isAdminOrOperator)
            {
                return (false, null, "Forbid");
            }

            User? user = await _context.Users.FindAsync(id);
            if (user == null) return (false, null, "NotFound");

            if (!string.IsNullOrWhiteSpace(dto.UserName)) user.UserName = dto.UserName;
            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
            {
                user.PhoneNumber = dto.PhoneNumber;
                user.PhoneNumberConfirmed = false;
            }            
            if (dto.RegionId != null)
            {
                bool regionExists = await _context.Regions.AnyAsync(r => r.Id == dto.RegionId);
                if (!regionExists) return (false, null, "Invalid RegionId");
                
                user.RegionId = dto.RegionId;
            }

            user.UpdatedAt = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            UserDto userDto = _mapper.Map<UserDto>(user);
            return (true, userDto, null);
        }

        public async Task<(bool Success, string? ErrorMessage)> DeleteUserAsync(int targetId, ClaimsPrincipal currentUserPrincipal)
        {
            User? user = await _userManager.GetUserAsync(currentUserPrincipal);
            if (user == null) return (false, "Unauthorized");

            User? targetUser = await _context.Users.FindAsync(targetId);
            if (targetUser == null) return (false, "NotFound");

            bool isAdminOrOperator = currentUserPrincipal.IsInRole("Admin") || currentUserPrincipal.IsInRole("Operator");

            if (user.Id != targetId && !isAdminOrOperator)
            {
                return (false, "Forbid");
            }

            _context.Users.Remove(targetUser);
            await _context.SaveChangesAsync();
            return (true, null);
        }

        public async Task<(bool Success, string? ErrorMessage)> AssignRoleAsync(int userId, string roleName, ClaimsPrincipal currentUserPrincipal)
        {
            User? targetUser = await _context.Users.FindAsync(userId);
            if (targetUser == null) return (false, "NotFound");

            if ((roleName == "Admin" || roleName == "Operator") && !currentUserPrincipal.IsInRole("Admin"))
                return (false, "Forbid");

            if (!await _roleManager.RoleExistsAsync(roleName))
                return (false, "Role does not exist.");

            if (!await _userManager.IsInRoleAsync(targetUser, roleName))
            {
                await _userManager.AddToRoleAsync(targetUser, roleName);
            }

            return (true, null);
        }

        public async Task<(bool Success, string? ErrorMessage)> RemoveRoleAsync(int userId, string roleName, ClaimsPrincipal currentUserPrincipal)
        {
            User? targetUser = await _context.Users.FindAsync(userId);
            if (targetUser == null) return (false, "NotFound");

            if ((roleName == "Admin" || roleName == "Operator") && !currentUserPrincipal.IsInRole("Admin"))
                return (false, "Forbid");

            if (!await _roleManager.RoleExistsAsync(roleName))
                return (false, "Role does not exist.");

            if (await _userManager.IsInRoleAsync(targetUser, roleName))
            {
                await _userManager.RemoveFromRoleAsync(targetUser, roleName);
            }

            return (true, null);
        }
    }
}
