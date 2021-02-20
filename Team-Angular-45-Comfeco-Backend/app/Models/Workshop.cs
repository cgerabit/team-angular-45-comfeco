using System;
using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.Models
{
    public class Workshop
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public int TechnologyId { get; set; }

        public Technology Technology { get; set; }

        [Required]
        public DateTime WorkShopDate { get; set; }

    }
}
