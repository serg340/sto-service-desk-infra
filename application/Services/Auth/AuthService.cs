using Microsoft.AspNetCore.Identity;
using STO_Desk_backend.Models.DTOs.Auth;
using STO_Desk_backend.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace STO_Desk_backend.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<(bool Success, string? Token, string? Email, IEnumerable<IdentityError>? Errors, string? ErrorMessage)> SignUpAsync(SignUpDto dto)
        {
            if (dto.Password != dto.PasswordRepeat)
                return (false, null, null, null, "Passwords do not match.");

            User? userExists = await _userManager.FindByEmailAsync(dto.Email);
            if (userExists != null)
                return (false, null, null, null, "User with this email already exists.");
            
            var user = new User
            {
                Email = dto.Email,
                UserName = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return (false, null, null, result.Errors, null);

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.GenerateToken(user, roles);

            return (true, token, user.Email, null, null);
        }

        public async Task<(bool Success, string? Token, string? Email, string? ErrorMessage)> LogInAsync(LogInDto dto)
        {
            User? user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return (false, null, null, "Invalid email or password.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (!result.Succeeded)
                return (false, null, null, "Invalid email or password.");

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.GenerateToken(user, roles);

            return (true, token, user.Email, null);
        }
    }
}
