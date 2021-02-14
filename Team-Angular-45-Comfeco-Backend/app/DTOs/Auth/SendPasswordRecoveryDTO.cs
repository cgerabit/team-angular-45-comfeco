using System.ComponentModel.DataAnnotations;

namespace TeamAngular45Backend.DTOs.Auth
{
    public class SendPasswordRecoveryDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
