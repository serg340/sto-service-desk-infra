using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STO_Desk_backend.Models;
using STO_Desk_backend.Models.DTOs.RoleTicketThemes;
using STO_Desk_backend.Models.Entities;
using System.Security.Claims;

namespace STO_Desk_backend.Services.RoleTicketThemes
{
    public class RoleTicketThemeCommandService : IRoleTicketThemeCommandService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RoleTicketThemeCommandService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(bool Success, RoleTicketThemeDto? RoleTicketTheme, string? ErrorMessage)> CreateAsync(RoleTicketThemeCreateDto dto, ClaimsPrincipal currentUserPrincipal)
        {
            if (!currentUserPrincipal.IsInRole("Admin")) return (false, null, "Forbid");

            RoleTicketTheme theme = new RoleTicketTheme { Name = dto.Name, TargetRole = dto.TargetRole!.Value, CategoryId = dto.CategoryId!.Value };
            _context.RoleTicketThemes.Add(theme);
            await _context.SaveChangesAsync();

            RoleTicketThemeDto themeDto = _mapper.Map<RoleTicketThemeDto>(theme);
            return (true, themeDto, null);
        }

        public async Task<(bool Success, RoleTicketThemeDto? RoleTicketTheme, string? ErrorMessage)> UpdateAsync(int id, RoleTicketThemeUpdateDto dto, ClaimsPrincipal currentUserPrincipal)
        {
            if (!currentUserPrincipal.IsInRole("Admin")) return (false, null, "Forbid");

            if (id != dto.Id) return (false, null, "ID mismatch");

            RoleTicketTheme? theme = await _context.RoleTicketThemes.FindAsync(id);
            if (theme == null) return (false, null, "NotFound");

            if (!string.IsNullOrWhiteSpace(dto.Name)) theme.Name = dto.Name;
            if (dto.TargetRole != null) theme.TargetRole = dto.TargetRole.Value;

            if (dto.CategoryId != null)
            {
                bool categoryExists = await _context.RoleTicketCategories.AnyAsync(c => c.Id == dto.CategoryId);
                if (!categoryExists) return (false, null, "Invalid CategoryId");

                theme.CategoryId = dto.CategoryId.Value;
            }

            await _context.SaveChangesAsync();

            RoleTicketThemeDto themeDto = _mapper.Map<RoleTicketThemeDto>(theme);
            return (true, themeDto, null);
        }

        public async Task<(bool Success, string? ErrorMessage)> RemoveAsync(int id, ClaimsPrincipal currentUserPrincipal)
        {
            if (!currentUserPrincipal.IsInRole("Admin")) return (false, "Forbid");

            RoleTicketTheme? theme = await _context.RoleTicketThemes.FindAsync(id);
            if (theme == null) return (false, "NotFound");

            _context.RoleTicketThemes.Remove(theme);
            await _context.SaveChangesAsync();

            return (true, null);
        }
    }
}
