using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using up_meet_api.Entities;
using System.Security.Cryptography;
using up_meet_api.DTOs;

namespace up_meet_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly EventDbContext _context;
        private ILogger<UsersController> _logger;

        public UsersController(EventDbContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        

        //Function to get User by id
        // GET: api/Users/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("User not Found");
            }

            UserDto userDto = new UserDto()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                LoginId = user.LoginId,
            };

            return userDto;
        }

        //To creates User
        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> PostUser([FromBody] UserDto userDto)
        {
            this._logger.LogInformation("Post called");

            //Check if user login id already exists in Database
            User user = this._context.Users.Where(data => data.LoginId == userDto.LoginId).FirstOrDefault();

            if (user != null)
            {
                //Login id is already present, error out
                return BadRequest("Login Id already exists");
            }

            user = new User() //create new user
            {
                Name = userDto.Name,
                Email = userDto.Email,
                LoginId = userDto.LoginId,
                Password = this.EncryptPassowrd(userDto.Password),
                Phone = userDto.Phone
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, userDto);

        }

        //Function to authenticate user id and password
        // GET: api/Users/5
        [HttpPost]
        [Route("Authenticate")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]            
        public async Task<ActionResult<UserDto>> AuthenticateUser([FromBody] UserDto userDto)
        {
            this._logger.LogInformation("Authenticate User called");
            byte[] password = EncryptPassowrd(userDto.Password);
            User? user = await _context.Users
                .Where(data => data.LoginId.ToLower() == userDto.LoginId.ToLower() && data.Password.SequenceEqual(password))
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound("User or Password is invalid");
            }

            userDto = new UserDto()
            {
                Id = user.Id,
                LoginId = user.LoginId,
                Phone = user.Phone,
                Email = user.Email,
                Name = user.Name
            };
            // userDto.Token = GenerateToken(userDto);
            return Ok(userDto);
        }


        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        //This function returns encoded password
        private byte[] EncryptPassowrd(string password)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] hashvalue;
            UTF8Encoding utfEncoding = new UTF8Encoding();
            hashvalue = sha256.ComputeHash(utfEncoding.GetBytes(password));
            return hashvalue;
        }


        /*
                // GET: api/Users
                [HttpGet]
                public async Task<ActionResult<IEnumerable<User>>> GetUsers()
                {
                    return await _context.Users.ToListAsync();
                }


                // PUT: api/Users/5
                // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
                [HttpPut("{id}")]
                public async Task<IActionResult> PutUser(int id, User user)
                {
                    if (id != user.Id)
                    {
                        return BadRequest();
                    }

                    _context.Entry(user).State = EntityState.Modified;

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserExists(id))
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



                // DELETE: api/Users/5
                [HttpDelete("{id}")]
                public async Task<IActionResult> DeleteUser(int id)
                {
                    var user = await _context.Users.FindAsync(id);
                    if (user == null)
                    {
                        return NotFound();
                    }

                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();

                    return NoContent();
                }*/


    }
}
