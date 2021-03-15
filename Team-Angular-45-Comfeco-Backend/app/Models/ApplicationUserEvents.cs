using System;

namespace BackendComfeco.Models
{
    public class ApplicationUserEvents
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
        public int EventId { get; set; }

        public Event Event { get; set; }

        public DateTime InscriptionDate { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
    }
}
