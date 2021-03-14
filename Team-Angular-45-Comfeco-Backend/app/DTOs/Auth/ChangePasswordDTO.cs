using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.Auth
{
    public class ChangePasswordDTO
    {
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
