using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace STO_Desk_backend.Models.DTOs.Auth
{
    public class SignUpDto
    {
        /// <summary>
        /// Unique user email.
        /// </summary>
        /// <example>myemail@gmail.com</example>
        [EmailAddress(ErrorMessage = "Not supported format.")]
        [Required(ErrorMessage = "Email is required.")]
        [StringLength(maximumLength: 128, ErrorMessage = "Email has a limit of 128 characters.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// User account password.
        /// </summary>
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(maximumLength: 128, MinimumLength = 8, ErrorMessage = "Password should be in range from 8 to 128 characters.")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{8,}$", ErrorMessage = "Password should include 1 uppercase, 1 lowercase letters and atleast 1 digit.")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Password repeat.
        /// </summary>
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "PasswordRepeat is required.")]
        [StringLength(maximumLength: 128, MinimumLength = 8, ErrorMessage = "PasswordRepeat should be in range from 8 to 128 charecters.")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{8,}$", ErrorMessage = "PasswordRepeat should include atleas t1 uppercase, 1 lowercase letters and 1 digit.")]
        [Compare("Password", ErrorMessage = "PasswordRepeat and Password do not match.")]
        public string PasswordRepeat { get; set; } = string.Empty;

    }
}
