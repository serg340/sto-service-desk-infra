using STO_Desk_backend.Models.DTOs.Users;
using STO_Desk_backend.Models.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace STO_Desk_backend.Services.Users
{
    public interface IUserCommandService
    {
        Task<(bool Success, UserDto? User, string? ErrorMessage)> UpdateUserAsync(int id, UserUpdateDto dto, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, string? ErrorMessage)> DeleteUserAsync(int targetId, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, string? ErrorMessage)> AssignRoleAsync(int userId, string roleName, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, string? ErrorMessage)> RemoveRoleAsync(int userId, string roleName, ClaimsPrincipal currentUserPrincipal);
    }
}
