using System.ComponentModel.DataAnnotations;

namespace TeamAngular45Backend.DTOs.Auth
{
    public class ApplicationUserCreationDTO
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}
