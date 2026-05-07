using System.ComponentModel.DataAnnotations;

namespace STO_Desk_backend.Models.DTOs.Tickets
{
    public class TicketAssignDto
    {
        /// <summary>
        /// Id of the assigned user with mechanic role.
        /// </summary>
        [Required(ErrorMessage = "MechanicId is required.")]
        public int? MechanicId { get; set; }
    }
}
