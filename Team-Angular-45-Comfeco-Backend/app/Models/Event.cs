using System;
using System.Collections.Generic;

namespace BackendComfeco.Models
{
    public class Event :IIdHelper
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public bool IsActive { get; set; } = false;

        public List<ApplicationUserEvents> ApplicationUserEvents { get; set; }
    }
}
