using Growtify.Application.Common.Mappings;
using Growtify.Application.DTOs.Account;
using Growtify.Application.Interfaces.Services;
using Growtify.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Growtify.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ITokenService tokenService;

        public AccountService(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
        }

        public async Task<(UserDto user, string refreshToken)?> RegisterAsync(RegisterDto dto)
        {
            if (await userManager.FindByEmailAsync(dto.Email) != null)
                return null;

            AppUser user = new AppUser
            {
                DisplayName = dto.DisplayName,
                Email = dto.Email,
                UserName = dto.Email,
            };

            IdentityResult? result = await userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded) return null;

            user.Member = new Member
            {
                Id = user.Id,
                UserName = dto.Email,
                Gender = dto.Gender,
                City = dto.City,
                Country = dto.Country,
                DateOfBirth = dto.DateOfBirth
            };

            await userManager.UpdateAsync(user);

            await userManager.AddToRoleAsync(user, "Member");

            string refreshToken = tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await userManager.UpdateAsync(user);

            UserDto userDto = await user.ToDto(tokenService);

            return (userDto, refreshToken);
        }

        public async Task<(UserDto user, string refreshToken)?> LoginAsync(LoginDto dto)
        {
            AppUser? user = await userManager.FindByEmailAsync(dto.Email);
            if (user == null) return null;

            bool passwordValid = await userManager.CheckPasswordAsync(user, dto.Password);
            if (!passwordValid) return null;

            var refreshToken = tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await userManager.UpdateAsync(user);

            var userDto = await user.ToDto(tokenService);

            return (userDto, refreshToken);
        }

        public async Task<(UserDto user, string refreshToken)?> RefreshTokenAsync(string refreshToken)
        {
            AppUser? user = await userManager.Users
                .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken &&
                                          x.RefreshTokenExpiry > DateTime.UtcNow);

            if (user == null) return null;

            string newRefreshToken = tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await userManager.UpdateAsync(user);

            UserDto userDto = await user.ToDto(tokenService);

            return (userDto, newRefreshToken);
        }
    }
}
