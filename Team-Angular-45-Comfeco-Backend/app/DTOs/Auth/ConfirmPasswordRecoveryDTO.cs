using System.ComponentModel.DataAnnotations;

namespace TeamAngular45Backend.DTOs.Auth
{
    public class ConfirmPasswordRecoveryDTO
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
