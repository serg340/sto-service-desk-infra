using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STO_Desk_backend.Models;
using STO_Desk_backend.Models.DTOs.Regions;
using STO_Desk_backend.Models.Entities;

namespace STO_Desk_backend.Services.Regions
{
    public class RegionCommandService : IRegionCommandService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RegionCommandService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(bool Success, RegionDto? Region, string? ErrorMessage)> CreateAsync(RegionCreateDto dto)
        {
            Region region = new Region { Name = dto.Name };

            _context.Regions.Add(region);
            await _context.SaveChangesAsync();

            RegionDto regionDto = _mapper.Map<RegionDto>(region);
            return (true, regionDto, null);
        }

        public async Task<(bool Success, RegionDto? Region, string? ErrorMessage)> UpdateAsync(int id, RegionUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return (false, null, "ID mismatch");
            }

            Region? region = await _context.Regions.FindAsync(id);
            if (region == null)
            {
                return (false, null, "NotFound");
            }

            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                region.Name = dto.Name;
            }

            await _context.SaveChangesAsync();

            RegionDto regionDto = _mapper.Map<RegionDto>(region);
            return (true, regionDto, null);
        }

        public async Task<(bool Success, string? ErrorMessage)> RemoveAsync(int id)
        {
            Region? region = await _context.Regions.FindAsync(id);
            if (region == null)
            {
                return (false, "NotFound");
            }

            _context.Regions.Remove(region);
            await _context.SaveChangesAsync();

            return (true, null);
        }
    }
}
