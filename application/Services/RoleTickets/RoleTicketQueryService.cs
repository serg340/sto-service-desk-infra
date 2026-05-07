using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using STO_Desk_backend.Models;
using STO_Desk_backend.Models.DTOs.RoleTickets;
using STO_Desk_backend.Models.DTOs.Shared;
using STO_Desk_backend.Models.Entities;
using STO_Desk_backend.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace STO_Desk_backend.Services.RoleTickets
{
    public class RoleTicketQueryService : IRoleTicketQueryService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public RoleTicketQueryService(
            ApplicationDbContext context,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<(bool Success, List<RoleTicketDto>? RoleTickets, string? ErrorMessage)> GetAllRoleTicketsAsync(ClaimsPrincipal currentUserPrincipal)
        {
            List<RoleTicket> roleTickets = await _context.RoleTickets.ToListAsync();

            List<RoleTicketDto> roleTicketDtos = _mapper.Map<List<RoleTicketDto>>(roleTickets);
            return (true, roleTicketDtos, null);
        }

        public async Task<(bool Success, List<RoleTicketDto>? RoleTickets, string? ErrorMessage)> GetRoleTicketsByStoIdAsync(int id, ClaimsPrincipal currentUserPrincipal)
        {
            User? user = await _userManager.GetUserAsync(currentUserPrincipal);
            if (user == null) return (false, null, "Unauthorized");

            Sto? sto = await _context.Stos.FindAsync(id);
            if (sto == null) return (false, null, "NotFound");

            bool isAdminOrOperator = currentUserPrincipal.IsInRole("Admin") || currentUserPrincipal.IsInRole("Operator");

            // check if user is sto's owner
            bool isStoOwner = sto.OwnerId == user.Id;

            if (!isStoOwner && !isAdminOrOperator)
            {
                return (false, null, "Forbid");
            }

            List<RoleTicket> tickets = await _context.RoleTickets
                .Where(rt => rt.StoId == id)
                .ToListAsync();

            List<RoleTicketDto> ticketDtos = _mapper.Map<List<RoleTicketDto>>(tickets);
            return (true, ticketDtos, null);
        }

        public async Task<(bool Success, List<RoleTicketDto>? RoleTickets, string? ErrorMessage)> GetRoleTicketsByUserIdAsync(int id, ClaimsPrincipal currentUserPrincipal)
        {
            User? user = await _userManager.GetUserAsync(currentUserPrincipal);
            if (user == null) return (false, null, "Unauthorized");

            bool isAdminOrOperator = currentUserPrincipal.IsInRole("Admin") || currentUserPrincipal.IsInRole("Operator");

            // if user is Operator/Admin he can get tickets of any user..
            if (!isAdminOrOperator)
            {
                // ..if he is not he can only get his tickets
                if (user.Id != id) return (false, null, "Forbid");
            }

            List<RoleTicket> roleTickets = await _context.RoleTickets
                .Where(rt => rt.UserId == id)
                .ToListAsync();

            List<RoleTicketDto> roleTicketDtos = _mapper.Map<List<RoleTicketDto>>(roleTickets);
            return (true, roleTicketDtos, null);
        }

        public async Task<(bool Success, RoleTicketDto? RoleTicket, string? ErrorMessage)> GetRoleTicketByIdAsync(int id, ClaimsPrincipal currentUserPrincipal)
        {
            User? user = await _userManager.GetUserAsync(currentUserPrincipal);
            if (user == null) return (false, null, "Unauthorized");

            RoleTicket? roleTicket = await _context.RoleTickets.FindAsync(id);
            if (roleTicket == null) return (false, null, "NotFound");

            bool isAdminOrOperator = currentUserPrincipal.IsInRole("Admin") || currentUserPrincipal.IsInRole("Operator");

            if (!isAdminOrOperator)
            {
                if (user.Id != roleTicket.UserId)
                {
                    bool isStoOwner = roleTicket.Sto != null && roleTicket.Sto.OwnerId == user.Id;
                    if (!isStoOwner)
                    {
                        return (false, null, "Forbid");
                    }
                }
            }

            RoleTicketDto ticketDto = _mapper.Map<RoleTicketDto>(roleTicket);
            return (true, ticketDto, null);
        }

        public List<EnumItemDto> GetStatuses()
        {
            var statuses = Enum.GetValues(typeof(TicketStatus))
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

        public async Task<(bool Success, List<EnumItemDto>? Statuses, string? ErrorMessage)> GetAvailableStatusesAsync(int id, ClaimsPrincipal currentUserPrincipal)
        {
            User? user = await _userManager.GetUserAsync(currentUserPrincipal);
            if (user == null) return (false, null, "Unauthorized");

            RoleTicket? roleTicket = await _context.RoleTickets.Include(t => t.Sto).ThenInclude(s => s.Mechanics).FirstOrDefaultAsync(t => t.Id == id);
            if (roleTicket == null) return (false, null, "NotFound");

            bool isAdmin = currentUserPrincipal.IsInRole("Admin");
            // other operators cannot access available statuses and do not particepate in roleticket's review process
            bool isTicketReviewer = roleTicket.ReviewerId == user.Id;

            if (!isAdmin && !isTicketReviewer && roleTicket.UserId != user.Id)
            {
                return (false, null, "Forbid");
            }

            List<TicketStatus> allowedStatuses = new List<TicketStatus>();

            if (roleTicket.Status == TicketStatus.Pending)
            {
                allowedStatuses.Add(TicketStatus.UnderReview);
            }
            else if (roleTicket.Status == TicketStatus.UnderReview)
            {
                allowedStatuses.AddRange(new[] { TicketStatus.Completed, TicketStatus.Rejected });
            }
            else if (roleTicket.Status == TicketStatus.Assigned)
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
    }
}
