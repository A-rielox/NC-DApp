using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Entities;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _context.AppUsers.ToListAsync();

            return users;
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            var user = await _context.AppUsers.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, AppUser appUser)
        {
            if (id != appUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(appUser).State = EntityState.Modified;

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

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AppUser>> PostUser(AppUser appUser)
        {
            _context.AppUsers.Add(appUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = appUser.Id }, appUser);
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var appUser = await _context.AppUsers.FindAsync(id);
            if (appUser == null)
            {
                return NotFound();
            }

            _context.AppUsers.Remove(appUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        private bool UserExists(int id)
        {
            return _context.AppUsers.Any(e => e.Id == id);
        }
    }
}
