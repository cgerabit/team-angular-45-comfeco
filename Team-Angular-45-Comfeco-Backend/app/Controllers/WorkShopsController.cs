using AutoMapper;

using BackendComfeco.DTOs.Area;
using BackendComfeco.DTOs.SocialNetwork;
using BackendComfeco.DTOs.UserRelations;
using BackendComfeco.DTOs.WorkShop;
using BackendComfeco.ExtensionMethods;
using BackendComfeco.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendComfeco.Controllers
{
    [ApiController]
    [Route("api/workshops")]

    public class WorkShopsController : ExtendedBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public WorkShopsController(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<WorkShopDTO>>> Get ([FromQuery]WorkShopFilter workShopFilter)
        {
            var queryable = context.Workshops.AsQueryable();

            queryable = queryable.Include(x => x.User)
                .Include(x => x.Technology)
                .ThenInclude(x => x.Area);

            if (!string.IsNullOrEmpty(workShopFilter.TitleContains))
            {
                queryable = queryable.Where(w => w.Title.Contains(workShopFilter.TitleContains));
            }

            if (!string.IsNullOrEmpty(workShopFilter.UserNameContains))
            {
                queryable = queryable.Where(w =>w.User.RealName.Contains(workShopFilter.UserNameContains));

            }
            if (!string.IsNullOrEmpty(workShopFilter.UserId))
            {
                queryable = queryable.Where(w => w.UserId == workShopFilter.UserId);


            }
            if(workShopFilter.AreaIds!=null && workShopFilter.AreaIds.Count > 0)
            {
                queryable = queryable.Where(w => workShopFilter.AreaIds.Contains(w.Technology.AreaId));
            } 

            if(workShopFilter.TechnologyIds !=null && workShopFilter.TechnologyIds.Count > 0)
            {
                queryable = queryable.Where(w => workShopFilter.TechnologyIds.Contains(w.TechnologyId));
            }

            if(workShopFilter.AfterThan != default)
            {
                queryable = queryable.Where(w => w.WorkShopDate > workShopFilter.AfterThan);
            }
            if (workShopFilter.BeforeThan != default)
            {
                queryable = queryable.Where(w => w.WorkShopDate < workShopFilter.BeforeThan);
            }

            await HttpContext.InserPaginationHeader(queryable);

            queryable = queryable.Paginate(workShopFilter.PaginationDTO).OrderByDescending(x=>x.WorkShopDate);


            var entities = await queryable.ToListAsync();


            var dtos = mapper.Map<List<WorkShopDTO>>(entities);

            var userIds = dtos.Select(x => x.UserId);

            var principalSocialNetworks = await context.ApplicationUserSocialNetworks.Where(x => x.IsPrincipal && userIds.Contains(x.UserId)).ToListAsync();

            principalSocialNetworks.ForEach(s =>
            {
                var entity = dtos.FirstOrDefault(x => x.UserId == s.UserId);

                if (entity != null)
                {
                    entity.PrincipalSocialNetwork = mapper.Map<ApplicationUserSocialNetworkDTO>(s);

                }

            });
          
            return dtos;


        } 

        [HttpGet("{id:int}",Name = "GetWorkshop")]
        public async Task<ActionResult<WorkShopDTO>> Get(int id)
        {
            var entity = await context.Workshops.Include(x => x.Technology)
                .ThenInclude(x => x.Area)
                .Include(x => x.User).FirstOrDefaultAsync(x=>x.Id==id);

            if(entity == null)
            {
                return NotFound();
            }

            var dto =  mapper.Map<WorkShopDTO>(entity);

            var principalSocialNetwork = await context.ApplicationUserSocialNetworks.FirstOrDefaultAsync(x => x.IsPrincipal && x.UserId == entity.UserId);

            if (principalSocialNetwork != null)
            {
                dto.PrincipalSocialNetwork = mapper.Map<ApplicationUserSocialNetworkDTO>(principalSocialNetwork);
            }

            return dto;
        }

        [HttpPost]

        public async Task<ActionResult<WorkShopDTO>> Post(WorkShopCreationDTO workShopCreationDTO)
        {
            return await Post<WorkShopCreationDTO,Workshop,WorkShopDTO>(workShopCreationDTO, "GetWorkshop");
        }

        [HttpPut("{id:int}")]

        public async Task<ActionResult<WorkShopDTO>> Put(int id, WorkShopCreationDTO workShopCreationDTO)
        {
            return await Put<WorkShopCreationDTO,Workshop>(id,workShopCreationDTO);
        }

        [HttpDelete("{id:int}")]

        public async Task<ActionResult<WorkShopDTO>> Delete(int id)
        {
            return await Delete<Workshop>(id);
        }

    }
}
