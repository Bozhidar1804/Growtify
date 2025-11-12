using Growtify.Application.DTOs.Account;
using Growtify.Application.Interfaces;
using Growtify.Domain.Entities;

namespace Growtify.API.Extensions
{
    public static class AppUserExtensions
    {
        public static UserDto ToDto(this AppUser user, ITokenService tokenService)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Token = tokenService.CreateToken(user)
            };
        }
    }
}
