using System.ComponentModel.DataAnnotations;

namespace STO_Desk_backend.Models.DTOs.RoleTickets
{
    public class RoleTicketAssignDto
    {
        /// <summary>
        /// Id of the assigned user with operator or admin role.
        /// </summary>
        [Required(ErrorMessage = "ReviewerId is required.")]
        public int? ReviewerId { get; set; }
    }
}
