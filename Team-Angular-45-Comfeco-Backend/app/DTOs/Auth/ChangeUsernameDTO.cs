using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.Auth
{
    public class ChangeUsernameDTO
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string newUsername { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
