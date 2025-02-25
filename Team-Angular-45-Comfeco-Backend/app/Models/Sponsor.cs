﻿using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.Models
{
    public class Sponsor: IIdHelper
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }


        public string SponsorIcon { get; set; }
    }
}
