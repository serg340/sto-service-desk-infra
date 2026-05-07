using System;
using System.ComponentModel.DataAnnotations;

namespace STO_Desk_backend.Models.DTOs.Stos
{
    public class StoCreateDto
    {
        /// <summary>
        /// Name of the STO.
        /// </summary>
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(maximumLength: 128, ErrorMessage = "Name has a limit of 128 characters.")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Description of the STO.
        /// </summary>
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(maximumLength: 512, ErrorMessage = "Description has a limit of 512 characters.")]
        public string Body { get; set; } = string.Empty;

        /// <summary>
        /// Id of the STO's owner.
        /// </summary>
        public int? OwnerId { get; set; }
        /// <summary>
        /// Id of the Region to which STO is connected.
        /// </summary>
        public int? RegionId { get; set; }
    }
}
