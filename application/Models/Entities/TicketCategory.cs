namespace STO_Desk_backend.Models.Entities
{
    /// <summary>
    /// Has connections with TicketThemes.
    /// </summary>
    public class TicketCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<TicketTheme>? TicketThemes { get; set; }
         
    }
}
