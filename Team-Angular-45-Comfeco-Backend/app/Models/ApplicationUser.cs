using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;

namespace BackendComfeco.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfilePicture { get; set; }

        public string RealName { get; set; }

        public string Biography { get; set; }

        public List<ApplicationUserTechnology> ApplicationUserTechnology { get; set; }

        public List<ApplicationUserSocialNetwork> ApplicationUserSocialNetworks { get; set; }

        public List<Workshop> Workshops { get; set; }

        public List<ApplicationUserEvents> ApplicationUserEvents { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; } = new List<IdentityUserRole<string>>();

        public Area Specialty { get; set; }
        public int? SpecialtyId { get; set; }

        public Gender Gender { get; set; }
        public int? GenderId { get; set; }

        public Country Country { get; set; }
        public int? CountryId { get; set; }

        public int? GroupId { get; set; }
        public Group Group { get; set; }
         public DateTime BornDate{ get; set; }

        public List<ApplicationUserBadges> ApplicationUserBadges { get; set; }

        

    }
}
