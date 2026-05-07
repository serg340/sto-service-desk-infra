using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using STO_Desk_backend.Models;
using STO_Desk_backend.Models.DTOs.Stos;
using STO_Desk_backend.Models.Entities;
using STO_Desk_backend.Models.Enums;
using STO_Desk_backend.Services.Users;
using System.Security.Claims;

namespace STO_Desk_backend.Services.Stos
{
    public class StoCommandService : IStoCommandService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IUserCommandService _userCommandService;
        private readonly IMapper _mapper;

        public StoCommandService(
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

        public async Task<(bool Success, StoDto? Sto, string? ErrorMessage)> CreateStoAsync(StoCreateDto dto, ClaimsPrincipal currentUserPrincipal)
        {
            User? user = await _userManager.GetUserAsync(currentUserPrincipal);
            if (user == null) return (false, null, "Unauthorized");

            // not required since we don't even use dto.id here
            // if (user.Id != dto.OwnerId) return BadRequest(new { message = "ID mismatch" });

            Sto sto = new Sto
            {
                Name = dto.Name,
                Body = dto.Body,
                OwnerId = user.Id,
            };

            // check if dto id exists
            if (dto.RegionId != null)
            {
                // check if the id is correct
                if (!await _context.Regions.AnyAsync(r => r.Id == dto.RegionId)) return (false, null, "Invalid RegionId");
                sto.RegionId = dto.RegionId;
            }
            else if (user.RegionId != null)
            {
                sto.RegionId = user.RegionId;
            }

            _context.Stos.Add(sto);
            await _context.SaveChangesAsync();

            StoDto stoDto = _mapper.Map<StoDto>(sto);
            return (true, stoDto, null);
        }

        public async Task<(bool Success, string? ErrorMessage)> DeleteStoAsync(int id, ClaimsPrincipal currentUserPrincipal)
        {
            User? user = await _userManager.GetUserAsync(currentUserPrincipal);
            if (user == null) return (false, "Unauthorized");

            Sto? sto = await _context.Stos.FindAsync(id);
            if (sto == null) return (false, "NotFound");

            bool isAdminOrOperator = currentUserPrincipal.IsInRole("Admin") || currentUserPrincipal.IsInRole("Operator");

            if (user.Id != sto.OwnerId && !isAdminOrOperator)
            {
                return (false, "Forbid");
            }

            _context.Stos.Remove(sto);
            await _context.SaveChangesAsync();
            return (true, null);
        }

        public async Task<(bool Success, string? ErrorMessage)> RemoveMechanicFromStoAsync(int stoId, int mechanicId, ClaimsPrincipal currentUserPrincipal)
        {
            User? user = await _userManager.GetUserAsync(currentUserPrincipal);
            if (user == null) return (false, "Unauthorized");

            Sto? sto = await _context.Stos
                .Include(s => s.Mechanics)
                .Include(s => s.Tickets)
                .FirstOrDefaultAsync(s => s.Id == stoId);
            if (sto == null) return (false, "NotFound");

            // must be Admin/Operator, or STO Owner
            bool isAdminOrOperator = currentUserPrincipal.IsInRole("Admin") || currentUserPrincipal.IsInRole("Operator");
            if (sto.OwnerId != user.Id && !isAdminOrOperator)
            {
                return (false, "Forbid");
            }

            User? mechanic = await _context.Users.FindAsync(mechanicId);
            if (mechanic == null) return (false, "NotFound");

            // check if the mechanic actually works at this STO
            if (mechanic.StoId != stoId)
            {
                return (false, "This mechanic does not work at the specified STO.");
            }

            mechanic.StoId = null;
            // sto.Mechanics.Remove(mechanic)

            //if (await _userManager.IsInRoleAsync(mechanic, "Mechanic"))
            //{
            //    await _userManager.RemoveFromRoleAsync(mechanic, "Mechanic");
            //}

            var result = await _userCommandService.RemoveRoleAsync(mechanicId, "Mechanic", currentUserPrincipal);

            if (!result.Success)
            {
                if (result.ErrorMessage == "NotFound") return (false, "NotFound");
                if (result.ErrorMessage == "Forbid") return (false, "Forbid");
                return (false, result.ErrorMessage);
            }

            // reassigning mechanic's active tickets back to UnderReview
            if (sto.Tickets != null)
            {
                List<Ticket> activeTickets = (List<Ticket>)sto.Tickets
                    .Where(t => t.MechanicId == mechanicId && t.Status != TicketStatus.Assigned);
                foreach (Ticket t in activeTickets) { t.MechanicId = null; t.Status = TicketStatus.UnderReview; }
            }
            await _context.SaveChangesAsync();

            return (true, null);
        }

        public async Task<(bool Success, StoDto? Sto, string? ErrorMessage)> UpdateStoAsync(int id, StoUpdateDto dto, ClaimsPrincipal currentUserPrincipal)
        {
            // NOTE: owner cannot be changed here
            if (id != dto.Id) return (false, null, "ID mismatch");

            User? user = await _userManager.GetUserAsync(currentUserPrincipal);
            if (user == null) return (false, null, "Unauthorized");

            Sto? sto = await _context.Stos.FindAsync(id);
            if (sto == null) return (false, null, "NotFound");

            bool isAdminOrOperator = currentUserPrincipal.IsInRole("Admin") || currentUserPrincipal.IsInRole("Operator");

            if (user.Id != sto.OwnerId && !isAdminOrOperator)
            {
                return (false, null, "Forbid");
            }

            if (!string.IsNullOrWhiteSpace(dto.Name)) sto.Name = dto.Name;
            if (!string.IsNullOrWhiteSpace(dto.Body)) sto.Body = dto.Body;

            if (dto.RegionId != null)
            {
                if (!await _context.Regions.AnyAsync(r => r.Id == dto.RegionId)) return (false, null, "Invalid RegionId");

                sto.RegionId = dto.RegionId;
            }

            sto.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            StoDto stoDto = _mapper.Map<StoDto>(sto);
            return (true, stoDto, null);
        }
    }
}
