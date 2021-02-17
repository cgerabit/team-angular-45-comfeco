using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.Auth
{
    public class ExternalLoginDTO
    {
        [Required]
        public string Provider { get; set; }
        [Required]
        public string returnUrl { get; set; }
        [Required]
        public string SecurityKeyHash { get; set; }
    }
}
