using System.Collections.Generic;

namespace BackendComfeco.Models
{
    public class Badge :IIdHelper
    {
        public int Id { get; set; }

        public string BadgeIcon { get; set; }

        public string Name { get; set; }

        public List<ApplicationUserBadges> ApplicationUserBadges { get; set; }
    }
}
