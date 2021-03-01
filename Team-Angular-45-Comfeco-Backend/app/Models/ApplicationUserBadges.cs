using System;

namespace BackendComfeco.Models
{
    public class ApplicationUserBadges
    {

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
        public int BadgeId { get; set; }

        public Badge Badge { get; set; }

        public DateTime GetDate { get; set; } = DateTime.UtcNow;
    }
}
