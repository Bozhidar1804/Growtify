using Growtify.Domain.Entities;

namespace Growtify.Application.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task<AppUser?> GetUserByEmailAsync(string email);
        Task AddUserAsync(AppUser user);
        Task<bool> SaveChangesAsync();
    }
}
