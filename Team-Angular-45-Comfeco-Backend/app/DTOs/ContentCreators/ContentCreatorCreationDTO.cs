using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.ContentCreators
{
    public class ContentCreatorCreationDTO
    {
     
        [Required]
        public string UserId { get; set; }
    }
}
