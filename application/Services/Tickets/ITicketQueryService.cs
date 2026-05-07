using Microsoft.AspNetCore.Mvc;
using STO_Desk_backend.Models.DTOs.Shared;
using STO_Desk_backend.Models.Entities;
using System.Security.Claims;
using System.Threading.Channels;

namespace STO_Desk_backend.Services.Tickets
{
    public interface ITicketQueryService
    {
        Task<(bool Success, Ticket? Ticket, string? ErrorMessage)> GetTicketByIdAsync(int id, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, List<Ticket>? Tickets, string? ErrorMessage)> GetTicketsByUserIdAsync(int id, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, List<Ticket>? Tickets, string? ErrorMessage)> GetTicketsByStoIdAsync(int id, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, List<Ticket>? Tickets, string? ErrorMessage)> GetAllTicketsAsync(ClaimsPrincipal currentUserPrincipal);
        List<EnumItemDto> GetStatusesAsync();
        Task<(bool Success, List<EnumItemDto>? Statuses, string? ErrorMessage)> GetAvailableStatuses(int id, ClaimsPrincipal currentUserPrincipal);
    }
}
