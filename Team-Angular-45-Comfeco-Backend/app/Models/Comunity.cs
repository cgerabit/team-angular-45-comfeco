﻿using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.Models
{
    public class Comunity : IIdHelper
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string Url { get; set; }
    }
}
