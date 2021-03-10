using Microsoft.AspNetCore.Http;

using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.Badge
{
    public class BadgeCreationDTO
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        public IFormFile BadgeIcon { get; set; }
        [Required]
        [MinLength(3)]
        public string Description { get; set; }
        [Required]
        [MinLength(3)]
        public string Instructions { get; set; }



    }
}
