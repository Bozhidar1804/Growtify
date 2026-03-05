using Growtify.Domain.Entities;
using Growtify.Application.Common.Pagination;

namespace Growtify.Application.Interfaces.Repositories
{
    public interface IMemberRepository
    {
        Task<PaginatedResult<Member>> GetMembersAsync(MemberParams memberParams);
        Task<Member?> GetMemberByIdAsync(string memberId);
        Task<List<Photo>> GetPhotosForMemberAsync(string memberId);
        void UpdateMember(Member member);
        public Task<Member?> GetMemberForUpdate(string id);
        Task<bool> SaveChangesAsync();
    }
}
