using STO_Desk_backend.Models.DTOs.RoleTicketThemes;
using System.Security.Claims;

namespace STO_Desk_backend.Services.RoleTicketThemes
{
    public interface IRoleTicketThemeCommandService
    {
        Task<(bool Success, RoleTicketThemeDto? RoleTicketTheme, string? ErrorMessage)> CreateAsync(RoleTicketThemeCreateDto dto, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, RoleTicketThemeDto? RoleTicketTheme, string? ErrorMessage)> UpdateAsync(int id, RoleTicketThemeUpdateDto dto, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, string? ErrorMessage)> RemoveAsync(int id, ClaimsPrincipal currentUserPrincipal);
    }
}
