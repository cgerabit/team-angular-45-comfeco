using System;
using System.Collections.Generic;

namespace BackendComfeco.Models
{
    public class Event :IIdHelper
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EventPicture { get; set; }
        public DateTime Date { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsTimer { get; set; } = false;
        public List<ApplicationUserEvents> ApplicationUserEvents { get; set; }
    }
}
