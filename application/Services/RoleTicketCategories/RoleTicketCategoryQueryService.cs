using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STO_Desk_backend.Models;
using STO_Desk_backend.Models.DTOs.RoleTicketCategories;
using STO_Desk_backend.Models.Entities;

namespace STO_Desk_backend.Services.RoleTicketCategories
{
    public class RoleTicketCategoryQueryService : IRoleTicketCategoryQueryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RoleTicketCategoryQueryService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(bool Success, List<RoleTicketCategoryDto>? RoleTicketCategories, string? ErrorMessage)> GetAllAsync()
        {
            List<RoleTicketCategory> categories = await _context.RoleTicketCategories.ToListAsync();

            List<RoleTicketCategoryDto> categoryDtos = _mapper.Map<List<RoleTicketCategoryDto>>(categories);
            return (true, categoryDtos, null);
        }

        public async Task<(bool Success, RoleTicketCategoryDto? RoleTicketCategory, string? ErrorMessage)> GetByIdAsync(int id)
        {
            RoleTicketCategory? category = await _context.RoleTicketCategories.FindAsync(id);
            if (category == null) return (false, null, "NotFound");

            RoleTicketCategoryDto categoryDto = _mapper.Map<RoleTicketCategoryDto>(category);
            return (true, categoryDto, null);
        }
    }
}
