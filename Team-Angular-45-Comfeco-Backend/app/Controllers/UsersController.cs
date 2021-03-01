using AutoMapper;

using BackendComfeco.DTOs.Event;
using BackendComfeco.DTOs.Shared;
using BackendComfeco.DTOs.SocialNetwork;
using BackendComfeco.DTOs.Technology;
using BackendComfeco.DTOs.UserRelations;
using BackendComfeco.DTOs.Users;
using BackendComfeco.ExtensionMethods;
using BackendComfeco.Helpers;
using BackendComfeco.Models;
using BackendComfeco.Settings;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;

using MimeKit.Encodings;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public async Task<ActionResult> UpdateUserProfile(string userId, [FromForm] UpdateUserProfileDTO updateUserProfileDTO)
        {
            //TODO
            //Chequear el role de administrador o que el usuario este editando su propio perfil

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


            await applicationDbContext.SaveChangesAsync();


            return NoContent();

        }

        [HttpGet("profile")]
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
        public async Task<ActionResult<UserProfileDTO>> GetUser(string userId)
        {
            var user = await applicationDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            return mapper.Map<UserProfileDTO>(user);
        }
      

        [HttpGet("profile/{userId}/socialnetworks")]
        public async Task<ActionResult<List<ApplicationUserSocialNetworkDTO>>> GetSocialNetWork(string userId)
        {
            var socialNetworks = await applicationDbContext.Users
                .Include(x => x.ApplicationUserSocialNetworks)
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
        public async Task<ActionResult> DeleteUserSocialNetwork( string userId, int socialNetworkId)
        {
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

                if(remaimingUserSocialNetwork != null)
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
        public async Task<ActionResult> CreateOrReplaceSocialNetwork(string userId, UserSocialNetworkCreateDTO socialNetworkCreationDTO)
        {
            var userExist =await applicationDbContext.Users.AnyAsync(x => x.Id == userId);
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
            if(currentUserSocialNetwork !=null && currentUserSocialNetwork.IsPrincipal)
            {
                socialNetworkCreationDTO.IsPrincipal = true;
            }
            else if (!socialNetworkCreationDTO.IsPrincipal)
            {
                bool isThereSomePrincipal =await  applicationDbContext
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
            
            if(currentUserSocialNetwork == null)
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

        [HttpGet("profile/{userId}/technologies")]
        public async Task<ActionResult<List<UserTechnologyDTO>>> GetUserTechnologies(string userId)
        {

            bool userExist = await applicationDbContext.Users.AnyAsync(u => u.Id == userId);

            if (!userExist)
            {
                return NotFound();
            }

            var technologies = await 
                applicationDbContext.ApplicationUserTechnologies.Include(x=>x.Technology).Where(x => x.UserId == userId).ToListAsync();


            var dto = mapper.Map<List<UserTechnologyDTO>>(technologies);



            return dto;


        }

        [HttpDelete("profile/{userId}/technologies/{technologyId}")]
        public async Task<ActionResult> DeleteTechnology(string userId, int technologyId)
        {
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
        public async Task<ActionResult> CreateOrReplaceTechnology(string userId,UserTechnologyCreationDTO userTechnologyCreationDTO)
        {
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
            if(userTechnology!=null && userTechnology.IsPrincipal)
            {
                userTechnologyCreationDTO.IsPrincipal = true;
            }
            else if (!userTechnologyCreationDTO.IsPrincipal)
            {
                bool principalExist = await applicationDbContext.ApplicationUserTechnologies.AnyAsync( u => u.UserId == userId && u.IsPrincipal);

                userTechnologyCreationDTO.IsPrincipal = !principalExist;
            }
            else
            {
                var principalUserTechnology = await applicationDbContext.ApplicationUserTechnologies.FirstOrDefaultAsync(u => u.UserId == userId && u.IsPrincipal);

                if (principalUserTechnology!=null)
                {
                    applicationDbContext.Attach(principalUserTechnology);
                    principalUserTechnology.IsPrincipal = false;
                }
            }


            if (userTechnology != null)
            {
                mapper.Map(userTechnologyCreationDTO,userTechnology);

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



    }
}
