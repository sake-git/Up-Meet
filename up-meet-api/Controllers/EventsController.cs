using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using up_meet_api.DTOs;
using up_meet_api.Entities;


namespace up_meet_api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly EventDbContext _context;        
        private ILogger<EventsController> _logger;

        public EventsController(EventDbContext context, ILogger<EventsController> logger)
        {
            _context = context;
            this._logger = logger;
        }

        //Get all events
        // GET: api/Events
        
        [HttpGet]       
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents()
        {
            List<EventDto> eventsDto = await this._context.Events
                .Include(x => x.CreatedByNavigation)
                .Select(data =>
                new EventDto()
                {
                    Id = data.Id,
                    Location = data.Location,
                    Name = data.Name,
                    EventDateTime = data.EventDateTime,
                    ImgUrl = data.ImgUrl,
                    Description = data.Description,
                    Price = data.Price,
                    KidsAllowed = data.KidsAllowed,
                    Duration = data.Duration,
                    CreatedBy = data.CreatedBy,
                    CreatedByUser = data.CreatedByNavigation.LoginId

                }
            ).ToListAsync();
            return Ok(eventsDto);
        }

        //Get event based on event id and user id. User id is current user who want to see the event details.
        //It will be used to check if the event is user's favourite or not
        // GET: api/Events/5/1
        [HttpGet("{eventId}/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EventDto>> GetEvent(int eventId, int userId)
        {
            //Get the event and related favourite list
            Event? userEvent = await _context.Events
                .Include(x => x.CreatedByNavigation) // users
                .Include(x=> x.FavouriteEvents.Where(data=> data.UserId == userId))//favouriteEvents
                .Where(data => data.Id == eventId).FirstOrDefaultAsync();

            // Event userEvent = await _context.Events.FirstOrDefaultAsync(data => data.Id == id);

            if (userEvent == null)
            {
                //Event not found error out
                return NotFound($"Event with id {eventId} not found");
            }
                                
            //Event found. Create a DTO object and send the data
              EventDto eventDto = new EventDto()
              {
                  Id = userEvent.Id,
                  Location = userEvent.Location,
                  Name = userEvent.Name,
                  EventDateTime = userEvent.EventDateTime,
                  ImgUrl = userEvent.ImgUrl,
                  Description = userEvent.Description,
                  Price = userEvent.Price,
                  KidsAllowed = userEvent.KidsAllowed,
                  Duration = userEvent.Duration,
                  CreatedBy = userEvent.CreatedBy,
                  CreatedByUser = userEvent.CreatedByNavigation.LoginId,
                  //Check if this is user's favourite event            
                  isFavourite = userEvent.FavouriteEvents?.ToList().Count != 0 ? true : false
              };

              return Ok(eventDto);            
        }

        //Create event
        // POST: api/Events
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EventDto>> PostEvent(EventDto eventDto)
        {
            _logger.LogInformation("Event creation called");
            Event userEvent = new Event()
            {
                Location = eventDto.Location,
                EventDateTime = eventDto.EventDateTime,
                Name = eventDto.Name,
                ImgUrl = eventDto.ImgUrl,
                Description = eventDto.Description,
                Price = eventDto.Price,
                KidsAllowed = eventDto.KidsAllowed,
                Duration = eventDto.Duration,
                CreatedBy = eventDto.CreatedBy,
                CreatedDate = DateTime.Now
            };
            _context.Events.Add(userEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { eventId = eventDto.Id, userId = eventDto.CreatedBy }, eventDto);
        }

        //Delete event
        // DELETE: api/Events/5/1
        [HttpDelete("{userId}/{eventId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteEvent(int userId, int eventId)
        {
            Event? userEvent = await _context.Events.FindAsync(eventId);

            if (userEvent == null)
            {
                //User event not found, error out
                return NotFound($"Event Id {eventId} not found ");
            }
            if (userId != userEvent.CreatedBy)
            {
                //This user didn't not create the event. Hence not authorize to delete. Error out! 
                return Unauthorized("User is not authorized to delete this event");
            }

            //Delete related records from favourite
            var favourites = _context.FavouriteEvents.Where(data => data.EventId == eventId).ToList(); ;

            foreach (var favouriteEvent in favourites)
            {
                _context.FavouriteEvents.Remove(favouriteEvent);

            }
            await _context.SaveChangesAsync();

            _context.Events.Remove(userEvent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

/*
        // PUT: api/Events/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event @event)
        {
            if (id != @event.Id)
            {
                return BadRequest();
            }

            _context.Entry(@event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
*/
       
    }
}
