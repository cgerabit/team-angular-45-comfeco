using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.Comunity
{
    public class ComunityCreationDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Url { get; set; }
    }
}
