using Microsoft.AspNetCore.Http;

using System;

namespace BackendComfeco.DTOs.Event
{
    public class EventCreationDTO
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }
        public IFormFile EventPicture { get; set; }

        public bool IsTimer { get; set; }
    }
}
