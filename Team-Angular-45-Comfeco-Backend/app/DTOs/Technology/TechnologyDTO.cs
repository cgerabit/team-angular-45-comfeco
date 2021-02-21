

using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.Technology
{
    public class TechnologyDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string TechnologyIcon { get; set; }

        [Required]
        public int AreaId { get; set; }
    }
}
