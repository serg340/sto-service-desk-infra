using STO_Desk_backend.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace STO_Desk_backend.Models.DTOs.RoleTickets
{
    public class RoleTicketCreateDto
    {
        /// <summary>
        /// Title of the role ticket.
        /// </summary>
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(maximumLength: 256, ErrorMessage = "Title has a limit of 256 characters.")]
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Description of the role ticket.
        /// </summary>
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(maximumLength: 512, ErrorMessage = "Description has a limit of 512 characters.")]
        public string Body { get; set; } = string.Empty;
        /// <summary>
        /// Connected Theme.
        /// </summary>
        [Required(ErrorMessage = "ThemeId is required.")]
        public int? ThemeId { get; set; }
        /// <summary>
        /// Id of the user who created the role ticket.
        /// </summary>
        [Required(ErrorMessage = "UserId is required.")]
        public int? UserId { get; set; }
        /// <summary>
        /// Id of the STO to which role ticket is connected.
        /// </summary>
        [Required(ErrorMessage = "StoId is required.")]
        public int? StoId { get; set; }
    }
}
