using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STO_Desk_backend.Models;
using STO_Desk_backend.Models.DTOs.TicketThemes;
using STO_Desk_backend.Models.Entities;
using System.Security.Claims;

namespace STO_Desk_backend.Services.TicketThemes
{
    public class TicketThemeCommandService : ITicketThemeCommandService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TicketThemeCommandService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(bool Success, TicketThemeDto? TicketTheme, string? ErrorMessage)> CreateAsync(TicketThemeCreateDto dto, ClaimsPrincipal currentUserPrincipal)
        {
            if (!currentUserPrincipal.IsInRole("Admin")) return (false, null, "Forbid");

            TicketTheme theme = new TicketTheme { Name = dto.Name, CategoryId = dto.CategoryId!.Value };
            _context.TicketThemes.Add(theme);
            await _context.SaveChangesAsync();

            TicketThemeDto themeDto = _mapper.Map<TicketThemeDto>(theme);
            return (true, themeDto, null);
        }

        public async Task<(bool Success, TicketThemeDto? TicketTheme, string? ErrorMessage)> UpdateAsync(int id, TicketThemeUpdateDto dto, ClaimsPrincipal currentUserPrincipal)
        {
            if (!currentUserPrincipal.IsInRole("Admin")) return (false, null, "Forbid");

            if (id != dto.Id) return (false, null, "ID mismatch");

            TicketTheme? theme = await _context.TicketThemes.FindAsync(id);
            if (theme == null) return (false, null, "NotFound");

            if (!string.IsNullOrWhiteSpace(dto.Name)) theme.Name = dto.Name;

            if (dto.CategoryId != null)
            {
                bool categoryExists = await _context.TicketCategories.AnyAsync(c => c.Id == dto.CategoryId);
                if (!categoryExists) return (false, null, "Invalid CategoryId");

                theme.CategoryId = dto.CategoryId.Value;
            }

            await _context.SaveChangesAsync();

            TicketThemeDto themeDto = _mapper.Map<TicketThemeDto>(theme);
            return (true, themeDto, null);
        }

        public async Task<(bool Success, string? ErrorMessage)> RemoveAsync(int id, ClaimsPrincipal currentUserPrincipal)
        {
            if (!currentUserPrincipal.IsInRole("Admin")) return (false, "Forbid");

            TicketTheme? theme = await _context.TicketThemes.FindAsync(id);
            if (theme == null) return (false, "NotFound");

            _context.TicketThemes.Remove(theme);
            await _context.SaveChangesAsync();

            return (true, null);
        }
    }
}
