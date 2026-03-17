using Growtify.Application.Common.Pagination;
using Growtify.Application.DTOs.Account;
using Growtify.Domain.Entities;

namespace Growtify.Application.Interfaces.Services
{
    public interface IMemberService
    {
        Task<bool> UpdateMemberAsync(string memberId, MemberUpdateDto dto);
        Task<Member?> GetMemberByIdAsync(string id);
        Task<PaginatedResult<Member>> GetAllAsync(MemberParams memberParams);
        Task<List<Photo>> GetPhotosForMemberAsync(string memberId);
        Task<bool> SaveAllAsync();
    }
}
