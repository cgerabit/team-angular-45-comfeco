using BackendComfeco.DTOs.Technology;
using BackendComfeco.DTOs.UserRelations;
using BackendComfeco.DTOs.Users;

using System.Collections.Generic;

namespace BackendComfeco.DTOs.ContentCreators
{
    public class ContentCreatorDTO
    {
        public string ProfilePicture { get; set; }
        public string RealName { get; set; }

        public List<UserTechnologyDTO> ApplicationUserTechnology { get; set; }

        public List<ApplicationUserSocialNetworkDTO> ApplicationUserSocialNetworks { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
    }
}
