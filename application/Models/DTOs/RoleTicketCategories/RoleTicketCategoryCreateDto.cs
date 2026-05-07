using System.ComponentModel.DataAnnotations;

namespace STO_Desk_backend.Models.DTOs.RoleTicketCategories
{
    public class RoleTicketCategoryCreateDto
    {
        /// <summary>
        /// Name of the category.
        /// </summary>
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(maximumLength: 128, ErrorMessage = "Name has a limit of 128 characters.")]
        public string Name { get; set; } = string.Empty;
    }
}
