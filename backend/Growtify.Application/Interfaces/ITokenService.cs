using Growtify.Domain.Entities;

namespace Growtify.Application.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
