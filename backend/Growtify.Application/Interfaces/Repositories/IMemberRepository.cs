using Growtify.Domain.Entities;

namespace Growtify.Application.Interfaces.Repositories
{
    public interface IMemberRepository
    {
        Task<List<Member>> GetMembersAsync();
        Task<Member?> GetMemberByIdAsync(string memberId);
        Task<List<Photo>> GetPhotosForMemberAsync(string memberId);
        void UpdateMember(Member member);
    }
}
