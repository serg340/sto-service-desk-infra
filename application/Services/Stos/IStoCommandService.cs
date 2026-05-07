using STO_Desk_backend.Models.DTOs.Stos;
using System.Security.Claims;

namespace STO_Desk_backend.Services.Stos
{
    public interface IStoCommandService
    {
        Task<(bool Success, StoDto? Sto, string? ErrorMessage)> CreateStoAsync(StoCreateDto dto, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, StoDto? Sto, string? ErrorMessage)> UpdateStoAsync(int id, StoUpdateDto dto, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, string? ErrorMessage)> DeleteStoAsync(int id, ClaimsPrincipal currentUserPrincipal);
        Task<(bool Success, string? ErrorMessage)> RemoveMechanicFromStoAsync(int stoId, int mechanicId, ClaimsPrincipal currentUserPrincipal);
    }
}
