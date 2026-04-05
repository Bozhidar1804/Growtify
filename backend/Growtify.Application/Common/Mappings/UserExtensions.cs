using Growtify.Application.DTOs.Account;
using Growtify.Application.Interfaces.Services;
using Growtify.Domain.Entities;

namespace Growtify.Application.Common.Mappings
{
    public static class UserExtensions
    {
        public static async Task<UserDto> ToDto(this AppUser user, ITokenService tokenService)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                DisplayName = user.DisplayName,
                ImageUrl = user.ImageUrl,
                Token = await tokenService.CreateToken(user)
            };
        }
    }
}
