using System;
using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.Models
{
    public class UserActivity
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        public string Text { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

    }
}
