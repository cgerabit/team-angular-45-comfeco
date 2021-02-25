using AutoMapper;

using BackendComfeco.DTOs.Event;
using BackendComfeco.DTOs.Shared;
using BackendComfeco.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendComfeco.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventsController : ExtendedBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public EventsController(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<EventDTO>>> GetEvents([FromQuery]PaginationDTO paginationDTO)
        {
            return await Get<Event, EventDTO>(paginationDTO);
        }

        [HttpGet("{id:int}", Name = "GetEvent")]
        public async Task<ActionResult<EventDTO>> GetEvent(int id)
        {
            return await Get<Event, EventDTO>(id);
        }

        [HttpGet("active")]
        public async Task<ActionResult<EventDTO>> GetActiveEvent()
        {
            var model = await context.Events.FirstOrDefaultAsync(e => e.IsActive);
            if (model == null)
            {
                return NotFound();
            }

            return mapper.Map<EventDTO>(model);

        }

        [HttpPost]
        public async Task<ActionResult> Post(EventCreationDTO eventCreationDTO)
        {
            if (eventCreationDTO.IsActive)
            {
                await HideOldEvent();
            }

            return await Post<EventCreationDTO, Event, EventDTO>(eventCreationDTO, "GetEvent");

        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, EventCreationDTO eventCreationDTO)
        {
            if (eventCreationDTO.IsActive)
            {
                await HideOldEvent();
            }

            return await Put<EventCreationDTO, Event>(id, eventCreationDTO);

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Event>(id);
        }


        private async Task HideOldEvent()
        {
            var eventActive = await context.Events.FirstOrDefaultAsync(x => x.IsActive);
            if(eventActive == null)
            {
                return;
            }
            context.Attach(eventActive);
            eventActive.IsActive = false;
            await context.SaveChangesAsync();
        }


    }
}
