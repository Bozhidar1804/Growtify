using Growtify.Domain.Entities;

namespace Growtify.Application.Interfaces.Repositories
{
    public interface ILikesRepository
    {
        Task<MemberLike?> GetMemberLikeAsync(string sourceMemberId, string targetMemberId);
        Task<IReadOnlyList<Member>> GetMemberLikesAsync(string predicate, string memberId);
        Task<IReadOnlyList<string>> GetCurrentMemberLikeIdsAsync(string memberId);
        void DeleteLike(MemberLike like);
        void AddLike(MemberLike like);
        Task<bool> SaveChangesAsync();
    }
}
