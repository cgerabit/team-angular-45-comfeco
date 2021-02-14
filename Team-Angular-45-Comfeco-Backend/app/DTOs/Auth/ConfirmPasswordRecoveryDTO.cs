using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.Auth
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
