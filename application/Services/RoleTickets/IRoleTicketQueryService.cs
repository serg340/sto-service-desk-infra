using STO_Desk_backend.Models.DTOs.RoleTickets;
using STO_Desk_backend.Models.DTOs.Shared;
using System.Security.Claims;

namespace STO_Desk_backend.Services.RoleTickets
{
    public interface IRoleTicketQueryService
    {
        Task<(bool Success, List<RoleTicketDto>? RoleTickets, string? ErrorMessage)> GetAllRoleTicketsAsync(ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, List<RoleTicketDto>? RoleTickets, string? ErrorMessage)> GetRoleTicketsByStoIdAsync(int id, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, List<RoleTicketDto>? RoleTickets, string? ErrorMessage)> GetRoleTicketsByUserIdAsync(int id, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, RoleTicketDto? RoleTicket, string? ErrorMessage)> GetRoleTicketByIdAsync(int id, ClaimsPrincipal currentUserPrincipal);
        List<EnumItemDto> GetStatuses();
        Task<(bool Success, List<EnumItemDto>? Statuses, string? ErrorMessage)> GetAvailableStatusesAsync(int id, ClaimsPrincipal currentUserPrincipal);
    }
}
