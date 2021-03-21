using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.Auth
{
    public class ApplicationUserCreationDTO
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]

        public string Password { get; set; }

        [Required]
        [MinLength(4)]
        public string UserName { get; set; }
    }
}
