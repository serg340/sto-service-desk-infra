using System.Collections.Generic;
using STO_Desk_backend.Models.Entities;

namespace STO_Desk_backend.Services.Auth
{
    public interface ITokenService
    {
        string GenerateToken(User user, IList<string> roles);
    }
}
