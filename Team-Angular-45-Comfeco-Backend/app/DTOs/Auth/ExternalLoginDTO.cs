using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.Auth
{
    public class ExternalLoginDTO :BaseRedirectLoginDTO
    {
        [Required]
        public string Provider { get; set; }
    }
}
