using BackendComfeco.ValidationAtributes;

using Microsoft.AspNetCore.Http;

using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.SocialNetwork
{
    public class SocialNetworkCreationDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [RegularExpression("^(ht)tp(s?)\\:\\/\\/[0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*(:(0-9)*)*(\\/?)( [a-zA-Z0-9\\-\\.\\?\\,\'\\/\\\\+&%\\$#_]*)?$",ErrorMessage ="Por favor ingresa una url valida")]
        public string Hostname { get; set; }
        [FileType(FileTypeGroup.Image)]
        [FileWeight(4096)]
        public IFormFile SocialNetworkIcon{ get; set; }
    }
}
