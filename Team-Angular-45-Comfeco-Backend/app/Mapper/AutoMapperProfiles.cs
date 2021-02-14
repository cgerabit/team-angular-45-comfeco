using AutoMapper;

using TeamAngular45Backend.DTOs.Auth;
using TeamAngular45Backend.Models;

using Microsoft.AspNetCore.Authentication;

namespace TeamAngular45Backend.Mapper
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
