using Microsoft.AspNetCore.Http;

using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.Technology
{
    public class TechnologyCreationDTO
    {


        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public IFormFile TechnologyIcon { get; set; }

        [Required]
        public int AreaId { get; set; }

    }
}
