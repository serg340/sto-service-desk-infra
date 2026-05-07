using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STO_Desk_backend.Models;
using STO_Desk_backend.Models.DTOs.RoleTicketCategories;
using STO_Desk_backend.Models.Entities;
using System.Security.Claims;

namespace STO_Desk_backend.Services.RoleTicketCategories
{
    public class RoleTicketCategoryCommandService : IRoleTicketCategoryCommandService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RoleTicketCategoryCommandService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(bool Success, RoleTicketCategoryDto? RoleTicketCategory, string? ErrorMessage)> CreateAsync(RoleTicketCategoryCreateDto dto, ClaimsPrincipal currentUserPrincipal)
        {
            if (!currentUserPrincipal.IsInRole("Admin")) return (false, null, "Forbid");

            RoleTicketCategory category = new RoleTicketCategory { Name = dto.Name };
            _context.RoleTicketCategories.Add(category);
            await _context.SaveChangesAsync();

            RoleTicketCategoryDto categoryDto = _mapper.Map<RoleTicketCategoryDto>(category);
            return (true, categoryDto, null);
        }

        public async Task<(bool Success, RoleTicketCategoryDto? RoleTicketCategory, string? ErrorMessage)> UpdateAsync(int id, RoleTicketCategoryUpdateDto dto, ClaimsPrincipal currentUserPrincipal)
        {
            if (!currentUserPrincipal.IsInRole("Admin")) return (false, null, "Forbid");

            if (id != dto.Id) return (false, null, "ID mismatch");

            RoleTicketCategory? category = await _context.RoleTicketCategories.FindAsync(dto.Id);
            if (category == null) return (false, null, "NotFound");

            if (!string.IsNullOrWhiteSpace(dto.Name)) category.Name = dto.Name;

            await _context.SaveChangesAsync();

            RoleTicketCategoryDto categoryDto = _mapper.Map<RoleTicketCategoryDto>(category);
            return (true, categoryDto, null);
        }

        public async Task<(bool Success, string? ErrorMessage)> RemoveAsync(int id, ClaimsPrincipal currentUserPrincipal)
        {
            if (!currentUserPrincipal.IsInRole("Admin")) return (false, "Forbid");

            RoleTicketCategory? category = await _context.RoleTicketCategories.FindAsync(id);
            if (category == null) return (false, "NotFound");

            _context.RoleTicketCategories.Remove(category);
            await _context.SaveChangesAsync();

            return (true, null);
        }
    }
}
