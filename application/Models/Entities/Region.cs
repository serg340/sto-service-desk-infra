using System;
using System.Collections.Generic;

namespace STO_Desk_backend.Models.Entities
{
    /// <summary>
    /// Represents a region. <br></br>
    /// Has connections with Users, Stos.
    /// </summary>
    public class Region
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<User>? Users { get; set; }
        public ICollection<Sto>? Stos { get; set; }
    }
}
