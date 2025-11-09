using System.Security.Cryptography;
using System.Text;

using Microsoft.AspNetCore.Mvc;

using Growtify.Domain.Entities;
using Growtify.Infrastructure.Data;

namespace Growtify.API.Controllers
{
    public class AccountController(GrowtifyDbContext dbContext) : BaseApiController
    {
        [HttpPost("register")] // POST: api/account/register
        public async Task<ActionResult<AppUser>> Register(string email, string userName, string password)
        {
            using var hmac = new HMACSHA512();

            AppUser newUser = new AppUser
            {
                Email = email,
                UserName = userName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key
            };

            dbContext.AppUsers.Add(newUser);
            await dbContext.SaveChangesAsync();

            return newUser;
        }
    }
}
