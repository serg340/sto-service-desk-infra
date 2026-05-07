using STO_Desk_backend.Models.DTOs.TicketThemes;
using System.Security.Claims;

namespace STO_Desk_backend.Services.TicketThemes
{
    public interface ITicketThemeCommandService
    {
        Task<(bool Success, TicketThemeDto? TicketTheme, string? ErrorMessage)> CreateAsync(TicketThemeCreateDto dto, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, TicketThemeDto? TicketTheme, string? ErrorMessage)> UpdateAsync(int id, TicketThemeUpdateDto dto, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, string? ErrorMessage)> RemoveAsync(int id, ClaimsPrincipal currentUserPrincipal);
    }
}
