using BackendComfeco.Settings;

using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.Auth
{
    public class AuthCodeClaimDTO
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string SecurityKey { get; set; }

        [Required]
        public string Purpose { get; set; } = ApplicationConstants.ExternalLoginTokenPurposeName;
    }
}
