using BackendComfeco.ValidationAtributes;

using Microsoft.AspNetCore.Http;

namespace BackendComfeco.DTOs.Users
{
    public class UpdateUserProfileDTO
    {
        [FileType(FileTypeGroup.Image)]
        [FileWeight(4096)]
        public IFormFile ProfilePicture { get; set; }

        public string RealName { get; set; }



    }
}
