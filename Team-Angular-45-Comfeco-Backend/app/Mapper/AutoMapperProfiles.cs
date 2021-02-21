using AutoMapper;

using BackendComfeco.DTOs.Area;
using BackendComfeco.DTOs.Auth;
using BackendComfeco.DTOs.Comunity;
using BackendComfeco.DTOs.SocialNetwork;
using BackendComfeco.DTOs.Sponsor;
using BackendComfeco.DTOs.Technology;
using BackendComfeco.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

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
            CreateMap<ApplicationUser, UserInfo>()
                .ForMember(member => member.UserId, options => options.MapFrom(u => u.Id));
            CreateMap<AuthenticationScheme, ExternalProvidersDTO>();

            // =========================================================
            //                          SPONSORS 
            // =========================================================
            CreateMap<Sponsor, SponsorDTO>().ReverseMap();

            CreateMap<SponsorCreationDTO, Sponsor>().ForMember(x => x.SponsorIcon,
                options => options.Ignore());
            // =========================================================
            //                          Communities 
            // =========================================================
            CreateMap<ComunityCreationDTO, Comunity>();
            CreateMap<Comunity, ComunityDTO>().ReverseMap();

            // =========================================================
            //                          Areas
            // =========================================================
            CreateMap<Area, AreaDTO>().ReverseMap();
            CreateMap<AreaCreationDTO, Area>()
                .ForMember(x => x.AreaIcon, options => options.Ignore());
            // =========================================================
            //                          Technology
            // =========================================================
            CreateMap<Technology, TechnologyDTO>().ReverseMap();
            CreateMap<TechnologyCreationDTO, Technology>()
                .ForMember(x => x.TechnologyIcon, options => options.Ignore());

            // =========================================================
            //                          Social Network
            // =========================================================

            CreateMap<SocialNetwork, SocialNetworkDTO>().ReverseMap();
            CreateMap<SocialNetworkCreationDTO, SocialNetwork>();
        }


    }
}
