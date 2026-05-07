using Minio.DataModel;
using STO_Desk_backend.Models.DTOs.Users;
using STO_Desk_backend.Models.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace STO_Desk_backend.Services.Users
{
    public interface IUserQueryService
    {
        Task<(bool Success, UserDto? User, string? ErrorMessage)> GetCurrentUserAsync(ClaimsPrincipal principal);
        Task<(bool Success, UserDto? User, string? ErrorMessage)> GetUserByIdAsync(int id, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, List<UserDto>? Users, string? ErrorMessage)> GetAllUsersAsync(ClaimsPrincipal currentUserPrincipal);
    }
}
