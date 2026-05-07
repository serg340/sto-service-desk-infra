using STO_Desk_backend.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace STO_Desk_backend.Models.DTOs.RoleTicketThemes
{
    public class RoleTicketThemeUpdateDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Name of the theme.
        /// </summary>
        [StringLength(maximumLength: 128, ErrorMessage = "Name has a limit of 128 characters.")]
        public string? Name { get; set; } = string.Empty;
        /// <summary>
        /// Represents the role that is added to user.
        /// </summary>
        public TargetRole? TargetRole { get; set; }
        /// <summary>
        /// Id of the category to which theme is connected.
        /// </summary>
        public int? CategoryId { get; set; }
    }
}
