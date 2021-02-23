using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackendComfeco.DTOs.Users
{
    public class UserSocialNetworkCreateDTO
    {
        [Required]
        public int SocialNetworkId { get; set; }
        [Required]
        [RegularExpression("^(ht)tp(s?)\\:\\/\\/[\\w\\-]+(\\.[\\w\\-]+)+[/#?]?.*$", ErrorMessage = "Por favor ingresa una url valida")]
        public string Url { get; set; }

        public bool IsPrincipal { get; set; } = false;
    }
}
