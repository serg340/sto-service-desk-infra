using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using STO_Desk_backend.Models;
using STO_Desk_backend.Models.DTOs.Tickets;
using STO_Desk_backend.Models.Entities;
using STO_Desk_backend.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace STO_Desk_backend.Services.Tickets
{
    public class TicketCommandService : ITicketCommandService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public TicketCommandService(
            IMapper mapper,
            ApplicationDbContext context,
            UserManager<User> userManager)
        {
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
        }

        public async Task<(bool Success, TicketDto? Ticket, string? ErrorMessage)> AssignTicketAsync(int id, TicketAssignDto dto, ClaimsPrincipal currentUserPrincipal)
        {
            User? user = await _userManager.GetUserAsync(currentUserPrincipal);
            if (user == null) return (false, null, "Unauthorized");

            Ticket? ticket = await _context.Tickets.Include(t => t.Sto).ThenInclude(s => s.Mechanics).FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null) return (false, null, "NotFound");

            bool isStoOwner = ticket.Sto.OwnerId == user.Id;
            bool isStoMechanic = ticket.Sto.Mechanics != null && ticket.Sto.Mechanics.Any(m => m.Id == user.Id);

            if (!isStoOwner && !isStoMechanic)
            {
                return (false, null, "Forbid");
            }

            bool isValidTargetMechanic = ticket.Sto.Mechanics != null && ticket.Sto.Mechanics.Any(m => m.Id == dto.MechanicId);
            if (!isValidTargetMechanic && ticket.Sto.OwnerId != dto.MechanicId)
            {
                return (false, null, "The assigned user is not a mechanic of this STO.");
            }

            if (ticket.Status == TicketStatus.Canceled || ticket.Status == TicketStatus.Completed || ticket.Status == TicketStatus.Rejected)
            {
                return (false, null, "Cannot assign a closed ticket.");
            }

            ticket.MechanicId = dto.MechanicId;
            ticket.Status = TicketStatus.Assigned;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            TicketDto ticketDto = _mapper.Map<TicketDto>(ticket);
            return (true, ticketDto, null);
        }

        public async Task<(bool Success, TicketDto? Ticket, string? ErrorMessage)> CancelTicketAsync(int id, ClaimsPrincipal currentUserPrincipal)
        {
            User? user = await _userManager.GetUserAsync(currentUserPrincipal);
            if (user == null) return (false, null, "Unauthorized");

            Ticket? ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return (false, null, "NotFound");

            if (user.Id != ticket.ClientId)
            {
                return (false, null, "Forbid");
            }

            if (ticket.Status == TicketStatus.Completed || ticket.Status == TicketStatus.Rejected || ticket.Status == TicketStatus.Canceled)
            {
                return (false, null, "Ticket is already closed.");
            }

            ticket.Status = TicketStatus.Canceled;
            ticket.UpdatedAt = DateTime.UtcNow;
            ticket.ClosedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            TicketDto ticketDto = _mapper.Map<TicketDto>(ticket);
            return (true, ticketDto, null);
        }

        public async Task<(bool Success, TicketDto? Ticket, string? ErrorMessage)> CreateTicketAsync(TicketCreateDto dto, ClaimsPrincipal currentUserPrincipal)
        {
            User? user = await _userManager.GetUserAsync(currentUserPrincipal);
            if (user == null) return (false, null, "Unauthorized");

            if (user.Id != dto.ClientId) return (false, null, "ID mismatch");

            Ticket ticket = new Ticket
            {
                Title = dto.Title,
                Body = dto.Body,
                ClientId = dto.ClientId!.Value,
            };

            if (!await _context.TicketThemes.AnyAsync(tt => tt.Id == dto.ThemeId)) return (false, null, "Invalid ThemeId");
            else ticket.ThemeId = dto.ThemeId!.Value;

            if (!await _context.Stos.AnyAsync(s => s.Id == dto.StoId)) return (false, null, "Invalid StoId");
            else ticket.StoId = dto.StoId!.Value;

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            TicketDto ticketDto = _mapper.Map<TicketDto>(ticket);
            return (true, ticketDto, null);
        }

        public async Task<(bool Success, string? ErrorMessage)> DeleteTicketAsync(int targetId, ClaimsPrincipal currentUserPrincipal)
        {
            User? user = await _userManager.GetUserAsync(currentUserPrincipal);
            if (user == null) return (false, "Unauthorized");

            bool isAdminOrOperator = currentUserPrincipal.IsInRole("Admin") || currentUserPrincipal.IsInRole("Operator");

            if (user.Id != targetId && !isAdminOrOperator)
            {
                return (false, "Forbid");
            }

            Ticket? ticket = await _context.Tickets.FindAsync(targetId.ToString());
            if (ticket == null) return (false, "NotFound");

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return (true, null);
        }

        public async Task<(bool Success, TicketDto? Ticket, string? ErrorMessage)> UpdateTicketAsync(int id, TicketUpdateDto dto, ClaimsPrincipal currentUserPrincipal)
        {
            if (id != dto.Id) return (false, null, "ID mismatch");

            User? user = await _userManager.GetUserAsync(currentUserPrincipal);
            if (user == null) return (false, null, "Unauthorized");

            Ticket? ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return (false, null, "NotFound");

            bool isAdminOrOperator = currentUserPrincipal.IsInRole("Admin") || currentUserPrincipal.IsInRole("Operator");

            // only client or Admin/Operator can edit main fields
            if (user.Id != ticket.ClientId && !isAdminOrOperator)
            {
                return (false, null, "Forbid");
            }

            // once the ticket leaves the initial phases, it cannot be edited
            if (ticket.Status != TicketStatus.Pending && ticket.Status != TicketStatus.UnderReview && !isAdminOrOperator)
            {
                return (false, null, "Cannot edit a ticket after it has been assigned or closed.");
            }

            if (!string.IsNullOrWhiteSpace(dto.Title)) ticket.Title = dto.Title;
            if (!string.IsNullOrWhiteSpace(dto.Body)) ticket.Body = dto.Body;

            if (dto.ThemeId != null)
            {
                if (!await _context.TicketThemes.AnyAsync(r => r.Id == dto.ThemeId)) return (false, null, "Invalid ThemeId");
                ticket.ThemeId = dto.ThemeId.Value;
            }

            ticket.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TicketDto ticketDto = _mapper.Map<TicketDto>(ticket);
            return (true, ticketDto, null);
        }

        public async Task<(bool Success, TicketDto? Ticket, string? ErrorMessage)> UpdateTicketStatusAsync(int id, TicketStatusUpdateDto dto, ClaimsPrincipal currentUserPrincipal)
        {
            User? user = await _userManager.GetUserAsync(currentUserPrincipal);
            if (user == null) return (false, null, "Unauthorized");

            Ticket? ticket = await _context.Tickets.Include(t => t.Sto).ThenInclude(s => s.Mechanics).FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null) return (false, null, "NotFound");

            // assigning only via /assign endpoint
            if (dto.Status == TicketStatus.Assigned)
            {
                return (false, null, "Cannot manually set status to Assigned. Please use the /assign endpoint.");
            }

            // canceling only available to client and only via its endpoint
            if (dto.Status == TicketStatus.Canceled)
            {
                return (false, null, "Cannot manually set status to Canceled. Please use the /cancel endpoint.");
            }

            // cannot set to pending as it is the initial status
            if (dto.Status == TicketStatus.Pending)
            {
                return (false, null, "Cannot manually set status to Pending.");
            }

            // ticket should be firstly put UnderReview and only after so called 'review' it can be changed to other statuses
            if (ticket.Status == TicketStatus.Pending && dto.Status != TicketStatus.UnderReview)
            {
                return (false, null, "A Pending ticket can only be transitioned to UnderReview.");
            }

            // cannot change closed ticket
            if (ticket.Status == TicketStatus.Canceled || ticket.Status == TicketStatus.Completed || ticket.Status == TicketStatus.Rejected)
            {
                return (false, null, "Cannot change the status of a closed ticket.");
            }

            if (ticket.Status == TicketStatus.Assigned && dto.Status != TicketStatus.UnderReview && dto.Status != TicketStatus.Completed && dto.Status != TicketStatus.Rejected)
            {
                return (false, null, "An Assigned ticket can only be transitioned to UnderReview, Completed, or Rejected.");
            }

            bool isStoOwner = ticket.Sto.OwnerId == user.Id;

            if (ticket.Status == TicketStatus.Assigned)
            {
                if (ticket.MechanicId != user.Id && !isStoOwner)
                {
                    return (false, null, "Forbid");
                }

                if (dto.Status == TicketStatus.UnderReview)
                {
                    ticket.MechanicId = null;
                }
            }
            else
            {
                bool isStoMechanic = ticket.Sto.Mechanics != null && ticket.Sto.Mechanics.Any(m => m.Id == user.Id);
                if (!isStoOwner && !isStoMechanic)
                {
                    return (false, null, "Forbid");
                }
            }

            ticket.Status = dto.Status!.Value;
            ticket.UpdatedAt = DateTime.UtcNow;

            if (dto.Status == TicketStatus.Canceled || dto.Status == TicketStatus.Completed || dto.Status == TicketStatus.Rejected)
            {
                ticket.ClosedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            TicketDto ticketDto = _mapper.Map<TicketDto>(ticket);
            return (true, ticketDto, null);
        }
    }
}
