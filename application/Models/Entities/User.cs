using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace STO_Desk_backend.Models.Entities
{
    /// <summary>
    /// Represents a user (custom). <br></br>
    /// It has additional fields inherited from IdentityUser.
    /// </summary>
    public class User : IdentityUser<int>
    {
        public int? RegionId { get; set; }
        public Region? Region { get; set; }

        public int? StoId { get; set; }
        public Sto? Sto { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        [InverseProperty("Client")]
        public ICollection<Ticket>? Tickets { get; set; }

        [InverseProperty("User")]
        public ICollection<RoleTicket>? RoleTickets { get; set; }
        
        [InverseProperty("Mechanic")]
        public ICollection<Ticket>? MechanicTickets { get; set; }
        
        [InverseProperty("Reviewer")]
        public ICollection<RoleTicket>? ReviewedRoleTickets { get; set; }
        
        [InverseProperty("Owner")]
        public ICollection<Sto>? OwnedStos { get; set; }
    }
}
