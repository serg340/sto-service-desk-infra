using System;
using System.ComponentModel.DataAnnotations;

namespace STO_Desk_backend.Models.DTOs.Regions
{
    public class RegionCreateDto
    {
        /// <summary>
        /// Name of the region.
        /// </summary>
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(maximumLength: 256, ErrorMessage = "Name has a limit of 256 characters.")]
        public string Name { get; set; } = string.Empty;
    }
}
