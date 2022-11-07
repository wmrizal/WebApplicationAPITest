using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationAPITest;
using WebApplicationAPITest.Data;

namespace WebApplicationAPITest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly WebApplicationAPITestContext _context;

        public UsersController(WebApplicationAPITestContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers([FromQuery] string? sortby = null, string? email = null, string? phone = null)
        {
            var contextOperation = _context.Users.Where(x => 1==1);
            if(email != null)
            {
                contextOperation = contextOperation.Where(x => x.Email.Contains(email));  
            }
            if(phone != null)
            {
                contextOperation = contextOperation.Where(x => string.IsNullOrEmpty(x.PhoneNumber) ? false : x.PhoneNumber.Contains(phone));
            }

            if (sortby != null)
            {
                switch (sortby)
                {
                    case "id":
                        return await contextOperation.OrderBy(x=>x.Id).ToListAsync();
                    case "name":
                        return await contextOperation.OrderBy(x => x.FullName).ToListAsync();
                    case "email":
                        return await contextOperation.OrderBy(x => x.Email).ToListAsync();
                    case "phone":
                        return await contextOperation.OrderBy(x => x.PhoneNumber).ToListAsync();
                    case "age":
                        return await contextOperation.OrderBy(x => x.Age).ToListAsync();
                    default:
                        return BadRequest("sortby valid values are [id,name,email,phone,age]");
                }
            }
            else
            {
                return await contextOperation.ToListAsync();
            }
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUsers(Guid id)
        {
            var users = await _context.Users.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers(Guid id, Users users)
        {
            if (id != users.Id)
            {
                return BadRequest();
            }

            _context.Entry(users).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Users>> PostUsers(Users users)
        {
            if( String.IsNullOrEmpty(users.FullName) || String.IsNullOrEmpty(users.Email))
            {
                return BadRequest("FullName or Email missing");
            }

            users.Id = Guid.NewGuid();

            _context.Users.Add(users);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsers", new { id = users.Id }, users);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsers(Guid id)
        {
            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            _context.Users.Remove(users);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsersExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
