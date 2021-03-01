using AutoMapper;

using BackendComfeco.DTOs.Badge;
using BackendComfeco.ExtensionMethods;
using BackendComfeco.Helpers;
using BackendComfeco.Models;
using BackendComfeco.Settings;

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
    [Route("api/badges")]
    public class BadgesController : ExtendedBaseController
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext context;
        private readonly IFileStorage fileStorage;

        public BadgesController(IMapper mapper,
            ApplicationDbContext context,
            IFileStorage fileStorage)
            : base(context, mapper)
        {
            this.mapper = mapper;
            this.context = context;
            this.fileStorage = fileStorage;
        }
        [HttpGet]
        public async Task<ActionResult<List<BadgeDTO>>> Get()
        {
            return await Get<Badge, BadgeDTO>();
        }

        [HttpGet("{id:int}", Name = "GetBadge")]
        public async Task<ActionResult<BadgeDTO>> Get(int id)
        {
            return await Get<Badge, BadgeDTO>(id);
        }

        [HttpPost]

        public async Task<ActionResult> Post([FromForm] BadgeCreationDTO badgeCreationDTO)
        {
            var entity = mapper.Map<Badge>(badgeCreationDTO);

            if (badgeCreationDTO.BadgeIcon != null)
            {
                entity.BadgeIcon = await SaveIcon(badgeCreationDTO.BadgeIcon);
            }

            context.Add(entity);
            await context.SaveChangesAsync();


            return new CreatedAtRouteResult("GetBadge", new { id = entity.Id }, mapper.Map<BadgeDTO>(entity));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] BadgeCreationDTO badgeCreationDTO)
        {
            var entity = await context.Badges.FirstOrDefaultAsync(area => area.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            entity = mapper.Map(badgeCreationDTO, entity);
            if (badgeCreationDTO.BadgeIcon != null)
            {
                if (!string.IsNullOrEmpty(entity.BadgeIcon))
                {
                    await fileStorage.RemoveFile(entity.BadgeIcon, ApplicationConstants.ImageContainerNames.BadgePicturesContainer);

                }
                entity.BadgeIcon = await SaveIcon(badgeCreationDTO.BadgeIcon);

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
                 ApplicationConstants.ImageContainerNames.BadgePicturesContainer,
                 sponsorIcon.ContentType, Guid.NewGuid().ToString());
        }



    }
}
