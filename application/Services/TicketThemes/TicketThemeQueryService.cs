using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STO_Desk_backend.Models;
using STO_Desk_backend.Models.DTOs.TicketThemes;
using STO_Desk_backend.Models.Entities;

namespace STO_Desk_backend.Services.TicketThemes
{
    public class TicketThemeQueryService : ITicketThemeQueryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TicketThemeQueryService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(bool Success, List<TicketThemeDto>? TicketThemes, string? ErrorMessage)> GetAllAsync()
        {
            List<TicketTheme> themes = await _context.TicketThemes.ToListAsync();

            List<TicketThemeDto> themeDtos = _mapper.Map<List<TicketThemeDto>>(themes);
            return (true, themeDtos, null);
        }

        public async Task<(bool Success, List<TicketThemeDto>? TicketThemes, string? ErrorMessage)> GetByCategoryIdAsync(int id)
        {
            List<TicketTheme> themes = await _context.TicketThemes
                .Where(t => t.CategoryId == id)
                .ToListAsync();

            List<TicketThemeDto> themeDtos = _mapper.Map<List<TicketThemeDto>>(themes);
            return (true, themeDtos, null);
        }

        public async Task<(bool Success, TicketThemeDto? TicketTheme, string? ErrorMessage)> GetByIdAsync(int id)
        {
            TicketTheme? theme = await _context.TicketThemes.FindAsync(id);
            if (theme == null) return (false, null, "NotFound");

            TicketThemeDto themeDto = _mapper.Map<TicketThemeDto>(theme);
            return (true, themeDto, null);
        }

    }
}
