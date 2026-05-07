using STO_Desk_backend.Models.Enums;

namespace STO_Desk_backend.Models.Entities
{
    /// <summary>
    /// Has a (role) cetegory connection and connections with RoleTickets.
    /// </summary>
    public class RoleTicketTheme
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public TargetRole TargetRole { get; set; }
        public int CategoryId { get; set; }
        public RoleTicketCategory Category { get; set; } = null!;

        public ICollection<RoleTicket>? RoleTickets { get; set; }
    }
}
