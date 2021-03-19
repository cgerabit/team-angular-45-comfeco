using AutoMapper;

using BackendComfeco.DTOs.Shared;
using BackendComfeco.DTOs.SocialNetwork;
using BackendComfeco.ExtensionMethods;
using BackendComfeco.Helpers;
using BackendComfeco.Models;
using BackendComfeco.Settings;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BackendComfeco.Controllers
{
    [ApiController]
    [Route("api/socialnetworks")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Policy =ApplicationConstants.Roles.AdminRoleName)]
    public class SocialNetworksController : ExtendedBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStorage fileStorage;

        public SocialNetworksController(ApplicationDbContext context, IMapper mapper,
            IFileStorage fileStorage) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileStorage = fileStorage;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<SocialNetworkDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            return await Get<SocialNetwork, SocialNetworkDTO>(paginationDTO);
        }

        [HttpGet("{id:int}", Name = "GetSocialNetwork")]
        [AllowAnonymous]
        public async Task<ActionResult<SocialNetworkDTO>> Get(int id)
        {

            return await Get<SocialNetwork, SocialNetworkDTO>(id);
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromForm]SocialNetworkCreationDTO socialNetworkCreationDTO)
        {

            var entity = mapper.Map<SocialNetwork>(socialNetworkCreationDTO);

            if (socialNetworkCreationDTO.SocialNetworkIcon!=null)
            {
                entity.SocialNetworkIcon = await SaveIcon(socialNetworkCreationDTO.SocialNetworkIcon);
            }

            context.Add(entity);
            await context.SaveChangesAsync();

            var dto = mapper.Map<SocialNetworkDTO>(entity);

            return new CreatedAtRouteResult("GetSocialNetwork", new { id = entity.Id }, dto);

           
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put([FromRoute] int id, [FromForm]SocialNetworkCreationDTO socialNetworkCreationDTO)
        {
            var entity = await context.SocialNetworks.FirstOrDefaultAsync(s => s.Id == id);

            if (entity ==null)
            {
                return NotFound();
            }

            mapper.Map(socialNetworkCreationDTO, entity);

            if (socialNetworkCreationDTO.SocialNetworkIcon != null)
            {
                if(entity.SocialNetworkIcon != null)
                {
                    await fileStorage.RemoveFile(entity.SocialNetworkIcon, ApplicationConstants.ImageContainerNames.SocialNetworkIconContainer);

                }
                entity.SocialNetworkIcon = await SaveIcon(socialNetworkCreationDTO.SocialNetworkIcon);


            }

            context.Attach(entity).State = EntityState.Modified;

            await context.SaveChangesAsync();

            return NoContent();

        }

        private async Task<string> SaveIcon(IFormFile icon)
        {
            var content = await icon.ConvertToByteArray();

            string iconPath = await fileStorage.SaveFile(content, Path.GetExtension(icon.FileName), ApplicationConstants.ImageContainerNames.SocialNetworkIconContainer, icon.ContentType, Guid.NewGuid().ToString());

            return iconPath;
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<SocialNetwork>(id);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<SocialNetworkCreationDTO> jsonPatchDocument)
        {
            return await Patch<SocialNetwork, SocialNetworkCreationDTO>(id, jsonPatchDocument);
        }

    }
}
