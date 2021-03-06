using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.Models
{
    public class SocialNetwork :IIdHelper
    {

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Hostname { get; set; }

        public string SocialNetworkIcon { get; set; }
        public List<ApplicationUserSocialNetwork> ApplicationUserSocialNetworks { get; set; }
    }
}
