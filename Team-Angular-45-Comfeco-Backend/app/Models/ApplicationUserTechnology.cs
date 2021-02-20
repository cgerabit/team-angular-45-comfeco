using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.Models
{
    public class ApplicationUserTechnology
    {
        [Required]
        public int TechnologyId { get; set; }
        [Required]
        public string UserId { get; set; }

       

        public ApplicationUser User { get; set; }
        public Technology Technology { get; set; }

        public bool IsPrincipal { get; set; }

    }
}
