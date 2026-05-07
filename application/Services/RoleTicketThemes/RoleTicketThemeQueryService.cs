using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STO_Desk_backend.Models;
using STO_Desk_backend.Models.DTOs.RoleTicketThemes;
using STO_Desk_backend.Models.DTOs.Shared;
using STO_Desk_backend.Models.Entities;
using STO_Desk_backend.Models.Enums;
using System.Security.Claims;

namespace STO_Desk_backend.Services.RoleTicketThemes
{
    public class RoleTicketThemeQueryService : IRoleTicketThemeQueryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RoleTicketThemeQueryService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(bool Success, List<RoleTicketThemeDto>? RoleTicketThemes, string? ErrorMessage)> GetAllAsync()
        {
            List<RoleTicketTheme> themes = await _context.RoleTicketThemes.ToListAsync();
            List<RoleTicketThemeDto> themeDtos = _mapper.Map<List<RoleTicketThemeDto>>(themes);
            return (true, themeDtos, null);
        }

        public async Task<(bool Success, List<RoleTicketThemeDto>? RoleTicketThemes, string? ErrorMessage)> GetByCategoryIdAsync(int categoryId)
        {
            List<RoleTicketTheme> themes = await _context.RoleTicketThemes
                .Where(t => t.CategoryId == categoryId)
                .ToListAsync();

            List<RoleTicketThemeDto> themeDtos = _mapper.Map<List<RoleTicketThemeDto>>(themes);
            return (true, themeDtos, null);
        }

        public async Task<(bool Success, RoleTicketThemeDto? RoleTicketTheme, string? ErrorMessage)> GetByIdAsync(int id)
        {
            RoleTicketTheme? theme = await _context.RoleTicketThemes.FindAsync(id);
            if (theme == null) return (false, null, "NotFound");

            RoleTicketThemeDto themeDto = _mapper.Map<RoleTicketThemeDto>(theme);
            return (true, themeDto, null);
        }

        public (bool Success, List<EnumItemDto>? Roles, string? ErrorMessage) GetTargetRoles(ClaimsPrincipal currentUserPrincipal)
        {
            if (!currentUserPrincipal.IsInRole("Admin")) return (false, null, "Forbid");

            var roles = Enum.GetValues(typeof(TargetRole))
                .Cast<TargetRole>()
                .Select(e => new EnumItemDto
                {
                    Id = (int)e,
                    Name = e.ToString(),
                    DisplayName = e.GetTargetRoleName()
                }).ToList();
            return (true, roles, null);
        }
    }
}
