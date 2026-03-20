using Growtify.Application.DTOs.Account;
using Growtify.Application.Interfaces.Services;
using Growtify.Domain.Entities;

namespace Growtify.Application.Common.Mappings
{
    public static class UserMappings
    {
        public static UserDto ToDto(this AppUser user, ITokenService tokenService)
        {   
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Token = tokenService.CreateToken(user),
                ImageUrl = user.Member?.ImageUrl
            };
        }
    }
}
