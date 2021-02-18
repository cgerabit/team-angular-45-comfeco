using System;

namespace BackendComfeco.Models
{
    public class UserAuthenticationCode
    {
        public string UserId { get; set; }

        public string SecurityKey { get; set; }
        
        public string Token { get; set; }

        public DateTime Expiration { get; set; } = DateTime.UtcNow.AddMinutes(5);


    }


}
