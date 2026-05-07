using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STO_Desk_backend.Models;
using STO_Desk_backend.Models.DTOs.TicketCategories;
using STO_Desk_backend.Models.Entities;

namespace STO_Desk_backend.Services.TicketCategories
{
    public class TicketCategoryQueryService : ITicketCategoryQueryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TicketCategoryQueryService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(bool Success, List<TicketCategoryDto>? TicketCategories, string? ErrorMessage)> GetAllAsync()
        {
            List<TicketCategory> categories = await _context.TicketCategories.ToListAsync();

            List<TicketCategoryDto> categoryDtos = _mapper.Map<List<TicketCategoryDto>>(categories);
            return (true, categoryDtos, null);
        }

        public async Task<(bool Success, TicketCategoryDto? TicketCategory, string? ErrorMessage)> GetByIdAsync(int id)
        {
            TicketCategory? category = await _context.TicketCategories.FindAsync(id);
            if (category == null) return (false, null, "NotFound");

            TicketCategoryDto categoryDto = _mapper.Map<TicketCategoryDto>(category);
            return (true, categoryDto, null);
        }

    }
}
