using AutoMapper;

using BackendComfeco.DTOs.Group;
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
using System.Linq;
using System.Threading.Tasks;

namespace BackendComfeco.Controllers
{
    [ApiController]
    [Route("api/groups")]
    public class GroupsController :ExtendedBaseController
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IFileStorage fileStorage;

        public GroupsController(IMapper mapper, ApplicationDbContext applicationDbContext,
            IFileStorage fileStorage)
            :base(applicationDbContext,mapper)
        {
            this.mapper = mapper;
            this.applicationDbContext = applicationDbContext;
            this.fileStorage = fileStorage;
        }

        [HttpGet]
        public async Task<ActionResult<List<GroupDTO>>> Get([FromQuery] GroupFilter groupFilter)
        {
            var queryable = applicationDbContext.Groups.AsQueryable();

            queryable = queryable.Include(g=>g.Technology);
            if (!string.IsNullOrEmpty(groupFilter.Name))
            {
                queryable = queryable
                    .Where(g => g.Name.Contains(groupFilter.Name));
            }

            if (groupFilter.TechnologyId.HasValue)
            {
                queryable = queryable.Where(g => g.TechnologyId == groupFilter.TechnologyId.Value);
            }

            var groups = await queryable.ToListAsync();


            return mapper.Map<List<GroupDTO>>(groups);


        }
        [HttpGet("{id}",Name ="GetGroup")]
        public async Task<ActionResult<GroupDTO>> Get(int id)
        {
            var group = await applicationDbContext.Groups.Include(g => g.Technology).FirstOrDefaultAsync(g => g.Id == id);
            if (group==null)
            {
                return NotFound();
            }

            return mapper.Map<GroupDTO>(group);
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromForm]GroupCreationDTO groupCreationDTO)
        {
            var entity = mapper.Map<Group>(groupCreationDTO);

            if (groupCreationDTO.GroupImage != null)
            {
                entity.GroupImage = await SaveImg(groupCreationDTO.GroupImage);
            }


            applicationDbContext.Add(entity);
            await applicationDbContext.SaveChangesAsync();

            var dto = mapper.Map<GroupDTO>(entity);


            return new CreatedAtRouteResult("GetGroup", new { id = entity.Id }, dto);

        }



        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id , [FromForm]GroupCreationDTO groupCreationDTO)
        {
            var entity = await applicationDbContext.Groups.FirstOrDefaultAsync(g => g.Id == id);
            if (entity==null)
            {
                return NotFound();
            }

            mapper.Map(groupCreationDTO, entity);
            if (groupCreationDTO.GroupImage != null)
            {

                if (entity.GroupImage!=null)
                {
                    await fileStorage.RemoveFile(entity.GroupImage, ApplicationConstants.ImageContainerNames.GroupImagesContainer);

                }
                    
                entity.GroupImage= await SaveImg(groupCreationDTO.GroupImage);
            }

            applicationDbContext.Entry(entity).State = EntityState.Modified;
           await applicationDbContext.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("{id}")]
        public  Task<ActionResult> Delete(int id)
        {
            return Delete<Group>(id);
        }

        private async Task<string> SaveImg(IFormFile formFile)
        {
            var bytes = await formFile.ConvertToByteArray();


            return await fileStorage.SaveFile(bytes, Path.GetExtension(formFile.FileName), ApplicationConstants.ImageContainerNames.GroupImagesContainer, formFile.ContentType, Guid.NewGuid().ToString());


        }
    }
}
