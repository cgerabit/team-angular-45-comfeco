using AutoMapper;

using BackendComfeco.DTOs.Shared;
using BackendComfeco.DTOs.Sponsor;
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
    [Route("api/sponsors")]
    public class SponsorsController : ExtendedBaseController
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;
        private readonly IFileStorage fileStorage;

        public SponsorsController(ApplicationDbContext applicationDbContext,
            IMapper mapper,
            IFileStorage fileStorage) :
            base(applicationDbContext, mapper)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
            this.fileStorage = fileStorage;
        }

        [HttpGet]
        public async Task<ActionResult<List<SponsorDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            return await Get<Sponsor,SponsorDTO>(paginationDTO);

        }

        [HttpGet("{id:int}", Name = "GetSponsor")]
        public async Task<ActionResult<SponsorDTO>> Get(int Id)
        {
            return await Get<Sponsor,SponsorDTO>(Id);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] SponsorCreationDTO sponsorCreationDTO)
        {

            var entity = mapper.Map<Sponsor>(sponsorCreationDTO);


            if (sponsorCreationDTO.SponsorIcon != null)
            {

                entity.SponsorIcon = await SaveIcon(sponsorCreationDTO.SponsorIcon);

            }

            applicationDbContext.Add(entity);
            await applicationDbContext.SaveChangesAsync();

            var readDTO = mapper.Map<SponsorDTO>(entity);

            return new CreatedAtRouteResult("GetSponsor", new { id = readDTO.Id }, readDTO);

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] SponsorCreationDTO sponsorCreationDTO)
        {
            var entity = await applicationDbContext.Sponsors.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null) { return NotFound(); }

            entity = mapper.Map(sponsorCreationDTO, entity);

            if (sponsorCreationDTO.SponsorIcon != null)
            {
                if (!string.IsNullOrEmpty(entity.SponsorIcon))
                {
                    await fileStorage.RemoveFile(entity.SponsorIcon, ApplicationConstants.ImageContainerNames.SponsorContainer);
                }
                entity.SponsorIcon = await SaveIcon(sponsorCreationDTO.SponsorIcon);
            }

            applicationDbContext.Entry(entity).State = EntityState.Modified;

            await applicationDbContext.SaveChangesAsync();


            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult>Delete(int id)
        {
            return await Delete<Sponsor>(id);

        }

        private async Task<string> SaveIcon(IFormFile sponsorIcon)
        {
            var bytes = await sponsorIcon.ConvertToByteArray();
            return await fileStorage.SaveFile(bytes,
                 Path.GetExtension(sponsorIcon.FileName),
                 ApplicationConstants.ImageContainerNames.SponsorContainer,
                 sponsorIcon.ContentType, Guid.NewGuid().ToString());
        }




    }
}
