using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.Models
{
    public class Area :IIdHelper
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string AreaIcon { get; set; }

        public List<Technology> Technologies { get; set; }

        public List<ApplicationUser> Specialists { get; set; }



    }
}
