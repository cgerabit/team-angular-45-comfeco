using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.Auth
{
    public class BaseRedirectLoginDTO
    {
        
        [Required]
        public string returnUrl { get; set; }
        [Required]
        public string SecurityKeyHash { get; set; }
        
    }
}
