using AutoMapper;

using BackendComfeco.DTOs.ContentCreators;
using BackendComfeco.DTOs.Shared;
using BackendComfeco.DTOs.Technology;
using BackendComfeco.DTOs.Users;
using BackendComfeco.ExtensionMethods;
using BackendComfeco.Models;
using BackendComfeco.Settings;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendComfeco.Controllers
{
    [ApiController]
    [Route("api/users/contentcreators")]
    public class ContentCreatorsController : ControllerBase
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public ContentCreatorsController(ApplicationDbContext applicationDbContext,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<ContentCreatorDTO>>> Get([FromQuery] PaginationDTO paginationDTO,
            [FromQuery] ContentCreatorFilters contentCreatorFilters)
        {
            var queryable = GetContentCreatorQueryable();

            if (!string.IsNullOrEmpty(contentCreatorFilters.UserNameContains))
            {
                queryable = queryable.Where(u => u.RealName.Contains(contentCreatorFilters.UserNameContains) || u.UserName.Contains(contentCreatorFilters.UserNameContains));
            }

            if (contentCreatorFilters.TechnologyId != null && contentCreatorFilters.TechnologyId.Count > 0)
            {
                queryable = queryable.Where(u => u.ApplicationUserTechnology.Any(t => contentCreatorFilters.TechnologyId.Contains(t.TechnologyId)));
            }


            await HttpContext.InserPaginationHeader(queryable);

            queryable = queryable.Paginate(paginationDTO);

            var entities = await queryable.ToListAsync();


            var dto = mapper.Map<List<ContentCreatorDTO>>(entities);

            //Mapear tecnologias
            var technologyIdUserGroup = entities.Select(x => new
            {
                x.Id,
                TechnologyIds = x.ApplicationUserTechnology.Select(y => y.TechnologyId)

            }).ToList();

            var totalIds = technologyIdUserGroup.Aggregate(new List<int>(), (before, next) =>
            {
                before.AddRange(next.TechnologyIds);
                return before;
            }).Distinct();

            var technologies = await applicationDbContext.Technologies.Where(t => totalIds.Contains(t.Id)).ToListAsync();

            technologyIdUserGroup.ForEach(g =>
            {
                var creator = dto.FirstOrDefault(c => g.Id == c.UserId);

                var creatorTechnologies = g.TechnologyIds.Select(x => technologies.FirstOrDefault(t => t.Id == x));

                creator.ApplicationUserTechnology = mapper.Map<List<UserTechnologyDTO>>(creatorTechnologies);

                var currentUser = entities.FirstOrDefault(u => u.Id == g.Id);

                int principalIndex = currentUser.ApplicationUserTechnology.FindIndex(x => x.IsPrincipal);
                if (principalIndex != -1)
                {
                    creator.ApplicationUserTechnology[principalIndex].IsPrincipal = true;
                }
                

            });


            return dto;
        }

        [HttpPost]
        public async Task<ActionResult> AddContentCreator(ContentCreatorCreationDTO contentCreatorCreationDTO)
        {
            var user = await applicationDbContext.Users.FirstOrDefaultAsync(u => u.Id == contentCreatorCreationDTO.UserId);

            if (user==null)
            {
                return NotFound();
            }


            var result=  await userManager.AddToRoleAsync(user, ApplicationConstants.Roles.ContentCreatorRoleName);

            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result);
        }


        [HttpGet("{userId}")]

        public async Task<ActionResult<ContentCreatorDTO>> GetContentCreator(string userId) 
        {
            var queryable = GetContentCreatorQueryable();

            var user = await queryable.FirstOrDefaultAsync(u => u.Id == userId);
            if (user ==null)
            {
                return NotFound();
            }

            var dto = mapper.Map<ContentCreatorDTO>(user);


            var userTechnologiesIds = user.ApplicationUserTechnology.Select(x => x.TechnologyId);

            var technologies =await applicationDbContext.Technologies
                .Where(t =>  userTechnologiesIds.Contains(t.Id))
                .ToListAsync();

            dto.ApplicationUserTechnology = mapper.Map<List<UserTechnologyDTO>>(technologies);

            int principalIndex = user.ApplicationUserTechnology.FindIndex(x => x.IsPrincipal == true);
            if (principalIndex != -1)
            {
                dto.ApplicationUserTechnology[principalIndex].IsPrincipal = true;
            }
         

            return dto;
        
        }


        [HttpDelete("{userId}")]
        public async Task<ActionResult> Delete(string userId)
        {
            var queryable = GetContentCreatorQueryable();
            var user = await queryable.FirstOrDefaultAsync(u => u.Id == userId);

            if (user==null)
            {
                return NotFound();
            }

            var result = await userManager.RemoveFromRoleAsync(user, ApplicationConstants.Roles.ContentCreatorRoleName);

            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(result);
        }

        
        




        private IQueryable<ApplicationUser> GetContentCreatorQueryable()
        {
            var queryable = applicationDbContext.Users.AsQueryable();
            queryable = queryable
                .Include(u => u.Roles)
                .Include(u => u.ApplicationUserSocialNetworks)
                .Include(u => u.ApplicationUserTechnology)
                .Where(u => u.Roles.Any(r => r.RoleId == ApplicationConstants.Roles.ContentCreatorRoleId));

            return queryable;
        }


    }
}
