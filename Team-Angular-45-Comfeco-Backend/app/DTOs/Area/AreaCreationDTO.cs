using Microsoft.AspNetCore.Http;

using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.Area
{
    public class AreaCreationDTO
    {

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public IFormFile AreaIcon { get; set; }
    }
}
