using STO_Desk_backend.Models.DTOs.RoleTickets;
using System.Security.Claims;

namespace STO_Desk_backend.Services.RoleTickets
{
    public interface IRoleTicketCommandService
    {
        Task<(bool Success, RoleTicketDto? RoleTicket, string? ErrorMessage)> CreateRoleTicketAsync(RoleTicketCreateDto dto, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, RoleTicketDto? RoleTicket, string? ErrorMessage)> UpdateRoleTicketAsync(int id, RoleTicketUpdateDto dto, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, RoleTicketDto? RoleTicket, string? ErrorMessage)> CancelRoleTicketAsync(int id, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, RoleTicketDto? RoleTicket, string? ErrorMessage)> AssignRoleTicketAsync(int id, RoleTicketAssignDto dto, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, RoleTicketDto? RoleTicket, string? ErrorMessage)> UpdateRoleTicketStatusAsync(int id, RoleTicketStatusUpdateDto dto, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, string? ErrorMessage)> DeleteRoleTicketAsync(int id, ClaimsPrincipal currentUserPrincipal);
    }
}
