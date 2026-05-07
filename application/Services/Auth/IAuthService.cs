using Microsoft.AspNetCore.Identity;
using STO_Desk_backend.Models.DTOs.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace STO_Desk_backend.Services.Auth
{
    public interface IAuthService
    {
        Task<(bool Success, string? Token, string? Email, IEnumerable<IdentityError>? Errors, string? ErrorMessage)> SignUpAsync(SignUpDto dto);
        Task<(bool Success, string? Token, string? Email, string? ErrorMessage)> LogInAsync(LogInDto dto);
    }
}
