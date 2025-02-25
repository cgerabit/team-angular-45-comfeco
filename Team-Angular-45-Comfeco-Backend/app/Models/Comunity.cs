﻿using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.Models
{
    public class Comunity : IIdHelper
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^(ht|f)tp(s?)\\:\\/\\/[0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*(:(0-9)*)*(\\/?)( [a-zA-Z0-9\\-\\.\\?\\,\'\\/\\\\+&%\\$#_]*)?$", ErrorMessage = "Por favor ingresa una url valida")]
        public string Url { get; set; }
    }
}
