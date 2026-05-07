using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using STO_Desk_backend.Models;
using STO_Desk_backend.Models.DTOs.RoleTickets;
using STO_Desk_backend.Models.Entities;
using STO_Desk_backend.Models.Enums;
using STO_Desk_backend.Services.Users;
using System.Security.Claims;

namespace STO_Desk_backend.Services.RoleTickets
{
    public class RoleTicketCommandService : IRoleTicketCommandService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IUserCommandService _userCommandService;
        private readonly IMapper _mapper;

        public RoleTicketCommandService(
            ApplicationDbContext context,
            UserManager<User> userManager,
            IUserCommandService userCommandService,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _userCommandService = userCommandService;
            _mapper = mapper;
        }

        public async Task<(bool Success, RoleTicketDto? RoleTicket, string? ErrorMessage)> CreateRoleTicketAsync(RoleTicketCreateDto dto, ClaimsPrincipal currentUserPrincipal)
        {
            User? user = await _userManager.GetUserAsync(currentUserPrincipal);
            if (user == null) return (false, null, "Unauthorized");

            if (user.Id != dto.UserId) return (false, null, "ID mismatch");

            RoleTicket roleTicket = new RoleTicket
            {
                Title = dto.Title,
                Body = dto.Body,
                UserId = dto.UserId.Value,
            };

            if (dto.ThemeId != null)
            {
                if (!await _context.RoleTicketThemes.AnyAsync(tt => tt.Id == dto.ThemeId)) return (false, null, "Invalid ThemeId");
                else roleTicket.ThemeId = dto.ThemeId.Value;
            }

            if (dto.StoId != null)
            {
                if (!await _context.Stos.AnyAsync(s => s.Id == dto.StoId)) return (false, null, "Invalid StoId");
                else roleTicket.StoId = dto.StoId;
            }

            _context.RoleTickets.Add(roleTicket);
            await _context.SaveChangesAsync();

            RoleTicketDto roleTicketDto = _mapper.Map<RoleTicketDto>(roleTicket);
            return (true, roleTicketDto, null);
        }

        public async Task<(bool Success, RoleTicketDto? RoleTicket, string? ErrorMessage)> UpdateRoleTicketAsync(int id, RoleTicketUpdateDto dto, ClaimsPrincipal currentUserPrincipal)
        {
            if (id != dto.Id) return (false, null, "ID mismatch");

            User? user = await _userManager.GetUserAsync(currentUserPrincipal);
            if (user == null) return (false, null, "Unauthorized");

            RoleTicket? roleTicket = await _context.RoleTickets.FindAsync(id);
            if (roleTicket == null) return (false, null, "NotFound");

            bool isAdminOrOperator = currentUserPrincipal.IsInRole("Admin") || currentUserPrincipal.IsInRole("Operator");

            // only client or Admin/Operator can edit main fields
            if (user.Id != roleTicket.UserId && !isAdminOrOperator)
            {
                return (false, null, "Forbid");
            }

            // once the ticket leaves the initial phases, it cannot be edited
            if (roleTicket.Status != TicketStatus.Pending && roleTicket.Status != TicketStatus.UnderReview && !isAdminOrOperator)
            {
                return (false, null, "Cannot edit a ticket after it has been assigned or closed.");
            }

            if (!string.IsNullOrWhiteSpace(dto.Title)) roleTicket.Title = dto.Title;
            if (!string.IsNullOrWhiteSpace(dto.Body)) roleTicket.Body = dto.Body;

            if (dto.ThemeId != null)
            {
                if (!await _context.TicketThemes.AnyAsync(r => r.Id == dto.ThemeId)) return (false, null, "Invalid ThemeId");
                roleTicket.ThemeId = dto.ThemeId.Value;
            }

            roleTicket.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            RoleTicketDto roleTicketDto = _mapper.Map<RoleTicketDto>(roleTicket);
            return (true, roleTicketDto, null);
        }

        public async Task<(bool Success, RoleTicketDto? RoleTicket, string? ErrorMessage)> CancelRoleTicketAsync(int id, ClaimsPrincipal currentUserPrincipal)
        {
            User? user = await _userManager.GetUserAsync(currentUserPrincipal);
            if (user == null) return (false, null, "Unauthorized");

            RoleTicket? roleTicket = await _context.RoleTickets.FindAsync(id);
            if (roleTicket == null) return (false, null, "NotFound");

            if (user.Id != roleTicket.UserId)
            {
                return (false, null, "Forbid");
            }

            if (roleTicket.Status == TicketStatus.Completed || roleTicket.Status == TicketStatus.Rejected || roleTicket.Status == TicketStatus.Canceled)
            {
                return (false, null, "Ticket is already closed.");
            }

            roleTicket.Status = TicketStatus.Canceled;
            roleTicket.UpdatedAt = DateTime.UtcNow;
            roleTicket.ClosedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            RoleTicketDto ticketDto = _mapper.Map<RoleTicketDto>(roleTicket);
            return (true, ticketDto, null);
        }

