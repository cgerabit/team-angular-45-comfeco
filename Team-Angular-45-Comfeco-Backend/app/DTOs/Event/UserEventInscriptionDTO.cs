

using System;

namespace BackendComfeco.DTOs.Event
{
    public class UserEventInscriptionDTO
    {
        public string UserId { get; set; }

        public int EventId { get; set; }

        public DateTime InscriptionDate { get; set; } 

        public bool IsActive { get; set; }


        public string EventName { get; set; }
    }
}
