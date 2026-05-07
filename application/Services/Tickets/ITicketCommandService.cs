using Microsoft.AspNetCore.Mvc;
using STO_Desk_backend.Models.DTOs.Tickets;
using STO_Desk_backend.Models.Entities;
using System.Security.Claims;
using System.Threading.Channels;

namespace STO_Desk_backend.Services.Tickets
{
    public interface ITicketCommandService
    {
        Task<(bool Success, TicketDto? Ticket, string? ErrorMessage)> AssignTicketAsync(int id, TicketAssignDto dto, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, TicketDto? Ticket, string? ErrorMessage)> CancelTicketAsync(int id, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, TicketDto? Ticket, string? ErrorMessage)> CreateTicketAsync(TicketCreateDto dto, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, TicketDto? Ticket, string? ErrorMessage)> UpdateTicketAsync(int id, TicketUpdateDto dto, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, TicketDto? Ticket, string? ErrorMessage)> UpdateTicketStatusAsync(int id, TicketStatusUpdateDto dto, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, string? ErrorMessage)> DeleteTicketAsync(int targetId, ClaimsPrincipal currentUserPrincipal);
    }
}
