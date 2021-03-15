using AutoMapper;

using BackendComfeco.DTOs.Area;
using BackendComfeco.DTOs.Auth;
using BackendComfeco.DTOs.Badge;
using BackendComfeco.DTOs.Comunity;
using BackendComfeco.DTOs.ContentCreators;
using BackendComfeco.DTOs.Country;
using BackendComfeco.DTOs.Event;
using BackendComfeco.DTOs.Gender;
using BackendComfeco.DTOs.Group;
using BackendComfeco.DTOs.SocialNetwork;
using BackendComfeco.DTOs.Sponsor;
using BackendComfeco.DTOs.Technology;
using BackendComfeco.DTOs.UserRelations;
using BackendComfeco.DTOs.Users;
using BackendComfeco.DTOs.WorkShop;
using BackendComfeco.Models;

using Microsoft.AspNetCore.Authentication;

using System.Linq;

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
            CreateMap<SocialNetworkCreationDTO, SocialNetwork>()
                .ForMember(m => m.SocialNetworkIcon, options => options.Ignore());

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
             .MapFrom(o => !string.IsNullOrEmpty(o.User.RealName) ? o.User.RealName : o.User.UserName))
            .ReverseMap();

            CreateMap<WorkShopCreationDTO, Workshop>();




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
                    .ForMember(s => s.SocialNetworkName, options => options.MapFrom(x => x.SocialNetwork.Name))
                    .ForMember(s => s.SocialNetworkIcon, options => options.MapFrom(x => x.SocialNetwork.SocialNetworkIcon))
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



            // =========================================================
            //                          Events
            // =========================================================

            CreateMap<Event, EventDTO>().ReverseMap();
            CreateMap<EventCreationDTO, Event>()
                .ForMember(e => e.EventPicture, options => options.Ignore());
            CreateMap<ApplicationUserEvents, UserEventInscriptionDTO>()
                .ForMember(e => e.EventName, o => o.MapFrom(p => p.Event.Name));

            // =========================================================
            //                          Countries
            // =========================================================
            CreateMap<Country, CountryDTO>().ReverseMap();
            CreateMap<CountryCreationDTO, Country>();
            // =========================================================
            //                          Genders
            // =========================================================
            CreateMap<Gender, GenderDTO>().ReverseMap();
            CreateMap<GenderCreationDTO, Gender>();
            // =========================================================
            //                          Badges
            // =========================================================

            CreateMap<Badge, BadgeDTO>().ReverseMap();
            CreateMap<BadgeCreationDTO, Badge>()
                .ForMember(b => b.BadgeIcon, options => options.Ignore());

            CreateMap<ApplicationUserBadges, UserBadgeDTO>()
                .ForMember(x => x.BadgeIcon, options => options.MapFrom(y => y.Badge.BadgeIcon))
                .ForMember(x => x.BadgeName, options => options.MapFrom(y => y.Badge.Name))
                .ForMember(x => x.Instructions, options => options.MapFrom(y => y.Badge.Instructions))
                .ForMember(x => x.Description, options => options.MapFrom(y => y.Badge.Description));

            // =========================================================
            //                          Groups
            // =========================================================


            CreateMap<Group, GroupDTO>()
                .ForMember(x => x.TechnologyName, options => options.MapFrom(x => x.Technology.Name))
                .ReverseMap();
            CreateMap<GroupCreationDTO, Group>()
                .ForMember(m => m.GroupImage, options => options.Ignore());

            CreateMap<Group, UserGroupDTO>()
                .ForMember(y => y.GroupName, options => options.MapFrom(g => g.Name))
                .ForMember(y => y.Members, options => options.MapFrom(g => g.Users.Select(u => new GroupMember
                {
                    Name = u.UserName,
                    ProfilePicture = u.ProfilePicture,
                    IsGroupLeader = u.IsGroupLeader
                })));
            // =========================================================
            //                          User Activity 
            // =========================================================

            CreateMap<UserActivity, UserActivityDTO>();

        }





    }
}

