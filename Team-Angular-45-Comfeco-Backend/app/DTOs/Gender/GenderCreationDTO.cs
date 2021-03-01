using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.Gender
{
    public class GenderCreationDTO
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
    }
}
