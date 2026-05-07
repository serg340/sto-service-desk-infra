using STO_Desk_backend.Models.DTOs.TicketThemes;

namespace STO_Desk_backend.Services.TicketThemes
{
    public interface ITicketThemeQueryService
    {
        Task<(bool Success, List<TicketThemeDto>? TicketThemes, string? ErrorMessage)> GetAllAsync();
        Task<(bool Success, List<TicketThemeDto>? TicketThemes, string? ErrorMessage)> GetByCategoryIdAsync(int id);
        Task<(bool Success, TicketThemeDto? TicketTheme, string? ErrorMessage)> GetByIdAsync(int id);
    }
}
