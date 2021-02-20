using System.Collections.Generic;

namespace BackendComfeco.Models
{
    public class ApplicationUserSocialNetwork
    {
        public int SocialNetworkId { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public bool IsPrincipal { get; set; } = false;

        public string Url { get; set; }


        public SocialNetwork SocialNetwork {get; set; }
    }
}
