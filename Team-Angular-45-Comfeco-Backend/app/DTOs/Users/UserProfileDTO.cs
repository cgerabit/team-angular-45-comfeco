using System;

namespace BackendComfeco.DTOs.Users
{
    public class UserProfileDTO
    {
        public string ProfilePicture { get; set; }

        public string RealName { get; set; }

        public string UserId { get; set; }

        public string Biography { get; set; }

        public int SpecialtyId { get; set; }
        public int GenderId { get; set; }

        public int CountryId { get; set; }

        public string UserName { get; set; }

        public DateTime BornDate { get; set; }

    }
}
