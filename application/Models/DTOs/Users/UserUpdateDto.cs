using System.ComponentModel.DataAnnotations;

namespace STO_Desk_backend.Models.DTOs.Users
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Username. Not an indexer, can be used as an alternative instead of email for display.
        /// </summary>
        [StringLength(maximumLength: 128, ErrorMessage = "Username has a limit of 128 characters.")]
        public string? UserName { get; set; }

        /// <summary>
        /// Unique user phone number.
        /// </summary>
        /// <example>+380YYXXXXXXX</example>
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(maximumLength: 32, ErrorMessage = "PhoneNumber has a limit of 128 characters.")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Id of the Region to which user is connected.
        /// </summary>
        public int? RegionId { get; set; }
    }
}
