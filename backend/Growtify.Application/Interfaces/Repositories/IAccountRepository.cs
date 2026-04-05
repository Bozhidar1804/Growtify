using Growtify.Application.DTOs.Account;
using Growtify.Domain.Entities;

namespace Growtify.Application.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task AddUserAsync(UserDto user);
        Task<bool> SaveChangesAsync();
    }
}
