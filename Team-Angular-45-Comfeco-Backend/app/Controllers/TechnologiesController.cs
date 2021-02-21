using AutoMapper;

using BackendComfeco.DTOs.Shared;
using BackendComfeco.DTOs.Technology;
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
    [Route("api/technologies")]
    public class TechnologiesController : ExtendedBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStorage fileStorage;

        public TechnologiesController(ApplicationDbContext context, IMapper mapper,
            IFileStorage fileStorage) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileStorage = fileStorage;
        }

        [HttpGet]
        public async Task<ActionResult<List<TechnologyDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            return await Get<Technology, TechnologyDTO>(paginationDTO);
        }

        [HttpGet("{id:int}", Name = "GetTechnology")]

        public async Task<ActionResult<TechnologyDTO>> Get(int id)
        {
            return await Get<Technology, TechnologyDTO>(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm]TechnologyCreationDTO technologyCreationDTO)
        {

            var entity = mapper.Map<Technology>(technologyCreationDTO);

            if (technologyCreationDTO.TechnologyIcon != null)
            {
                entity.TechnologyIcon = await SaveIcon(technologyCreationDTO.TechnologyIcon);
            }

            context.Add(entity);
            await context.SaveChangesAsync();

            return new CreatedAtRouteResult("GetTechnology", new { id = entity.Id }, mapper.Map<TechnologyDTO>(entity));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] TechnologyCreationDTO technologyCreationDTO)
        {
            var entity = await context.Technologies.FirstOrDefaultAsync(t => t.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            entity = mapper.Map(technologyCreationDTO, entity);

            if (technologyCreationDTO.TechnologyIcon != null)
            {
                if (!string.IsNullOrEmpty(entity.TechnologyIcon))
                {
                    await fileStorage.RemoveFile(entity.TechnologyIcon, ApplicationConstants.ImageContainerNames.TechnologyContainer);
                }
                entity.TechnologyIcon = await SaveIcon(technologyCreationDTO.TechnologyIcon);
            }

            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]

        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Technology>(id);
        }

        private async Task<string> SaveIcon(IFormFile technologyIcon)
        {
            var bytes = await technologyIcon.ConvertToByteArray();
            return await fileStorage.SaveFile(bytes,
                 Path.GetExtension(technologyIcon.FileName),
                 ApplicationConstants.ImageContainerNames.TechnologyContainer,
                 technologyIcon.ContentType, Guid.NewGuid().ToString());
        }


    }
}
