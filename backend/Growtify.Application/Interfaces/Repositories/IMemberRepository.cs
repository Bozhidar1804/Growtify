using Growtify.Domain.Entities;

namespace Growtify.Application.Interfaces.Repositories
{
    public interface IMemberRepository
    {
        Task<IReadOnlyList<Member>> GetMembersAsync();
        Task<Member?> GetMemberByIdAsync(string memberId);
        Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(string memberId);
        void UpdateMember(Member member);
    }
}
