using Growtify.Application.DTOs.Account;
using Growtify.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Growtify.API.Controllers
{
    public class AccountController(IAccountService accountService) : BaseApiController
    {
        [HttpPost("register")] // POST: api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto dto)
        {
            var result = await accountService.RegisterAsync(dto);

            if (result == null)
                return BadRequest("Registration failed.");

            SetRefreshTokenCookie(result.Value.refreshToken);

            return Ok(result.Value.user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto dto)
        {
            var result = await accountService.LoginAsync(dto);

            if (result == null)
                return Unauthorized();

            SetRefreshTokenCookie(result.Value.refreshToken);

            return Ok(result.Value.user);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<UserDto>> RefreshToken()
        {
            string refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken == null) return NoContent();

            var result = await accountService.RefreshTokenAsync(refreshToken);

            if (result == null) return Unauthorized();

            SetRefreshTokenCookie(result.Value.refreshToken);

            return Ok(result.Value.user);
        }

        private void SetRefreshTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
