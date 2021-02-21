using System;
using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.Area
{
    public class WorkShopCreationDTO
    {

        [Required]
        public string UserId { get; set; }


        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public int TechnologyId { get; set; }

        [Required]
        public DateTime WorkShopDate { get; set; }
    }
}
