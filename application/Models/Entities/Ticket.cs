using STO_Desk_backend.Models.Enums;
using System;

namespace STO_Desk_backend.Models.Entities
{
    /// <summary>
    /// Represents a ticket, is created by Users. <br></br>
    /// Has connection with TicketTheme, User (Client), Sto, User (Mechanic).
    /// </summary>
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public TicketStatus Status { get; set; } = TicketStatus.Pending;

        public int ThemeId { get; set; }
        public TicketTheme Theme { get; set; } = null!;

        public int ClientId { get; set; }
        public User Client { get; set; } = null!;

        public int StoId { get; set; }
        public Sto Sto { get; set; } = null!;

        public int? MechanicId { get; set; }
        public User? Mechanic { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
    }
}
