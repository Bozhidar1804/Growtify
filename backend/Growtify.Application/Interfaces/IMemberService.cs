using Growtify.Application.DTOs.Account;
using Growtify.Domain.Entities;

namespace Growtify.Application.Interfaces
{
    public interface IMemberService
    {
        Task<bool> UpdateMemberAsync(string memberId, MemberUpdateDto dto);
        Task<Member?> GetMemberByIdAsync(string id);
        Task<List<Member>> GetAllAsync();
        Task<List<Photo>> GetPhotosForMemberAsync(string memberId);
        Task<bool> SaveAllAsync();
    }
}
