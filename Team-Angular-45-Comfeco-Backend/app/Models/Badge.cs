using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.Models
{
    public class Badge :IIdHelper
    {
        public int Id { get; set; }

        public string BadgeIcon { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]

        public string Description { get; set; }
        [Required]
        public string Instructions { get; set; }

        public List<ApplicationUserBadges> ApplicationUserBadges { get; set; }
    }
}
