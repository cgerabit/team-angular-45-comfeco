using Microsoft.AspNetCore.Identity;

using System.Collections.Generic;

namespace BackendComfeco.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfilePicture { get; set; }

        public string RealName { get; set; }

        public List<ApplicationUserTechnology> ApplicationUserTechnology { get; set; }

        public List<ApplicationUserSocialNetwork> ApplicationUserSocialNetworks { get; set; }

        public List<Workshop> Workshops { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; } = new List<IdentityUserRole<string>>();

        
        
    }
}
