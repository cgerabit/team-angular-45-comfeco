using BackendComfeco.DTOs.Technology;

namespace BackendComfeco.DTOs.Users
{
    public class UserTechnologyDTO : TechnologyDTO
    {
        public bool IsPrincipal { get; set; } = false;
    }
}
