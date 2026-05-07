using STO_Desk_backend.Models.DTOs.TicketCategories;
using System.Security.Claims;

namespace STO_Desk_backend.Services.TicketCategories
{
    public interface ITicketCategoryCommandService
    {
        Task<(bool Success, TicketCategoryDto? TicketCategory, string? ErrorMessage)> CreateAsync(TicketCategoryCreateDto dto, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, TicketCategoryDto? TicketCategory, string? ErrorMessage)> UpdateAsync(int id, TicketCategoryUpdateDto dto, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, string? ErrorMessage)> RemoveAsync(int id, ClaimsPrincipal currentUserPrincipal);
    }
}
