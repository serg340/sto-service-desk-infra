using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STO_Desk_backend.Models;
using STO_Desk_backend.Models.DTOs.Regions;
using STO_Desk_backend.Models.Entities;

namespace STO_Desk_backend.Services.Regions
{
    public class RegionQueryService : IRegionQueryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RegionQueryService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(bool Success, List<RegionDto>? Regions, string? ErrorMessage)> GetAllAsync()
        {
            List<Region> regions = await _context.Regions.ToListAsync();
            List<RegionDto> regionDtos = _mapper.Map<List<RegionDto>>(regions);
            return (true, regionDtos, null);
        }

        public async Task<(bool Success, RegionDto? Region, string? ErrorMessage)> GetByIdAsync(int id)
        {
            Region? region = await _context.Regions.FindAsync(id);
            if (region == null)
            {
                return (false, null, "NotFound");
            }

            RegionDto regionDto = _mapper.Map<RegionDto>(region);
            return (true, regionDto, null);
        }
    }
}
