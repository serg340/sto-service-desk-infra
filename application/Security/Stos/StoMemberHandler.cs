using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using STO_Desk_backend.Models;
using System.Security.Claims;

namespace STO_Desk_backend.Security.Stos
{
    public class StoMemberRequirement : IAuthorizationRequirement
    {
    }

    public class StoMemberHandler : AuthorizationHandler<StoMemberRequirement>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StoMemberHandler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, StoMemberRequirement requirement)
        {
            // temp ai comments for myself
            // 1. Get STO ID from route
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return; // Not in a web request
            }

            var routeValue = httpContext.Request.RouteValues["id"];
            if (routeValue == null || !int.TryParse(routeValue.ToString(), out int stoId))
            {
                return; // No STO ID in route, let other handlers decide or fail
            }

            // 2. Admins and Operators always have access
            if (context.User.IsInRole("Admin") || context.User.IsInRole("Operator"))
            {
                context.Succeed(requirement);
                return;
            }

            // 3. Get Current User ID
            var userIdString = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
            {
                return; // User is not authenticated or ID is missing
            }

            // 4. Check if User is the STO Owner or a Mechanic at this STO
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return;
            }

            // Is the user the owner? We need to check the STO
            var sto = await _context.Stos.FindAsync(stoId);
            if (sto == null)
            {
                return; // STO doesn't exist
            }

            if (sto.OwnerId == userId || user.StoId == stoId)
            {
                context.Succeed(requirement);
                return;
            }

            // If we reach here, the user is neither an Admin/Operator, nor the STO Owner, nor a Mechanic at the STO.
            // The requirement is not met.
            return;
        }
    }
}
