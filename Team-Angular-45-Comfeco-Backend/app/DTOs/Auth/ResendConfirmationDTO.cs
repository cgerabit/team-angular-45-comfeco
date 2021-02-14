using System.ComponentModel.DataAnnotations;

namespace TeamAngular45Backend.DTOs.Auth
{
    public class ResendConfirmationDTO

    {


        [Required]
        public string Email { get; set; }
    }
}
