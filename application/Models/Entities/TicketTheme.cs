namespace STO_Desk_backend.Models.Entities
{
    /// <summary>
    /// Has a cetegory connection and connections with Tickets.
    /// </summary>
    public class TicketTheme
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public TicketCategory Category { get; set; } = null!;

        public ICollection<Ticket>? Tickets { get; set; }
    }
}
