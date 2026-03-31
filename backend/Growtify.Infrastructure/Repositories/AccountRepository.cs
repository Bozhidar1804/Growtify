using Growtify.Application.DTOs.Account;
using Growtify.Application.Interfaces.Repositories;
using Growtify.Domain.Entities;
using Growtify.Infrastructure.Data;
using Growtify.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;


namespace Growtify.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly GrowtifyDbContext context;

        public AccountRepository(GrowtifyDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await context.AppUsers.AnyAsync(x => x.Email == email);
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            AppUser? user = await context.AppUsers
                    .Include(x => x.Member)
                    .SingleOrDefaultAsync(x => x.Email == email);

            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                DisplayName = user.UserName!,
                ImageUrl = user.Member?.ImageUrl,
                Token = "" // token is set later
            };
        }

        public async Task AddUserAsync(UserDto userDto)
        {
            AppUser appUser = new AppUser
            {
                Id = userDto.Id,
                Email = userDto.Email,
                UserName = userDto.Email,
                DisplayName = userDto.DisplayName
            };

            await context.AppUsers.AddAsync(appUser);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
