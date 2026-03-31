using Growtify.Application.Common.Mappings;
using Growtify.Application.DTOs.Account;
using Growtify.Application.Interfaces.Repositories;
using Growtify.Application.Interfaces.Services;
using Growtify.Domain.Entities;
using System.Security.Cryptography;
using System.Text;


namespace Growtify.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository accountRepository;
        private readonly ITokenService tokenService;

        public AccountService(IAccountRepository accountRepository, ITokenService tokenService)
        {
            this.accountRepository = accountRepository;
            this.tokenService = tokenService;
        }

        public async Task<UserDto?> RegisterAsync(RegisterDto dto)
        {
            if (await accountRepository.EmailExistsAsync(dto.Email))
                return null;

            UserDto userDto = new UserDto
            {
                Id = Guid.NewGuid().ToString(),
                Email = dto.Email,
                DisplayName = dto.DisplayName,
                ImageUrl = null,
                Token = ""
            };

            await accountRepository.AddUserAsync(userDto);

            if (!await accountRepository.SaveChangesAsync())
                return null;

            userDto.Token = tokenService.CreateToken(userDto);

            return userDto;
        }

        public async Task<UserDto?> LoginAsync(LoginDto dto)
        {
            var user = await accountRepository.GetUserByEmailAsync(dto.Email);
            if (user == null) return null;

            user.Token = tokenService.CreateToken(user);

            return user;
        }
    }
}
