

using System;

namespace BackendComfeco.DTOs.Users
{
    public class UserActivityDTO
    {
        public string Text { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}
