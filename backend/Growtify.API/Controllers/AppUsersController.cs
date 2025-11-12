using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Growtify.Domain.Entities;
using Growtify.Infrastructure.Data;

namespace Growtify.API.Controllers
{
    public class AppUsersController(GrowtifyDbContext dbContext) : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            List<AppUser> users = await dbContext.AppUsers.ToListAsync();

            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id}")] // localhost:5001/api/appusers/{id}
        public async Task<IActionResult> GetUserById(string id)
        {
            AppUser? user = await dbContext.AppUsers.FindAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] string userName)
        {
            AppUser newUser = new AppUser
            {
                UserName = userName,
                Email = $"{userName.ToLower()}@example.com",
                PasswordHash = Array.Empty<byte>(),
                PasswordSalt = Array.Empty<byte>()
            };

            dbContext.AppUsers.Add(newUser);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
        }
    }
}
