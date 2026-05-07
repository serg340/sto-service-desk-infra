using System.ComponentModel.DataAnnotations;

namespace STO_Desk_backend.Models.DTOs.Auth
{
    public class RemoveRoleDto
    {
        /// <summary>
        /// Represents a role's name to be removed. 
        /// </summary>
        [Required(ErrorMessage = "RoleName is required.")]
        [StringLength(maximumLength: 128, ErrorMessage = "RoleName has a limit of 128 characters.")]
        public string RoleName { get; set; } = string.Empty;
    }
}
