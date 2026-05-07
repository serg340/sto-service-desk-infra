using STO_Desk_backend.Models.DTOs.TicketCategories;

namespace STO_Desk_backend.Services.TicketCategories
{
    public interface ITicketCategoryQueryService
    {
        Task<(bool Success, List<TicketCategoryDto>? TicketCategories, string? ErrorMessage)> GetAllAsync();
        Task<(bool Success, TicketCategoryDto? TicketCategory, string? ErrorMessage)> GetByIdAsync(int id);
    }
}
