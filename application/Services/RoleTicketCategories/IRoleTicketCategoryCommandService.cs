using STO_Desk_backend.Models.DTOs.RoleTicketCategories;
using System.Security.Claims;

namespace STO_Desk_backend.Services.RoleTicketCategories
{
    public interface IRoleTicketCategoryCommandService
    {
        Task<(bool Success, RoleTicketCategoryDto? RoleTicketCategory, string? ErrorMessage)> CreateAsync(RoleTicketCategoryCreateDto dto, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, RoleTicketCategoryDto? RoleTicketCategory, string? ErrorMessage)> UpdateAsync(int id, RoleTicketCategoryUpdateDto dto, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, string? ErrorMessage)> RemoveAsync(int id, ClaimsPrincipal currentUserPrincipal);
    }
}
