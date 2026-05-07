using STO_Desk_backend.Models.DTOs.RoleTicketCategories;

namespace STO_Desk_backend.Services.RoleTicketCategories
{
    public interface IRoleTicketCategoryQueryService
    {
        Task<(bool Success, List<RoleTicketCategoryDto>? RoleTicketCategories, string? ErrorMessage)> GetAllAsync();
        Task<(bool Success, RoleTicketCategoryDto? RoleTicketCategory, string? ErrorMessage)> GetByIdAsync(int id);
    }
}