        public async Task<(bool Success, RoleTicketDto? RoleTicket, string? ErrorMessage)> AssignRoleTicketAsync(int id, RoleTicketAssignDto dto, ClaimsPrincipal currentUserPrincipal)
        {
            User? user = await _userManager.GetUserAsync(currentUserPrincipal);
            if (user == null) return (false, null, "Unauthorized");

            RoleTicket? roleTicket = await _context.RoleTickets
                .Include(rt => rt.Theme)
                .FirstOrDefaultAsync(rt => rt.Id == id);
            if (roleTicket == null) return (false, null, "NotFound");

            User? reviewer = await _context.Users.FindAsync(dto.ReviewerId);
            if (reviewer == null) return (false, null, "NotFound");

            bool isOperator = currentUserPrincipal.IsInRole("Operator");
            bool isAdmin = currentUserPrincipal.IsInRole("Admin");

            // operator can only assign to himself
            if (isOperator && !isAdmin && reviewer.Id != user.Id) return (false, null, "Forbid");

            // only admins can assign tickets with Admin role and only to themselves
            if (roleTicket.Theme.TargetRole == TargetRole.Admin && !isAdmin && reviewer.Id != user.Id) return (false, null, "Forbid");

            // only admin can re-assign the ticket
            if (!isAdmin && roleTicket.Status == TicketStatus.Assigned)
            {
                return (false, null, "Cannot assign an assigned ticket.");
            }

            if (roleTicket.Status == TicketStatus.Canceled || roleTicket.Status == TicketStatus.Completed || roleTicket.Status == TicketStatus.Rejected)
            {
                return (false, null, "Cannot assign a closed ticket.");
            }

            roleTicket.ReviewerId = dto.ReviewerId;
            roleTicket.Status = TicketStatus.Assigned;
            roleTicket.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            RoleTicketDto ticketDto = _mapper.Map<RoleTicketDto>(roleTicket);
            return (true, ticketDto, null);
        }

        public async Task<(bool Success, RoleTicketDto? RoleTicket, string? ErrorMessage)> UpdateRoleTicketStatusAsync(int id, RoleTicketStatusUpdateDto dto, ClaimsPrincipal currentUserPrincipal)
        {
            User? user = await _userManager.GetUserAsync(currentUserPrincipal);
            if (user == null) return (false, null, "Unauthorized");

            RoleTicket? roleTicket = await _context.RoleTickets
                .Include(rt => rt.Theme)
                .Include(rt => rt.Sto)
                .FirstOrDefaultAsync(rt => rt.Id == id);
            if (roleTicket == null) return (false, null, "NotFound");

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
            if (roleTicket.Status == TicketStatus.Pending && dto.Status != TicketStatus.UnderReview)
            {
                return (false, null, "A Pending ticket can only be transitioned to UnderReview.");
            }

            // cannot change closed ticket
            if (roleTicket.Status == TicketStatus.Canceled || roleTicket.Status == TicketStatus.Completed || roleTicket.Status == TicketStatus.Rejected)
            {
                return (false, null, "Cannot change the status of a closed ticket.");
            }

            if (roleTicket.Status == TicketStatus.Assigned && dto.Status != TicketStatus.UnderReview && dto.Status != TicketStatus.Completed && dto.Status != TicketStatus.Rejected)
            {
                return (false, null, "An Assigned ticket can only be transitioned to UnderReview, Completed, or Rejected.");
            }

            bool isAdmin = currentUserPrincipal.IsInRole("Admin");

            if (roleTicket.Status == TicketStatus.Assigned)
            {
                if (roleTicket.ReviewerId != user.Id && !isAdmin)
                {
                    return (false, null, "Forbid");
                }

                if (dto.Status == TicketStatus.UnderReview)
                {
                    roleTicket.ReviewerId = null;
                }
            }

            roleTicket.Status = dto.Status!.Value;
            roleTicket.UpdatedAt = DateTime.UtcNow;

            if (dto.Status == TicketStatus.Canceled || dto.Status == TicketStatus.Completed || dto.Status == TicketStatus.Rejected)
            {
                roleTicket.ClosedAt = DateTime.UtcNow;

                // if RT is Completed (approved would sound better) we assign roles
                if (dto.Status == TicketStatus.Completed)
                {
                    User? targetUser = await _context.Users.FindAsync(roleTicket.UserId);
                    string targetRoleName = roleTicket.Theme.TargetRole.GetTargetRoleName();
                    if (targetUser != null && !string.IsNullOrWhiteSpace(targetRoleName))
                    {
                        // checking if they already have the role to prevent duplicates
                        var result = await _userCommandService.AssignRoleAsync(id, targetRoleName, currentUserPrincipal);

                        if (!result.Success)
                        {
                            if (result.ErrorMessage == "NotFound") return (false, null, "NotFound");
                            if (result.ErrorMessage == "Forbid") return (false, null, "Forbid");
                            return (false, null, result.ErrorMessage);
                        }

                        if (roleTicket.Sto != null && roleTicket.Sto.OwnerId != null && roleTicket.Theme.TargetRole == TargetRole.NewStoOwner)
                        {
                            // remove current owners role
                            result = await _userCommandService.RemoveRoleAsync(roleTicket.Sto.OwnerId.Value, targetRoleName, currentUserPrincipal);

                            if (!result.Success)
                            {
                                if (result.ErrorMessage == "NotFound") return (false, null, "NotFound");
                                if (result.ErrorMessage == "Forbid") return (false, null, "Forbid");
                                return (false, null, result.ErrorMessage);
                            }
                            // swap owner
                            roleTicket.Sto.OwnerId = targetUser.Id;
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();

            RoleTicketDto ticketDto = _mapper.Map<RoleTicketDto>(roleTicket);
            return (true, ticketDto, null);
        }

        public async Task<(bool Success, string? ErrorMessage)> DeleteRoleTicketAsync(int id, ClaimsPrincipal currentUserPrincipal)
        {
            RoleTicket? roleTicket = await _context.RoleTickets.FindAsync(id);
            if (roleTicket == null) return (false, "NotFound");

            _context.RoleTickets.Remove(roleTicket);
            await _context.SaveChangesAsync();
            return (true, null);
        }
    }
}
