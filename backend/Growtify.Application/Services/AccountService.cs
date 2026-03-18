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

            using var hmac = new HMACSHA512();

            AppUser? user = new AppUser
            {
                Email = dto.Email,
                UserName = dto.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
                PasswordSalt = hmac.Key,
                Member = new Member
                {
                    UserName = dto.UserName,
                    Gender = dto.Gender,
                    City = dto.City,
                    Country = dto.Country,
                    DateOfBirth = dto.DateOfBirth,
                    Created = DateTime.UtcNow,
                    LastActive = DateTime.UtcNow
                }
            };

            await accountRepository.AddUserAsync(user);

            if (!await accountRepository.SaveChangesAsync())
                return null;

            return user.ToDto(tokenService);
        }

        public async Task<UserDto?> LoginAsync(LoginDto dto)
        {
            AppUser? user = await accountRepository.GetUserByEmailAsync(dto.Email);
            if (user == null) return null;

            using var hmac = new HMACSHA512(user.PasswordSalt);

            byte[]? computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return null;
            }

            return user.ToDto(tokenService);
        }
    }
}
