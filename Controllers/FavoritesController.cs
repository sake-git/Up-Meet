using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpMeet.Api.Data;
using UpMeet.Api.Models;

namespace UpMeet.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoritesController : ControllerBase
    {
        private readonly EventDbContext _context;

        public FavoritesController(EventDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetFavorites([FromQuery] int userId)
        {
            var favorites = await _context.FavoriteEvents
                .Where(f => f.UserId == userId)
                .Include(f => f.Event)
                .ToListAsync();

            return Ok(favorites);
        }
    }
}
