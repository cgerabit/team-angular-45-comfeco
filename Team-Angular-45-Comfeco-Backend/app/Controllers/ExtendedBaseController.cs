using AutoMapper;

using BackendComfeco.DTOs.Shared;
using BackendComfeco.ExtensionMethods;
using BackendComfeco.Models;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendComfeco.Controllers
{
    public class ExtendedBaseController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ExtendedBaseController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        protected async Task<List<TDTO>> Get<TEntity, TDTO>() where TEntity : class
        {
            var entities = await context.Set<TEntity>()
                .AsNoTracking()
                .ToListAsync();

            return mapper.Map<List<TDTO>>(entities);
        }

        protected async Task<List<TDTO>> Get<TEntity, TDTO>(PaginationDTO paginationDTO)
            where TEntity : class
        {
            var queryable = context
                .Set<TEntity>()
                .AsQueryable();

            return await Get<TEntity, TDTO>(paginationDTO, queryable);
        }

        protected async Task<List<TDTO>> Get<TEntity, TDTO>(PaginationDTO paginationDTO,
            IQueryable<TEntity> queryable)
            where TEntity : class
        {
            await HttpContext
                .InserPaginationHeader(queryable);

            var entities = await queryable
                .Paginate(paginationDTO)
                .ToListAsync();

            return mapper.Map<List<TDTO>>(entities);
        }

        protected async Task<ActionResult<TDTO>> Get<TEntity, TDTO>(int id) where TEntity : class, IIdHelper
        {
            var entity = await context.Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return NotFound();
            }

            return mapper.Map<TDTO>(entity);
        }

        protected async Task<ActionResult> Post<TCreate, TEntity, TRead>
            (TCreate creationDTO, string routeName) where TEntity : class, IIdHelper
        {
            var entity = mapper.Map<TEntity>(creationDTO);
            context.Add(entity);
            await context.SaveChangesAsync();
            var dto = mapper.Map<TRead>(entity);

            return new CreatedAtRouteResult(routeName, new { id = entity.Id }, dto);
        }

        protected async Task<ActionResult> Put<TCreate, TEntity>
            (int id, TCreate creationDTO) where TEntity : class, IIdHelper
        {
            var exist = await context
                .Set<TEntity>()
                .AnyAsync(x => x.Id == id);

            if (!exist)
            {
                return NotFound();
            }

            var entity = mapper.Map<TEntity>(creationDTO);
            entity.Id = id;
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return NoContent();
        }

        protected async Task<ActionResult> Delete<TEntity>(int id) where TEntity : class, IIdHelper, new()
        {
            var exist = await context
                .Set<TEntity>()
                .AnyAsync(x => x.Id == id);

            if (!exist)
            {
                return NotFound();
            }

            context.Remove(new TEntity() { Id = id });
            await context.SaveChangesAsync();

            return NoContent();
        }

        protected async Task<ActionResult> Patch<TEntity, TDTO>(int id, JsonPatchDocument<TDTO> patchDocument)
            where TDTO : class
            where TEntity : class, IIdHelper
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var entityDb = await context
                .Set<TEntity>()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entityDb == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<TDTO>(entityDb);

            patchDocument.ApplyTo(dto, ModelState);

            var isValid = TryValidateModel(dto);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(dto, entityDb);

            await context.SaveChangesAsync();

            return NoContent();
        }

    }
}
