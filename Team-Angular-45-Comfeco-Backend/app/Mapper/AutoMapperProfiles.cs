using AutoMapper;

using BackendComfeco.DTOs.Auth;
using BackendComfeco.Models;

using Microsoft.AspNetCore.Authentication;

namespace BackendComfeco.Mapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // =========================================================
            //                          AUTH
            // =========================================================
            CreateMap<ApplicationUserCreationDTO, ApplicationUser>();
            CreateMap<ApplicationUser, UserInfo>().ForMember(member => member.UserId, options=> options.MapFrom( u => u.Id));
            CreateMap<AuthenticationScheme, ExternalProvidersDTO>();
        }
    }
}
