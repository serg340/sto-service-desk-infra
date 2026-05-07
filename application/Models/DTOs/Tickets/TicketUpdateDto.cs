using STO_Desk_backend.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace STO_Desk_backend.Models.DTOs.Tickets
{
    public class TicketUpdateDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Title of the ticket.
        /// </summary>
        [StringLength(maximumLength: 256, ErrorMessage = "Title has a limit of 256 characters.")]
        public string? Title { get; set; }
        /// <summary>
        /// Description of the ticket.
        /// </summary>
        [StringLength(maximumLength: 512, ErrorMessage = "Description has a limit of 512 characters.")]
        public string? Body { get; set; }

        /// <summary>
        /// Connected Theme.
        /// </summary>
        public int? ThemeId { get; set; }
    }
}
