using System.ComponentModel.DataAnnotations;

namespace STO_Desk_backend.Models.DTOs.Shared
{
    /// <summary>
    /// Is used in to return specific data based on Enum.
    /// </summary>
    public class EnumItemDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Name of the item.
        /// </summary>
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(maximumLength: 128, ErrorMessage = "Name has a limit of 128 characters.")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Alternate name for display instead of basic Name.
        /// </summary>
        [StringLength(maximumLength: 128, ErrorMessage = "DisplayName has a limit of 128 characters.")]
        public string? DisplayName { get; set; }
    }
}
