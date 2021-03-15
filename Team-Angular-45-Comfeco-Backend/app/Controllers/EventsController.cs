using AutoMapper;

using BackendComfeco.DTOs.Event;
using BackendComfeco.DTOs.Shared;
using BackendComfeco.ExtensionMethods;
using BackendComfeco.Helpers;
using BackendComfeco.Models;
using BackendComfeco.Settings;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BackendComfeco.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventsController : ExtendedBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStorage fileStorage;

        public EventsController(ApplicationDbContext context, IMapper mapper,
            IFileStorage fileStorage) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileStorage = fileStorage;
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
            var model = await context.Events.FirstOrDefaultAsync(e => e.IsTimer);
            if (model == null)
            {
                return NotFound();
            }

            return mapper.Map<EventDTO>(model);

        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm]EventCreationDTO eventCreationDTO)
        {
            if (eventCreationDTO.IsTimer)
            {
                await HideOldEvent();
            }

            var entity = mapper.Map<Event>(eventCreationDTO);
            if(eventCreationDTO.EventPicture!=null)
            {
                entity.EventPicture = await SaveImage(eventCreationDTO.EventPicture);
            }

            context.Add(entity);
            await context.SaveChangesAsync();


            var dto = mapper.Map<EventDTO>(entity);

            return new CreatedAtRouteResult("GetEvent", new { id = entity.Id }, dto);

        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm]EventCreationDTO eventCreationDTO)
        {

            var currentEvent = await context.Events.FirstOrDefaultAsync(e => e.Id == id);
            if (currentEvent == null)
            {
                return NotFound();
            }

            if (eventCreationDTO.IsTimer)
            {
                await HideOldEvent();
            }

            mapper.Map(eventCreationDTO, currentEvent);
            if (eventCreationDTO.EventPicture != null)
            {
                if (string.IsNullOrEmpty(currentEvent.EventPicture))
                {
                 await   fileStorage.RemoveFile(currentEvent.EventPicture, ApplicationConstants.ImageContainerNames.EventImagesContainer);
                }
                currentEvent.EventPicture = await SaveImage(eventCreationDTO.EventPicture);

            }
            context.Entry(currentEvent);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Event>(id);
        }

        [HttpPost("{eventId:int}/adduser")]
        public async Task<ActionResult> AddUserToEvent(int eventId , AddUserToEventDTO addUserToEventDTO)
        {
            var userExist = await context.Users.AnyAsync(u => u.Id == addUserToEventDTO.UserId);

            if (!userExist)
            {
                return NotFound();
            }
            var actualEvent = await context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
            if (actualEvent == null )
            {
                return NotFound();
            }

            var userInscription = new ApplicationUserEvents
            {
                EventId = eventId,
                UserId = addUserToEventDTO.UserId,
                InscriptionDate = DateTime.Now,
                IsActive =true
            };

            context.Add(userInscription);


            var eventActivity = new UserActivity
            { 
                Text = $"Te has unido al evento {actualEvent.Name}",
                UserId = addUserToEventDTO.UserId
            };

            context.Add(eventActivity);


            bool haveBadge = await context.ApplicationUserBadges.AnyAsync(b => b.UserId == addUserToEventDTO.UserId && b.BadgeId == 2);
            if (!haveBadge)
            {
                var userBadge = new ApplicationUserBadges
                {
                    BadgeId = 2,
                    UserId = addUserToEventDTO.UserId
                };
                context.Add(userBadge);
            }


            await context.SaveChangesAsync();



            return NoContent();
        }
        



        [HttpDelete("{eventId:int}/removeuser/{userId}")]
        public async Task<ActionResult> RemoveUserFromEvent(int eventId, string userId)
        {
            var eventInscription = await context.ApplicationUserEvents
                .Include(ei => ei.Event)
                .FirstOrDefaultAsync(ei => ei.EventId == eventId && ei.UserId == userId);
            if (eventInscription == null)
            {
                return NotFound();
            }
            if(!eventInscription.IsActive)
            {
                return BadRequest();
            }

            context.Attach(eventInscription);
            eventInscription.IsActive = false;

            var eventActivity = new UserActivity
            {
                Text = $"Has abandonado el evento {eventInscription.Event.Name}",
                UserId = userId
            };

            context.Add(eventActivity);

            await context.SaveChangesAsync();

            return NoContent();

        } 

        [HttpGet("userEvents/{userId}")]
        public async Task<ActionResult<List<UserEventInscriptionDTO>>> GetUserEvents(string userId)
        {
            var userEvents = await context.ApplicationUserEvents.Include(x => x.Event)
                .Where(ei => ei.UserId == userId && ei.Event.IsActive).OrderByDescending(ei => ei.InscriptionDate).ToListAsync();

            var dtos = mapper.Map<List<UserEventInscriptionDTO>>(userEvents);


            return dtos;
            

        }



        private async Task HideOldEvent()
        {
            var eventActive = await context.Events.FirstOrDefaultAsync(x => x.IsTimer);
            if(eventActive == null)
            {
                return;
            }
            context.Attach(eventActive);
            eventActive.IsTimer = false;
            await context.SaveChangesAsync();
        }
        private async Task<string> SaveImage(IFormFile formFile)
        {
            var bytes = await formFile.ConvertToByteArray();

            return await fileStorage.SaveFile(bytes, Path.GetExtension(formFile.FileName), ApplicationConstants.ImageContainerNames.EventImagesContainer, formFile.ContentType, Guid.NewGuid().ToString());
        }


    }
}
