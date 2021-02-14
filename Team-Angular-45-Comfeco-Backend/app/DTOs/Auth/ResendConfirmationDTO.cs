using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.Auth
{
    public class ResendConfirmationDTO

    {


        [Required]
        public string Email { get; set; }
    }
}
