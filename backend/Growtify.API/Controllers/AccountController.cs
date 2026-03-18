using Microsoft.AspNetCore.Mvc;
using Growtify.Application.DTOs.Account;
using Growtify.Application.Interfaces.Services;

namespace Growtify.API.Controllers
{
    public class AccountController(IAccountService accountService) : BaseApiController
    {
        [HttpPost("register")] // POST: api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto dto)
        {
            var result = await accountService.RegisterAsync(dto);

            if (result == null)
                return BadRequest("Email is already taken.");

            return Ok(result);
        }

        [HttpPost("login")] // POST: api/account/login
        public async Task<ActionResult<UserDto>> Login(LoginDto dto)
        {
            var result = await accountService.LoginAsync(dto);

            if (result == null)
                return Unauthorized("Invalid credentials");

            return Ok(result);
        }
    }
}
