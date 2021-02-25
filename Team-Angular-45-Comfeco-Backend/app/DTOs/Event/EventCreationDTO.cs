using System;

namespace BackendComfeco.DTOs.Event
{
    public class EventCreationDTO
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public bool IsActive { get; set; }
    }
}
