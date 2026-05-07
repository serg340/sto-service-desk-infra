namespace STO_Desk_backend.Models.Entities
{
    /// <summary>
    /// Has connections with RoleTicketThemes.
    /// </summary>
    public class RoleTicketCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<RoleTicketTheme>? RoleTicketThemes { get; set; }
    }
}
