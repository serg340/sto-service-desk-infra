using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STO_Desk_backend.Models;
using STO_Desk_backend.Models.DTOs.Stos;
using STO_Desk_backend.Models.DTOs.Users;
using STO_Desk_backend.Models.Entities;
using System.Security.Claims;

namespace STO_Desk_backend.Services.Stos
{
    public class StoQueryService : IStoQueryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public StoQueryService(
            ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(bool Success, List<StoDto>? Stos, string? ErrorMessage)> GetAllStosAsync()
        {
            List<Sto> stos = await _context.Stos.ToListAsync();

            List<StoDto> stoDtos = _mapper.Map<List<StoDto>>(stos);
            return (true, stoDtos, null);
        }

        public async Task<(bool Success, StoDto? Sto, string? ErrorMessage)> GetStoByIdAsync(int id)
        {
            Sto? sto = await _context.Stos.FindAsync(id);
            if (sto == null) return (false, null, "NotFound");

            StoDto stoDto = _mapper.Map<StoDto>(sto);
            return (true, stoDto, null);
        }

        public async Task<(bool Success, List<StoDto>? Stos, string? ErrorMessage)> GetStosByRegionIdAsync(int id)
        {
            if (!_context.Regions.Any(r => r.Id == id)) return (false, null, "Wrong RegionId");

            List<Sto> stos = await _context.Stos
                .Where(s => s.RegionId == id)
                .ToListAsync();

            List<StoDto> stoDtos = _mapper.Map<List<StoDto>>(stos);
            return (true, stoDtos, null);
        }

        public async Task<(bool Success, List<UserDto>? Members, string? ErrorMessage)> GetStoMembersAsync(int id)
        {
            Sto? sto = await _context.Stos
                .Include(s => s.Mechanics)
                .Include(s => s.Owner)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sto == null) return (false, null, "NotFound");

            var members = new List<User>();
            if (sto.Owner != null)
            {
                members.Add(sto.Owner);
            }
            if (sto.Mechanics != null)
            {
                members.AddRange(sto.Mechanics);
            }

            // Remove duplicates just in case the owner is also listed as a mechanic
            members = members.DistinctBy(u => u.Id).ToList();

            List<UserDto> memberDtos = _mapper.Map<List<UserDto>>(members);
            return (true, memberDtos, null);
        }
    }
}
