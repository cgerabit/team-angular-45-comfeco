using System;

namespace BackendComfeco.Models
{
    public class Event :IIdHelper
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public bool IsActive { get; set; } = false;
    }
}
