using BackendComfeco.DTOs.SocialNetwork;
using BackendComfeco.ValidationAtributes;

using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackendComfeco.DTOs.Users
{
    public class UpdateUserProfileDTO
    {
        [FileType(FileTypeGroup.Image)]
        [FileWeight(4096)]
        public IFormFile ProfilePicture { get; set; }

        [Required]
        public string RealName { get; set; }
        [Required]
        public string Biography { get; set; }
        [Required]
        public int SpecialtyId { get; set; }

        [Required]
        public int GenderId { get; set; }
        [Required]
        public int CountryId { get; set; }

        [Required]
        public DateTime BornDate { get; set; }
    }
}
