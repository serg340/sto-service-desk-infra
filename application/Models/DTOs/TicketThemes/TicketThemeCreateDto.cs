using System.ComponentModel.DataAnnotations;

namespace STO_Desk_backend.Models.DTOs.TicketThemes
{
    public class TicketThemeCreateDto
    {
        /// <summary>
        /// Name of the theme.
        /// </summary>
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(maximumLength: 128, ErrorMessage = "Name has a limit of 128 characters.")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Id of the category to which theme is connected.
        /// </summary>
        [Required(ErrorMessage = "CategoryId is required.")]
        public int? CategoryId { get; set; }
    }
}
