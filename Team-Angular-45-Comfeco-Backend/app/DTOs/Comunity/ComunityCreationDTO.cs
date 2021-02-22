using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.Comunity
{
    public class ComunityCreationDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^(ht)tp(s?)\\:\\/\\/[\\w\\-]+(\\.[\\w\\-]+)+[/#?]?.*$", ErrorMessage = "Por favor ingresa una url valida")]
        public string Url { get; set; }
    }
}
