using Growtify.Domain.Entities;

namespace Growtify.Application.Interfaces.Services
{
    public interface ILikeService
    {
        Task<bool> ToggleLikeAsync(string sourceMemberId, string targetMemberId);

        Task<IReadOnlyList<string>> GetCurrentMemberLikeIdsAsync(string memberId);

        Task<IReadOnlyList<Member>> GetMemberLikesAsync(string predicate, string memberId);
    }
}
