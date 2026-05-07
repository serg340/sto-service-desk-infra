using STO_Desk_backend.Models.DTOs.Regions;

namespace STO_Desk_backend.Services.Regions
{
    public interface IRegionQueryService
    {
        Task<(bool Success, List<RegionDto>? Regions, string? ErrorMessage)> GetAllAsync();
        Task<(bool Success, RegionDto? Region, string? ErrorMessage)> GetByIdAsync(int id);
    }
}
