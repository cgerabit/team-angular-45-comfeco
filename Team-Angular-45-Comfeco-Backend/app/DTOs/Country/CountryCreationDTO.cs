using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.Country
{
    public class CountryCreationDTO
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
    }
}
