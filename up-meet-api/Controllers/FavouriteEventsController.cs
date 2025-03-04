using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using up_meet_api.DTOs;
using up_meet_api.Entities;

namespace up_meet_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouriteEventsController : ControllerBase
    {
        private readonly EventDbContext _context;
        private readonly ILogger<FavouriteEventsController> _logger;

        public FavouriteEventsController(EventDbContext context, ILogger<FavouriteEventsController> logger)
        {
            _context = context;
            _logger = logger;
        }


        // GET: api/FavouriteEvents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FavouriteEvent>> GetFavouriteEvent(int id)
        {
            var favouriteEvent = await _context.FavouriteEvents.FindAsync(id);

            if (favouriteEvent == null)
            {
                return NotFound();
            }

            return favouriteEvent;
        }



        //Get the favourite events of given user
        // GET: api/FavouriteEvents
        [HttpGet]
        [Route("list/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetFavouriteEventList(int id)
        {
            //Get list of favourite items  
            List<FavouriteEvent> listFavEvents = _context.FavouriteEvents.Where(data =>
               data.UserId == id).ToList();            

            //Convert favourite item to list of eventDto and send it as response
            List<EventDto> listFavEventsDto = new List<EventDto>();
            listFavEvents.ForEach(data =>
            {
                Event userEvent = _context.Events.Include(x => x.CreatedByNavigation).Where(evnt => evnt.Id == data.EventId).FirstOrDefault();
                _logger.LogInformation(userEvent.ToString());
                listFavEventsDto.Add(new EventDto()
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
                    CreatedByUser = userEvent.CreatedByNavigation.LoginId

                });
            });

            return Ok(listFavEventsDto);
        }

        //Add item to Favourite list for given user
        // POST: api/FavouriteEvents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FavouriteEventDto>> PostFavouriteEvent([FromBody] FavouriteEventDto obj)
        {
            _logger.LogInformation($"UserId: {obj.UserId} EventId: {obj.EventId}");

            FavouriteEvent favouriteEvent = new FavouriteEvent()
            {
                EventId = obj.EventId,
                UserId = obj.UserId,
            };

            _context.FavouriteEvents.Add(favouriteEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFavouriteEvent", new { id = favouriteEvent.Id }, favouriteEvent);
        }

        //Delete the item from favourite list for given user
        // DELETE: api/FavouriteEvents/5/1
        [HttpDelete("{userId}/{eventId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteFavouriteEvent(int userId, int eventId)
        {
            var favouriteEvent = _context.FavouriteEvents.Where(data => data.UserId == userId && data.EventId == eventId).FirstOrDefault();
            if (favouriteEvent == null)
            {
                return NotFound("Favourite item not found");
            }

            _context.FavouriteEvents.Remove(favouriteEvent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /*

        // GET: api/FavouriteEvents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FavouriteEvent>>> GetFavouriteEvents()
        {
            return await _context.FavouriteEvents.ToListAsync();
        }


        // PUT: api/FavouriteEvents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFavouriteEvent(int id, FavouriteEvent favouriteEvent)
        {
            if (id != favouriteEvent.Id)
            {
                return BadRequest();
            }

            _context.Entry(favouriteEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FavouriteEventExists(id))
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

        

        private bool FavouriteEventExists(int id)
        {
            return _context.FavouriteEvents.Any(e => e.Id == id);
        }*/
    }
}
