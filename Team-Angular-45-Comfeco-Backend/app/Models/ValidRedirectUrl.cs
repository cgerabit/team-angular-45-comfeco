using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.Models
{
    public class ValidRedirectUrl
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Url { get; set; }
    }
}
