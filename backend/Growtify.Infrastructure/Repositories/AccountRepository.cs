using Growtify.Application.Interfaces.Repositories;
using Growtify.Domain.Entities;
using Growtify.Infrastructure.Data;
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

        public async Task<AppUser?> GetUserByEmailAsync(string email)
        {
            return await context.AppUsers
                .Include(x => x.Member)
                .SingleOrDefaultAsync(x => x.Email == email);
        }

        public async Task AddUserAsync(AppUser user)
        {
            await context.AppUsers.AddAsync(user);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
