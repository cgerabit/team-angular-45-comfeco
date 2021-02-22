using AutoMapper;

using BackendComfeco.DTOs.Area;
using BackendComfeco.DTOs.Auth;
using BackendComfeco.DTOs.Comunity;
using BackendComfeco.DTOs.ContentCreators;
using BackendComfeco.DTOs.SocialNetwork;
using BackendComfeco.DTOs.Sponsor;
using BackendComfeco.DTOs.Technology;
using BackendComfeco.DTOs.UserRelations;
using BackendComfeco.DTOs.Users;
using BackendComfeco.DTOs.WorkShop;
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

            // =========================================================
            //                          Workshop
            // =========================================================
            CreateMap<Workshop, WorkShopDTO>().ForMember(x => x.Area, options => options.MapFrom(o => new AreaDTO
            {
                Name = o.Technology.Area.Name,
                AreaIcon = o.Technology.Area.AreaIcon,
                Id = o.Technology.Area.Id

            })).ForMember(x => x.Technology, options => options.MapFrom(o => new TechnologyDTO
            {
                Id = o.Technology.Id,
                TechnologyIcon = o.Technology.TechnologyIcon,
                Name = o.Technology.Name,
                AreaId = o.Technology.AreaId
            })).ForMember(x => x.UserName, options => options
             .MapFrom(o => !string.IsNullOrEmpty(o.User.RealName) ? o.User.RealName : o.User.UserName));


            // =========================================================
            //                          Content Creator
            // =========================================================
            CreateMap<ApplicationUser, ContentCreatorDTO>()
                .ForMember(u => u.ApplicationUserTechnology, options => options.Ignore())
                .ForMember(u => u.UserId, options => options.MapFrom(x => x.Id))
                .ReverseMap();
            // =========================================================
            //                          Users
            // =========================================================
            CreateMap<ApplicationUserSocialNetwork, ApplicationUserSocialNetworkDTO>()
                .ReverseMap();

            CreateMap<UpdateUserProfileDTO, ApplicationUser>().ForMember(u => u.ProfilePicture, options => options.Ignore());


            CreateMap<ApplicationUserTechnology, UserTechnologyDTO>()
                .ForMember(m => m.Id, options => options.MapFrom(m => m.Technology.Id))
                .ForMember(m => m.Name, options => options.MapFrom(m => m.Technology.Name))
                .ForMember(m => m.AreaId, options => options.MapFrom(m => m.Technology.AreaId))
                .ForMember(m => m.TechnologyIcon, options => options.MapFrom(m => m.Technology.TechnologyIcon));
            CreateMap<ApplicationUser, UserProfileDTO>().ForMember(u => u.UserId, options => options.MapFrom(u => u.Id));

            CreateMap<Technology, UserTechnologyDTO>();


            CreateMap<UserSocialNetworkCreateDTO, ApplicationUserSocialNetwork>()
                .ReverseMap();

            CreateMap<UserTechnologyCreationDTO, ApplicationUserTechnology>()
                .ReverseMap();
        }




    }
}
