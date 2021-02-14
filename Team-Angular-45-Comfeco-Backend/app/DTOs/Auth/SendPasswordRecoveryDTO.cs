using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.Auth
{
    public class SendPasswordRecoveryDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
