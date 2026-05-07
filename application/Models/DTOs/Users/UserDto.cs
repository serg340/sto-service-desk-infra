using System;
using System.ComponentModel.DataAnnotations;

namespace STO_Desk_backend.Models.DTOs.Users
{
    public class UserDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Username. Not an indexer, can be used as an alternative instead of email for display.
        /// </summary>
        [StringLength(maximumLength: 128, ErrorMessage = "Username has a limit of 128 characters.")]
        public string? UserName { get; set; }
        /// <summary>
        /// Unique user email.
        /// </summary>
        /// <example>myemail@gmail.com</example>
        [EmailAddress(ErrorMessage = "Not supported format.")]
        [Required(ErrorMessage = "Email is required.")]
        [StringLength(maximumLength: 128, ErrorMessage = "Email has a limit of 128 characters.")]
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Email confirmation status.
        /// </summary>
        public bool EmailConfirmed { get; set; }
        /// <summary>
        /// Unique user phone number.
        /// </summary>
        /// <example>+380YYXXXXXXX</example>
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(maximumLength: 32, ErrorMessage = "PhoneNumber has a limit of 128 characters.")]
        public string? PhoneNumber { get; set; }
        /// <summary>
        /// Phone number confirmation status.
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Id of the Region to which user is connected.
        /// </summary>
        public int? RegionId { get; set; }
        /// <summary>
        /// Id of the STO to which user is connected.
        /// </summary>
        public int? StoId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
