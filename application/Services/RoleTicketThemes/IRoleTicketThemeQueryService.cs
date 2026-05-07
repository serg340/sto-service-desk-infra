using STO_Desk_backend.Models.DTOs.RoleTicketThemes;
using STO_Desk_backend.Models.DTOs.Shared;
using System.Security.Claims;

namespace STO_Desk_backend.Services.RoleTicketThemes
{
    public interface IRoleTicketThemeQueryService
    {
        Task<(bool Success, List<RoleTicketThemeDto>? RoleTicketThemes, string? ErrorMessage)> GetAllAsync();
        Task<(bool Success, List<RoleTicketThemeDto>? RoleTicketThemes, string? ErrorMessage)> GetByCategoryIdAsync(int categoryId);
        Task<(bool Success, RoleTicketThemeDto? RoleTicketTheme, string? ErrorMessage)> GetByIdAsync(int id);
        (bool Success, List<EnumItemDto>? Roles, string? ErrorMessage) GetTargetRoles(ClaimsPrincipal currentUserPrincipal);
    }
}
