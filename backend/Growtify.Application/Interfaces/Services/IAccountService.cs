using Growtify.Application.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Growtify.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<UserDto?> RegisterAsync(RegisterDto dto);
        Task<UserDto?> LoginAsync(LoginDto dto);
    }
}
