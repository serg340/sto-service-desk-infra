using STO_Desk_backend.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace STO_Desk_backend.Models.DTOs.Tickets
{
    public class TicketStatusUpdateDto
    {
        /// <summary>
        /// Ticket's status.
        /// </summary>
        [Required(ErrorMessage = "Status is required.")]
        public TicketStatus? Status { get; set; }
    }
}
