using BackendComfeco.DTOs.Area;
using BackendComfeco.DTOs.Technology;

using System;
using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.WorkShop
{
    public class WorkShopDTO
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public string UserName { get; set; }


        [Required]
        [StringLength(100)]
        public string Title { get; set; }


        public TechnologyDTO Technology { get; set; }

        public AreaDTO Area { get; set; }

        [Required]
        public DateTime WorkShopDate { get; set; }
    }
}
