
using System;

namespace BackendComfeco.DTOs.Users
{
    public class UserBadgeDTO
    {
        public int BadgeId { get; set; }

        public string BadgeName { get; set; }

        public string BadgeIcon { get; set; }

        public string Description { get; set; }
        public string Instructions { get; set; }
        public DateTime GetDate { get; set; }
    }

}
