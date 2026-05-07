using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using STO_Desk_backend.Models;
using STO_Desk_backend.Models.DTOs.Shared;
using STO_Desk_backend.Models.DTOs.Tickets;
using STO_Desk_backend.Models.Entities;
using STO_Desk_backend.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace STO_Desk_backend.Services.Tickets
{
    public class TicketQueryService : ITicketQueryService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public TicketQueryService(
            IMapper mapper,
            ApplicationDbContext context,
            UserManager<User> userManager)
        {
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
        }

        public async Task<(bool Success, List<Ticket>? Tickets, string? ErrorMessage)> GetAllTicketsAsync(ClaimsPrincipal currentUserPrincipal)
        {
            User? user = await _userManager.GetUserAsync(currentUserPrincipal);
            if (user == null) return (false, null, "Unauthorized");

            bool isAdminOrOperator = currentUserPrincipal.IsInRole("Admin") || currentUserPrincipal.IsInRole("Operator");

            if (!isAdminOrOperator)
            {
                return (false, null, "Forbid");
            }

            List<Ticket> tickets = await _context.Tickets.ToListAsync();

            return (true, tickets, null);
        }

        public async Task<(bool Success, List<EnumItemDto>? Statuses, string? ErrorMessage)> GetAvailableStatuses(int id, ClaimsPrincipal currentUserPrincipal)
        {
            User? user = await _userManager.GetUserAsync(currentUserPrincipal);
            if (user == null) return (false, null, "Unauthorized");

            Ticket? ticket = await _context.Tickets.Include(t => t.Sto).ThenInclude(s => s.Mechanics).FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null) return (false, null, "NotFound");

            // statuses can only be knoawn by Admins/Operators Sto Owner and the assigned Mechanic
            bool isAdminOrOperator = currentUserPrincipal.IsInRole("Admin") || currentUserPrincipal.IsInRole("Operator");
            bool isStoOwner = ticket.Sto.OwnerId == user.Id;
            // other mechanics of that sto currently cannot access available statuses and do not particepate in ticket's work process at all
            bool isTicketMechanic = ticket.MechanicId == user.Id;

            if (!isAdminOrOperator && !isStoOwner && !isTicketMechanic && ticket.ClientId != user.Id)
            {
                return (false, null, "Forbid");
            }

            List<TicketStatus> allowedStatuses = new List<TicketStatus>();

            if (ticket.Status == TicketStatus.Pending)
            {
                allowedStatuses.Add(TicketStatus.UnderReview);
            }
            else if (ticket.Status == TicketStatus.UnderReview)
            {
                allowedStatuses.AddRange(new[] { TicketStatus.Completed, TicketStatus.Rejected });
            }
            else if (ticket.Status == TicketStatus.Assigned)
            {
                allowedStatuses.AddRange(new[] { TicketStatus.UnderReview, TicketStatus.Completed, TicketStatus.Rejected });
            }

            var statuses = allowedStatuses
                .Select(e => new EnumItemDto
                {
                    Id = (int)e,
                    Name = e.ToString(),
                    DisplayName = e.GetType()
                        .GetMember(e.ToString())
                        .First()
                        .GetCustomAttributes(typeof(DisplayAttribute), false)
                        .Cast<DisplayAttribute>()
                        .FirstOrDefault()?.Name ?? e.ToString()
                }).ToList();
            return (true, statuses, null);
        }

        public List<EnumItemDto> GetStatusesAsync()
        {
            List<EnumItemDto> statuses = Enum.GetValues(typeof(TicketStatus))
                .Cast<TicketStatus>()
                .Select(e => new EnumItemDto
                {
                    Id = (int)e,
                    Name = e.ToString(),
                    DisplayName = e.GetType()
                        .GetMember(e.ToString())
                        .First()
                        .GetCustomAttributes(typeof(DisplayAttribute), false)
                        .Cast<DisplayAttribute>()
                        .FirstOrDefault()?.Name ?? e.ToString()
                }).ToList();
            return statuses;
        }

        public async Task<(bool Success, Ticket? Ticket, string? ErrorMessage)> GetTicketByIdAsync(int id, ClaimsPrincipal currentUserPrincipal)
        {
            User? user = await _userManager.GetUserAsync(currentUserPrincipal);
            if (user == null) return (false, null, "Unauthorized");

            Ticket? ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return (false, null, "NotFound");

            bool isAdminOrOperator = currentUserPrincipal.IsInRole("Admin") || currentUserPrincipal.IsInRole("Operator");

            if (!isAdminOrOperator)
            {
                if (user.Id != ticket.ClientId)
                {
                    bool isStoOwner = ticket.Sto.OwnerId == user.Id;
                    bool isStoMechanic = ticket.Sto.Mechanics != null && ticket.Sto.Mechanics.Any(m => m.Id == user.Id);

                    if (!isStoOwner && !isStoMechanic)
                    {
                        return (false, null, "Forbid");
                    }
                }
            }

            return (true, ticket, null);
        }

        public async Task<(bool Success, List<Ticket>? Tickets, string? ErrorMessage)> GetTicketsByStoIdAsync(int id, ClaimsPrincipal currentUserPrincipal)
        {
            User? user = await _userManager.GetUserAsync(currentUserPrincipal);
            if (user == null) return (false, null, "Unauthorized");

            Sto? sto = await _context.Stos.Include(s => s.Mechanics).FirstOrDefaultAsync(s => s.Id == id);
            if (sto == null) return (false, null, "NotFound");

            bool isAdminOrOperator = currentUserPrincipal.IsInRole("Admin") || currentUserPrincipal.IsInRole("Operator");

            // check if user is either one of the sto's mechanics or its owner
            bool isRelatedToStoMechanicOrStoOwner = sto.Mechanics != null && sto.Mechanics.Any(m => m.Id == user.Id) || sto.OwnerId == user.Id;

            if (!isRelatedToStoMechanicOrStoOwner && !isAdminOrOperator)
            {
                return (false, null, "Forbid");
            }

            List<Ticket> tickets = await _context.Tickets
                .Where(t => t.StoId == id)
                .ToListAsync();

            List<TicketDto> ticketDtos = _mapper.Map<List<TicketDto>>(tickets);
            return (true, tickets, null);
        }

        public async Task<(bool Success, List<Ticket>? Tickets, string? ErrorMessage)> GetTicketsByUserIdAsync(int id, ClaimsPrincipal currentUserPrincipal)
        {
            User? user = await _userManager.GetUserAsync(currentUserPrincipal);
            if (user == null) return (false, null, "Unauthorized");

            User? targetUser = await _context.Users.FindAsync(id);
            if (targetUser == null) return (false, null, "NotFound");

            bool isAdminOrOperator = currentUserPrincipal.IsInRole("Admin") || currentUserPrincipal.IsInRole("Operator");

            // if user is Operator/Admin he can get tickets of any user..
            if (!isAdminOrOperator)
            {
                // ..if he is not he can only get his tickets
                if (user.Id != id) return (false, null, "Forbid");
            }

            List<Ticket> tickets = await _context.Tickets
                .Where(t => t.ClientId == id)
                .ToListAsync();

            // List<TicketDto> ticketDtos = _mapper.Map<List<TicketDto>>(tickets);
            return (true, tickets, null);
        }
    }
}
