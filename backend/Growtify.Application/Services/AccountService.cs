using Growtify.Application.DTOs.Account;
using Growtify.Application.Interfaces.Services;
using Growtify.Domain.Entities;
using Microsoft.AspNetCore.Identity;

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

        public async Task<UserDto?> RegisterAsync(RegisterDto dto)
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

            UserDto userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                DisplayName = user.DisplayName,
                ImageUrl = user.Member?.ImageUrl,
                Token = ""
            };

            userDto.Token = tokenService.CreateToken(userDto);

            return userDto;
        }

        public async Task<UserDto?> LoginAsync(LoginDto dto)
        {
            AppUser? user = await userManager.FindByEmailAsync(dto.Email);
            if (user == null) return null;

            bool passwordValid = await userManager.CheckPasswordAsync(user, dto.Password);
            if (!passwordValid) return null;

            UserDto userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                DisplayName = user.DisplayName,
                ImageUrl = user.Member?.ImageUrl,
                Token = ""
            };

            userDto.Token = tokenService.CreateToken(userDto);

            return userDto;
        }
    }
}
