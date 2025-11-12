using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Growtify.Domain.Entities;
using Growtify.Infrastructure.Data;
using Growtify.Application.DTOs.Account;
using Growtify.Application.Interfaces;
using Growtify.API.Extensions;

namespace Growtify.API.Controllers
{
    public class AccountController(GrowtifyDbContext dbContext, ITokenService tokenService) : BaseApiController
    {
        [HttpPost("register")] // POST: api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await EmailExists(registerDto.Email))
            {
                return BadRequest("Email is already taken.");
            }

            using var hmac = new HMACSHA512();

            AppUser newUser = new AppUser
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            dbContext.AppUsers.Add(newUser);
            await dbContext.SaveChangesAsync();

            return newUser.ToDto(tokenService); 
        }

        [HttpPost("login")] // POST: api/account/login
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await dbContext.AppUsers.SingleOrDefaultAsync(x => x.Email == loginDto.Email);

            if (user == null) return Unauthorized("Invalid email address");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (user.PasswordHash[i] != computedHash[i]) return Unauthorized("Invalid password");
            }

            return user.ToDto(tokenService);
        }

        private async Task<bool> EmailExists(string email)
        {
            return await dbContext.AppUsers.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }
    }
}
