using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.Models
{
    public class Gender :IIdHelper
    {

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public List<ApplicationUser> Users { get; set; }
    }
}
