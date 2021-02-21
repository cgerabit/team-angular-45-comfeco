using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.Comunity
{
    public class ComunityCreationDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^(ht|f)tp(s?)\\:\\/\\/[0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*(:(0-9)*)*(\\/?)( [a-zA-Z0-9\\-\\.\\?\\,\'\\/\\\\+&%\\$#_]*)?$", ErrorMessage = "Por favor ingresa una url valida")]
        public string Url { get; set; }
    }
}
