using System;
using System.Collections.Generic;

namespace STO_Desk_backend.Models.Entities
{
    /// <summary>
    /// Represents an STO. Can be created with StoOwner role. <br></br>
    /// Has connetion with User, Region and connections with User (Mechanics), Tickets, RoleTickets.
    /// </summary>
    public class Sto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;

        public int? OwnerId { get; set; }
        public User? Owner { get; set; }

        public int? RegionId { get; set; }
        public Region? Region { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }


        public ICollection<User>? Mechanics { get; set; }
        public ICollection<Ticket>? Tickets { get; set; }
        public ICollection<RoleTicket>? RoleTickets { get; set; }
    }
}
