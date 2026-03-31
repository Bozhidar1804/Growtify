using Growtify.Application.DTOs.Account;
using Growtify.Domain.Entities;

namespace Growtify.Application.Interfaces.Services
{
    public interface ITokenService
    {
        string CreateToken(UserDto user);
    }
}
