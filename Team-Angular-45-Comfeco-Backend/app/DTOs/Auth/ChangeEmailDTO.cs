using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.Auth
{
    public class ChangeEmailDTO
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string NewEmail { get; set; }
    }
}
