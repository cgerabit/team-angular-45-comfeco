
using AutoMapper;

using BackendComfeco.DTOs.Area;
using BackendComfeco.DTOs.Shared;
using BackendComfeco.ExtensionMethods;
using BackendComfeco.Helpers;
using BackendComfeco.Models;
using BackendComfeco.Settings;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BackendComfeco.Controllers
{
    [ApiController]
    [Route("api/areas")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Policy =ApplicationConstants.Roles.AdminRoleName)]
    public class AreasController : ExtendedBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStorage fileStorage;

        public AreasController(ApplicationDbContext context, IMapper mapper,
            IFileStorage fileStorage) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileStorage = fileStorage;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<AreaDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            return await Get<Area, AreaDTO>(paginationDTO);
        }

        [HttpGet("{id:int}", Name = "GetArea")]
        [AllowAnonymous]
        public async Task<ActionResult<AreaDTO>> Get(int id)
        {
            return await Get<Area, AreaDTO>(id);
        }

        [HttpPost]

        public async Task<ActionResult> Post([FromForm] AreaCreationDTO areaCreationDTO)
        {
            var entity = mapper.Map<Area>(areaCreationDTO);

            if (areaCreationDTO.AreaIcon != null)
            {
                entity.AreaIcon = await SaveIcon(areaCreationDTO.AreaIcon);
            }

            context.Add(entity);
            await context.SaveChangesAsync();


            return new CreatedAtRouteResult("GetArea", new { id = entity.Id }, mapper.Map<AreaDTO>(entity));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] AreaCreationDTO areaCreationDTO)
        {
            var entity = await context.Areas.FirstOrDefaultAsync(area => area.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            entity = mapper.Map(areaCreationDTO, entity);
            if (areaCreationDTO.AreaIcon != null)
            {
                if (!string.IsNullOrEmpty(entity.AreaIcon))
                {
                    await fileStorage.RemoveFile(entity.AreaIcon, ApplicationConstants.ImageContainerNames.AreaContainer);

                }
                entity.AreaIcon = await SaveIcon(areaCreationDTO.AreaIcon);

            }

            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Area>(id);
        }


        private async Task<string> SaveIcon(IFormFile sponsorIcon)
        {
            var bytes = await sponsorIcon.ConvertToByteArray();
            return await fileStorage.SaveFile(bytes,
                 Path.GetExtension(sponsorIcon.FileName),
                 ApplicationConstants.ImageContainerNames.AreaContainer,
                 sponsorIcon.ContentType, Guid.NewGuid().ToString());
        }

    }
}
