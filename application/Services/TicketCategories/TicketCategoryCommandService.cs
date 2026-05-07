using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STO_Desk_backend.Models;
using STO_Desk_backend.Models.DTOs.TicketCategories;
using STO_Desk_backend.Models.Entities;
using System.Security.Claims;

namespace STO_Desk_backend.Services.TicketCategories
{
    public class TicketCategoryCommandService : ITicketCategoryCommandService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TicketCategoryCommandService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(bool Success, TicketCategoryDto? TicketCategory, string? ErrorMessage)> CreateAsync(TicketCategoryCreateDto dto, ClaimsPrincipal currentUserPrincipal)
        {
            if (!currentUserPrincipal.IsInRole("Admin")) return (false, null, "Forbid");

            TicketCategory category = new TicketCategory { Name = dto.Name };
            _context.TicketCategories.Add(category);
            await _context.SaveChangesAsync();

            TicketCategoryDto categoryDto = _mapper.Map<TicketCategoryDto>(category);
            return (true, categoryDto, null);
        }

        public async Task<(bool Success, TicketCategoryDto? TicketCategory, string? ErrorMessage)> UpdateAsync(int id, TicketCategoryUpdateDto dto, ClaimsPrincipal currentUserPrincipal)
        {
            if (!currentUserPrincipal.IsInRole("Admin")) return (false, null, "Forbid");

            if (id != dto.Id) return (false, null, "ID mismatch");

            TicketCategory? category = await _context.TicketCategories.FindAsync(dto.Id);
            if (category == null) return (false, null, "NotFound");

            if (!string.IsNullOrWhiteSpace(dto.Name)) category.Name = dto.Name;

            await _context.SaveChangesAsync();

            TicketCategoryDto categoryDto = _mapper.Map<TicketCategoryDto>(category);
            return (true, categoryDto, null);
        }

        public async Task<(bool Success, string? ErrorMessage)> RemoveAsync(int id, ClaimsPrincipal currentUserPrincipal)
        {
            if (!currentUserPrincipal.IsInRole("Admin")) return (false, "Forbid");

            TicketCategory? category = await _context.TicketCategories.FindAsync(id);
            if (category == null) return (false, "NotFound");

            _context.TicketCategories.Remove(category);
            await _context.SaveChangesAsync();

            return (true, null);
        }
    }
}
