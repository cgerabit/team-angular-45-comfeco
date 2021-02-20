using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.Models
{
    public class Technology
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public int Name { get; set; }

        public string TechnologyIcon { get; set; }

        [Required]
        public int AreaId { get; set; }

        public Area Area { get; set; }
        public List<ApplicationUserTechnology> ApplicationUserTechnologies { get; set; }

        public List<Workshop> Workshops { get; set; }



    }
}
