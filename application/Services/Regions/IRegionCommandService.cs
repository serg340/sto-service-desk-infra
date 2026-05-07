using STO_Desk_backend.Models.DTOs.Regions;

namespace STO_Desk_backend.Services.Regions
{
    public interface IRegionCommandService
    {
        Task<(bool Success, RegionDto? Region, string? ErrorMessage)> CreateAsync(RegionCreateDto dto);
        Task<(bool Success, RegionDto? Region, string? ErrorMessage)> UpdateAsync(int id, RegionUpdateDto dto);
        Task<(bool Success, string? ErrorMessage)> RemoveAsync(int id);
    }
}
