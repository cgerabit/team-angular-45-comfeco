using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.Auth
{
    public class ConfirmChangeUsername
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Token { get; set; }

        [Required]
        public string NewUsername { get; set; }
    }
}
