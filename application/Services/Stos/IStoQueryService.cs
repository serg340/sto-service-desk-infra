using STO_Desk_backend.Models.DTOs.Stos;
using STO_Desk_backend.Models.DTOs.Users;
using System.Security.Claims;

namespace STO_Desk_backend.Services.Stos
{
    public interface IStoQueryService
    {
        Task<(bool Success, List<StoDto>? Stos, string? ErrorMessage)> GetAllStosAsync();
        Task<(bool Success, List<StoDto>? Stos, string? ErrorMessage)> GetStosByRegionIdAsync(int id);
        Task<(bool Success, StoDto? Sto, string? ErrorMessage)> GetStoByIdAsync(int id);
        Task<(bool Success, List<UserDto>? Members, string? ErrorMessage)> GetStoMembersAsync(int id);
    }
}
