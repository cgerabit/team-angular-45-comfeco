﻿using AutoMapper;

using BackendComfeco.DTOs.Shared;
using BackendComfeco.DTOs.UserRelations;
using BackendComfeco.DTOs.Users;
using BackendComfeco.ExtensionMethods;
using BackendComfeco.Helpers;
using BackendComfeco.Models;
using BackendComfeco.Settings;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BackendComfeco.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;
        private readonly IFileStorage fileStorage;

        public UsersController(ApplicationDbContext applicationDbContext,
            IMapper mapper,
            IFileStorage fileStorage)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
            this.fileStorage = fileStorage;
        }

        [HttpPut("profile/{userId}")]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> UpdateUserProfile(string userId, [FromForm] UpdateUserProfileDTO updateUserProfileDTO)
        {
            //TODO
            //Chequear el role de administrador o que el usuario este editando su propio perfil

            var userIdC = HttpContext.User.Claims.FirstOrDefault(y => y.Type == ClaimTypes.NameIdentifier);
            if (userIdC == null || userIdC.Value != userId)
            {
                return Forbid();
            }

            var user = await applicationDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            mapper.Map(updateUserProfileDTO, user);


            if (updateUserProfileDTO.ProfilePicture != null)
            {
                var bytes = await updateUserProfileDTO.ProfilePicture.ConvertToByteArray();

                if (!string.IsNullOrEmpty(user.ProfilePicture))
                {
                    await fileStorage.RemoveFile(user.ProfilePicture, ApplicationConstants.ImageContainerNames.ProfilePicturesContainer);

                }


                user.ProfilePicture = await fileStorage.SaveFile(bytes, Path.GetExtension(updateUserProfileDTO.ProfilePicture.FileName), ApplicationConstants.ImageContainerNames.ProfilePicturesContainer, updateUserProfileDTO.ProfilePicture.ContentType, Guid.NewGuid().ToString());
            }


            applicationDbContext.Entry(user).State = EntityState.Modified;


            bool haveSociableBadge = await applicationDbContext.ApplicationUserBadges.AnyAsync(x => x.UserId == userId && x.BadgeId == 1);
            if (!haveSociableBadge)
            {
                var sociableBadge = new ApplicationUserBadges
                {
                    UserId = userId,
                    BadgeId = 1,
                    GetDate = DateTime.UtcNow
                };

                applicationDbContext.Add(sociableBadge);
            }

            await applicationDbContext.SaveChangesAsync();

            return NoContent();

        }

        [HttpGet("profile")]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme,Policy =ApplicationConstants.Roles.AdminRoleName)]
        public async Task<ActionResult<List<UserProfileDTO>>> GetUsers([FromQuery] PaginationDTO paginationDTO,
            [FromQuery] UserFilter userFilter)
        {

            // TODO : 
            // proteger este endpoint
            var queryable = applicationDbContext.Users.AsQueryable();

            if (!string.IsNullOrEmpty(userFilter.Username))
            {
                queryable = queryable.Where(x => x.UserName.ToLower().Contains(userFilter.Username.ToLower()));
            }

            await HttpContext.InserPaginationHeader(queryable);
            queryable = queryable.Paginate(paginationDTO);

            var users = await queryable.ToListAsync();



            return mapper.Map<List<UserProfileDTO>>(users);
        }

        [HttpGet("profile/{userId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserProfileDTO>> GetUser(string userId)
        {
            var userIdC = HttpContext.User.Claims.FirstOrDefault(y => y.Type == ClaimTypes.NameIdentifier);
            if (userIdC == null || userIdC.Value != userId)
            {
                return Forbid();
            }
            var user = await applicationDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            return mapper.Map<UserProfileDTO>(user);
        }

        [HttpGet("profile/{userId}/badges")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<UserBadgeDTO>>> GetUserBadges(string userId)
        {
            var userIdC = HttpContext.User.Claims.FirstOrDefault(y => y.Type == ClaimTypes.NameIdentifier);
            if (userIdC == null || userIdC.Value != userId)
            {
                return Forbid();
            }

            var badges = await applicationDbContext.ApplicationUserBadges
                .Include(b => b.Badge).Where(x => x.UserId == userId).OrderBy(y => y.GetDate).ToListAsync();

            var dto = mapper.Map<List<UserBadgeDTO>>(badges);

            return dto;

        }

        [HttpGet("profile/{userId}/socialnetworks")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<ApplicationUserSocialNetworkDTO>>> GetSocialNetWork(string userId)
        {
            var userIdC = HttpContext.User.Claims.FirstOrDefault(y => y.Type == ClaimTypes.NameIdentifier);
            if (userIdC == null || userIdC.Value != userId)
            {
                return Forbid();
            }
            var socialNetworks = await applicationDbContext.Users
                .Include(x => x.ApplicationUserSocialNetworks)
                .ThenInclude(x => x.SocialNetwork)
                .Where(u => u.Id == userId)
                .AsNoTracking()
                .Select(u => u.ApplicationUserSocialNetworks)
                .FirstOrDefaultAsync();
            if (socialNetworks == null)
            {
                return NotFound();
            }


            var dto = mapper.Map<List<ApplicationUserSocialNetworkDTO>>(socialNetworks);



            return dto;

        }
        [HttpDelete("profile/{userId}/socialnetworks/{socialNetworkId:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> DeleteUserSocialNetwork(string userId, int socialNetworkId)
        {
            var userIdC = HttpContext.User.Claims.FirstOrDefault(y => y.Type == ClaimTypes.NameIdentifier);
            if (userIdC == null || userIdC.Value != userId)
            {
                return Forbid();
            }
            var socialNetwork = await applicationDbContext.ApplicationUserSocialNetworks.FindAsync(userId, socialNetworkId);

            if (socialNetwork == null)
            {
                return NotFound();
            }

            if (socialNetwork.IsPrincipal)
            {
                var remaimingUserSocialNetwork = await applicationDbContext
                    .ApplicationUserSocialNetworks
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.SocialNetworkId != socialNetwork.SocialNetworkId);

                if (remaimingUserSocialNetwork != null)
                {
                    applicationDbContext.Attach(remaimingUserSocialNetwork);
                    remaimingUserSocialNetwork.IsPrincipal = true;
                }
            }
            applicationDbContext.Entry(socialNetwork).State = EntityState.Deleted;

            await applicationDbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("profile/{userId}/socialnetworks")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> CreateOrReplaceSocialNetwork(string userId, UserSocialNetworkCreateDTO socialNetworkCreationDTO)
        {
            var userIdC = HttpContext.User.Claims.FirstOrDefault(y => y.Type == ClaimTypes.NameIdentifier);
            if (userIdC == null || userIdC.Value != userId)
            {
                return Forbid();
            }

            var userExist = await applicationDbContext.Users.AnyAsync(x => x.Id == userId);
            if (!userExist)
            {
                return NotFound("Usuario no existe");
            }
            var socialNetworkExist = await applicationDbContext.SocialNetworks.AnyAsync(x => x.Id == socialNetworkCreationDTO.SocialNetworkId);
            if (!socialNetworkExist)
            {
                return NotFound("Red social incorrecta");
            }

            var currentUserSocialNetwork = await applicationDbContext
                .ApplicationUserSocialNetworks.FindAsync(userId, socialNetworkCreationDTO.SocialNetworkId);
            // Asegurar que el usuario tenga al menos una red social principal
            if (currentUserSocialNetwork != null && currentUserSocialNetwork.IsPrincipal)
            {
                socialNetworkCreationDTO.IsPrincipal = true;
            }
            else if (!socialNetworkCreationDTO.IsPrincipal)
            {
                bool isThereSomePrincipal = await applicationDbContext
                    .ApplicationUserSocialNetworks
                    .Where(s => s.UserId == userId)
                    .AnyAsync(s => s.IsPrincipal);
                socialNetworkCreationDTO.IsPrincipal =
                    !isThereSomePrincipal;
            }
            else
            {
                var principalSocialNetwork = await applicationDbContext.ApplicationUserSocialNetworks.FirstOrDefaultAsync(s => s.UserId == userId && s.IsPrincipal);

                if (principalSocialNetwork != null)
                {
                    applicationDbContext.Attach(principalSocialNetwork);

                    principalSocialNetwork.IsPrincipal = false;
                }
            }

            if (currentUserSocialNetwork == null)
            {
                var entity = mapper.Map<ApplicationUserSocialNetwork>(socialNetworkCreationDTO);
                entity.UserId = userId;

                applicationDbContext.Add(entity);
            }
            else
            {
                mapper.Map(socialNetworkCreationDTO, currentUserSocialNetwork);
                applicationDbContext.Entry(currentUserSocialNetwork).State = EntityState.Modified;
            }

            await applicationDbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("profile/{userId}/fillsocialnetworks")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> CreateOrReplaceSocialNetworks(string userId, List<UserSocialNetworkCreateDTO> socialNetworkCreationDTO)
        {
            var userIdC = HttpContext.User.Claims.FirstOrDefault(y => y.Type == ClaimTypes.NameIdentifier);
            if (userIdC == null || userIdC.Value != userId)
            {
                return Forbid();
            }
            var user = await applicationDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user==null)
            {
                return NotFound();
            }

            for(int i = 0; i < socialNetworkCreationDTO.Count; i++)
            {
                var SocialNetwork = await 
                    applicationDbContext.ApplicationUserSocialNetworks
                    .FirstOrDefaultAsync(s => s.SocialNetworkId == socialNetworkCreationDTO[i].SocialNetworkId && s.UserId == userId);

                if (SocialNetwork !=null)
                {
                    applicationDbContext.Attach(SocialNetwork);
                    SocialNetwork.Url = socialNetworkCreationDTO[i].Url;
                    await applicationDbContext.SaveChangesAsync();
                }
                else
                {
                    var socialNetworkUser = mapper.Map<ApplicationUserSocialNetwork>(socialNetworkCreationDTO[i]);

                    socialNetworkUser.UserId = userId;

                    applicationDbContext.Add(socialNetworkUser);

                    await applicationDbContext.SaveChangesAsync();
                }

            }

            return NoContent();
        }

        [HttpGet("profile/{userId}/technologies")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<UserTechnologyDTO>>> GetUserTechnologies(string userId)
        {
            var userIdC = HttpContext.User.Claims.FirstOrDefault(y => y.Type == ClaimTypes.NameIdentifier);
            if (userIdC == null || userIdC.Value != userId)
            {
                return Forbid();
            }
            bool userExist = await applicationDbContext.Users.AnyAsync(u => u.Id == userId);

            if (!userExist)
            {
                return NotFound();
            }

            var technologies = await
                applicationDbContext.ApplicationUserTechnologies.Include(x => x.Technology).Where(x => x.UserId == userId).ToListAsync();


            var dto = mapper.Map<List<UserTechnologyDTO>>(technologies);



            return dto;


        }

        [HttpDelete("profile/{userId}/technologies/{technologyId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> DeleteTechnology(string userId, int technologyId)
        {
            var userIdC = HttpContext.User.Claims.FirstOrDefault(y => y.Type == ClaimTypes.NameIdentifier);
            if (userIdC == null || userIdC.Value != userId)
            {
                return Forbid();
            }
            var technology = await applicationDbContext.ApplicationUserTechnologies.FindAsync(userId, technologyId);

            if (technology == null)
            {
                return NotFound();
            }

            if (technology.IsPrincipal)
            {
                var remainingTechnology = await applicationDbContext.ApplicationUserTechnologies.FirstOrDefaultAsync(t => t.UserId == userId && t.TechnologyId != technologyId);

                if (remainingTechnology != null)
                {
                    applicationDbContext.Attach(remainingTechnology);
                    remainingTechnology.IsPrincipal = true;
                }
            }
            applicationDbContext.Entry(technology).State = EntityState.Deleted;

            await applicationDbContext.SaveChangesAsync();

            return NoContent();
        }



        [HttpPost("profile/{userId}/technologies")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> CreateOrReplaceTechnology(string userId, UserTechnologyCreationDTO userTechnologyCreationDTO)
        {
            var userIdC = HttpContext.User.Claims.FirstOrDefault(y => y.Type == ClaimTypes.NameIdentifier);
            if (userIdC == null || userIdC.Value != userId)
            {
                return Forbid();
            }
            bool userExist = await applicationDbContext.Users.AnyAsync(u => u.Id == userId);
            if (!userExist)
            {
                return NotFound();
            }
            bool technologyExist = await applicationDbContext.Technologies.AnyAsync(u => u.Id == userTechnologyCreationDTO.TechnologyId);
            if (!technologyExist)
            {
                return NotFound();
            }

            var userTechnology = await applicationDbContext.ApplicationUserTechnologies.FirstOrDefaultAsync(u => u.UserId == userId && u.TechnologyId == userTechnologyCreationDTO.TechnologyId);
            //Asegurar tecnologia principal
            if (userTechnology != null && userTechnology.IsPrincipal)
            {
                userTechnologyCreationDTO.IsPrincipal = true;
            }
            else if (!userTechnologyCreationDTO.IsPrincipal)
            {
                bool principalExist = await applicationDbContext.ApplicationUserTechnologies.AnyAsync(u => u.UserId == userId && u.IsPrincipal);

                userTechnologyCreationDTO.IsPrincipal = !principalExist;
            }
            else
            {
                var principalUserTechnology = await applicationDbContext.ApplicationUserTechnologies.FirstOrDefaultAsync(u => u.UserId == userId && u.IsPrincipal);

                if (principalUserTechnology != null)
                {
                    applicationDbContext.Attach(principalUserTechnology);
                    principalUserTechnology.IsPrincipal = false;
                }
            }


            if (userTechnology != null)
            {
                mapper.Map(userTechnologyCreationDTO, userTechnology);

                applicationDbContext.Entry(userTechnology).State = EntityState.Modified;
            }
            else
            {
                var entity = mapper.Map<ApplicationUserTechnology>(userTechnologyCreationDTO);
                entity.UserId = userId;

                applicationDbContext.Add(entity);
            }

            await applicationDbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("profile/{userId}/activity")]
        public async Task<ActionResult<List<UserActivityDTO>>> GetActivities(string userId)
        {
            var userActivities = await applicationDbContext
                .UserActivities
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.Date)
                .Take(10)
                .ToListAsync();

            var dto = mapper.Map<List<UserActivityDTO>>(userActivities);

            return dto;

        }



    }
}
