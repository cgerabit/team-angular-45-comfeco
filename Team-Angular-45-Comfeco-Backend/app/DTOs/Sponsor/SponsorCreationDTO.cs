using BackendComfeco.ValidationAtributes;

using Microsoft.AspNetCore.Http;

using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.Sponsor
{
    public class SponsorCreationDTO
    {


        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [FileType(FileTypeGroup.Image)]
        [FileWeight(4096)]
        public IFormFile SponsorIcon { get; set; }
    }
}
