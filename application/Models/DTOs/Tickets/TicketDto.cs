using STO_Desk_backend.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace STO_Desk_backend.Models.DTOs.Tickets
{
    public class TicketDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Title of the ticket.
        /// </summary>
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(maximumLength: 256, ErrorMessage = "Title has a limit of 256 characters.")]
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Description of the ticket.
        /// </summary>
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(maximumLength: 512, ErrorMessage = "Description has a limit of 512 characters.")]
        public string Body { get; set; } = string.Empty;
        /// <summary>
        /// Ticket's status.
        /// </summary>
        public TicketStatus? Status { get; set; }
        /// <summary>
        /// Connected Theme.
        /// </summary>
        [Required(ErrorMessage = "ThemeId is required.")]
        public int? ThemeId { get; set; }
        /// <summary>
        /// Id of the user who created the ticket.
        /// </summary>
        [Required(ErrorMessage = "ClientId is required.")]
        public int? ClientId { get; set; }
        /// <summary>
        /// Id of the STO to which ticket is connected.
        /// </summary>
        [Required(ErrorMessage = "StoId is required.")]
        public int? StoId { get; set; }
        /// <summary>
        /// Id of the assigned user with mechanic role.
        /// </summary>
        public int? MechanicId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
    }
}
